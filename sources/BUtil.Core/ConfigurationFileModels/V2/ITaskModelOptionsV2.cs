using System.Text.Json.Serialization;

namespace BUtil.Core.ConfigurationFileModels.V2;

[JsonDerivedType(typeof(IncrementalBackupModelOptionsV2), "Incremental")]
[JsonDerivedType(typeof(ImportMediaTaskModelOptionsV2), "ImportMedia")]
[JsonDerivedType(typeof(SynchronizationTaskModelOptionsV2), "Synchronization")]
public interface ITaskModelOptionsV2
{

}
