using System;
using System.Windows.Forms;
using BUtil.Core.Storages;

namespace BUtil.Configurator
{
    internal interface IStorageConfigurationForm: IDisposable
    {
        StorageSettings GetStorageSettings();

        DialogResult ShowDialog();
    }
}
