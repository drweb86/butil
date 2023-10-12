
using BUtil.Core.Localization;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class TaskV2
    {
        public ITaskModelOptionsV2 Model { get; set; } = new IncrementalBackupModelOptionsV2();

        public string Name { get; set; } = Resources.Task_Field_Name_NewDefaultValue;
    }
}