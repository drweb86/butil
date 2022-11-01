using BUtil.Core.Storages;
using System;

namespace BUtil.Core.Events
{
    public class StorageStatusEventArgs : EventArgs
    {
        public StorageSettings Settings { get; private set; }

        public ProcessingStatus Status { get; private set; }

        public StorageStatusEventArgs(StorageSettings settings, ProcessingStatus status)
        {
            Settings = settings;
            Status = status;
        }
    }
}
