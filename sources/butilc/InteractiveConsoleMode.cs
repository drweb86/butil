using BUtil.Core;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

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
            Resources.TechnicalTool_PreventSleep_Title,
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
                RunTechnicalCommand(
                    Resources.TechnicalTool_EncryptAes256_Title,
                    true,
                    (ioc, input, output, password) => ioc.EncryptionService.EncryptAes256File(input, output, password),
                    (input, password) => PlatformSpecificExperience.Instance.SupportManager.GetConsoleCommandLineForEncrypt(input, password));
                break;
            case MenuAction.DecryptAes256:
                RunTechnicalCommand(
                    Resources.TechnicalTool_DecryptAes256_Title,
                    true,
                    (ioc, input, output, password) => ioc.EncryptionService.DecryptAes256File(input, output, password),
                    (input, password) => PlatformSpecificExperience.Instance.SupportManager.GetConsoleCommandLineForDecrypt(input, password));
                break;
            case MenuAction.CompressBrotli:
                RunTechnicalCommand(
                    Resources.TechnicalTool_CompressBrotli_Title,
                    false,
                    (ioc, input, output, _) => ioc.CompressionService.CompressBrotliFile(input, output),
                    (input, _) => PlatformSpecificExperience.Instance.SupportManager.GetConsoleCommandLineForCompress(input));
                break;
            case MenuAction.DecompressBrotli:
                RunTechnicalCommand(
                    Resources.TechnicalTool_DecompressBrotli_Title,
                    false,
                    (ioc, input, output, _) => ioc.CompressionService.DecompressBrotliFile(input, output),
                    (input, _) => PlatformSpecificExperience.Instance.SupportManager.GetConsoleCommandLineForDecompress(input));
                break;
            case MenuAction.PreventSleep:
                RunPreventSleep();
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

        var taskName = taskNames[ConsoleSelector.SelectWithArrowKeys(Resources.Task_Field_Choose, taskNames)];
        WriteCommandLineHint(PlatformSpecificExperience.Instance.SupportManager.GetConsoleCommandLineForTask(taskName));
        return taskName;
    }

    private static void RunTechnicalCommand(
        string title,
        bool requiresPassword,
        Action<CommonServicesIoc, string, string, string> run,
        Func<string, string, string> buildCommandLine)
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
            Console.WriteLine(Resources.TechnicalTool_Error_FileNotFound + Environment.NewLine + input);
            Console.WriteLine(input);
            Environment.Exit(-1);
        }

        if (requiresPassword && string.IsNullOrEmpty(password))
        {
            Console.WriteLine(Resources.TechnicalTool_Error_PasswordRequired);
            Environment.Exit(-1);
        }

        WriteCommandLineHint(buildCommandLine(input, password));

        using var ioc = new CommonServicesIoc(new ConsoleLog(), _ => { });
        run(ioc, input, output, password);
        Console.WriteLine(Resources.TechnicalTool_Completed);
    }

    private static void RunPreventSleep()
    {
        Console.WriteLine(Resources.TechnicalTool_PreventSleep_Title);
        Console.WriteLine(Resources.TechnicalTool_PreventSleep_Duration_Help);
        var raw = ReadConsoleField(Resources.DurationMinutes_Field);
        if (!TryParseDurationMinutes(raw, out var durationMinutes))
        {
            Console.WriteLine(Resources.TechnicalTool_PreventSleep_Duration_Help);
            Environment.Exit(-1);
        }

        Console.WriteLine(Resources.TechnicalTool_PreventSleep_PressAnyKey);
        WaitForKeyOrTimeout(durationMinutes > 0 ? TimeSpan.FromMinutes(durationMinutes) : null);
        Console.WriteLine(Resources.TechnicalTool_Completed);
    }

    private static bool TryParseDurationMinutes(string raw, out long durationMinutes)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            durationMinutes = 0;
            return true;
        }

        if ((long.TryParse(raw, NumberStyles.Integer, CultureInfo.CurrentCulture, out durationMinutes)
            || long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out durationMinutes))
            && durationMinutes >= 0)
        {
            return true;
        }

        durationMinutes = 0;
        return false;
    }

    private static void WaitForKeyOrTimeout(TimeSpan? timeout)
    {
        try
        {
            if (timeout is null)
            {
                Console.ReadKey(true);
                return;
            }

            var end = DateTime.UtcNow + timeout.Value;
            while (DateTime.UtcNow < end)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    return;
                }

                Thread.Sleep(200);
            }
        }
        catch (InvalidOperationException)
        {
            if (timeout is { } wait)
                Thread.Sleep(wait);
            else
                Console.ReadLine();
        }
    }

    private static void WriteCommandLineHint(string command)
    {
        Console.WriteLine();
        Console.WriteLine(Resources.CommandLineArguments_Hint);
        Console.WriteLine(command);
        Console.WriteLine();
    }

    private static string ReadConsoleField(string label)
    {
        Console.Write(label + " ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
