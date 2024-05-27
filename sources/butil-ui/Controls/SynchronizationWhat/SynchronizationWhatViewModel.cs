using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls;

public class SynchronizationWhatViewModel : ObservableObject
{
    public SynchronizationWhatViewModel(
        string folder,
        string? subfolder,
        SynchronizationTaskModelMode synchronizationMode)
    {
        Folder = folder;
        Subfolder = subfolder;
        SynchronizationMode = synchronizationMode;
    }

    #region Labels
    public string LeftMenu_What => Resources.LeftMenu_What;
    public string Field_Folder => Resources.Field_Folder;
    public string Field_Folder_Browse => Resources.Field_Folder_Browse;

    public string RepositorySubfolder_Field => Resources.RepositorySubfolder_Field;
    public string RepositorySubfolder_Help => Resources.RepositorySubfolder_Help;
    public string SynchronizationMode_Field => Resources.SynchronizationMode_Field;
    public string SynchronizationMode_Field_TwoWay => Resources.SynchronizationMode_Field_TwoWay;
    public string SynchronizationMode_Field_Read => Resources.SynchronizationMode_Field_Read;

    #endregion

    #region Subfolder

    private string? _subfolder;

    public string? Subfolder
    {
        get
        {
            return _subfolder;
        }
        set
        {
            if (value == _subfolder)
                return;
            _subfolder = value;
            OnPropertyChanged(nameof(Subfolder));
        }
    }

    #endregion

    #region SynchronizationMode

    private SynchronizationTaskModelMode _synchronizationMode;

    public SynchronizationTaskModelMode SynchronizationMode
    {
        get
        {
            return _synchronizationMode;
        }
        set
        {
            if (value == _synchronizationMode)
                return;
            _synchronizationMode = value;
            OnPropertyChanged(nameof(SynchronizationMode));
        }
    }

    #endregion

    #region Folder

    private string _folder = string.Empty;

    public string Folder
    {
        get
        {
            return _folder;
        }
        set
        {
            if (value == _folder)
                return;
            _folder = value;
            OnPropertyChanged(nameof(Folder));
        }
    }

    #endregion
}
