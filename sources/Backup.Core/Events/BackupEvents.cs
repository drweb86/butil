using BUtil.Core.Options;
using BUtil.Core.Storages;
using System;

namespace BUtil.Core.Events
{
    public class BackupEvents
    {
        public event EventHandler<ExecuteProgramStatusEventArgs> OnExecuteProgramStatusUpdate;
        public event EventHandler<SourceItemStatusEventArgs> OnSourceItemStatusUpdate;
        public event EventHandler<StorageStatusEventArgs> OnStorageStatusUpdate;
        public event EventHandler<ImagePackingEventArgs> OnImagePacking;// TODO:

        public event EventHandler OnFinished;
        public event EventHandler<ErrorEventArgs> OnError;

        public void ExecuteProgramStatusUpdate(ExecuteProgramTaskInfo taskInfo, ProcessingStatus status)
        {
            var handler = OnExecuteProgramStatusUpdate;
            if (handler == null)
                return;
            handler(this, new ExecuteProgramStatusEventArgs(taskInfo, status));
        }
        public void SourceItemStatusUpdate(SourceItem item, ProcessingStatus status)
        {
            var handler = OnSourceItemStatusUpdate;
            if (handler == null)
                return;
            handler(this, new SourceItemStatusEventArgs(item, status));
        }

        public void StorageStatusUpdate(StorageSettings settings, ProcessingStatus status)
        {
            var handler = OnStorageStatusUpdate;
            if (handler == null)
                return;
            handler(this, new StorageStatusEventArgs(settings, status));
        }

        public void ImagePacking(ProcessingStatus status)
        {
            var handler = OnImagePacking;
            if (handler == null)
                return;
            handler(this, new ImagePackingEventArgs(status));
        }

        public void Finished()
        {
            var handler = OnFinished;
            if (handler == null)
                return;
            handler(this, new EventArgs());
        }

        public void Error(string error)
        {
            var handler = OnError;
            if (handler == null)
                return;
            handler(this, new ErrorEventArgs(error));
        }
    }
}
