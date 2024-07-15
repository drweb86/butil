using System.Collections.Generic;

namespace BUtil.Core.ConfigurationFileModels.V1;

public class BackupTaskV1
{
    public BackupTaskV1()
    {
        Items = [];
        Storages = [];
        Model = new IncrementalBackupModelOptionsV1();
        FileExcludePatterns = [];
    }

    public IBackupModelOptionsV1 Model { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<IStorageSettingsV1> Storages { get; set; }
    public string Password { get; set; } = string.Empty;

    public List<SourceItemV1> Items { get; set; }
    public List<string> FileExcludePatterns { get; set; }
}