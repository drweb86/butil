using System.Text.Json.Serialization;

namespace BUtil.Core.ConfigurationFileModels.V1
{
    [JsonDerivedType(typeof(IncrementalBackupModelOptionsV1), "Incremental")]
    public interface IBackupModelOptionsV1
    {

    }
}