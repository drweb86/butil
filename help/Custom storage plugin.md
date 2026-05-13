# Custom Storage Plugin

BUtil supports a plugin architecture for storage backends. You can implement your own storage type and make it available to BUtil by placing a compiled plugin assembly in one of the designated plugin folders.

## How Storage Plugins Work

Each storage plugin is a .NET class library that:

1. Implements `IStorageSettingsV2` — a serializable settings class that holds connection parameters.
2. Implements `IStorageSettingsProvider` — describes the UI fields and handles serialization/deserialization of settings.
3. Implements `IStorage` — the runtime client exposed to the backup engine (`Upload`, `Download`, listings, deletes, `Test()`, etc.).
4. Exposes `IStoragePlugin` — a concrete class with an instance method `Register()` that calls `StorageProviderRegistry.Register(...)` to wire the provider and factory together.

At startup BUtil scans the plugin folders, loads assemblies, and calls `Register()` on every public concrete class that implements `IStoragePlugin`.

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
    IReadOnlyList<string> ProtectedFieldKeys { get; }
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
`ProtectedFieldKeys` lists field keys whose corresponding string settings properties should be encrypted in saved task JSON.

Field types (`StorageFieldType`): `Text`, `Password`, `Integer`, `Folder`, `File`, `Enum`.

### `IStorage`

```csharp
public interface IStorage : IDisposable
{
    IStorageUploadResult Upload(string sourceFile, string relativeFileName);
    void Delete(string relativeFileName);
    void Move(string fromRelativeFileName, string toRelativeFileName);
    void Download(string relativeFileName, string targetFileName);
    bool Exists(string relativeFileName);
    void DeleteFolder(string relativeFolderName);
    string[] GetFolders(string relativeFolderName, string? mask = null);
    string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly);
    DateTime GetModifiedTime(string relativeFileName);
    string? Test();
}
```

The host constructs your implementation via the factory delegate passed to `StorageProviderRegistry.Register`. That factory receives `ILog` from `BUtil.Interop.Logs` (and settings plus an “autodetect” flag) so your client can emit diagnostics consistent with built-in storages.

### Registration (`IStoragePlugin`)

Plugins dropped into the folders use a **concrete type** implementing `IStoragePlugin`; the host resolves `IStoragePlugin` from the **`BUtil.Interop`** assembly only.

Built-in storages in this repository ship as separate projects and typically use **static** `Register()` helpers called from platform startup—they are **not** loaded via `IStoragePlugin`.

## Plugin Discovery

Place your compiled `.dll` (and any dependency DLLs) in **either** of these optional locations:

1. **Portable (next to the application)** — `plugins/storages` under the same directory as the main application binaries (the folder that contains `BUtil.Core.dll` and the platform experience library). Use this for USB / self-contained installs so plugins travel with the executable.
2. **Per-user (BUtil application data)** — `plugins/storages` under the same root BUtil uses for settings and logs:
   - **Release:** `%AppData%\BUtil\plugins\storages\` on Windows (typically `…\Roaming\BUtil\plugins\storages\`)
   - **Debug builds** use `BUtil-Development` instead of `BUtil` (`%AppData%\BUtil-Development\plugins\storages\`).
   - **Linux / macOS:** `<Application Data>/BUtil/plugins/storages/` — the same `BUtil` profile root Settings and Logs use (see .NET `Environment.SpecialFolder.ApplicationData` mapping on your OS).

BUtil loads plugins from the **portable folder first**, then from the **user folder**. If the same storage type were registered twice, the earlier registration wins. You normally use different plugins or only one location. No configuration file is required beyond dropping the DLLs in place.

BUtil scans all `.dll` files in each folder that exists at startup and calls `Register()` on each discovered `IStoragePlugin` implementation.

> **Note:** The plugin API may evolve between BUtil versions. Pin your plugin to the BUtil SDK version you compiled against. The `StorageId` string must remain stable across plugin versions because it is persisted in task configuration files as the JSON `"$type"` discriminator.

## Dependency on BUtil.Interop

Implementations should reference **`BUtil.Interop` only**. That assembly contains everything needed for authoring a storage backend:

- `IStorageSettingsV2` (under `BUtil.Core.ConfigurationFileModels.V2` for historical layout), `FolderStorageSettingsV2` sample model
- `IStorageSettingsProvider`, `StorageFieldDescriptor`, `StorageFieldType`, related enum/UI helpers (`StorageEnum*` types)
- `IStorage`, `IStorageUploadResult`
- **`ILog` and `LoggingEvent`** in **`BUtil.Interop.Logs`**
- `StorageProviderRegistry`, `IStoragePlugin`

Nothing from **`BUtil.Core`** is required in your plugin project. If building from source, add a project reference to **`sources/BUtil.Interop/BUtil.Interop.csproj`**.

**Repository note:** Existing first-party backends under `sources/BUtil.Storages.*` reference `BUtil.Core` because they share infrastructure with the hosting app—they are starting points only; trim them down to **`BUtil.Interop`** for a standalone drop-in DLL.

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

    public IReadOnlyList<string> ProtectedFieldKeys { get; } = ["apiKey"];

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
// MyStorage.cs — implement IStorage for your backend; hold settings and optionally ILog from the factory
using BUtil.Core.Storages;
using BUtil.Interop.Logs;
using System;
using System.IO;

public sealed class MyStorage(ILog log, MyStorageSettingsV2 settings, bool testingConnection) : IStorage
{
    private readonly MyStorageSettingsV2 Settings = settings;

    public void Dispose() { /* close connections */ }

    public IStorageUploadResult Upload(string sourceFile, string relativeFileName) =>
        throw new NotImplementedException();

    public void Delete(string relativeFileName) => throw new NotImplementedException();
    public void Move(string fromRelativeFileName, string toRelativeFileName) => throw new NotImplementedException();
    public void Download(string relativeFileName, string targetFileName) => throw new NotImplementedException();
    public bool Exists(string relativeFileName) => throw new NotImplementedException();
    public void DeleteFolder(string relativeFolderName) => throw new NotImplementedException();
    public string[] GetFolders(string relativeFolderName, string? mask = null) => throw new NotImplementedException();
    public string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly) => throw new NotImplementedException();
    public DateTime GetModifiedTime(string relativeFileName) => throw new NotImplementedException();
    public string? Test()
    {
        log.WriteLine(LoggingEvent.Debug, "MyStorage.Test");
        return null; /* or validation error message */
    }
}
```

```csharp
// MyStoragePlugin.cs
using BUtil.Core.Storages;

public class MyStoragePlugin : IStoragePlugin
{
    public void Register()
    {
        StorageProviderRegistry.Register(
            new MyStorageSettingsProvider(),
            typeof(MyStorageSettingsV2),
            (log, s, detect) => new MyStorage(log, (MyStorageSettingsV2)s, detect));
    }
}
```
