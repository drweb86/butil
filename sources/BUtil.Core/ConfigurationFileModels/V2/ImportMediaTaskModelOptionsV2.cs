using BUtil.Core.Localization;
using System.IO;
using System;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class ImportMediaTaskModelOptionsV2 : ITaskModelOptionsV2
    {
        public string TransformFileName { get; set; }

        public IStorageSettingsV2 From { get; set; }
        public string DestinationFolder { get; set; }
        public bool SkipAlreadyImportedFiles { get; set; }

        public static ImportMediaTaskModelOptionsV2 CreateDefault()
        {
            return new ImportMediaTaskModelOptionsV2
            {
                DestinationFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "Camera Roll"),
                SkipAlreadyImportedFiles = true,
                TransformFileName = "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
            };
        }
    }
}
