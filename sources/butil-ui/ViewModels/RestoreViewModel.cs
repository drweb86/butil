using Avalonia.Threading;
using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.IncrementalModel;
using butil_ui.Controls;
using System;
using System.Threading.Tasks;

namespace butil_ui.ViewModels;

public class RestoreViewModel : ViewModelBase
{
    public RestoreViewModel(IStorageSettingsV2? storageSettingsV2, string? password)
    {
        WindowTitle = Resources.Task_Restore;

        WhereTaskViewModel = new WhereTaskViewModel(storageSettingsV2 ?? new FolderStorageSettingsV2(), Resources.Task_Restore, "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png");
        EncryptionTaskViewModel = new EncryptionTaskViewModel(password ?? string.Empty, false);
        ProgressTaskViewModel = new ProgressTaskViewModel();
        VersionsListViewModel = new VersionsListViewModel();
    }

    #region IsSetupVisible

    private bool _isSetupVisible = true;

    public bool IsSetupVisible
    {
        get
        {
            return _isSetupVisible;
        }
        set
        {
            if (value == _isSetupVisible)
                return;
            _isSetupVisible = value;
            OnPropertyChanged(nameof(IsSetupVisible));
        }
    }

    #endregion

    public ProgressTaskViewModel ProgressTaskViewModel { get; }
    public WhereTaskViewModel WhereTaskViewModel { get; }
    public EncryptionTaskViewModel EncryptionTaskViewModel { get; }

    public VersionsListViewModel VersionsListViewModel { get; }

    #region Commands

    public void CloseCommand()
    {
        Environment.Exit(0);
    }

    public async Task ContinueCommand()
    {
        var storageOptions = WhereTaskViewModel.GetStorageSettings();
        if (storageOptions == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(EncryptionTaskViewModel.Password))
        {
            await Messages.ShowErrorBox(Resources.Password_Field_Validation_NotSpecified);
            return;
        }

        if (string.IsNullOrWhiteSpace(EncryptionTaskViewModel.Password))
        {
            await Messages.ShowErrorBox(Resources.Password_Field_Validation_NotSpecified);
            return;
        }

        this.ProgressTaskViewModel.IsVisible = true;
        this.ProgressTaskViewModel.Activate(async progress =>
        {
            progress(10);
            var error = StorageFactory.Test(new StubLog(), storageOptions);
            if (!string.IsNullOrWhiteSpace(error))
            {
                await Dispatcher.UIThread.InvokeAsync(async () => await Messages.ShowErrorBox(error));
                this.ProgressTaskViewModel.IsVisible = false;
                return;
            }
            progress(20);
            using (var storage = StorageFactory.Create(new StubLog(), storageOptions))
            {
                if (!storage.Exists(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile))
                {
                    error = string.Format(Resources.RestoreFrom_Field_Validation_NoStateFiles, IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);
                    await Dispatcher.UIThread.InvokeAsync(async () => await Messages.ShowErrorBox(error));
                    this.ProgressTaskViewModel.IsVisible = false;
                    return;
                }
            }
            progress(30);
            var commonServicesIoc = new CommonServicesIoc();
            using var services = new StorageSpecificServicesIoc(new StubLog(), storageOptions, commonServicesIoc.HashService);
            if (!services.IncrementalBackupStateService.TryRead(EncryptionTaskViewModel.Password, out var state))
            {
                await Dispatcher.UIThread.InvokeAsync(async () => await Messages.ShowErrorBox(Resources.RestoreFrom_Field_Validation_StateInvalid));
                this.ProgressTaskViewModel.IsVisible = false;
                return;
            }
            this.ProgressTaskViewModel.IsVisible = false;
            IsSetupVisible = false;
            progress(100);
            VersionsListViewModel.Initialize(state, storageOptions);
        });

    }

    #endregion

    #region Labels
    public string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;
    public string Button_Close => Resources.Button_Close;
    public string AfterTaskSelection_Help => Resources.AfterTaskSelection_Help;
    public string Button_Continue => Resources.Button_Continue;
    public string Task_Restore => Resources.Task_Restore;

    #endregion
}
