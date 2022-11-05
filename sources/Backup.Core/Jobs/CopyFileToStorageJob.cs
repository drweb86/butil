using System;
using System.Globalization;
using System.Threading;
using BUtil.Core.Storages;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.Core.Localization;
using BUtil.Core.Events;

namespace BUtil.Core.Jobs
{
    class CopyFileToStorageJob : IJob
    {
        readonly StorageSettings _storageSettings;
        readonly string _imageFileToCopy;
        readonly LogBase _log;
        private readonly BackupEvents _events;
        EventHandler<JobThreadEventArgs> _finished;

        EventHandler<JobThreadEventArgs> IJob.TaskFinished
        {
            get { return _finished; }
            set { _finished = value; }
        }

        public CopyFileToStorageJob(LogBase log, BackupEvents events, StorageSettings storageSettings, string imageFileToCopy)
        {
            _storageSettings = storageSettings;
            _imageFileToCopy = imageFileToCopy;
            _log = log;
            _events = events;
        }

        public void DoJob()
        {
            try
            {
                bool succesfull = true;

                _log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InstalledUICulture, "Processing storage '{0}'", _storageSettings.Name));

                try
                {
                    var storage = StorageFactory.Create(_storageSettings);
                    StatusUpdate(ProcessingStatus.InProgress);
                    storage.Open(_log);
                    _log.WriteLine(LoggingEvent.Debug, "Storage opened successfully");
                    // storage.Put(_imageFileToCopy);
                    _log.WriteLine(LoggingEvent.Debug, "File was copyied to storage successfully");
                }
                catch (LogException e)
                {
                    _log.WriteLine(LoggingEvent.Error, string.Format(CultureInfo.InstalledUICulture, Resources.ErrorDiscoveredWhenOpeningStorage01, _storageSettings.Name, e.Message));
                    succesfull = false;
                }
                catch (Exception exc)// ok
                {
                    if (exc is ThreadInterruptedException)
                    {
                        _log.WriteLine(LoggingEvent.Error, string.Format("Copying to storage {0} aborted by user", _storageSettings.Name));
                    }

                    _log.WriteLine(LoggingEvent.Error, string.Format(CultureInfo.InstalledUICulture, Resources.ErrorDiscoveredWhenCopyingFileToStorage01, _storageSettings.Name, exc.Message));
                    succesfull = false;
                }

                StatusUpdate(succesfull ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
            }
            catch (ThreadInterruptedException)
            {
                _log.WriteLine(LoggingEvent.Error, string.Format("Copying to storage {0} aborted by user", _storageSettings.Name));
            }
            finally
            {
                if (_finished != null)
                    _finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
            }
        }

        void StatusUpdate(ProcessingStatus status)
        {
            //_events.StorageStatusUpdate(_storageSettings, status);
        }
    }
}
