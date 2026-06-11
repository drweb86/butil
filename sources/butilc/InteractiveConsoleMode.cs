using BUtil.Core;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System;
using System.IO;
using System.Linq;

namespace butilc;

public static class InteractiveConsoleMode
{
    public static string Run()
    {
        Console.WriteLine(string.Format(Resources.CommandLineArguments_Help, ApplicationLinks.HomePage));
        Console.WriteLine();

        var options = new[]
        {
            Resources.Task_Launch,
            Resources.LogFile_BrowseLogsFolder,
            Resources.TechnicalTool_EncryptAes256_Title,
            Resources.TechnicalTool_DecryptAes256_Title,
            Resources.TechnicalTool_CompressBrotli_Title,
            Resources.TechnicalTool_DecompressBrotli_Title,
        };

        var selected = ConsoleSelector.SelectWithArrowKeys(Resources.Task_Field_Choose, options);
        var action = (MenuAction)selected;

        switch (action)
        {
            case MenuAction.OpenLogsFolder:
                PlatformSpecificExperience.Instance.GetFolderService().OpenFolderInShell(Directories.LogsFolder);
                Environment.Exit(0);
                throw new InvalidOperationException();
            case MenuAction.EncryptAes256:
                RunTechnicalCommand(Resources.TechnicalTool_EncryptAes256_Title, true, (ioc, input, output, password) => ioc.EncryptionService.EncryptAes256File(input, output, password));
                break;
            case MenuAction.DecryptAes256:
                RunTechnicalCommand(Resources.TechnicalTool_DecryptAes256_Title, true, (ioc, input, output, password) => ioc.EncryptionService.DecryptAes256File(input, output, password));
                break;
            case MenuAction.CompressBrotli:
                RunTechnicalCommand(Resources.TechnicalTool_CompressBrotli_Title, false, (ioc, input, output, _) => ioc.CompressionService.CompressBrotliFile(input, output));
                break;
            case MenuAction.DecompressBrotli:
                RunTechnicalCommand(Resources.TechnicalTool_DecompressBrotli_Title, false, (ioc, input, output, _) => ioc.CompressionService.DecompressBrotliFile(input, output));
                break;
            case MenuAction.SelectAndRunTask:
                return SelectTaskName();
            default:
                throw new InvalidOperationException();
        }

        Environment.Exit(0);
        throw new InvalidOperationException();
    }

    private static string SelectTaskName()
    {
        var taskStore = new TaskStore(new LocalFileSystem());
        var taskNames = taskStore.GetNames().ToArray();
        if (taskNames.Length == 0)
        {
            Console.WriteLine(string.Format(Resources.Task_Validation_NotFound, string.Empty));
            Environment.Exit(-1);
        }

        return taskNames[ConsoleSelector.SelectWithArrowKeys(Resources.Task_Field_Choose, taskNames)];
    }

    private static void RunTechnicalCommand(
        string title,
        bool requiresPassword,
        Action<CommonServicesIoc, string, string, string> run)
    {
        Console.WriteLine(title);
        var input = ReadConsoleField(Resources.TechnicalTool_SourceFile);
        var output = ReadConsoleField(Resources.TechnicalTool_OutputFile);
        var password = requiresPassword ? ReadConsoleField(Resources.Password_Field) : string.Empty;

        if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(output))
        {
            Console.WriteLine(Resources.TechnicalTool_Error_PathRequired);
            Environment.Exit(-1);
        }

        if (!File.Exists(input))
        {
            Console.WriteLine(Resources.TechnicalTool_Error_FileNotFound);
            Console.WriteLine(input);
            Environment.Exit(-1);
        }

        if (requiresPassword && string.IsNullOrEmpty(password))
        {
            Console.WriteLine(Resources.TechnicalTool_Error_PasswordRequired);
            Environment.Exit(-1);
        }

        using var ioc = new CommonServicesIoc(new ConsoleLog(), _ => { });
        run(ioc, input, output, password);
        Console.WriteLine(Resources.TechnicalTool_Completed);
    }

    private static string ReadConsoleField(string label)
    {
        Console.Write(label + " ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
