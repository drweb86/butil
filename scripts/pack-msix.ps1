# Pack unsigned Store MSIX from published butil-ui.Desktop output (x64 and arm64).
# Partner Center re-signs after certification; do not Authenticode-sign these packages.

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$Version,
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
)

$ErrorActionPreference = "Stop"
trap {
    Write-Error $_
    exit 1
}

function ConvertTo-MsixVersion([string]$ChangelogVersion) {
    $parts = @($ChangelogVersion.Trim().Split(".") | ForEach-Object { [int]$_ })
    while ($parts.Count -lt 4) {
        $parts += 0
    }
    if ($parts.Count -gt 4) {
        $parts = $parts[0..3]
    }
    foreach ($n in $parts) {
        if ($n -lt 0 -or $n -gt 65535) {
            throw "MSIX version segment $n is outside 0-65535 (from '$ChangelogVersion')."
        }
    }
    return ($parts -join ".")
}

function Find-MakeAppx {
    $searchRoots = @(
        (Join-Path ${env:ProgramFiles(x86)} "Windows Kits\10\bin"),
        (Join-Path $env:ProgramFiles "Windows Kits\10\bin")
    )
    $exe = $null
    foreach ($kitsBin in $searchRoots) {
        if (-not (Test-Path $kitsBin)) {
            continue
        }
        $exe = Get-ChildItem $kitsBin -Recurse -Filter "makeappx.exe" -ErrorAction SilentlyContinue |
            Where-Object { $_.Directory.Name -eq "x64" } |
            Sort-Object { $_.Directory.Parent.Name } -Descending |
            Select-Object -First 1
        if ($exe) {
            break
        }
    }
    if (-not $exe) {
        throw "makeappx.exe not found under Windows Kits. Install the Windows 10/11 SDK (MakeAppx)."
    }
    return $exe.FullName
}

$msixVersion = ConvertTo-MsixVersion $Version
$makeAppx = Find-MakeAppx
$packageDir = Join-Path $RepoRoot "sources\butil-ui.Desktop.Package"
$template = Join-Path $packageDir "Package.appxmanifest"
$assetsSrc = Join-Path $packageDir "Assets"
$publishRoot = Join-Path $RepoRoot "Output\publish"
$outDir = Join-Path $RepoRoot "Output"

if (-not (Test-Path $template)) {
    throw "Missing $template"
}
if (-not (Test-Path $assetsSrc)) {
    throw "Missing $assetsSrc"
}

New-Item -ItemType Directory -Force -Path $outDir | Out-Null

$arches = @(
    @{ Folder = "x64"; ManifestArch = "x64" }
    @{ Folder = "arm64"; ManifestArch = "arm64" }
)

foreach ($arch in $arches) {
    $publishDir = Join-Path $publishRoot $arch.Folder
    $exe = Join-Path $publishDir "butil-ui.Desktop.exe"
    if (-not (Test-Path $exe)) {
        throw "Missing $exe. Publish win-$($arch.Folder) before packing MSIX."
    }

    $staging = Join-Path $RepoRoot "Output\msix-staging\$($arch.Folder)"
    if (Test-Path $staging) {
        Remove-Item $staging -Recurse -Force
    }
    New-Item -ItemType Directory -Force -Path $staging | Out-Null

    foreach ($name in @("THIRD-PARTY-NOTICES.md", "CREDITS.md")) {
        $source = Join-Path $RepoRoot $name
        if (-not (Test-Path $source)) {
            throw "Missing $source"
        }
        Copy-Item $source (Join-Path $publishDir $name) -Force
    }
    Copy-Item -Path (Join-Path $publishDir "*") -Destination $staging -Recurse -Force
    $assetsDest = Join-Path $staging "Assets"
    if (Test-Path $assetsDest) {
        Remove-Item $assetsDest -Recurse -Force
    }
    New-Item -ItemType Directory -Force -Path $assetsDest | Out-Null
    Copy-Item -Path (Join-Path $assetsSrc "*") -Destination $assetsDest -Recurse -Force

    $manifestText = [System.IO.File]::ReadAllText($template)
    if ($manifestText.Length -gt 0 -and [int]$manifestText[0] -eq 0xFEFF) {
        $manifestText = $manifestText.Substring(1)
    }
    $manifestText = $manifestText.Replace("__VERSION__", $msixVersion)
    $manifestText = $manifestText.Replace("__ARCH__", $arch.ManifestArch)
    $manifestPath = Join-Path $staging "AppxManifest.xml"
    $utf8NoBom = New-Object System.Text.UTF8Encoding $false
    [System.IO.File]::WriteAllText($manifestPath, $manifestText, $utf8NoBom)

    $msixName = "butil_${Version}_windows_$($arch.Folder).msix"
    $msixPath = Join-Path $outDir $msixName
    if (Test-Path $msixPath) {
        Remove-Item $msixPath -Force
    }

    Write-Output "Packing $msixName (Identity Version $msixVersion, $($arch.ManifestArch)) with $makeAppx"
    & $makeAppx pack /d $staging /p $msixPath /o
    if ($LASTEXITCODE -ne 0) {
        throw "makeappx failed for $($arch.Folder) with exit code $LASTEXITCODE"
    }
}

Remove-Item (Join-Path $RepoRoot "Output\msix-staging") -Recurse -Force -ErrorAction SilentlyContinue
Write-Output "MSIX packages written to $outDir"
exit 0
