# Build the Android APK, start an emulator if none is running, install the APK,
# and load sample photos, documents, and nested folders for manual checks.
# Usage (from the repo root):
#   powershell -File run-android-emulator.ps1
#   powershell -File run-android-emulator.ps1 -Release
#   powershell -File run-android-emulator.ps1 -Avd Medium_Phone_API_37.0
#   powershell -File run-android-emulator.ps1 -SkipSamples
# Samples land in the emulator at /sdcard/Download/BUtilSamples.

param(
    [switch]$Release,
    [switch]$SkipSamples,
    [string]$Avd = ""
)

$ErrorActionPreference = "Stop"
$ProjectRoot = $PSScriptRoot
$PackageId = "com.siarheikuchuk.butil"
$DeviceSampleRoot = "/sdcard/Download/BUtilSamples"

function Find-AndroidSdk {
    $candidates = @(
        $env:ANDROID_HOME,
        $env:ANDROID_SDK_ROOT,
        "${env:LOCALAPPDATA}\Android\Sdk",
        "${env:LOCALAPPDATA}\BUtilAndroidSdk",
        "${env:LOCALAPPDATA}\FtpsServerAndroidSdk"
    )
    foreach ($c in $candidates) {
        if ([string]::IsNullOrWhiteSpace($c)) { continue }
        if (Test-Path (Join-Path $c "platforms")) { return $c }
    }
    throw "Android SDK not found. Set ANDROID_HOME to your SDK directory."
}

function Get-OnlineSerials([string]$Adb) {
    $serials = @()
    foreach ($line in (& $Adb devices)) {
        if ($line -match '^(?<id>\S+)\s+device$') {
            $serials += $Matches.id
        }
    }
    return $serials
}

function Get-AvdName([string]$Emulator, [string]$Requested) {
    $avds = @(& $Emulator -list-avds | Where-Object {
        $_ -and ($_ -notmatch '^(INFO|ERROR|WARNING)\b')
    })
    if ($avds.Count -eq 0) {
        throw "No Android Virtual Device found. Create one in Android Studio Device Manager, then run this script again."
    }
    if ($Requested) {
        $match = $avds | Where-Object { $_ -eq $Requested } | Select-Object -First 1
        if (-not $match) {
            throw "AVD '$Requested' was not found. Available: $($avds -join ', ')"
        }
        return $match
    }
    $preferred = $avds | Where-Object { $_ -eq "Medium_Phone_API_37.0" } | Select-Object -First 1
    if ($preferred) { return $preferred }
    return $avds[0]
}

function Wait-ForBoot([string]$Adb, [string]$Serial) {
    Write-Host "Waiting for $Serial to finish booting ..."
    & $Adb -s $Serial wait-for-device | Out-Null
    $deadline = (Get-Date).AddMinutes(4)
    do {
        $boot = (& $Adb -s $Serial shell getprop sys.boot_completed 2>$null)
        if ($boot) { $boot = $boot.Trim() }
        if ($boot -eq "1") {
            & $Adb -s $Serial shell input keyevent 82 | Out-Null
            return
        }
        Start-Sleep -Seconds 2
    } while ((Get-Date) -lt $deadline)
    throw "Emulator $Serial did not finish booting within 4 minutes."
}

function Install-Apk([string]$Adb, [string]$Serial, [string]$Apk) {
    Write-Host "Installing $PackageId on $Serial ..."
    $last = ""
    for ($attempt = 1; $attempt -le 8; $attempt++) {
        $output = & $Adb -s $Serial install -r $Apk 2>&1 | Out-String
        $last = $output.Trim()
        if ($LASTEXITCODE -eq 0 -and $output -match "Success") { return }
        Write-Host "Install attempt $attempt did not succeed yet. $last"
        Start-Sleep -Seconds 3
    }
    throw "adb install failed for $PackageId. $last"
}

function New-SampleImage {
    param(
        [string]$Path,
        [string]$Label,
        [System.Drawing.Color]$Color,
        [System.Drawing.Imaging.ImageFormat]$Format,
        [int]$Width = 640,
        [int]$Height = 480
    )
    $directory = Split-Path $Path -Parent
    New-Item -ItemType Directory -Force -Path $directory | Out-Null
    $bitmap = New-Object System.Drawing.Bitmap $Width, $Height
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.Clear($Color)
        $font = New-Object System.Drawing.Font "Segoe UI", 26, ([System.Drawing.FontStyle]::Bold)
        $brush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::Black)
        try {
            $graphics.DrawString($Label, $font, $brush, (New-Object System.Drawing.PointF 28, 180))
            $graphics.DrawString((Split-Path $Path -Leaf), $font, $brush, (New-Object System.Drawing.PointF 28, 240))
        } finally {
            $font.Dispose()
            $brush.Dispose()
        }
        $bitmap.Save($Path, $Format)
    } finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }
}

function New-SamplePdf([string]$Path, [string]$Title) {
    $safe = ($Title -replace '[^A-Za-z0-9 .,-]', ' ')
    $stream = "BT`n/F1 24 Tf`n72 720 Td`n($safe) Tj`nET`n"
    $objects = @(
        "1 0 obj`n<< /Type /Catalog /Pages 2 0 R >>`nendobj`n",
        "2 0 obj`n<< /Type /Pages /Kids [3 0 R] /Count 1 >>`nendobj`n",
        "3 0 obj`n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 4 0 R /Resources << /Font << /F1 5 0 R >> >> >>`nendobj`n",
        "4 0 obj`n<< /Length $($stream.Length) >>`nstream`n$stream`nendstream`nendobj`n",
        "5 0 obj`n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>`nendobj`n"
    )
    $builder = New-Object System.Text.StringBuilder
    [void]$builder.Append("%PDF-1.4`n")
    $offsets = @()
    foreach ($obj in $objects) {
        $offsets += $builder.Length
        [void]$builder.Append($obj)
    }
    $xref = $builder.Length
    [void]$builder.Append("xref`n0 6`n")
    [void]$builder.Append("0000000000 65535 f `n")
    foreach ($offset in $offsets) {
        [void]$builder.Append(("{0:D10} 00000 n `n" -f $offset))
    }
    [void]$builder.Append("trailer`n<< /Size 6 /Root 1 0 R >>`nstartxref`n$xref`n%%EOF`n")
    $directory = Split-Path $Path -Parent
    New-Item -ItemType Directory -Force -Path $directory | Out-Null
    [System.IO.File]::WriteAllText($Path, $builder.ToString())
}

function New-SampleWav([string]$Path, [int]$Frequency) {
    $rate = 8000
    $count = [int]($rate * 0.5)
    $pcm = New-Object byte[] ($count * 2)
    for ($i = 0; $i -lt $count; $i++) {
        $sample = [int](12000 * [Math]::Sin(2 * [Math]::PI * $Frequency * $i / $rate))
        $pcm[$i * 2] = [byte]($sample -band 0xFF)
        $pcm[$i * 2 + 1] = [byte](($sample -shr 8) -band 0xFF)
    }
    $memory = New-Object System.IO.MemoryStream
    $writer = New-Object System.IO.BinaryWriter $memory
    try {
        $writer.Write([Text.Encoding]::ASCII.GetBytes("RIFF"))
        $writer.Write([int](36 + $pcm.Length))
        $writer.Write([Text.Encoding]::ASCII.GetBytes("WAVE"))
        $writer.Write([Text.Encoding]::ASCII.GetBytes("fmt "))
        $writer.Write([int]16)
        $writer.Write([int16]1)
        $writer.Write([int16]1)
        $writer.Write([int]$rate)
        $writer.Write([int]($rate * 2))
        $writer.Write([int16]2)
        $writer.Write([int16]16)
        $writer.Write([Text.Encoding]::ASCII.GetBytes("data"))
        $writer.Write([int]$pcm.Length)
        $writer.Write($pcm)
        $writer.Flush()
        $directory = Split-Path $Path -Parent
        New-Item -ItemType Directory -Force -Path $directory | Out-Null
        [System.IO.File]::WriteAllBytes($Path, $memory.ToArray())
    } finally {
        $writer.Dispose()
        $memory.Dispose()
    }
}

function Write-SampleText([string]$Path, [string]$Content) {
    $directory = Split-Path $Path -Parent
    New-Item -ItemType Directory -Force -Path $directory | Out-Null
    Set-Content -Path $Path -Value $Content -Encoding utf8
}

function New-SampleTree {
    Add-Type -AssemblyName System.Drawing
    $root = Join-Path ([System.IO.Path]::GetTempPath()) "BUtilSamples"
    if (Test-Path $root) { Remove-Item $root -Recurse -Force }
    New-Item -ItemType Directory -Force -Path $root | Out-Null

    $jpeg = [System.Drawing.Imaging.ImageFormat]::Jpeg
    $png = [System.Drawing.Imaging.ImageFormat]::Png
    $gif = [System.Drawing.Imaging.ImageFormat]::Gif
    $bmp = [System.Drawing.Imaging.ImageFormat]::Bmp
    $tiff = [System.Drawing.Imaging.ImageFormat]::Tiff

    $photos = Join-Path $root "Photos"
    New-SampleImage (Join-Path $photos "2023\winter\snow.jpg") "Winter snow" ([System.Drawing.Color]::LightBlue) $jpeg
    New-SampleImage (Join-Path $photos "2023\winter\cabin.png") "Cabin" ([System.Drawing.Color]::BurlyWood) $png -Width 800 -Height 600
    New-SampleImage (Join-Path $photos "2024\summer-trip\beach.jpg") "Beach" ([System.Drawing.Color]::Khaki) $jpeg
    New-SampleImage (Join-Path $photos "2024\summer-trip\trail.gif") "Trail" ([System.Drawing.Color]::DarkSeaGreen) $gif
    New-SampleImage (Join-Path $photos "2024\summer-trip\sunset.jpeg") "Sunset" ([System.Drawing.Color]::SandyBrown) $jpeg -Width 320 -Height 240

    $docs = Join-Path $root "Documents"
    Write-SampleText (Join-Path $docs "work\notes\meeting.txt") "Meeting notes`r`nDiscuss backup folders and import rules.`r`nAttendees: Alex, Mira"
    Write-SampleText (Join-Path $docs "work\notes\budget.csv") "item,amount`r`nstorage,12`r`ncamera,40"
    New-SamplePdf (Join-Path $docs "work\contracts\agreement.pdf") "Sample agreement"
    Write-SampleText (Join-Path $docs "personal\letter.txt") "A short personal letter kept beside a copied photo."
    Copy-Item (Join-Path $photos "2024\summer-trip\beach.jpg") (Join-Path $docs "personal\beach-copy.jpg")

    $mixed = Join-Path $root "Mixed"
    New-SampleImage (Join-Path $mixed "overview.bmp") "Overview" ([System.Drawing.Color]::LightGray) $bmp
    Write-SampleText (Join-Path $mixed "readme.md") "# Mixed folder`r`nPhotos, notes, and a deeper nest live around this file."
    New-SampleImage (Join-Path $mixed "nested\deep\scan.tiff") "Scanned page" ([System.Drawing.Color]::WhiteSmoke) $tiff
    New-SamplePdf (Join-Path $mixed "nested\deep\invoice.pdf") "Sample invoice"
    New-SampleWav (Join-Path $mixed "nested\deep\voice-note.wav") 523
    Copy-Item (Join-Path $photos "2024\summer-trip\sunset.jpeg") (Join-Path $mixed "nested\deep\extra.jfif")

    $spaced = Join-Path $root "Folder with space"
    New-SampleImage (Join-Path $spaced "scan 01.png") "Scan 01" ([System.Drawing.Color]::LightSalmon) $png
    $cafeName = "caf$([char]0x00E9).png"
    New-SampleImage (Join-Path $spaced $cafeName) "Cafe" ([System.Drawing.Color]::PaleGreen) $png

    return @(
        @{ Rel = "Photos/2023/winter/snow.jpg"; Stamp = "202301151030.00" },
        @{ Rel = "Photos/2023/winter/cabin.png"; Stamp = "202301160915.00" },
        @{ Rel = "Photos/2024/summer-trip/beach.jpg"; Stamp = "202407041630.00" },
        @{ Rel = "Photos/2024/summer-trip/trail.gif"; Stamp = "202407051101.00" },
        @{ Rel = "Photos/2024/summer-trip/sunset.jpeg"; Stamp = "202407052045.00" },
        @{ Rel = "Documents/work/notes/meeting.txt"; Stamp = "202502101400.00" },
        @{ Rel = "Documents/work/notes/budget.csv"; Stamp = "202502111115.00" },
        @{ Rel = "Documents/work/contracts/agreement.pdf"; Stamp = "202503010900.00" },
        @{ Rel = "Documents/personal/letter.txt"; Stamp = "202412241800.00" },
        @{ Rel = "Documents/personal/beach-copy.jpg"; Stamp = "202407041630.00" },
        @{ Rel = "Mixed/overview.bmp"; Stamp = "202506011200.00" },
        @{ Rel = "Mixed/readme.md"; Stamp = "202506011205.00" },
        @{ Rel = "Mixed/nested/deep/scan.tiff"; Stamp = "202506021430.00" },
        @{ Rel = "Mixed/nested/deep/invoice.pdf"; Stamp = "202506021435.00" },
        @{ Rel = "Mixed/nested/deep/voice-note.wav"; Stamp = "202506021440.00" },
        @{ Rel = "Mixed/nested/deep/extra.jfif"; Stamp = "202308190812.00" },
        @{ Rel = "Folder with space/scan 01.png"; Stamp = "202509031010.00" },
        @{ Rel = "Folder with space/$cafeName"; Stamp = "202509031011.00" }
    )
}

function Send-SampleTree([string]$Adb, [string]$Serial) {
    Write-Host "Creating sample photos and documents ..."
    $timestamps = New-SampleTree
    $localRoot = Join-Path ([System.IO.Path]::GetTempPath()) "BUtilSamples"
    & $Adb -s $Serial shell "mkdir -p /sdcard/Download && rm -rf '$DeviceSampleRoot'" | Out-Null
    & $Adb -s $Serial push $localRoot /sdcard/Download/ | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "adb push of sample files failed." }
    foreach ($item in $timestamps) {
        $deviceFile = "$DeviceSampleRoot/$($item.Rel)"
        & $Adb -s $Serial shell "touch -t $($item.Stamp) '$deviceFile'" | Out-Null
    }
    Write-Host "Samples: $DeviceSampleRoot"
    Write-Host "  Photos/2023/winter and Photos/2024/summer-trip  (jpg, jpeg, png, gif)"
    Write-Host "  Documents/work and Documents/personal           (txt, csv, pdf, duplicate jpg)"
    Write-Host "  Mixed/nested/deep                               (bmp, tiff, pdf, wav, jfif)"
    Write-Host "  Folder with space                               (name with a space, and a non-ASCII name)"
    Write-Host "beach.jpg and beach-copy.jpg are the same bytes and the same modified time."
}

$sdk = Find-AndroidSdk
$adb = Join-Path $sdk "platform-tools\adb.exe"
$emulator = Join-Path $sdk "emulator\emulator.exe"
if (-not (Test-Path $adb)) { throw "adb not found at $adb" }
if (-not (Test-Path $emulator)) { throw "emulator not found at $emulator" }

$serial = Get-OnlineSerials $adb | Where-Object { $_ -like "emulator-*" } | Select-Object -First 1
if (-not $serial) {
    $avdName = Get-AvdName $emulator $Avd
    Write-Host "Starting emulator $avdName ..."
    Start-Process -FilePath $emulator -ArgumentList @("-avd", $avdName, "-netdelay", "none", "-netspeed", "full") | Out-Null
} else {
    Write-Host "Emulator already running: $serial"
}

$buildScript = Join-Path $ProjectRoot "build-android.ps1"
if ($Release) {
    & $buildScript -Release
    $apk = Join-Path $ProjectRoot "Output\butil_android.apk"
} else {
    & $buildScript
    $apk = Join-Path $ProjectRoot "Output\butil_android_debug.apk"
}
if (-not (Test-Path $apk)) { throw "APK was not produced: $apk" }

$serial = Get-OnlineSerials $adb | Where-Object { $_ -like "emulator-*" } | Select-Object -First 1
if (-not $serial) { throw "Emulator did not appear in adb devices." }
Wait-ForBoot $adb $serial
Install-Apk $adb $serial $apk
if (-not $SkipSamples) { Send-SampleTree $adb $serial }

Write-Host "Opening $PackageId ..."
& $adb -s $serial shell am start -n "$PackageId/.MainActivity" | Out-Null
if ($LASTEXITCODE -ne 0) { throw "Could not start $PackageId" }
Write-Host "OK"
