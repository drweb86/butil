# Build THIRD-PARTY-NOTICES.md from the NuGet packages restored for the Windows apps.
# Run from the repo after `dotnet restore` of butil-ui.Desktop and butilc.

[CmdletBinding()]
param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
)

$ErrorActionPreference = "Stop"

$assetFiles = @(
    (Join-Path $RepoRoot "sources\butil-ui.Desktop\obj\project.assets.json"),
    (Join-Path $RepoRoot "sources\butilc\obj\project.assets.json"),
    (Join-Path $RepoRoot "sources\BUtil.Windows\obj\project.assets.json")
)

$packages = @{}
foreach ($assetPath in $assetFiles) {
    if (-not (Test-Path $assetPath)) {
        throw "Missing $assetPath. Restore the Windows projects first."
    }
    $json = Get-Content $assetPath -Raw | ConvertFrom-Json
    foreach ($property in $json.libraries.PSObject.Properties) {
        $lib = $property.Value
        if ($lib.type -ne "package") { continue }
        $id, $version = $property.Name.Split("/", 2)
        $key = $id.ToLowerInvariant()
        if (-not $packages.ContainsKey($key)) {
            $packages[$key] = [pscustomobject]@{ Id = $id; Version = $version }
        }
    }
}

$nugetRoot = Join-Path $env:USERPROFILE ".nuget\packages"

function Read-NuspecLicense([string]$Id, [string]$Version) {
    $nuspec = Join-Path $nugetRoot ((Join-Path $Id.ToLowerInvariant() $Version) + "\" + $Id.ToLowerInvariant() + ".nuspec")
    if (-not (Test-Path $nuspec)) {
        $dir = Join-Path $nugetRoot $Id.ToLowerInvariant()
        $found = Get-ChildItem $dir -Recurse -Filter "*.nuspec" -ErrorAction SilentlyContinue |
            Where-Object { $_.Directory.Name -eq $Version } |
            Select-Object -First 1
        if (-not $found) { return $null }
        $nuspec = $found.FullName
    }
    [xml]$xml = Get-Content $nuspec
    $meta = $xml.package.metadata
    $license = $meta.license
    $expression = $null
    if ($license) {
        $type = $license.type
        if ($type -eq "expression") { $expression = [string]$license.InnerText }
        else { $expression = "file:" + [string]$license.InnerText }
    } elseif ($meta.licenseUrl) {
        $expression = [string]$meta.licenseUrl
    }
    [pscustomobject]@{
        Authors = [string]$meta.authors
        Copyright = [string]$meta.copyright
        License = $expression
        ProjectUrl = [string]$meta.projectUrl
    }
}

$skip = @{
    "avalonia.buildservices" = $true
}

$rows = foreach ($pkg in ($packages.Values | Sort-Object Id)) {
    if ($skip.ContainsKey($pkg.Id.ToLowerInvariant())) { continue }
    $info = Read-NuspecLicense $pkg.Id $pkg.Version
    $license = if ($info) { $info.License } else { "" }
    if ([string]::IsNullOrWhiteSpace($license) -and $pkg.Id -eq "Siarhei_Kuchuk.FtpsServerLibrary") {
        $license = "MIT"
    }
    if ([string]::IsNullOrWhiteSpace($license)) { $license = "UNKNOWN" }
    [pscustomobject]@{
        Id = $pkg.Id
        Version = $pkg.Version
        Authors = $(if ($info) { $info.Authors } else { "" })
        Copyright = $(if ($info) { $info.Copyright } else { "" })
        License = $license
        ProjectUrl = $(if ($info) { $info.ProjectUrl } else { "" })
    }
}

$rows = @($rows) + [pscustomobject]@{
    Id = ".NET Runtime"
    Version = "10"
    Authors = ""
    Copyright = "Copyright (c) .NET Foundation and Contributors"
    License = "MIT"
    ProjectUrl = "https://dot.net/"
}

function Add-ProseLine([string]$Line) {
    $trimmed = $Line.Trim()
    if ($trimmed.Length -eq 0) {
        [void]$sb.AppendLine("")
        return
    }
    if ($trimmed.StartsWith("#") -or $trimmed.StartsWith("- ") -or $trimmed.StartsWith("* ")) {
        $trimmed = '\' + $trimmed
    }
    [void]$sb.AppendLine($trimmed)
}

function Add-ProseFile([string]$Path) {
    foreach ($line in (Get-Content $Path)) {
        Add-ProseLine $line
    }
    [void]$sb.AppendLine("")
}

$sb = New-Object System.Text.StringBuilder
[void]$sb.AppendLine("# Third-party notices")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("BUtil's own code is dedicated to the public domain under CC0 1.0 Universal. The components listed here are not covered by that dedication. Each remains under the license stated for it. This file is distributed with the application.")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("The Windows builds include the .NET runtime. It is listed with the MIT components.")
[void]$sb.AppendLine("")

$groups = $rows | Group-Object License | Sort-Object Name
foreach ($group in $groups) {
    $heading = $group.Name
    if ($heading -eq "file:LICENSE") { $heading = "Bundled license" }
    [void]$sb.AppendLine("## $heading")
    [void]$sb.AppendLine("")
    foreach ($row in ($group.Group | Sort-Object Id)) {
        $detail = if ($row.Copyright) { $row.Copyright.Trim() } elseif ($row.Authors) { "Authors: $($row.Authors)" } else { "" }
        if ($detail -and -not $detail.EndsWith('.')) { $detail += '.' }
        $item = "- **$($row.Id)** $($row.Version)"
        if ($detail) { $item += ". $detail" }
        if ($row.ProjectUrl -match '^https?://') {
            $url = ($row.ProjectUrl -split '\?')[0]
            $item += " [$url]($url)"
        }
        [void]$sb.AppendLine($item)
    }
    [void]$sb.AppendLine("")
}

function Append-LicenseText([string]$Title, [string]$Path) {
    [void]$sb.AppendLine("## $Title")
    [void]$sb.AppendLine("")
    Add-ProseFile $Path
}

$texts = Join-Path $PSScriptRoot "license-texts"
[void]$sb.AppendLine("The copyright lines in each group above apply to that license text.")
[void]$sb.AppendLine("")
Append-LicenseText "MIT License text" (Join-Path $texts "MIT.txt")
Append-LicenseText "Apache License 2.0 text" (Join-Path $texts "Apache-2.0.txt")

$angle = $rows | Where-Object { $_.Id -eq "Avalonia.Angle.Windows.Natives" } | Select-Object -First 1
$angleLicense = if ($angle) { Join-Path $nugetRoot ("avalonia.angle.windows.natives\" + $angle.Version + "\LICENSE") } else { $null }
if ($angleLicense -and (Test-Path $angleLicense)) {
    Append-LicenseText "Avalonia.Angle.Windows.Natives license text" $angleLicense
}

[void]$sb.AppendLine("## Inter typeface (shipped in Avalonia.Fonts.Inter)")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("Copyright 2016 The Inter Project Authors (https://github.com/rsms/inter)")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("Reserved Font Name: Inter")
[void]$sb.AppendLine("")
Add-ProseFile (Join-Path $texts "OFL-1.1.txt")

$noticeNames = @("NOTICE", "NOTICE.txt", "NOTICE.md")
foreach ($row in ($rows | Sort-Object Id)) {
    if ($row.Id -eq ".NET Runtime") { continue }
    $dir = Join-Path $nugetRoot ($row.Id.ToLowerInvariant() + "\" + $row.Version)
    if (-not (Test-Path $dir)) { continue }
    $notice = Get-ChildItem $dir -Recurse -File -ErrorAction SilentlyContinue |
        Where-Object { $noticeNames -contains $_.Name } |
        Select-Object -First 1
    if (-not $notice) { continue }
    [void]$sb.AppendLine("## NOTICE from $($row.Id) $($row.Version)")
    [void]$sb.AppendLine("")
    Add-ProseFile $notice.FullName
}

$out = Join-Path $RepoRoot "THIRD-PARTY-NOTICES.md"
$stale = Join-Path $RepoRoot "THIRD-PARTY-NOTICES.txt"
if (Test-Path $stale) { Remove-Item $stale -Force }
$utf8 = New-Object System.Text.UTF8Encoding $false
[System.IO.File]::WriteAllText($out, $sb.ToString().TrimEnd() + [Environment]::NewLine, $utf8)
Write-Output "Wrote $($rows.Count) packages to $out"
$rows | Group-Object License | Sort-Object Count -Descending | ForEach-Object { "{0,4} {1}" -f $_.Count, $_.Name }
