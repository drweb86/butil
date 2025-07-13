
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree;

internal class GetStateOfSourceItemTask(TaskEvents events, SourceItemV2 sourceItem, IEnumerable<string> fileExcludePatterns, CommonServicesIoc commonServicesIoc) : SequentialBuTask(commonServicesIoc.Log, events, string.Format(BUtil.Core.Localization.Resources.SourceItem_State_Get, sourceItem.Target))
{
    private readonly IEnumerable<string> _fileExcludePatterns = fileExcludePatterns;
    private readonly CommonServicesIoc _commonServicesIoc = commonServicesIoc;
    private static readonly string[] _includeGroups = ["**/*"];

    public SourceItemState? SourceItemState { get; private set; }

    public SourceItemV2 SourceItem { get; } = sourceItem;

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        var files = new List<string>();
        if (SourceItem.IsFolder)
        {

            files = GetDirectoryFiles(SourceItem.Target);

        }
        else
        {
            files.Add(SourceItem.Target);
        }

        var getFileStateTasks = files
            .Select(file => new GetStateOfFileTask(Events, _commonServicesIoc, SourceItem, file))
            .ToList();
        Children = getFileStateTasks;
        Events.DuringExecutionTasksAdded(Id, Children);

        base.Execute();

        SourceItemState = new SourceItemState(SourceItem,
            getFileStateTasks
            .Where(x => x.IsSuccess)
            .Select(x => x.State!)
            .ToList());

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }

    private List<string> GetDirectoryFiles(string folder)
    {
        Matcher matcher = new();

        foreach (var excludeFilePattern in _fileExcludePatterns)
        {
            var actualPattern = excludeFilePattern;
            if (excludeFilePattern.StartsWith(folder) && (folder.Length + 1) < excludeFilePattern.Length)
                actualPattern = actualPattern[(folder.Length + 1)..];
            matcher.AddExclude(actualPattern);
        }

        matcher.AddIncludePatterns(_includeGroups);

        return matcher
            .Execute(new DirectoryInfoWrapperEx(new DirectoryInfo(folder)))
            .Files
            .Select(x => new FileInfo(Path.Combine(folder, x.Path)).FullName)
            .ToList();
    }
}
