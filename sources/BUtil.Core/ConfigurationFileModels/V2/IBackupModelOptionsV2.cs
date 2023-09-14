using System.Text.Json.Serialization;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    [JsonDerivedType(typeof(IncrementalBackupModelOptionsV2), "Incremental")]
    [JsonDerivedType(typeof(ImportMediaBackupModelOptionsV2), "ImportMedia")]
    public interface IBackupModelOptionsV2
    {

    }
}
