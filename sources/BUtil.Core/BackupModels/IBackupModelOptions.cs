using System.Text.Json.Serialization;

namespace BUtil.Core.BackupModels
{
    [JsonDerivedType(typeof(IncrementalBackupModelOptions), "Incremental")]
    [JsonDerivedType(typeof(MediaSyncBackupModelOptions), "MediaSync")]
    public interface IBackupModelOptions
    {

    }
}
