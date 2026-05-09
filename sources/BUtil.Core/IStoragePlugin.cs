namespace BUtil.Core.Storages;

/// <summary>
/// Implement this interface in a plugin assembly and place the DLL (plus any dependencies)
/// in the portable plugin folder next to the application binaries and/or in the user plugin folder
/// (<see cref="StoragePluginLoader.ApplicationPluginFolder"/> and
/// <see cref="StoragePluginLoader.PluginFolder"/>). StoragePluginLoader discovers and calls
/// <see cref="Register"/> at startup.
///
/// Minimal plugin example:
///   public class MyPlugin : IStoragePlugin
///   {
///       public void Register() =>
///           StorageProviderRegistry.Register(
///               new MyStorageSettingsProvider(),
///               typeof(MyStorageSettingsV2),
///               (log, s, _) => new MyStorage(log, (MyStorageSettingsV2)s));
///   }
/// </summary>
public interface IStoragePlugin
{
    void Register();
}
