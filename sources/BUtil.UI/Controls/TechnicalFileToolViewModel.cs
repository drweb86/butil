using Avalonia.Platform.Storage;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public class TechnicalFileToolViewModel : ViewModelBase
{
    public const string BrotliExtensionNoDot = "brotli";

    private string _lastSuggestedOutput = string.Empty;
    private string _inputPath = string.Empty;
    private string _outputPath = string.Empty;
    private string _password = string.Empty;

    public TechnicalFileToolViewModel(TechnicalFileToolKind kind)
    {
        Kind = kind;
        WindowTitle = kind switch
        {
            TechnicalFileToolKind.DecryptAes256 => Resources.TechnicalTool_DecryptAes256_Title,
            TechnicalFileToolKind.EncryptAes256 => Resources.TechnicalTool_EncryptAes256_Title,
            TechnicalFileToolKind.DecompressBrotli => Resources.TechnicalTool_DecompressBrotli_Title,
            TechnicalFileToolKind.CompressBrotli => Resources.TechnicalTool_CompressBrotli_Title,
            _ => throw new ArgumentOutOfRangeException(nameof(kind)),
        };
    }

    public TechnicalFileToolKind Kind { get; }

    public bool IsPasswordVisible =>
        Kind is TechnicalFileToolKind.DecryptAes256 or TechnicalFileToolKind.EncryptAes256;

    public string InputPath
    {
        get => _inputPath;
        set
        {
            if (value == _inputPath)
                return;
            _inputPath = value;
            OnPropertyChanged(nameof(InputPath));
            ApplySuggestedOutputFromInput();
        }
    }

    public string OutputPath
    {
        get => _outputPath;
        set
        {
            if (value == _outputPath)
                return;
            _outputPath = value;
            OnPropertyChanged(nameof(OutputPath));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (value == _password)
                return;
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public static string TechnicalTool_SourceFile => Resources.TechnicalTool_SourceFile;
    public static string TechnicalTool_OutputFile => Resources.TechnicalTool_OutputFile;
    public static string TechnicalTool_Run => Resources.TechnicalTool_Run;
    public static string TechnicalTool_Close => Resources.TechnicalTool_Close;
    public static string Password_Field => Resources.Password_Field;
    public static string Field_File_Browse => Resources.Field_File_Browse;

    public string PickSourceTitle => Resources.TechnicalTool_PickSourceFile;
    public string PickOutputTitle => Resources.TechnicalTool_PickOutputFile;

    public IReadOnlyList<FilePickerFileType> GetSourceFileTypeFilters()
    {
        var all = new FilePickerFileType(Resources.TechnicalTool_FileFilter_All) { Patterns = ["*"] };
        return Kind switch
        {
            TechnicalFileToolKind.DecryptAes256 =>
            [
                new FilePickerFileType(string.Format(Resources.TechnicalTool_FileFilter_Aes256V1, SourceItemHelper.AES256V1Extension))
                {
                    Patterns = ["*." + SourceItemHelper.AES256V1Extension],
                },
                all,
            ],
            TechnicalFileToolKind.DecompressBrotli =>
            [
                new FilePickerFileType(Resources.TechnicalTool_FileFilter_Brotli) { Patterns = ["*.brotli"] },
                all,
            ],
            _ => [all],
        };
    }

    public IReadOnlyList<FilePickerFileType> GetSaveFileTypeFilters()
    {
        var all = new FilePickerFileType(Resources.TechnicalTool_FileFilter_All) { Patterns = ["*"] };
        return Kind switch
        {
            TechnicalFileToolKind.EncryptAes256 =>
            [
                new FilePickerFileType(string.Format(Resources.TechnicalTool_FileFilter_Aes256V1, SourceItemHelper.AES256V1Extension))
                {
                    Patterns = ["*." + SourceItemHelper.AES256V1Extension],
                },
                all,
            ],
            TechnicalFileToolKind.CompressBrotli =>
            [
                new FilePickerFileType(Resources.TechnicalTool_FileFilter_Brotli) { Patterns = ["*.brotli"] },
                all,
            ],
            _ => [all],
        };
    }

    public string? SuggestedSaveFileName =>
        string.IsNullOrWhiteSpace(OutputPath) ? null : Path.GetFileName(OutputPath);

    public void CloseCommand()
    {
        WindowManager.SwitchView(new TasksViewModel());
    }

    public async Task RunCommand()
    {
        var input = InputPath.Trim();
        var output = OutputPath.Trim();
        if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(output))
        {
            await Messages.ShowErrorBox(Resources.TechnicalTool_Error_PathRequired);
            return;
        }

        if (!File.Exists(input))
        {
            await Messages.ShowErrorBox(Resources.TechnicalTool_Error_FileNotFound + Environment.NewLine + input);
            return;
        }

        if (IsPasswordVisible && string.IsNullOrEmpty(Password))
        {
            await Messages.ShowErrorBox(Resources.TechnicalTool_Error_PasswordRequired);
            return;
        }

        try
        {
            var log = new MemoryLog();
            using var ioc = new CommonServicesIoc(log, _ => { });
            switch (Kind)
            {
                case TechnicalFileToolKind.DecryptAes256:
                    ioc.EncryptionService.DecryptAes256File(input, output, Password);
                    break;
                case TechnicalFileToolKind.EncryptAes256:
                    ioc.EncryptionService.EncryptAes256File(input, output, Password);
                    break;
                case TechnicalFileToolKind.DecompressBrotli:
                    ioc.CompressionService.DecompressBrotliFile(input, output);
                    break;
                case TechnicalFileToolKind.CompressBrotli:
                    ioc.CompressionService.CompressBrotliFile(input, output);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
        catch (Exception e)
        {
            await Messages.ShowErrorBox(ExceptionHelper.ToString(e));
            return;
        }

        await Messages.ShowInformationBox(Resources.TechnicalTool_Completed);
    }

    private void ApplySuggestedOutputFromInput()
    {
        var suggested = ComputeSuggestedOutput(InputPath);
        if (string.IsNullOrEmpty(OutputPath) || OutputPath == _lastSuggestedOutput)
            OutputPath = suggested;
        _lastSuggestedOutput = suggested;
    }

    private string ComputeSuggestedOutput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return Kind switch
        {
            TechnicalFileToolKind.DecryptAes256 => StripSuffixCaseInsensitive(input, "." + SourceItemHelper.AES256V1Extension),
            TechnicalFileToolKind.EncryptAes256 => input + "." + SourceItemHelper.AES256V1Extension,
            TechnicalFileToolKind.DecompressBrotli => StripSuffixCaseInsensitive(input, "." + BrotliExtensionNoDot),
            TechnicalFileToolKind.CompressBrotli => input + "." + BrotliExtensionNoDot,
            _ => string.Empty,
        };
    }

    private static string StripSuffixCaseInsensitive(string path, string suffixWithDot)
    {
        if (path.EndsWith(suffixWithDot, StringComparison.OrdinalIgnoreCase) && path.Length > suffixWithDot.Length)
            return path[..^suffixWithDot.Length];
        return string.Empty;
    }
}
