$version = Get-Content "..\help\Version History (Changelog).md" -First 1
$version = $version.Substring(2)

Clear-Host
Write-Output "Version is $version"
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

Write-Output "Downloading 7-zip..."
$WebClient = New-Object System.Net.WebClient
$sevenZipVersion="7z2301"
$sevenZipFolder=[System.IO.Path]::GetTempPath() + "$($sevenZipVersion)"

if (-Not (Test-Path "$($sevenZipFolder)"))
{
	mkdir "$($sevenZipFolder)"
	$WebClient.DownloadFile("https://www.7-zip.org/a/$($sevenZipVersion)-x64.exe","$($sevenZipFolder)\x64.exe")
	$WebClient.DownloadFile("https://www.7-zip.org/a/$($sevenZipVersion)-arm64.exe","$($sevenZipFolder)\arm64.exe")

	& "c:\Program Files\7-Zip\7z.exe" x -y "$($sevenZipFolder)\x64.exe" -o"$($sevenZipFolder)\x64\7-zip"
	& "c:\Program Files\7-Zip\7z.exe" x -y "$($sevenZipFolder)\arm64.exe" -o"$($sevenZipFolder)\arm64\7-zip"
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

	Write-Output "Copy 7-zip"
	Copy-Item -Path "$($platform.Windows7ZipBinaries)\*" -Destination "../Output/$($platform.CoreRuntimeWindows)/bin" -Recurse -Force -Verbose
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

Write-Output "The following artefacts are produced:"
Get-ChildItem "..\Output"