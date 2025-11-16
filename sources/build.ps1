$version = Get-Content "..\help\Version History (Changelog).md" -First 1
$version = $version.Substring(2)
$changes = Get-Content -Path "..\help\Version History (Changelog).md" -Raw
$changes = $changes.Substring($changes.IndexOf("`n")).Trim("`n", "`r")
$changes = $changes.Substring(0, $changes.IndexOf("`n# ")).Trim("`n", "`r");
$changes = $changes.Replace("`r", "").Replace("`n", "\n").Replace("'", "").Replace("""", "")

Clear-Host
Write-Output "Version is $version"
Write-Output "Changes are $changes"
$ErrorActionPreference = "Stop"

Write-Output "Deleting everything untracked/non commited."
pause
Set-Location ..
git clean -ffdx
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Set-Location sources
	Exit 1
}
Set-Location sources

Write-Output "Sorting resources..."
Set-Location tools\ResxSorter
dotnet run --property WarningLevel=0
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}
Set-Location ../..

Write-Output "Run tests..."
& dotnet test
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Clear bin/obj folders..."
Get-ChildItem .\ -include bin,obj -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Clear Output folder..."
if (Test-Path "..\Output")
{
	Remove-Item "..\Output" -Confirm:$false -Recurse:$true
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}
}

class BuildInfo {
    [string]$CoreRuntimeWindows
	[string]$CoreRuntimeFolderPrefix

    BuildInfo(
		[string]$CoreRuntimeWindows,
		[string]$CoreRuntimeFolderPrefix) {
        $this.CoreRuntimeWindows = $CoreRuntimeWindows
		$this.CoreRuntimeFolderPrefix = $CoreRuntimeFolderPrefix
    }
}

$platforms = New-Object System.Collections.ArrayList
[void]$platforms.Add([BuildInfo]::new("win-x64", "x64"))
[void]$platforms.Add([BuildInfo]::new("win-arm64", "arm64"))

ForEach ($platform in $platforms)
{
	Write-Output "Platform: $($platform.CoreRuntimeWindows)"

	Write-Output "Publish..."
	& dotnet publish "/p:InformationalVersion=$version" `
		"/p:VersionPrefix=$version" `
		"/p:Version=$version" `
		"/p:AssemblyVersion=$version" `
		"--runtime=$($platform.CoreRuntimeWindows)" `
		/p:Configuration=Release `
		"/p:PublishDir=../../Output/publish/$($platform.CoreRuntimeFolderPrefix)" `
		/p:PublishReadyToRun=false `
		/p:RunAnalyzersDuringBuild=False `
		--self-contained true `
		--property WarningLevel=0
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}
}


Write-Output "Prepare to pack binaries"
Copy-Item "..\help\Readme.Binaries.md" "..\Output\publish\$($platform.CoreRuntimeFolderPrefix)\README.md"
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Setup..."
& "C:\Program Files (x86)\NSIS\Bin\makensis.exe" "setup.nsi" "/DPRODUCT_VERSION=$version"
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Pack binaries"
& "c:\Program Files\7-Zip\7z.exe" a -y "..\Output\BUtil_v$($version)_win-binaries.7z" "..\Output\publish\*" -mx9 -t7z -m0=lzma2 -ms=on -sccUTF-8 -ssw
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

ForEach ($platform in $platforms)
{
	Write-Output "Clear binaries"
	Remove-Item "..\Output\$($platform.CoreRuntimeWindows)" -Confirm:$false -Recurse:$true
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}
}

& "$PSScriptRoot\winget-pkgs.ps1"

Write-Output "Prepare ubuntu"
& ".\tools\Template-Copy.ps1"`
    -TemplateFilePath "tools\ubuntu-install-template.sh" `
    -DestinationFilePath "ubuntu-install.sh" `
    -Replacements @{ 'APP_VERSION_STRING' = $version }	

Write-Output "The following artefacts are produced. Release them"
Get-ChildItem "..\Output"

Write-Output "The following artefacts are produced. Release to win-get."
Get-ChildItem "..\Output" *.exe | Get-FileHash

Write-Output "A. Release files were put into win-get repo fork. Release it"
Write-Output "B. Commit changed ubuntu-install to main repository."