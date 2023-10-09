using System.IO;
using System;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class ImportMediaTaskModelOptionsV2 : ITaskModelOptionsV2
    {
        public string TransformFileName { get; set; } = "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}";

        public IStorageSettingsV2 From { get; set; } = new FolderStorageSettingsV2();
        public string DestinationFolder { get; set; } = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "Camera Roll");
        public bool SkipAlreadyImportedFiles { get; set; } = true;
    }
}
