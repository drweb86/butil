using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.BUtilServer.Server;
using BUtil.Interop.Tasks;

namespace BUtil.Tasks.BUtilServer;

public static class BUtilServerTaskPlugin
{
    public static void Register()
    {
        TaskProviderRegistry.Register<BUtilServerModelOptionsV2>(
            jsonType: "BUtilServer",
            information: Resources.FtpsServerTask_Help,
            factory: (log, task, events, onMsg) => new FtpsServerRootTask(log, events, task, onMsg),
            verifier: (log, options, writeMode) =>
            {
                if (string.IsNullOrWhiteSpace(options.Username))
                    return Resources.User_Field_Validation;

                if (string.IsNullOrWhiteSpace(options.Password))
                    return Resources.Password_Field_Validation_NotSpecified;

                var folderError = StorageFactory.Test(log, new FolderStorageSettingsV2 { DestinationFolder = options.Folder }, writeMode);
                if (folderError != null)
                    return folderError;

                if (options.Port < PlatformSpecificExperience.Instance.MinimumListenerPort)
                    return Resources.Server_Field_Port_Validation + $"(Min port {PlatformSpecificExperience.Instance.MinimumListenerPort})";

                return null;
            });
    }
}
