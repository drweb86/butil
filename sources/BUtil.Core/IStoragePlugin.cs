namespace BUtil.Core.Storages;

/// <summary>
/// Implement this interface in a plugin assembly and place the DLL (plus any dependencies)
/// in the user plugin folder. StoragePluginLoader discovers and calls Register() at startup.
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
