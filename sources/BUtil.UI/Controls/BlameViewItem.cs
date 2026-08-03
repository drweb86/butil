using BUtil.Core.State;

namespace BUtil.UI.Controls;

public class BlameViewItem(VersionState versionState, ChangeState state)
{
    public string Title { get; } = versionState.BackupDateUtc.ToString();
    public string Icon { get; } = ChangeStateIcons.Get(state);
}
