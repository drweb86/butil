using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls;

public class SynchronizationWhatViewModel : ObservableObject
{
    public SynchronizationWhatViewModel(
        string folder,
        SynchronizationTaskModelMode synchronizationMode)
    {
        Folder = folder;
        SynchronizationMode = synchronizationMode;
    }

    #region Labels
    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string Field_Folder => Resources.Field_Folder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;

    public static string SynchronizationMode_Field => Resources.SynchronizationMode_Field;
    public static string SynchronizationMode_Field_TwoWay => Resources.SynchronizationMode_Field_TwoWay;
    public static string SynchronizationMode_Field_Read => Resources.SynchronizationMode_Field_Read;

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
