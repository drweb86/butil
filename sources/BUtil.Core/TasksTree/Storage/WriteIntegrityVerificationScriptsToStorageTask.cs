using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage;

internal class WriteIntegrityVerificationScriptsToStorageTask(StorageSpecificServicesIoc services, TaskEvents events,
    Func<(bool versionIsNeeded, IncrementalBackupState updatedState)> getIncrementedState,
    BuTask writeSourceFilesToStorageTask,
    BuTask writeStateToStorageTask,
    Func<StorageFile> getStateStorageFile) : BuTaskV2(services.CommonServices.Log, events, BUtil.Core.Localization.Resources.File_IntegrityVerificationScript_Saving)
{
    protected override void ExecuteInternal()
    {
        var (versionIsNeeded, updatedState) = getIncrementedState();
        if (!versionIsNeeded)
        {
            LogDebug("Version is not needed.");
            IsSkipped = true;
            return;
        }

        if (!writeSourceFilesToStorageTask.IsSuccess)
            throw new Exception("Writing source files to storage has failed. Skipping.");

        var storage = services.Storage;

        if (!writeStateToStorageTask.IsSuccess)
            throw new Exception("Writing state was not successfull!");

        using var tempFolder = new TempFolder();
        var powershellFile = Path.Combine(tempFolder.Folder, BUtil.Core.Localization.Resources.File_IntegrityVerificationScript_Ps1);
        File.WriteAllText(powershellFile, GetPowershellScriptContent(updatedState));
        // TODO: check for null!
        var uploadedFile = storage.Upload(powershellFile, BUtil.Core.Localization.Resources.File_IntegrityVerificationScript_Ps1) ?? throw new Exception("Cannot save integrity verification scripts!");
        File.Delete(powershellFile);
    }

    private string GetPowershellScriptContent(IncrementalBackupState state)
    {
        var storageFiles = state.VersionStates
            .SelectMany(x => x.SourceItemChanges)
            .SelectMany(x =>
            {
                var items = new List<StorageFile>();
                items.AddRange(x.UpdatedFiles);
                items.AddRange(x.CreatedFiles);
                return items;
            })
            .ToList();
        storageFiles.Add((getStateStorageFile() ?? throw new Exception()));

        var lines = storageFiles
            .GroupBy(x => x.StorageRelativeFileName)
            .Select(x => x.First())
            .Select(x => $@"[void]$fileInfos.Add([FileInfo]::new(""{x.StorageRelativeFileName}"", {x.StorageFileNameSize}, ""{x.StorageIntegrityMethodInfo}""))")
            .ToList();
        var lineContent = string.Join("\r\n", lines);

        var scriptPrefix = @"<# 

This script verifies integrity of all backup files.

It is meant to be launched for 
a) folder storage - directly
b) server side scripts - on server (via SSH or remote desktop)

To be able to launch this script
1. Press Win button
2. Type Powershell
3. Right click on Powreshell and click 'Run as administrator'
4. Input command
Set-ExecutionPolicy -ExecutionPolicy Unrestricted
Press A (Yes for all)

Launching the script
1. Right click on script file in explorer
2. Click Execute in Powershell

#>

class FileInfo {
    [string]$RelativeFileName
    [long]$Size
    [string]$SHA512

    FileInfo([string]$RelativeFileName, [long]$Size, [string]$SHA512) {
        $this.RelativeFileName = $RelativeFileName
        $this.Size = $Size
        $this.SHA512 = $SHA512
    }
}

function Write-Error($message) {
    [Console]::ForegroundColor = 'red'
    [Console]::Error.WriteLine($message)
    [Console]::ResetColor()
}

$fileInfos = New-Object System.Collections.ArrayList
";

        var script = scriptPrefix +
            lineContent +
@"

Write-Host ""Verification by SHA512 and size:""

$isBackupValid = $true;

ForEach ($fileInfo in $fileInfos)
{
    $isSha512Valid = (Get-FileHash $fileInfo.RelativeFileName -Algorithm SHA512).Hash -eq $fileInfo.SHA512
    $isSizeValid = (Get-Item -Path $fileInfo.RelativeFileName).Length -eq $fileInfo.Size

    if ( $isSha512Valid -And $isSizeValid )
    {
        Write-Host ""[Good] $($fileInfo.RelativeFileName)"" -ForegroundColor green
    }
    else
    {
        Write-Error ""[Broken] $($fileInfo.RelativeFileName)""
        $isBackupValid = $false;
    }
}

if ( $isBackupValid )
{
    Write-Host ""Backup is good"" -ForegroundColor green
}
else
{
    Write-Error ""Backup is broken""
}

pause";
        return script;
    }
}
