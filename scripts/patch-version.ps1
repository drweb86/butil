# Bump the current release version by one patch (last number) after a failed build.
# Updates CHANGELOG.md, both Gradle version literals, and the Fastlane en-US changelog file.
# GitHub Actions already reads CHANGELOG.md and passes it to Gradle. F-Droid does not,
# so the literals in build.gradle.kts are the version that build publishes.
# Android UI strings are maintained in sources/android and are not generated from resx.

[CmdletBinding()]
param(
    [string]$RepoRoot
)

$ErrorActionPreference = "Stop"
trap {
    Write-Error $_
    exit 1
}

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $scriptDir = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Path }
    $RepoRoot = (Resolve-Path (Join-Path $scriptDir "..")).Path
}

$utf8 = [System.Text.UTF8Encoding]::new($false)
$changelogPath = Join-Path $RepoRoot "CHANGELOG.md"
$gradlePath = Join-Path $RepoRoot "sources\android\app\build.gradle.kts"
$fastlaneDir = Join-Path $RepoRoot "fastlane\metadata\android\en-US\changelogs"
$maxFastlaneBytes = 500

function Read-Utf8Text([string]$Path) {
    if (-not (Test-Path -LiteralPath $Path)) {
        throw "File not found: $Path"
    }
    return [System.IO.File]::ReadAllText($Path, [System.Text.Encoding]::UTF8)
}

function Write-Utf8Text([string]$Path, [string]$Text) {
    $directory = Split-Path -Parent $Path
    if (-not (Test-Path -LiteralPath $directory)) {
        New-Item -ItemType Directory -Path $directory | Out-Null
    }
    [System.IO.File]::WriteAllText($Path, $Text, $utf8)
}

function ConvertTo-VersionCode([int]$Year, [int]$Month, [int]$Patch) {
    return "{0}{1:D2}{2:D2}" -f $Year, $Month, $Patch
}

function ConvertTo-VersionName([int]$Year, [int]$Month, [int]$Patch) {
    return "{0}.{1:D2}.{2:D2}" -f $Year, $Month, $Patch
}

function ConvertTo-FastlaneChangelog([string]$Markdown) {
    $text = $Markdown -replace "`r`n", "`n" -replace "`r", "`n"
    $text = $text -replace '(?m)^\s*\(unreleased\)\s*$', ''
    $text = $text -replace '\*\*(.+?)\*\*', '$1'
    $text = $text -replace '\[([^\]]+)\]\([^)]+\)', '$1'
    $text = $text -replace '`([^`]+)`', '$1'

    $lines = foreach ($line in ($text -split "`n")) {
        $trimmed = $line.TrimEnd()
        if ($trimmed -match '^\s*$') {
            continue
        }
        if ($trimmed -match '^##\s+') {
            continue
        }
        if ($trimmed -match '^-\s+(.+)$') {
            $Matches[1].Trim()
            continue
        }
        $trimmed.Trim()
    }

    $joined = ($lines -join "`n").Trim()
    if (-not $joined) {
        $joined = "Bug fixes and improvements."
    }

    $ellipsis = "..."
    $bytes = $utf8.GetBytes($joined)
    if ($bytes.Length -le $maxFastlaneBytes) {
        return $joined + "`n"
    }

    $limit = $maxFastlaneBytes - $utf8.GetByteCount($ellipsis)
    $cut = $joined
    while ($utf8.GetByteCount($cut) -gt $limit -and $cut.Length -gt 0) {
        $cut = $cut.Substring(0, $cut.Length - 1)
    }
    $break = $cut.LastIndexOfAny(@("`n", " ", ".", ";", ","))
    if ($break -ge 40) {
        $cut = $cut.Substring(0, $break).TrimEnd()
    }
    return $cut.TrimEnd() + $ellipsis + "`n"
}

$changelog = Read-Utf8Text $changelogPath
if ($changelog.Length -gt 0 -and [int][char]$changelog[0] -eq 65279) {
    $changelog = $changelog.Substring(1)
}

$newline = if ($changelog.Contains("`r`n")) { "`r`n" } else { "`n" }
$lines = [regex]::Split($changelog, '\r?\n')
$headingLine = $lines[0]
if ($headingLine -notmatch '^[#] (\d{4})\.(\d{2})\.(\d+)\s*$') {
    throw "CHANGELOG.md must start with a heading like '# 2026.09.19' (got '$headingLine')."
}

$year = [int]$Matches[1]
$month = [int]$Matches[2]
$patch = [int]$Matches[3]
$oldName = ConvertTo-VersionName $year $month $patch
$oldCode = ConvertTo-VersionCode $year $month $patch

$patch += 1
$newName = ConvertTo-VersionName $year $month $patch
$newCode = ConvertTo-VersionCode $year $month $patch
if ([int]$newCode -le [int]$oldCode) {
    throw "New versionCode $newCode is not greater than $oldCode."
}

$lines[0] = "# $newName"
$sectionLines = New-Object System.Collections.Generic.List[string]
for ($i = 1; $i -lt $lines.Length; $i++) {
    if ($lines[$i] -match '^[#] \d{4}\.\d{2}\.\d+') {
        break
    }
    $sectionLines.Add($lines[$i]) | Out-Null
}
$section = [string]::Join("`n", $sectionLines.ToArray())
$fastlaneText = ConvertTo-FastlaneChangelog $section
$changelog = [string]::Join($newline, $lines)

$gradle = Read-Utf8Text $gradlePath
$codeReplaced = [regex]::Replace($gradle, '(?m)(val defaultVersionCode = )\d+', "`${1}$newCode")
$nameReplaced = [regex]::Replace($codeReplaced, '(?m)(val defaultVersionName = ")[^"]+(")', "`${1}$newName`${2}")
if ($nameReplaced -eq $gradle -or $nameReplaced -notmatch [regex]::Escape("val defaultVersionCode = $newCode") -or $nameReplaced -notmatch [regex]::Escape("val defaultVersionName = `"$newName`"")) {
    throw "Could not update both version literals in $gradlePath."
}

$oldFastlane = Join-Path $fastlaneDir "$oldCode.txt"
$newFastlane = Join-Path $fastlaneDir "$newCode.txt"
if ((Test-Path -LiteralPath $oldFastlane) -and ($oldFastlane -ne $newFastlane)) {
    Move-Item -LiteralPath $oldFastlane -Destination $newFastlane -Force
}

Write-Utf8Text $changelogPath $changelog
Write-Utf8Text $gradlePath $nameReplaced
Write-Utf8Text $newFastlane $fastlaneText

Write-Output "Version $oldName ($oldCode) -> $newName ($newCode)"
Write-Output "Updated: CHANGELOG.md"
Write-Output "Updated: sources/android/app/build.gradle.kts"
Write-Output "Updated: fastlane/metadata/android/en-US/changelogs/$newCode.txt"
