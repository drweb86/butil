using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Synchronization;

class SynchronizationDecisionService(string? repositorySubfolder)
{
    private readonly string? _repositorySubfolderNormalized = FileHelper.NormalizeRelativePath(repositorySubfolder);

    public IEnumerable<SynchronizationConsolidatedFileInfo> Decide(
        SynchronizationTaskModelMode synchronizationMode,
        SynchronizationState localState,
        SynchronizationState actualFiles,
        SynchronizationState remoteState)
    {
        var items = UnionStates(actualFiles, localState, remoteState);

        BuildRelations(items);

        ResolveActions(items, synchronizationMode);

        return items;
    }

    private static void ResolveActions(
        IEnumerable<SynchronizationConsolidatedFileInfo> items,
        SynchronizationTaskModelMode synchronizationMode)
    {
        foreach (var item in items)
        {
            ResolveAction(item, synchronizationMode);
        }
    }

    private static void ResolveAction(
        SynchronizationConsolidatedFileInfo item,
        SynchronizationTaskModelMode synchronizationMode)
    {
        item.ActualFileAction = SynchronizationDecision.DoNothing;
        item.RemoteAction = SynchronizationDecision.DoNothing;

        // TODO: hanble relative paths
        if (item.ActualFile != null &&
            item.RemoteState != null &&
            item.ActualFile.Equal(item.RemoteState))
        {
            item.ForceUpdateState = !item.RemoteState.Equal(item.LocalState);
            return;
        }

        switch (synchronizationMode)
        {
            case SynchronizationTaskModelMode.TwoWay:
                ResolveActionTwoWayMode(item);
                break;
            case SynchronizationTaskModelMode.Read:
                ResolveActionReadonlyMode(item);
                break;
        }
    }

    private static void ResolveActionTwoWayMode(SynchronizationConsolidatedFileInfo item)
    {
        item.ActualFileAction = SynchronizationDecision.DoNothing;
        item.RemoteAction = SynchronizationDecision.DoNothing;

        // test case #001
        if (item.ExistsLocally &&
            item.RemoteState == null &&
            item.LocalState == null &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Created &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged)
        {
            item.RemoteAction = SynchronizationDecision.Update;
            return;
        }

        // test case #002
        if (item.ExistsLocally &&
            item.RemoteState != null &&
            item.LocalState != null &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged)
        {
            return;
        }

        // test case #003
        if (item.ExistsLocally &&
            item.RemoteState != null &&
            item.LocalState != null &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed)
        {
            item.ActualFileAction = SynchronizationDecision.Update;
            return;
        }

        // #009
        if (!item.ExistsLocally &&
            item.RemoteState != null &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Deleted &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged)
        {
            item.RemoteAction = SynchronizationDecision.Delete;
            return;
        }

        if (item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed)
        {
            item.ActualFileAction = SynchronizationDecision.Delete;
            item.RemoteAction = SynchronizationDecision.Update;
        }

        // test case #004
        if (item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Deleted)
        {
            item.ActualFileAction = SynchronizationDecision.Delete;
            return;
        }

        // test case #005
        if (item.RemoteState != null &&
            item.ActualFile != null &&

            item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Created &&
            (item.RemoteStateToLocalStateRelation == SynchronizationRelation.Created ||
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed))
        {
            if (item.RemoteState.ModifiedAtUtc > item.ActualFile.ModifiedAtUtc)
            {
                item.ActualFileAction = SynchronizationDecision.Update;
            }
            else
            {
                item.RemoteAction = SynchronizationDecision.Update;
            }
            return;
        }

        if (item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Created &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Deleted)
        {
            item.RemoteAction = SynchronizationDecision.Update;
        }

        // #006
        if (item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Changed &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged)
        {
            item.RemoteAction = SynchronizationDecision.Update;
            return;
        }

        // #007
        if (item.RemoteState != null &&
            item.ActualFile != null &&

            item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Changed &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed)
        {
            if (item.RemoteState.ModifiedAtUtc > item.ActualFile.ModifiedAtUtc)
            {
                item.ActualFileAction = SynchronizationDecision.Update;
            }
            else
            {
                item.RemoteAction = SynchronizationDecision.Update;
            }
            return;
        }

        // #008
        if (item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Changed &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Deleted)
        {
            item.RemoteAction = SynchronizationDecision.Update;
            return;
        }

        // #010
        if (!item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Deleted &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed)
        {
            item.ActualFileAction = SynchronizationDecision.Update;
            return;
        }

        // #011
        if (!item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.Deleted &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Deleted)
        {
            item.ForceUpdateState = true;
            return;
        }

        // #012
        if (!item.ExistsLocally &&
            item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged &&
            item.RemoteStateToLocalStateRelation == SynchronizationRelation.Created)
        {
            item.ActualFileAction = SynchronizationDecision.Update;
            return;
        }
    }

    private static void ResolveActionReadonlyMode(SynchronizationConsolidatedFileInfo item)
    {
        if (item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged)
        {
            switch (item.RemoteStateToLocalStateRelation)
            {
                case SynchronizationRelation.NotChanged: 
                    item.ActualFileAction = SynchronizationDecision.DoNothing;
                    break;
                case SynchronizationRelation.Created: 
                case SynchronizationRelation.Changed: 
                    item.ActualFileAction = SynchronizationDecision.Update;
                    break;
                case SynchronizationRelation.Deleted: 
                    item.ActualFileAction = SynchronizationDecision.Delete;
                    break;
            }
        }

        if (item.ActualFileToLocalStateRelation == SynchronizationRelation.Created)
        {
            switch (item.RemoteStateToLocalStateRelation)
            {
                case SynchronizationRelation.NotChanged:
                    item.ActualFileAction = SynchronizationDecision.DoNothing;
                    break;
                case SynchronizationRelation.Created:
                    item.ActualFileAction = SynchronizationDecision.Update;
                    break;
            }
        }

        if (item.ActualFileToLocalStateRelation == SynchronizationRelation.Changed)
        {
            switch (item.RemoteStateToLocalStateRelation)
            {
                case SynchronizationRelation.NotChanged:
                case SynchronizationRelation.Changed:
                    item.ActualFileAction = SynchronizationDecision.Update;
                    break;
                case SynchronizationRelation.Deleted:
                    item.ActualFileAction = SynchronizationDecision.Delete;
                    break;
            }
        }

        if (item.ActualFileToLocalStateRelation == SynchronizationRelation.Deleted)
        {
            switch (item.RemoteStateToLocalStateRelation)
            {
                case SynchronizationRelation.NotChanged:
                case SynchronizationRelation.Changed:
                    item.ActualFileAction = SynchronizationDecision.Update;
                    break;
                case SynchronizationRelation.Deleted:
                    item.ActualFileAction = SynchronizationDecision.DoNothing;
                    break;
            }
        }
    }


    private static void BuildRelations(IEnumerable<SynchronizationConsolidatedFileInfo> items)
    {
        foreach (var item in items)
        {
            item.ActualFileToLocalStateRelation = ResolveRelation(
                item.ActualFile,
                item.LocalState);
            item.RemoteStateToLocalStateRelation = ResolveRelation(
                item.RemoteState,
                item.LocalState);
        }
    }



    private IEnumerable<SynchronizationConsolidatedFileInfo> UnionStates(
        SynchronizationState actualFiles,
        SynchronizationState localState,
        SynchronizationState remoteState)
    {
        actualFiles.EnsureNotNull("actual file state is expected to be filled.");
        localState.EnsureNotNull("invariant: local file state must be filled before.");
        remoteState.EnsureNotNull("invariant: remote files state must be filled before.");

        var items = new Dictionary<string, SynchronizationConsolidatedFileInfo>();

        AddState(items, actualFiles, (x, actualFile) => x.ActualFile = actualFile);
        AddState(items, localState, (x, localState) => x.LocalState = localState);
        AddState(items, remoteState, (x, remoteState) => x.RemoteState = remoteState, true);

        return [.. items.Values];
    }

    private void AddState(
        Dictionary<string, SynchronizationConsolidatedFileInfo> items,
        SynchronizationState state,
        Action<SynchronizationConsolidatedFileInfo, SynchronizationStateFile> filler,
        bool containsRepositorySubfolder = false)
    {
        foreach (var item in state.FileSystemEntries)
        {
            var actualRelativeFileName = GetActualRelativeFileName(item.RelativeFileName, containsRepositorySubfolder, out var isInvalid);
            if (isInvalid)
            {
                continue;
            }

#pragma warning disable CA1854 // Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method
            if (!items.ContainsKey(actualRelativeFileName))
            {
                items.Add(actualRelativeFileName, new SynchronizationConsolidatedFileInfo(actualRelativeFileName));
            }
#pragma warning restore CA1854 // Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method

            filler(items[actualRelativeFileName], item);
        }
    }

    private string GetActualRelativeFileName(string relativeFileName, bool containsRepositorySubfolder, out bool isInvalid)
    {
        if (containsRepositorySubfolder && _repositorySubfolderNormalized != null)
        {
            return FileHelper.TrimRelativePath(relativeFileName, _repositorySubfolderNormalized, out isInvalid);
        }
        isInvalid = false;
        return relativeFileName;
    }

    private static SynchronizationRelation ResolveRelation(
        SynchronizationStateFile? primary,
        SynchronizationStateFile? secondary)
    {
        if (primary == null)
        {
            return secondary == null 
                ? SynchronizationRelation.NotChanged
                : SynchronizationRelation.Deleted;
        }

        if (secondary == null)
        {
            return SynchronizationRelation.Created;
        }

        return primary.Equal(secondary) 
            ? SynchronizationRelation.NotChanged
            : SynchronizationRelation.Changed;
    }
}
