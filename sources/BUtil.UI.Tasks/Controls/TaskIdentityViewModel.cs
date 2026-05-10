using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BUtil.UI.Tasks.Controls;

public class TaskIdentityViewModel(bool isExpanded, ITaskModelOptionsV2 model, string name) : ObservableObject
{
    #region Labels
    public static string Name_Title => Resources.Name_Title;
    public static string Name_Field => Resources.Name_Field;
    public static string Icons_Help_Link => Resources.Icons_Help_Link;
    public string Help { get; } = TaskProviderRegistry.GetInformation(model.GetType());
    #endregion

    public bool IsExpanded { get; } = isExpanded;

    public bool CanOpenLink { get; } = PlatformSpecificExperience.Instance.SupportManager.CanOpenLink;

    #region Name

    private string _name = name;

    public string Name
    {
        get => _name;
        set
        {
            if (value == _name)
                return;
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    #endregion

    #region Commands

#pragma warning disable CA1822 // Mark members as static
    public void OpenCharsPageCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        PlatformSpecificExperience.Instance
            .SupportManager
            .OpenIcons();
    }

    #endregion
}
