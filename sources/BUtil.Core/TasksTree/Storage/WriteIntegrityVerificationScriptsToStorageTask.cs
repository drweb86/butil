using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage
{
    internal class WriteIntegrityVerificationScriptsToStorageTask : BuTask
    {
        private readonly StorageSpecificServicesIoc _services;
        private readonly CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask;
        private readonly WriteStateToStorageTask _writeStateToStorageTask;

        public WriteIntegrityVerificationScriptsToStorageTask(StorageSpecificServicesIoc services, TaskEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask,
            WriteSourceFilesToStorageTask writeSourceFilesToStorageTask,
            States.WriteStateToStorageTask writeStateToStorageTask)
            : base(services.Log, events, BUtil.Core.Localization.Resources.WriteIntegrityVerificationScriptsToStorage, TaskArea.Hdd)
        {
            _services = services;
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _writeSourceFilesToStorageTask = writeSourceFilesToStorageTask;
            _writeStateToStorageTask = writeStateToStorageTask;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            if (!_getIncrementedVersionTask.VersionIsNeeded)
            {
                LogDebug("Version is not needed.");
                IsSuccess = true;
                UpdateStatus(ProcessingStatus.FinishedSuccesfully);
                return;
            }

            if (!_writeSourceFilesToStorageTask.IsSuccess)
            {
                LogDebug("Writing source files to storage has failed. Skipping.");
                IsSuccess = false;
                UpdateStatus(ProcessingStatus.FinishedWithErrors);
                return;
            }

            var storage = _services.Storage;

            using (var tempFolder = new TempFolder())
            {
                var powershellFile = Path.Combine(tempFolder.Folder, BUtil.Core.Localization.Resources.IntegrityVerificationScriptPs1);
                File.WriteAllText(powershellFile, GetPowershellScriptContent());
                storage.Upload(powershellFile, BUtil.Core.Localization.Resources.IntegrityVerificationScriptPs1);
                File.Delete(powershellFile);
            }

            // TBD: 
            //var tempFile = Path.GetRandomFileName();
            //File.WriteAllText(tempFile, string.Empty);
            //storage.Upload(tempFile, BUtil.Core.Localization.Resources.IntegrityVerificationScriptSh);
            //File.Delete(tempFile);
            
            IsSuccess = true;
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }

        private string GetPowershellScriptContent()
        {
            var storageFiles = _getIncrementedVersionTask.IncrementalBackupState.VersionStates
                .SelectMany(x => x.SourceItemChanges)
                .SelectMany(x =>
                {
                    var items = new List<StorageFile>();
                    items.AddRange(x.UpdatedFiles);
                    items.AddRange(x.CreatedFiles);
                    return items;
                })
                .ToList();
            storageFiles.Add(_writeStateToStorageTask.StateStorageFile);

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
}
