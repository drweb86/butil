$ErrorActionPreference = "Stop"

$version = Get-Content "..\help\Version History (Changelog).md" -First 1
$version = $version.Substring(2)
$changes = Get-Content -Path "..\help\Version History (Changelog).md" -Raw
$changes = $changes.Substring($changes.IndexOf("`n")).Trim("`n", "`r")
$changes = $changes.Substring(0, $changes.IndexOf("`n# ")).Trim("`n", "`r");
$changes = $changes.Replace("`r", "").Replace("`n", "\n").Replace("'", "").Replace("""", "")

Write-Output "Version is $version"
Write-Output "Changes are $changes"

Write-Output "Checking if required repositories are checked out."
$wingetForkFolder="../../winget-pkgs"
if (-Not (Test-Path $wingetForkFolder))
{
	Write-Error "Checkout https://github.com/drweb86/winget-pkgs into folder $wingetForkFolder" 
	Exit 1
}

Write-Output "Prepare win-get release"
$wingetReleaseFolder="$($wingetForkFolder)\manifests\s\SiarheiKuchuk\BUtil\$($version)"
$wingetReleaseDateReplacement = $version -replace '\.', '-'
$wingetReleaseHashArm64 = Get-FileHash -Path "..\Output\BUtil_v$($version)_win-arm64.exe" -Algorithm SHA256
$wingetReleaseHashX64 = Get-FileHash -Path "..\Output\BUtil_v$($version)_win-x64.exe" -Algorithm SHA256

if (Test-Path $wingetReleaseFolder)
{
	Remove-Item $wingetReleaseFolder -Confirm:$false -Recurse:$true
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}
}
md "$($wingetReleaseFolder)"

$currentYear = "{0:yyyy}" -f (Get-Date)

& ".\tools\Template-Copy.ps1"`
    -TemplateFilePath "tools\winget-pkgs\SiarheiKuchuk.BUtil.installer.yaml" `
    -DestinationFilePath "$wingetReleaseFolder\SiarheiKuchuk.BUtil.installer.yaml" `
    -Replacements @{ 'APP_VERSION_STRING' = $version; '2001-01-01' = $wingetReleaseDateReplacement; 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa' = $wingetReleaseHashArm64.Hash; 'bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb' = $wingetReleaseHashX64.Hash; }
& ".\tools\Template-Copy.ps1"`
    -TemplateFilePath "tools\winget-pkgs\SiarheiKuchuk.BUtil.locale.en-US.yaml" `
    -DestinationFilePath "$wingetReleaseFolder\SiarheiKuchuk.BUtil.locale.en-US.yaml" `
    -Replacements @{ 'APP_VERSION_STRING' = $version; 'CURRENT_YEAR' = $currentYear }
& ".\tools\Template-Copy.ps1"`
	-TemplateFilePath "tools\winget-pkgs\SiarheiKuchuk.BUtil.yaml" `
    -DestinationFilePath "$wingetReleaseFolder\SiarheiKuchuk.BUtil.yaml" `
    -Replacements @{ 'APP_VERSION_STRING' = $version; }

Write-Output "Release files were put into win-get repo fork. Release it"
