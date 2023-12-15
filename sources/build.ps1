$version = Get-Content "..\help\Version History (Changelog).md" -First 1
$version = $version.Substring(2)

cls
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

Write-Output "Clear snaps..."
Remove-Item "..\*.snap" -Confirm:$false
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Clear snapcraft..."
if (Test-Path "..\snap\snapcraft.yaml")
{
	Remove-Item "..\snap\snapcraft.yaml" -Confirm:$false
	if ($LastExitCode -ne 0)
	{
		Write-Error "Fail." 
		Exit 1
	}
}

Write-Output "Publish..."
& dotnet publish "/p:InformationalVersion=$version" `
	"/p:VersionPrefix=$version" `
	"/p:Version=$version" `
	"/p:AssemblyVersion=$version" `
	/p:Configuration=Release `
	/p:PublishDir=../../Output/butil/bin `
	/p:PublishReadyToRun=false `
	/p:RunAnalyzersDuringBuild=False `
	--self-contained true `
	--property WarningLevel=0
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Setup..."
& "c:\Program Files (x86)\Inno Setup 6\ISCC.exe" setup.iss
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Prepare to pack binaries"
Copy-Item "..\help\Readme.Binaries.md" "..\Output\BUtil\README.md"
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Pack binaries"
& "c:\Program Files\7-Zip\7z.exe" a -y "..\Output\BUtil-$version-binaries-Windows.7z" "..\Output\BUtil\*" -mx9 -t7z -m0=lzma2 -ms=on -sccUTF-8 -ssw
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Create snapcraft with version"
(Get-Content "..\snap\local\snapcraft.yaml").Replace("###VERSION###", "$version") | Set-Content "../snap\snapcraft.yaml"
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}

Write-Output "Create snap"
Set-Location ..
& wsl.exe sudo snapcraft
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Set-Location sources
	Exit 1
}
Set-Location sources

Write-Output "Pack snap"
& "c:\Program Files\7-Zip\7z.exe" a -y ..\Output\BUtil-$version-Ubuntu_amd64.zip "..\*.snap" "..\help\ubuntu-snap\readme.md" -mx9 -t7z -m0=lzma2 -ms=on -sccUTF-8 -ssw
if ($LastExitCode -ne 0)
{
	Write-Error "Fail." 
	Exit 1
}
