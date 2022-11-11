using System;
using System.Windows.Forms;
using BUtil.Core.Storages;

namespace BUtil.Configurator
{
    internal interface IStorageConfigurationForm: IDisposable
    {
        IStorageSettings GetStorageSettings();

        DialogResult ShowDialog();
    }
}
