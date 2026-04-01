using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Security.Cryptography;

namespace butil_ui.Controls;

public class EncryptionTaskViewModel(string password, bool isPasswordCreateMode = true, bool isReadonly = false) : ObservableObject
{
    public void PasswordGenerateCommand()
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

    private string _password = password;

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
        }
    }

    public bool IsPasswordCreateMode { get; } = isPasswordCreateMode;
    public bool IsReadonly { get; } = isReadonly;

    #endregion
}
