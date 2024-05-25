using BUtil.Core.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Synchronization;

class SynchronizationDecisionService
{
    public IEnumerable<SynchronizationConsolidatedFileInfo> Decide(
        SynchronizationState localState,
        SynchronizationState actualFiles,
        SynchronizationState remoteState)
    {
        var items = UnionStates(actualFiles, localState, remoteState);

        BuildRelations(items);

        ResolveActions(items);

        return items;
    }

    private static void ResolveActions(IEnumerable<SynchronizationConsolidatedFileInfo> items)
    {
        foreach (var item in items)
        {
            ResolveAction(item);
        }
    }

    private static void ResolveAction(SynchronizationConsolidatedFileInfo item)
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
    }

    private void BuildRelations(IEnumerable<SynchronizationConsolidatedFileInfo> items)
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
        AddState(items, remoteState, (x, remoteState) => x.RemoteState = remoteState);

        return items.Values.ToList();
    }

    private static void AddState(
        Dictionary<string, SynchronizationConsolidatedFileInfo> items,
        SynchronizationState state,
        Action<SynchronizationConsolidatedFileInfo, SynchronizationStateFile> filler)
    {
        foreach (var item in state.FileSystemEntries)
        {
            if (!items.ContainsKey(item.RelativeFileName))
            {
                items.Add(item.RelativeFileName, new SynchronizationConsolidatedFileInfo(item.RelativeFileName));
            }

            filler(items[item.RelativeFileName], item);
        }
    }

    private SynchronizationRelation ResolveRelation(
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
