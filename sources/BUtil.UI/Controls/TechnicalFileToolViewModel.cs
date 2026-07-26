using Avalonia.Platform.Storage;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace BUtil.UI.Controls;

public class TechnicalFileToolViewModel : ViewModelBase
{
    public const string BrotliExtensionNoDot = "brotli";

    private string _lastSuggestedOutput = string.Empty;
    private string _inputPath = string.Empty;
    private string _outputPath = string.Empty;
    private string _password = string.Empty;
    private string? _inputPathError;
    private string? _outputPathError;
    private string? _passwordError;
    private string? _statusText;
    private MessageBarKind _statusKind;

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

    public bool IsBrotliTool =>
        Kind is TechnicalFileToolKind.DecompressBrotli or TechnicalFileToolKind.CompressBrotli;

    public bool IsAesTool => !IsBrotliTool;

    public string InputPath
    {
        get => _inputPath;
        set
        {
            if (value == _inputPath)
                return;
            _inputPath = value;
            OnPropertyChanged(nameof(InputPath));
            InputPathError = null;
            StatusText = null;
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
            OutputPathError = null;
            StatusText = null;
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
            PasswordError = null;
            StatusText = null;
        }
    }

    public string? InputPathError
    {
        get => _inputPathError;
        set
        {
            if (value == _inputPathError)
                return;
            _inputPathError = value;
            OnPropertyChanged(nameof(InputPathError));
        }
    }

    public string? OutputPathError
    {
        get => _outputPathError;
        set
        {
            if (value == _outputPathError)
                return;
            _outputPathError = value;
            OnPropertyChanged(nameof(OutputPathError));
        }
    }

    public string? PasswordError
    {
        get => _passwordError;
        set
        {
            if (value == _passwordError)
                return;
            _passwordError = value;
            OnPropertyChanged(nameof(PasswordError));
        }
    }

    public string? StatusText
    {
        get => _statusText;
        set
        {
            if (value == _statusText)
                return;
            _statusText = value;
            OnPropertyChanged(nameof(StatusText));
        }
    }

    public MessageBarKind StatusKind
    {
        get => _statusKind;
        set
        {
            if (value == _statusKind)
                return;
            _statusKind = value;
            OnPropertyChanged(nameof(StatusKind));
        }
    }

    public static string TechnicalTool_SourceFile => Resources.TechnicalTool_SourceFile;
    public static string TechnicalTool_OutputFile => Resources.TechnicalTool_OutputFile;
    public static string Button_Cancel => Resources.Button_Cancel;
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

    public void RunCommand()
    {
        StatusText = null;

        var input = InputPath.Trim();
        var output = OutputPath.Trim();

        InputPathError = string.IsNullOrWhiteSpace(input)
            ? Resources.TechnicalTool_Error_PathRequired
            : !File.Exists(input) ? Resources.TechnicalTool_Error_FileNotFound + Environment.NewLine + input : null;
        OutputPathError = string.IsNullOrWhiteSpace(output) ? Resources.TechnicalTool_Error_PathRequired : null;
        PasswordError = IsPasswordVisible && string.IsNullOrEmpty(Password) ? Resources.TechnicalTool_Error_PasswordRequired : null;

        if (InputPathError is not null || OutputPathError is not null || PasswordError is not null)
            return;

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
            StatusKind = MessageBarKind.Error;
            StatusText = ExceptionHelper.ToString(e);
            return;
        }

        StatusKind = MessageBarKind.Success;
        StatusText = Resources.TechnicalTool_Completed;
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
