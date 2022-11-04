using System;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.Core.FileSystem;
using BUtil.Core.Synchronization;
using BUtil.Core.ButilImage;
using BUtil.Core.Localization;
using BUtil.Core.Events;

namespace BUtil.Core.Jobs
{
    class CreateButilImageJob : IJob
    {
        #region Fields

        readonly SyncFile _imageFile;
        readonly LogBase _log;
        private readonly BackupEvents _events;
        private readonly object _eventKey;
        EventHandler<JobThreadEventArgs> _finished;
        readonly Collection<MetaRecord> _metarecords;
        public EventHandler NotificationEventHandler = null;

        #endregion

        #region Properties

        EventHandler<JobThreadEventArgs> IJob.TaskFinished
        {
            get { return _finished; }
            set { _finished = value; }
        }

        public CreateButilImageJob(
            LogBase log,
            BackupEvents events,
            object eventKey,
            SyncFile imageFile,
            Collection<MetaRecord> metarecords)
        {
            _imageFile = imageFile;
            _log = log;
            _events = events;
            _eventKey = eventKey;
            _metarecords = metarecords;
        }

        #endregion

        #region Public Methods

        public void DoJob()
        {
            bool succesfull = false;

            try
            {
                StatusUpdate(ProcessingStatus.InProgress);

                ReserveFileForBackupImage();
                ImageCreator imagePacker = new(_imageFile.FileName, new ImageHeader(_metarecords));
                imagePacker.Pack(true);
                succesfull = true;
            }
            catch (FileNotFoundException e)
            {
                _log.WriteLine(LoggingEvent.Error, string.Format(Resources.PackingAbortedComponentOfAnImageFile0DoesNotExistIsItEnoughSpaceInTemporaryFolder, e.Message));
                _log.WriteLine(LoggingEvent.Debug, e.ToString());
            }
            catch (IOException e)
            {
                _log.WriteLine(LoggingEvent.Error, string.Format(Resources.DuringPackingFilesToAnImage0AnErrorOccured1, _imageFile.FileName, e.Message));
                _log.WriteLine(LoggingEvent.Debug, e.ToString());
            }
            catch (ThreadInterruptedException)
            {
                _log.WriteLine(LoggingEvent.Error, Resources.PackingOfDataToAnImageWasAbortedByUser);
            }
            finally
            {
                StatusUpdate(succesfull ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);

                if (_finished != null)
                    _finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
            }
        }

        #endregion

        #region Private Methods

        void StatusUpdate(ProcessingStatus status)
        {
            //_events.CustomUpdate(_eventKey, status);
        }

        void ReserveFileForBackupImage()
        {
            _log.ProcedureCall("reserveFileForBackupImage");

            string templateFileName = Path.Combine(Directories.TempFolder,
                                                   "Backup{0}-" + DateTime.Now.ToString("dddd - dd - MMMM.yyyy HH.mm.ss", CultureInfo.InvariantCulture) + Files.ImageFilesExtension);

            for (int id = 0; id < int.MaxValue; id++)
            {
                string fileName;
                if (id > 0)
                {
                    fileName = string.Format(templateFileName, id, CultureInfo.InvariantCulture);
                }
                else
                {
                    fileName = string.Format(templateFileName, string.Empty, CultureInfo.InvariantCulture);
                }

                if (_imageFile.TrySyncFile(fileName))
                {
                    return;
                }
            }

            throw new InvalidOperationException("Fatal Error: could not reserve file name");
        }

        #endregion
    }
}
