using Avalonia;
using Avalonia.Controls;
using BUtil.Core;
using BUtil.Core.Misc;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Loc = BUtil.Core.Localization.Resources;

namespace BUtil.Tasks.IncrementalBackup.UI.Controls;

public partial class ExcludePatternsField : UserControl
{
    public static readonly StyledProperty<string?> PatternsProperty =
        AvaloniaProperty.Register<ExcludePatternsField, string?>(nameof(Patterns), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public string HeaderText => Loc.StorageItem_ExcludePattern_ExpanderHeader;

    public string Label => Loc.StorageItem_Field_ExcludePattern;

    public string ActionText => Loc.StorageItem_ExcludePattern_GlobbingNetPatterns;

    public string Help => PlatformSpecificExperience.Instance.GetFolderService().GetStorageItemExcludePatternHelp();

    public ICommand GlobbingHelpCommand { get; }

    public string? Patterns
    {
        get => GetValue(PatternsProperty);
        set => SetValue(PatternsProperty, value);
    }

    public ExcludePatternsField()
    {
        GlobbingHelpCommand = new RelayCommand(() =>
        {
            ProcessHelper.ShellExecute("https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats");
        });

        InitializeComponent();
    }
}
