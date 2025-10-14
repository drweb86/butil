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

$sevenZipVersion="7z2409"
$sevenZipFolder=[System.IO.Path]::GetTempPath() + "$($sevenZipVersion)"

Write-Output "Clear 7-zip folder..."
if (Test-Path $sevenZipFolder)
{
	Remove-Item $sevenZipFolder -Confirm:$false -Recurse:$true
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}
}

Write-Output "Downloading 7-zip..."
$WebClient = New-Object System.Net.WebClient

mkdir "$($sevenZipFolder)"
$WebClient.DownloadFile("https://www.7-zip.org/a/$($sevenZipVersion)-x64.exe","$($sevenZipFolder)\x64.exe")
$WebClient.DownloadFile("https://www.7-zip.org/a/$($sevenZipVersion)-arm64.exe","$($sevenZipFolder)\arm64.exe")

& "c:\Program Files\7-Zip\7z.exe" x -y "$($sevenZipFolder)\x64.exe" -o"$($sevenZipFolder)\x64\7-zip"
& "c:\Program Files\7-Zip\7z.exe" x -y "$($sevenZipFolder)\arm64.exe" -o"$($sevenZipFolder)\arm64\7-zip"

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
	[string]$InnoArchitectureWindows
	[string]$Windows7ZipBinaries

    BuildInfo(
		[string]$CoreRuntimeWindows,
		[string]$InnoArchitectureWindows,
		[string]$Windows7ZipBinaries) {
        $this.CoreRuntimeWindows = $CoreRuntimeWindows
		$this.InnoArchitectureWindows = $InnoArchitectureWindows
		$this.Windows7ZipBinaries = $Windows7ZipBinaries
    }
}

$platforms = New-Object System.Collections.ArrayList
[void]$platforms.Add([BuildInfo]::new("win-x64", "x64", "$($sevenZipFolder)\x64"))
[void]$platforms.Add([BuildInfo]::new("win-arm64", "arm64", "$($sevenZipFolder)\arm64"))

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
		"/p:PublishDir=../../Output/$($platform.CoreRuntimeWindows)/bin" `
		/p:PublishReadyToRun=false `
		/p:RunAnalyzersDuringBuild=False `
		--self-contained true `
		--property WarningLevel=0
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}

	Write-Output "Create Setup Script"
	$innoSetupScriptFile="../Output/$($platform.CoreRuntimeWindows)/_InnoSetup $($platform.CoreRuntimeWindows).iss"

	(Get-Content "setup.iss").Replace("###ARCHITECTURE###", $platform.InnoArchitectureWindows).Replace("###CORERUNTIME###", $platform.CoreRuntimeWindows).Replace("###VERSION###", $version) | Set-Content $innoSetupScriptFile
	Write-Output "Setup..."
	& "c:\Program Files (x86)\Inno Setup 6\ISCC.exe" $innoSetupScriptFile
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}

	Write-Output "Prepare to pack binaries"
	Copy-Item "..\help\Readme.Binaries.md" "..\Output\$($platform.CoreRuntimeWindows)\bin\README.md"
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}

	Write-Output "Pack binaries"
	& "c:\Program Files\7-Zip\7z.exe" a -y "..\Output\BUtil_v$($version)_$($platform.CoreRuntimeWindows)-binaries.7z" "..\Output\$($platform.CoreRuntimeWindows)\bin\*" -mx9 -t7z -m0=lzma2 -ms=on -sccUTF-8 -ssw
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}

	Write-Output "Clear binaries"
	Remove-Item "..\Output\$($platform.CoreRuntimeWindows)" -Confirm:$false -Recurse:$true
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}
}

& "winget-pkgs.ps1"

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