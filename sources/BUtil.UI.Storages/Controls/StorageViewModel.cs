using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Storages;
using BUtil.UI.Controls.StorageFields;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public sealed class StorageProviderItem
{
    internal StorageProviderItem(StorageProviderRegistry.ProviderEntry entry)
    {
        Entry = entry;
    }

    public string DisplayName => Entry.DisplayName;

    internal StorageProviderRegistry.ProviderEntry Entry { get; }
}

public class StorageViewModel : ObservableObject
{
    public StorageViewModel(IStorageSettingsV2 storageSettings, string title, bool isExpanded = false, string? help = null, string icon = "💾")
    {
        IsExpanded = isExpanded;
        Title = title;
        Help = help;
        Icon = icon;

        Providers = new ObservableCollection<StorageProviderItem>(
            StorageProviderRegistry.GetProviders().Select(entry => new StorageProviderItem(entry)));

        var selectedEntry = StorageProviderRegistry.FindForSettings(storageSettings);
        var provider = Providers.FirstOrDefault(provider => provider.Entry == selectedEntry)
            ?? Providers.FirstOrDefault()
            ?? throw new InvalidOperationException("No storage providers registered.");

        Quota = storageSettings.SingleBackupQuotaGb;
        MountScript = storageSettings.MountPowershellScript;
        UnmountScript = storageSettings.UnmountPowershellScript;
        MountScriptLaunchCommand = new AsyncRelayCommand(MountTaskLaunchCommand);
        UnmountScriptLaunchCommand = new AsyncRelayCommand(UnmountTaskLaunchCommand);

        _selectedProvider = provider;
        RebuildFields();
        PopulateFields(provider.Entry.Provider.GetFieldValues(storageSettings));
    }

    private void RebuildFields()
    {
        if (_selectedProvider == null)
            return;

        DetachFieldChangeHandlers();
        DetachEnumUiRuleHandlers();
        Fields.Clear();
        foreach (var descriptor in _selectedProvider.Entry.Provider.Fields)
        {
            var field = StorageFieldViewModelFactory.Create(descriptor);
            if (field is FolderFieldViewModel folder)
            {
                folder.BrowseCommand = new AsyncRelayCommand(async () =>
                {
                    if (BrowseFolderAsync != null)
                        await BrowseFolderAsync(folder);
                });
            }
            else if (field is FileFieldViewModel file)
            {
                file.BrowseCommand = new AsyncRelayCommand(async () =>
                {
                    if (BrowseFileAsync != null)
                        await BrowseFileAsync(file);
                });
            }
            Fields.Add(field);
        }
        AttachEnumUiRuleHandlers();
        AttachFieldChangeHandlers();
        ApplyEnumUiRules();
        ClearValidationErrors();
    }

    private void PopulateFields(IReadOnlyDictionary<string, string?> values)
    {
        foreach (var field in Fields)
        {
            if (values.TryGetValue(field.Descriptor.Key, out var value))
                field.SetValue(value);
        }
        ApplyEnumUiRules();
    }

    public IStorageSettingsV2 GetStorageSettings()
    {
        var fieldValues = Fields.ToDictionary(f => f.Descriptor.Key, f => f.GetValue());
        return _selectedProvider.Entry.Provider.GetSettings(fieldValues, Quota, MountScript, UnmountScript);
    }

    public bool Validate()
    {
        var ok = true;
        string? firstError = null;

        foreach (var field in Fields)
        {
            if (field.Validate())
                continue;

            ok = false;
            firstError ??= field.Error;
        }

        QuotaError = Quota < 0
            ? Resources.DataStorage_Field_UploadQuota_Validation
            : null;
        if (QuotaError != null)
        {
            ok = false;
            firstError ??= QuotaError;
        }

        Error = firstError;
        return ok;
    }

    public void ApplyExternalError(string error)
    {
        Error = error;
    }

    private void ClearValidationErrors()
    {
        foreach (var field in Fields)
            field.ClearError();
        QuotaError = null;
        Error = null;
    }

    public string? ApplyDetectedConnectionTrustAndBuildInfo(IStorageSettingsV2 testedSettings)
    {
        var currentValues = Fields.ToDictionary(f => f.Descriptor.Key, f => f.GetValue());
        var message = _selectedProvider.Entry.Provider.TryApplyDetectedTrust(testedSettings, currentValues, out var updated);
        if (updated != null)
        {
            PopulateFields(updated);
            var fieldError = message?
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            foreach (var key in updated.Keys)
            {
                var field = Fields.FirstOrDefault(f => f.Descriptor.Key == key);
                field?.SetError(fieldError);
            }
        }
        if (!string.IsNullOrWhiteSpace(message))
            ApplyExternalError(message);
        return message;
    }

    public Task MountTaskLaunchCommand()
    {
        if (string.IsNullOrWhiteSpace(MountScript)) return Task.CompletedTask;
        if (!PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts) return Task.CompletedTask;
        var memoryLog = new MemoryLog();
        if (PlatformSpecificExperience.Instance.SupportManager.LaunchScript(memoryLog, MountScript, "***"))
        {
            MountScriptMessageKind = MessageBarKind.Success;
            MountScriptMessage = Resources.DataStorage_Field_DisconnectionScript_Ok;
        }
        else
        {
            MountScriptMessageKind = MessageBarKind.Error;
            MountScriptMessage = Resources.DataStorage_Field_DisconnectionScript_Bad + Environment.NewLine + Environment.NewLine + memoryLog;
        }
        return Task.CompletedTask;
    }

    public Task UnmountTaskLaunchCommand()
    {
        if (string.IsNullOrWhiteSpace(UnmountScript)) return Task.CompletedTask;
        if (!PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts) return Task.CompletedTask;
        var memoryLog = new MemoryLog();
        if (PlatformSpecificExperience.Instance.SupportManager.LaunchScript(memoryLog, UnmountScript, "***"))
        {
            UnmountScriptMessageKind = MessageBarKind.Success;
            UnmountScriptMessage = Resources.DataStorage_Field_DisconnectionScript_Ok;
        }
        else
        {
            UnmountScriptMessageKind = MessageBarKind.Error;
            UnmountScriptMessage = Resources.DataStorage_Field_DisconnectionScript_Bad + Environment.NewLine + Environment.NewLine + memoryLog;
        }
        return Task.CompletedTask;
    }

    public string Title { get; }
    public string? Help { get; }
    public string Icon { get; }
    public bool IsExpanded { get; }
    public bool CanLaunchScripts { get; } = PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts;
    public ObservableCollection<StorageProviderItem> Providers { get; }
    public ObservableCollection<StorageFieldViewModel> Fields { get; } = [];
    public IAsyncRelayCommand MountScriptLaunchCommand { get; }
    public IAsyncRelayCommand UnmountScriptLaunchCommand { get; }

    internal Func<FolderFieldViewModel, Task>? BrowseFolderAsync { get; set; }
    internal Func<FileFieldViewModel, Task>? BrowseFileAsync { get; set; }

    private readonly List<(EnumFieldViewModel Vm, PropertyChangedEventHandler Handler)> _enumUiHandlers = [];
    private readonly List<(StorageFieldViewModel Vm, PropertyChangedEventHandler Handler)> _fieldChangeHandlers = [];

    private void AttachFieldChangeHandlers()
    {
        foreach (var field in Fields)
        {
            PropertyChangedEventHandler handler = (_, e) =>
            {
                if (e.PropertyName is nameof(TextFieldViewModel.Value)
                    or nameof(EnumFieldViewModel.SelectedDisplay)
                    or nameof(IntegerFieldViewModel.Value))
                {
                    Error = null;
                }
            };
            field.PropertyChanged += handler;
            _fieldChangeHandlers.Add((field, handler));
        }
    }

    private void DetachFieldChangeHandlers()
    {
        foreach (var (vm, handler) in _fieldChangeHandlers)
            vm.PropertyChanged -= handler;
        _fieldChangeHandlers.Clear();
    }

    private void AttachEnumUiRuleHandlers()
    {
        foreach (var enumVm in Fields.OfType<EnumFieldViewModel>())
        {
            PropertyChangedEventHandler handler = (_, e) =>
            {
                if (e.PropertyName == nameof(EnumFieldViewModel.SelectedDisplay))
                    ApplyEnumUiRules();
            };
            enumVm.PropertyChanged += handler;
            _enumUiHandlers.Add((enumVm, handler));
        }
    }

    private void DetachEnumUiRuleHandlers()
    {
        foreach (var (vm, handler) in _enumUiHandlers)
            vm.PropertyChanged -= handler;
        _enumUiHandlers.Clear();
    }

    private void SilencingEnumHandlers(Action action)
    {
        foreach (var (vm, handler) in _enumUiHandlers)
            vm.PropertyChanged -= handler;
        try
        {
            action();
        }
        finally
        {
            foreach (var (vm, handler) in _enumUiHandlers)
                vm.PropertyChanged += handler;
        }
    }

    private void ApplyEnumUiRules()
    {
        SilencingEnumHandlers(() =>
        {
            const int maxPasses = 24;
            for (var pass = 0; pass < maxPasses; pass++)
            {
                var byKey = Fields.ToDictionary(f => f.Descriptor.Key);

                foreach (var vm in Fields)
                    vm.ResetUiCustomization();

                var wroteValue = false;
                foreach (var field in Fields)
                {
                    if (field is not EnumFieldViewModel enumVm) continue;
                    var rules = enumVm.Descriptor.EnumSelectionUiRules;
                    if (rules == null || rules.Count == 0) continue;

                    var current = enumVm.GetValue();
                    if (current == null) continue;

                    var rule = rules.FirstOrDefault(r => r.WhenValue == current);
                    if (rule == null) continue;

                    foreach (var patch in rule.Patches)
                    {
                        if (!byKey.TryGetValue(patch.TargetFieldKey, out var target)) continue;

                        target.ApplyUiPatch(patch.LabelOverride, patch.Hidden, patch.PlaceholderOverride);

                        if (patch.ValueWhenSelected == null) continue;

                        var before = target.GetValue();
                        target.SetValue(patch.ValueWhenSelected);
                        if (before != target.GetValue())
                            wroteValue = true;
                    }
                }

                if (!wroteValue) break;
            }
        });
    }

    #region SelectedProvider

    private StorageProviderItem _selectedProvider;

    public StorageProviderItem SelectedProvider
    {
        get => _selectedProvider;
        set
        {
            if (value == _selectedProvider) return;
            _selectedProvider = value;
            OnPropertyChanged(nameof(SelectedProvider));
            RebuildFields();
        }
    }

    #endregion

    #region Quota

    private long _quota;
    private string? _quotaError;

    public long Quota
    {
        get => _quota;
        set
        {
            if (value == _quota) return;
            _quota = value;
            OnPropertyChanged(nameof(Quota));
            QuotaError = null;
            Error = null;
        }
    }

    public string? QuotaError
    {
        get => _quotaError;
        private set
        {
            if (value == _quotaError) return;
            _quotaError = value;
            OnPropertyChanged(nameof(QuotaError));
        }
    }

    #endregion

    #region Error

    private string? _error;

    public string? Error
    {
        get => _error;
        private set
        {
            if (value == _error) return;
            _error = value;
            OnPropertyChanged(nameof(Error));
        }
    }

    #endregion

    #region MountScript

    private string? _mountScript;
    private string? _mountScriptMessage;
    private MessageBarKind _mountScriptMessageKind;

    public string? MountScript
    {
        get => _mountScript;
        set
        {
            if (value == _mountScript) return;
            _mountScript = value;
            OnPropertyChanged(nameof(MountScript));
            MountScriptMessage = null;
        }
    }

    public string? MountScriptMessage
    {
        get => _mountScriptMessage;
        private set
        {
            if (value == _mountScriptMessage) return;
            _mountScriptMessage = value;
            OnPropertyChanged(nameof(MountScriptMessage));
        }
    }

    public MessageBarKind MountScriptMessageKind
    {
        get => _mountScriptMessageKind;
        private set
        {
            if (value == _mountScriptMessageKind) return;
            _mountScriptMessageKind = value;
            OnPropertyChanged(nameof(MountScriptMessageKind));
        }
    }

    #endregion

    #region UnmountScript

    private string? _unmountScript;
    private string? _unmountScriptMessage;
    private MessageBarKind _unmountScriptMessageKind;

    public string? UnmountScript
    {
        get => _unmountScript;
        set
        {
            if (value == _unmountScript) return;
            _unmountScript = value;
            OnPropertyChanged(nameof(UnmountScript));
            UnmountScriptMessage = null;
        }
    }

    public string? UnmountScriptMessage
    {
        get => _unmountScriptMessage;
        private set
        {
            if (value == _unmountScriptMessage) return;
            _unmountScriptMessage = value;
            OnPropertyChanged(nameof(UnmountScriptMessage));
        }
    }

    public MessageBarKind UnmountScriptMessageKind
    {
        get => _unmountScriptMessageKind;
        private set
        {
            if (value == _unmountScriptMessageKind) return;
            _unmountScriptMessageKind = value;
            OnPropertyChanged(nameof(UnmountScriptMessageKind));
        }
    }

    #endregion

    #region Labels

    public static string Field_TransportProtocol => Resources.Field_TransportProtocol;
    public static string DataStorage_Field_UploadQuota => Resources.DataStorage_Field_UploadQuota;
    public static string DataStorage_Field_UploadQuota_Help => Resources.DataStorage_Field_UploadQuota_Help;
    public static string DataStorage_Script_Help => string.Format(Resources.DataStorage_Script_Help, PlatformSpecificExperience.Instance.SupportManager.ScriptEngineName);
    public static string DataStorage_Scripts_Header => Resources.DataStorage_Scripts_Header;
    public static string DataStorage_Field_ConnectScript => Resources.DataStorage_Field_ConnectScript;
    public static string DataStorage_Field_DisconnectionScript => Resources.DataStorage_Field_DisconnectionScript;
    public static string Task_Launch => Resources.Task_Launch;

    #endregion
}
