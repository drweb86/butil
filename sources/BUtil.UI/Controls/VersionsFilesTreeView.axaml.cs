using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Platform.Storage;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Input;
using Loc = BUtil.Core.Localization.Resources;

namespace BUtil.UI.Controls;

public partial class VersionsFilesTreeView : UserControl
{
    public static readonly StyledProperty<string?> SearchTextProperty =
        AvaloniaProperty.Register<VersionsFilesTreeView, string?>(nameof(SearchText), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<IEnumerable?> NodesProperty =
        AvaloniaProperty.Register<VersionsFilesTreeView, IEnumerable?>(nameof(Nodes));

    public static readonly StyledProperty<FileTreeNode?> SelectedNodeProperty =
        AvaloniaProperty.Register<VersionsFilesTreeView, FileTreeNode?>(nameof(SelectedNode), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<ICommand?> RestoreCommandProperty =
        AvaloniaProperty.Register<VersionsFilesTreeView, ICommand?>(nameof(RestoreCommand));

    public static readonly DirectProperty<VersionsFilesTreeView, string> RestoreTextProperty =
        AvaloniaProperty.RegisterDirect<VersionsFilesTreeView, string>(
            nameof(RestoreText),
            o => o.RestoreText);

    public static readonly DirectProperty<VersionsFilesTreeView, bool> IsRestoreEnabledProperty =
        AvaloniaProperty.RegisterDirect<VersionsFilesTreeView, bool>(
            nameof(IsRestoreEnabled),
            o => o.IsRestoreEnabled);

    private string _restoreText = Loc.Task_Restore;
    private bool _isRestoreEnabled;

    public string Title => Loc.BackupVersion_Files_Title;
    public string Help => Loc.BackupVersion_Viewer_Help;
    public string SearchPlaceholder =>
        OperatingSystem.IsWindows() ? "\uD83D\uDD0D" : Loc.MainWindow_SearchWatermark;

    public string RestoreText
    {
        get => _restoreText;
        private set => SetAndRaise(RestoreTextProperty, ref _restoreText, value);
    }

    public bool IsRestoreEnabled
    {
        get => _isRestoreEnabled;
        private set => SetAndRaise(IsRestoreEnabledProperty, ref _isRestoreEnabled, value);
    }

    public string? SearchText
    {
        get => GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }

    public IEnumerable? Nodes
    {
        get => GetValue(NodesProperty);
        set => SetValue(NodesProperty, value);
    }

    public FileTreeNode? SelectedNode
    {
        get => GetValue(SelectedNodeProperty);
        set => SetValue(SelectedNodeProperty, value);
    }

    public ICommand? RestoreCommand
    {
        get => GetValue(RestoreCommandProperty);
        set => SetValue(RestoreCommandProperty, value);
    }

    public VersionsFilesTreeView()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedNodeProperty)
            UpdateRestoreState();
    }

    private void UpdateRestoreState()
    {
        IsRestoreEnabled = SelectedNode != null;
        RestoreText = SelectedNode == null
            ? Loc.Task_Restore
            : string.Format(Loc.Task_Restore_Selected, SelectedNode.Target);
    }

    public async Task BrowseRestoreFolderAsync()
    {
        if (!IsRestoreEnabled)
            return;

        var root = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");
        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = Loc.Field_Folder,
            SuggestedStartLocation = startLocation,
            AllowMultiple = false
        });

        if (folders.Count != 1)
            return;

        var path = folders[0].TryGetLocalPath() ?? folders[0].Path.ToString();
        if (RestoreCommand?.CanExecute(path) == true)
            RestoreCommand.Execute(path);
    }
}
