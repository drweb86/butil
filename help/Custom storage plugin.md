# Custom Storage Plugin

BUtil supports a plugin architecture for storage backends. You can implement your own storage type and make it available to BUtil by placing a compiled plugin assembly in the designated plugin folder.

## How Storage Plugins Work

Each storage plugin is a .NET class library that:

1. Implements `IStorageSettingsV2` — a serializable settings class that holds connection parameters.
2. Implements `IStorageSettingsProvider` — describes the UI fields and handles serialization/deserialization of settings.
3. Implements `IStorage` — the runtime client that performs upload, download, list, and delete operations.
4. Provides a static `Register()` method that calls `StorageProviderRegistry.Register(...)` to wire everything together.

At startup BUtil scans the plugin folder, loads assemblies, and calls the `Register()` method of any class named `*StoragePlugin` that is discovered.

## Quick Start: Copy an Existing Plugin

The easiest way to create a custom plugin is to copy an existing one from the repository and adapt it.

**Recommended starting point:** `BUtil.Storages.Sftp` (SFTP storage) — it is a self-contained, straightforward example with clear separation between settings, provider, and runtime client.

The relevant source files are:

| File | Purpose |
|------|---------|
| `SftpStorageSettingsV2.cs` | Settings POCO — properties serialized to/from JSON |
| `SftpStorageSettingsProvider.cs` | UI field descriptors, `CreateSettings`, `ExtractValues` |
| `SftpStorage.cs` | Runtime: connect, upload, download, list, delete |
| `SftpStoragePlugin.cs` | Static `Register()` entry point |

## Interface Reference

### `IStorageSettingsV2`

```csharp
public interface IStorageSettingsV2
{
    long SingleBackupQuotaGb { get; }
    string? MountPowershellScript { get; set; }
    string? UnmountPowershellScript { get; set; }
}
```

Implement this in a plain class with the connection parameters you need. All public properties are serialized to JSON automatically.

### `IStorageSettingsProvider`

```csharp
public interface IStorageSettingsProvider
{
    string StorageId { get; }          // unique key used as the JSON "$type" discriminator
    string DisplayName { get; }        // shown in the protocol dropdown
    int Order { get; }                 // position in the dropdown list
    bool IsSupported { get; }          // false to hide the provider on unsupported platforms
    IReadOnlyList<StorageFieldDescriptor> Fields { get; }
    bool CanHandle(IStorageSettingsV2 settings);
    IStorageSettingsV2 CreateSettings(IReadOnlyDictionary<string, string?> fieldValues,
                                      long quota, string? mountScript, string? unmountScript);
    IReadOnlyDictionary<string, string?> ExtractValues(IStorageSettingsV2 settings);
    string? TryApplyDetectedTrust(IStorageSettingsV2 testedSettings,
                                   IReadOnlyDictionary<string, string?> currentValues,
                                   out IReadOnlyDictionary<string, string?>? updatedValues);
}
```

`Fields` drives the UI entirely — no XAML changes are needed for new storage types.

Field types (`StorageFieldType`): `Text`, `Password`, `Integer`, `Folder`, `File`, `Enum`.

### `IStorage`

```csharp
public interface IStorage
{
    IStorageUploadResult Upload(ILog log, string localFile, string remotePath);
    void Download(ILog log, string remotePath, string localFile);
    IEnumerable<string> List(ILog log, string remoteFolder);
    void Delete(ILog log, string remotePath);
    void Test(ILog log);   // throws on failure — used by the connection test button
}
```

### Registration

```csharp
public static class MyStoragePlugin
{
    public static void Register()
    {
        StorageProviderRegistry.Register(
            new MyStorageSettingsProvider(),
            typeof(MyStorageSettingsV2),
            (log, settings, autodetect) => new MyStorage(log, (MyStorageSettingsV2)settings, autodetect));
    }
}
```

## Plugin Discovery

Place your compiled `.dll` (and any dependency DLLs) in the BUtil plugins folder:

- **Windows:** `%LOCALAPPDATA%\BUtil\Plugins\`
- **Linux:** `~/.local/share/BUtil/Plugins/`

BUtil scans all `.dll` files in that folder at startup, looks for a public static class whose name ends with `StoragePlugin`, and calls its `Register()` method via reflection. No configuration file is needed.

> **Note:** The plugin API may evolve between BUtil versions. Pin your plugin to the BUtil SDK version you compiled against. The `StorageId` string must remain stable across plugin versions because it is persisted in task configuration files as the JSON `"$type"` discriminator.

## Dependency on BUtil SDK

Add a NuGet reference (or project reference when building from source) to:

- `BUtil.Interop` — contains the `IStorage*` interfaces and `StorageFieldDescriptor`
- `BUtil.Core` — contains `StorageProviderRegistry`, `StorageFieldType`, and base classes

If building from source, these projects are located in the `sources/` directory of the repository.

## Example: Minimal Plugin Skeleton

```csharp
// MyStorageSettingsV2.cs
using BUtil.Core.ConfigurationFileModels.V2;

public class MyStorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }
    public string? MountPowershellScript { get; set; }
    public string? UnmountPowershellScript { get; set; }
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
```

```csharp
// MyStorageSettingsProvider.cs
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Storages;

public class MyStorageSettingsProvider : IStorageSettingsProvider
{
    public string StorageId => "MyStorage";
    public string DisplayName => "My Custom Storage";
    public int Order => 99;
    public bool IsSupported => true;

    public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
    [
        new StorageFieldDescriptor { Key = "endpoint", Label = "Endpoint URL", Type = StorageFieldType.Text },
        new StorageFieldDescriptor { Key = "apiKey",   Label = "API Key",      Type = StorageFieldType.Password },
    ];

    public bool CanHandle(IStorageSettingsV2 s) => s is MyStorageSettingsV2;

    public IStorageSettingsV2 CreateSettings(IReadOnlyDictionary<string, string?> v, long quota, string? mount, string? unmount) =>
        new MyStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mount,
            UnmountPowershellScript = unmount,
            Endpoint = v.GetValueOrDefault("endpoint") ?? string.Empty,
            ApiKey   = v.GetValueOrDefault("apiKey")   ?? string.Empty,
        };

    public IReadOnlyDictionary<string, string?> ExtractValues(IStorageSettingsV2 s)
    {
        var m = (MyStorageSettingsV2)s;
        return new Dictionary<string, string?> { ["endpoint"] = m.Endpoint, ["apiKey"] = m.ApiKey };
    }

    public string? TryApplyDetectedTrust(IStorageSettingsV2 _, IReadOnlyDictionary<string, string?> __, out IReadOnlyDictionary<string, string?>? u)
    { u = null; return null; }
}
```

```csharp
// MyStoragePlugin.cs
using BUtil.Core.Storages;

public static class MyStoragePlugin
{
    public static void Register()
    {
        StorageProviderRegistry.Register(
            new MyStorageSettingsProvider(),
            typeof(MyStorageSettingsV2),
            (log, s, _) => new MyStorage(log, (MyStorageSettingsV2)s));
    }
}
```
