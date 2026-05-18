using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Storages;
using BUtil.UI.Controls.StorageFields;
using CommunityToolkit.Mvvm.ComponentModel;
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
    public StorageViewModel(IStorageSettingsV2 storageSettings, string title, string iconUrl)
    {
        Title = title;
        IconSource = LoadFromResource(new Uri("avares://BUtil.UI" + iconUrl));

        Providers = new ObservableCollection<StorageProviderItem>(
            StorageProviderRegistry.GetProviders().Select(entry => new StorageProviderItem(entry)));

        var selectedEntry = StorageProviderRegistry.FindForSettings(storageSettings);
        var provider = Providers.FirstOrDefault(provider => provider.Entry == selectedEntry)
            ?? Providers.FirstOrDefault()
            ?? throw new InvalidOperationException("No storage providers registered.");

        Quota = storageSettings.SingleBackupQuotaGb;
        MountScript = storageSettings.MountPowershellScript;
        UnmountScript = storageSettings.UnmountPowershellScript;

        _selectedProvider = provider;
        RebuildFields();
        PopulateFields(provider.Entry.Provider.GetFieldValues(storageSettings));
    }

    private void RebuildFields()
    {
        DetachEnumUiRuleHandlers();
        Fields.Clear();
        foreach (var descriptor in _selectedProvider.Entry.Provider.Fields)
            Fields.Add(StorageFieldViewModelFactory.Create(descriptor));
        AttachEnumUiRuleHandlers();
        ApplyEnumUiRules();
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

    public string? ApplyDetectedConnectionTrustAndBuildInfo(IStorageSettingsV2 testedSettings)
    {
        var currentValues = Fields.ToDictionary(f => f.Descriptor.Key, f => f.GetValue());
        var message = _selectedProvider.Entry.Provider.TryApplyDetectedTrust(testedSettings, currentValues, out var updated);
        if (updated != null)
            PopulateFields(updated);
        return message;
    }

    public async Task MountTaskLaunchCommand()
    {
        if (string.IsNullOrWhiteSpace(MountScript)) return;
        if (!PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts) return;
        var memoryLog = new MemoryLog();
        if (PlatformSpecificExperience.Instance.SupportManager.LaunchScript(memoryLog, MountScript, "***"))
            await Messages.ShowInformationBox(Resources.DataStorage_Field_DisconnectionScript_Ok);
        else
            await Messages.ShowErrorBox(Resources.DataStorage_Field_DisconnectionScript_Bad + Environment.NewLine + Environment.NewLine + memoryLog);
    }

    public async Task UnmountTaskLaunchCommand()
    {
        if (string.IsNullOrWhiteSpace(UnmountScript)) return;
        if (!PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts) return;
        var memoryLog = new MemoryLog();
        if (PlatformSpecificExperience.Instance.SupportManager.LaunchScript(memoryLog, UnmountScript, "***"))
            await Messages.ShowInformationBox(Resources.DataStorage_Field_DisconnectionScript_Ok);
        else
            await Messages.ShowErrorBox(Resources.DataStorage_Field_DisconnectionScript_Bad + Environment.NewLine + Environment.NewLine + memoryLog);
    }

    private static Bitmap LoadFromResource(Uri resourceUri) =>
        new(AssetLoader.Open(resourceUri));

    public string Title { get; }
    public Bitmap? IconSource { get; }
    public bool CanLaunchScripts { get; } = PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts;
    public ObservableCollection<StorageProviderItem> Providers { get; }
    public ObservableCollection<StorageFieldViewModel> Fields { get; } = [];

    private readonly List<(EnumFieldViewModel Vm, PropertyChangedEventHandler Handler)> _enumUiHandlers = [];

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

    public long Quota
    {
        get => _quota;
        set
        {
            if (value == _quota) return;
            _quota = value;
            OnPropertyChanged(nameof(Quota));
        }
    }

    #endregion

    #region MountScript

    private string? _mountScript;

    public string? MountScript
    {
        get => _mountScript;
        set
        {
            if (value == _mountScript) return;
            _mountScript = value;
            OnPropertyChanged(nameof(MountScript));
        }
    }

    #endregion

    #region UnmountScript

    private string? _unmountScript;

    public string? UnmountScript
    {
        get => _unmountScript;
        set
        {
            if (value == _unmountScript) return;
            _unmountScript = value;
            OnPropertyChanged(nameof(UnmountScript));
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
