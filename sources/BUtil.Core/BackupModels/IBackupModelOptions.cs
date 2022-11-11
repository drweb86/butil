using System.Text.Json.Serialization;

namespace BUtil.Core.BackupModels
{
    [JsonDerivedType(typeof(IncrementalBackupModelOptions), "Incremental")]
    public interface IBackupModelOptions
    {

    }
}
