using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Security.Cryptography;

namespace BUtil.UI.Controls;

public class EncryptionTaskViewModel : ObservableObject
{
    public EncryptionTaskViewModel(string password, bool isPasswordCreateMode = true, bool isReadonly = false, bool isExpanded = false)
    {
        _password = password;
        IsPasswordCreateMode = isPasswordCreateMode;
        IsReadonly = isReadonly;
        IsExpanded = isExpanded;
        PasswordGenerateCommand = new RelayCommand(GeneratePassword, () => IsPasswordCreateMode && !IsReadonly);
    }

    public IRelayCommand PasswordGenerateCommand { get; }

    private void GeneratePassword()
    {
        int count = 255;
        string temp = string.Empty;
        bool suit;
        char ch;

        byte[] resultentropy = new byte[count];
        byte[] tempentropy = new byte[count];

        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            randomNumberGenerator.GetBytes(resultentropy);
            randomNumberGenerator.GetBytes(tempentropy);

            for (int i = 0; i < count; i++)
            {
                suit = false;
                do
                {
                    randomNumberGenerator.GetBytes(tempentropy);
                    ch = (char)tempentropy[i];

                    if (ch >= 'a' && ch <= 'z' || ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z') suit = true;
                }
                while (!suit);

                resultentropy[i] = tempentropy[i];
            }
        }

        for (int i = 0; i < count; i++) temp += Convert.ToChar(resultentropy[i]);

        Password = temp.ToString();
    }

    #region Labels
    public static string LeftMenu_Encryption => Resources.LeftMenu_Encryption;
    public static string Password_Field => Resources.Password_Field;
    public static string Password_Help => Resources.Password_Help;
    public static string Password_Generate => Resources.Password_Generate;
    #endregion

    #region Password

    private string _password;
    private string? _passwordError;

    public string Password
    {
        get
        {
            return _password;
        }
        set
        {
            if (value == _password)
                return;
            _password = value;
            OnPropertyChanged(nameof(Password));
            PasswordError = null;
        }
    }

    public string? PasswordError
    {
        get
        {
            return _passwordError;
        }
        private set
        {
            if (value == _passwordError)
                return;
            _passwordError = value;
            OnPropertyChanged(nameof(PasswordError));
        }
    }

    public bool Validate()
    {
        PasswordError = string.IsNullOrWhiteSpace(Password)
            ? Resources.Password_Field_Validation_NotSpecified
            : null;
        return PasswordError is null;
    }

    public string? Help => IsPasswordCreateMode ? Password_Help : null;

    public bool IsPasswordCreateMode { get; }
    public bool IsReadonly { get; }
    public bool IsExpanded { get; }

    #endregion
}
