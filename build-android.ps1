# Android debug/release build for the Kotlin stub.
# Usage (from the repo root):
#   powershell -File build-android.ps1
#   powershell -File build-android.ps1 -Install
#   powershell -File build-android.ps1 -Release
# Debug APK: Output/butil_android_debug.apk
# Release APK: Output/butil_<version>_android.apk
# Release reads CHANGELOG.md when -VersionName is omitted.
# Signing uses ANDROID_KEYSTORE_FILE, ANDROID_KEYSTORE_PASSWORD,
# ANDROID_KEY_ALIAS, and ANDROID_KEY_PASSWORD when they are set.

param(
    [switch]$Install,
    [switch]$Release,
    [string]$VersionName = "",
    [string]$VersionCode = ""
)

$ErrorActionPreference = "Stop"
$ProjectRoot = $PSScriptRoot
$AndroidRoot = Join-Path $ProjectRoot "sources\android"

function Find-JavaHome {
    $candidates = @(
        $env:JAVA_HOME,
        "C:\Program Files\Android\openjdk\jdk-21.0.8",
        "C:\Program Files\Android\openjdk\jdk-17.0.14",
        "C:\Program Files\Microsoft\jdk-21.0.8-hotspot",
        "C:\Program Files\Eclipse Adoptium\jdk-21*"
    )
    foreach ($c in $candidates) {
        if ([string]::IsNullOrWhiteSpace($c)) { continue }
        $resolved = Get-Item $c -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($null -eq $resolved) { continue }
        $java = Join-Path $resolved.FullName "bin\java.exe"
        if (Test-Path $java) { return $resolved.FullName }
    }
    throw "JDK 17+ not found. Install a JDK or set JAVA_HOME."
}

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

function Invoke-Gradle([string[]]$Tasks) {
    $gradleArgs = @("--no-daemon") + $Tasks
    if ($VersionName) { $gradleArgs += "-PappVersionName=$VersionName" }
    if ($VersionCode) { $gradleArgs += "-PappVersionCode=$VersionCode" }
    Write-Host "Building $($Tasks -join ', ') ..."
    Push-Location $AndroidRoot
    try {
        & .\gradlew.bat @gradleArgs
        if ($LASTEXITCODE -ne 0) { throw "Gradle $($Tasks -join ' ') failed with exit code $LASTEXITCODE" }
    } finally {
        Pop-Location
    }
}

function Copy-ToOutput($src, $fileName) {
    if (-not $src -or -not (Test-Path $src)) {
        throw "APK not found: $src"
    }
    $outDir = Join-Path $ProjectRoot "Output"
    New-Item -ItemType Directory -Force -Path $outDir | Out-Null
    $dest = Join-Path $outDir $fileName
    Copy-Item $src $dest -Force
    Write-Host ("{0}: {1}" -f $fileName, $dest)
    Write-Host ("  Size: {0:N2} MB" -f ((Get-Item $dest).Length / 1MB))
    return $dest
}

if ($Release -and [string]::IsNullOrWhiteSpace($VersionName)) {
    $heading = Get-Content (Join-Path $ProjectRoot "CHANGELOG.md") -First 1
    if ($heading.Length -gt 0 -and [int][char]$heading[0] -eq 65279) {
        $heading = $heading.Substring(1)
    }
    $VersionName = $heading.Substring(2).Trim()
    if ([string]::IsNullOrWhiteSpace($VersionCode)) {
        $VersionCode = $VersionName -replace '\.', ''
    }
    Write-Host "Version is $VersionName"
}

$javaHome = Find-JavaHome
$sdk = Find-AndroidSdk
$env:JAVA_HOME = $javaHome
$env:ANDROID_HOME = $sdk
$env:ANDROID_SDK_ROOT = $sdk
$env:Path = "$javaHome\bin;$sdk\platform-tools;$env:Path"

Write-Host "JAVA_HOME    = $javaHome"
Write-Host "ANDROID_HOME = $sdk"

$sdkDirProp = ($sdk -replace '\\', '\\') -replace ':', '\:'
Set-Content -Path (Join-Path $AndroidRoot "local.properties") -Value "sdk.dir=$sdkDirProp" -Encoding ASCII

$apkOutputs = Join-Path $AndroidRoot "app\build\outputs\apk"

if ($Release) {
    Invoke-Gradle @("assembleRelease")
    $releaseDir = Join-Path $apkOutputs "release"
    $apk = @("app-release.apk", "app-release-unsigned.apk") |
        ForEach-Object { Join-Path $releaseDir $_ } |
        Where-Object { Test-Path $_ } |
        Select-Object -First 1
    $copied = Copy-ToOutput $apk "butil_android.apk"
    if ($VersionName) {
        Copy-ToOutput $copied "butil_${VersionName}_android.apk" | Out-Null
    }
    $packageId = "com.siarheikuchuk.butil"
} else {
    Invoke-Gradle @("assembleDebug")
    $copied = Copy-ToOutput (Join-Path $apkOutputs "debug\app-debug.apk") "butil_android_debug.apk"
    $packageId = "com.siarheikuchuk.butil"
}

if ($Install) {
    $adb = Join-Path $sdk "platform-tools\adb.exe"
    if (-not (Test-Path $adb)) { throw "adb not found at $adb" }
    $devices = & $adb devices | Select-String "`tdevice$"
    if (-not $devices) {
        Write-Host "No Android device/emulator with status 'device'. Start an emulator or plug in a phone, then re-run with -Install."
        exit 0
    }
    Write-Host "Installing $packageId ..."
    & $adb install -r $copied
    if ($LASTEXITCODE -ne 0) { throw "adb install failed for $packageId" }
}

Write-Host "OK"
