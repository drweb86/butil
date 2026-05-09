namespace BUtil.Core.TasksTree;

/// <summary>
/// Implement this interface in a plugin assembly (reference <c>BUtil.Interop</c> only) and place the DLL
/// (plus any dependencies) in the portable folder next to binaries and/or the user plugins folder under
/// the BUtil profile directory. Host applications load assemblies and call <see cref="Register"/> at startup.
/// </summary>
public interface ITaskPlugin
{
    void Register();
}
