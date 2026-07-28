using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Interop.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BUtil.UI.Tasks.Controls;

public class TaskIdentityViewModel : ObservableObject
{
    public TaskIdentityViewModel(bool isExpanded, ITaskModelOptionsV2 model, string name)
    {
        IsExpanded = isExpanded;
        Help = TaskProviderRegistry.GetInformation(model.GetType());
        CanOpenLink = PlatformSpecificExperience.Instance.SupportManager.CanOpenLink && PlatformSpecificExperience.Instance.SupportManager.SupportsSmileIcons;
        _name = name;
        OpenCharsPageCommand = new RelayCommand(OpenCharsPage, () => CanOpenLink);
    }

    #region Labels
    public static string Name_Title => Resources.Name_Title;
    public static string Name_Field => Resources.Name_Field;
    public static string Icons_Help_Link => Resources.Icons_Help_Link;
    public string Help { get; }
    #endregion

    public bool IsExpanded { get; }

    public bool CanOpenLink { get; }

    #region Name

    private string _name;
    private string? _nameError;

    public string Name
    {
        get => _name;
        set
        {
            if (value == _name)
                return;
            _name = value;
            OnPropertyChanged(nameof(Name));
            NameError = null;
        }
    }

    public string? NameError
    {
        get => _nameError;
        private set
        {
            if (value == _nameError)
                return;
            _nameError = value;
            OnPropertyChanged(nameof(NameError));
        }
    }

    public bool Validate(string? originalTaskName)
    {
        var store = new TaskStore(new LocalFileSystem());
        if (store.TryValidate(Name.TrimEnd(), originalTaskName, out var error))
        {
            NameError = null;
            return true;
        }
        NameError = error;
        return false;
    }

    #endregion

    #region Commands

    public IRelayCommand OpenCharsPageCommand { get; }

    private void OpenCharsPage()
    {
        PlatformSpecificExperience.Instance
            .SupportManager
            .OpenIcons();
    }

    #endregion
}
