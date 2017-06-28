using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BUtil.Core.Options;
using BUtil.Core.Storages;

namespace BUtil.Configurator
{
    internal interface IStorageConfigurationForm: IDisposable
    {
        StorageBase Storage
        {
            get;
        }

        DialogResult ShowDialog();
    }
}
