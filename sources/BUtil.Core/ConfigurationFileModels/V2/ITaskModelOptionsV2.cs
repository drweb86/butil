using System.Text.Json.Serialization;

namespace BUtil.Core.ConfigurationFileModels.V2;

[JsonDerivedType(typeof(IncrementalBackupModelOptionsV2), "Incremental")]
[JsonDerivedType(typeof(ImportMediaTaskModelOptionsV2), "ImportMedia")]
[JsonDerivedType(typeof(SynchronizationTaskModelOptionsV2), "Synchronization")]
[JsonDerivedType(typeof(FileSenderClientModelOptionsV2), "FileSenderClient")]
[JsonDerivedType(typeof(FileSenderServerModelOptionsV2), "FileSenderServer")]
public interface ITaskModelOptionsV2
{

}
