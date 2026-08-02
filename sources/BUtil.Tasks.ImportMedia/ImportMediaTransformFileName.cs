using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using System.IO;

namespace BUtil.Tasks.ImportMedia;

public static class ImportMediaTransformFileName
{
    public static readonly string[] Presets =
    [
        "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}\\{DATE:MM}\\{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}\\{DATE:MM}\\{DATE:dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}\\{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy'-'MM}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        // Popular photo-library layouts
        "{DATE:yyyy}\\{DATE:MMMM}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}\\{DATE:MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}\\{DATE:MM}\\{DATE:yyyyMMdd_HHmmss}",
        "{DATE:yyyyMMdd_HHmmss}",
        "{DATE:yyyy}\\{DATE:MM}\\{DATE:dd}\\{DATE:HH'-'mm'-'ss}",
        "{DATE:yyyy'-'MM'-'dd}\\{DATE:HH'-'mm'-'ss}",
    ];

    public static string? Validate(string? transformFileName)
    {
        if (string.IsNullOrWhiteSpace(transformFileName))
            return Resources.ImportMediaTask_Field_TransformFileName_Validation_Empty;

        try
        {
            var str = DateTokenReplacer.ParseString(transformFileName, DateTime.Now);
            using var tempFolder = new TempFolder();
            var fullPath = Path.Combine(tempFolder.Folder, str);
            Directory.CreateDirectory(fullPath);
            return null;
        }
        catch
        {
            return Resources.ImportMediaTask_Field_TransformFileName_Validation_Invalid;
        }
    }

    public static string? TryBuildExample(string transformFileName, string outputFolder)
    {
        try
        {
            var fileName = "DCIM001.jpg";
            var modifiedAt = DateTime.Now;
            var destination = outputFolder.TrimEnd('\\').TrimEnd('/')
                + Path.DirectorySeparatorChar
                + DateTokenReplacer.ParseString(transformFileName, modifiedAt)
                + Path.GetExtension(fileName);

            return string.Format(
                Resources.ImportMediaTask_Field_TransformFileName_Example,
                fileName,
                modifiedAt,
                destination);
        }
        catch
        {
            return null;
        }
    }
}
