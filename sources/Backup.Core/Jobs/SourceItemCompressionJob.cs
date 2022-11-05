using System;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Events;

namespace BUtil.Core.Jobs
{
    class SourceItemCompressionJob : IJob
    {
        private readonly SourceItemCompressionJobOptions _packingParameter;
        private readonly BackupEvents _events;
        private string _compressionOutput = null;
        private readonly LogBase _log;
        private EventHandler<JobThreadEventArgs> _finished;

        EventHandler<JobThreadEventArgs> IJob.TaskFinished
        {
            get { return _finished; }
            set { _finished = value; }
        }

        public SourceItemCompressionJob(LogBase log, SourceItemCompressionJobOptions parameter, BackupEvents events)
        {
            _packingParameter = parameter;
            _events = events;
            _log = log;
        }

        public void DoJob()
        {
            _log.WriteLine(LoggingEvent.Debug, _packingParameter.ToString() + ":started");
            bool succesfull = false;
            using (Process compressionProcess = CreateCompressProcess(_packingParameter.Arguments))
            {
                try
                {
                    compressionProcess.Start();
                    Thread thread = new(new ParameterizedThreadStart(ReadOutput));
                    thread.Start(compressionProcess.StandardOutput);

                    SetPriorityHelper(compressionProcess);

                    // FIXME: This is a workaround of .Net 2 bugs with processing ThreadAbortException and ThreadInterruptException
                    // both bugs were posted to MS
                    // this code is subject to be removed when those bugs will be fixed

                    while (!compressionProcess.HasExited)
                    {
                        Thread.Sleep(100);
                    }

                    while (thread.IsAlive)
                    {
                        Thread.Sleep(1000);
                    }

                    succesfull = IsSuccessfull7ZipPacking(compressionProcess.ExitCode);
                    _log.ProcessPackerMessage(_compressionOutput, succesfull);
                }
                catch (ThreadInterruptedException)
                {
                    succesfull = false;
                    _finished = null;
                    _log.WriteLine(LoggingEvent.Debug, _packingParameter.ItemToCompress.Target + ": Packing task is aborting...");

                    try
                    {
                        compressionProcess.Kill();
                        _log.WriteLine(LoggingEvent.Debug, _packingParameter.ItemToCompress.Target + ": Packing task is aborted");
                    }
                    catch (InvalidOperationException e)
                    {
                        _log.WriteLine(LoggingEvent.Error,
                              string.Format(CultureInfo.CurrentCulture, _packingParameter.ItemToCompress.Target + ":" + Resources.CouldNotKillPackerProcess0, e.Message));
                    }
                    catch (Win32Exception e)
                    {
                        _log.WriteLine(LoggingEvent.Error,
                              string.Format(CultureInfo.CurrentCulture, _packingParameter.ItemToCompress.Target + ":" + Resources.CouldNotKillPackerProcess0, e.Message));
                    }
                }
                catch (ObjectDisposedException e)
                {
                    _log.WriteLine(LoggingEvent.Error,
                                   string.Format(CultureInfo.CurrentCulture,
                                                 _packingParameter.ItemToCompress.Target + ": " + Resources.CouldNotStartPackerDueTo0AbortingBackup,
                                                 e.Message));
                }
                catch (Win32Exception e)
                {
                    _log.WriteLine(LoggingEvent.Error,
                                   string.Format(CultureInfo.CurrentCulture,
                                                 _packingParameter.ItemToCompress.Target + ": " + Resources.CouldNotStartPackerDueTo0AbortingBackup,
                                                 e.Message));
                }
                finally
                {
                    if (_finished != null)
                        _finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
                }
            }

            _log.WriteLine(LoggingEvent.Debug, _packingParameter.ToString() + ":finished");
        }

        /// <summary>
        /// This is a workaround of .net issue with dead lock of 7-zip
        /// </summary>
        /// <param name="obj">StreamReader object</param>
        void ReadOutput(object obj)
        {
            StreamReader writer = (StreamReader)obj;
            _compressionOutput = writer.ReadToEnd();
        }

        void SetPriorityHelper(Process process)
        {
            try
            {
                process.PriorityClass = _packingParameter.Priority;
                _log.WriteLine(LoggingEvent.Debug, _packingParameter.ItemToCompress.Target + ": Process priority set to " + _packingParameter.Priority);
            }
            catch (PlatformNotSupportedException)
            {
                _log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, _packingParameter.ItemToCompress.Target + ": Could not set priority '{0}' - platform does not support it. Setting idle priority", _packingParameter.Priority.ToString()));
                _packingParameter.Priority = ProcessPriorityClass.Idle;
                SetPriorityHelper(process);
            }
            catch (InvalidOperationException e)
            {
                _log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, _packingParameter.ItemToCompress.Target + ": Could not set priority to process because it already exited: {0}", e.Message));
            }
            catch (Win32Exception e)
            {
                _log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, _packingParameter.ItemToCompress.Target + ": Could not set priority because '{0}'", e.Message));
            }
        }

        static Process CreateCompressProcess(string arguments)
        {
            Process result = new();
            result.StartInfo.UseShellExecute = false;
            result.StartInfo.RedirectStandardOutput = true;
            result.StartInfo.RedirectStandardError = false;
            result.StartInfo.CreateNoWindow = true;
            result.StartInfo.FileName = Files.SevenZipPacker;
            result.StartInfo.Arguments = arguments;

            return result;
        }

        bool IsSuccessfull7ZipPacking(int code)
        {
            bool result = false;
            switch (code)
            {
                case (int)SevenZipReturnCodes.OK:
                    _log.WriteLine(LoggingEvent.Debug, "Archivator did it work OK");
                    result = true;
                    break;
                case (int)SevenZipReturnCodes.ErrorsOccured: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.ArchivatorWarningNonFatalErrorSForExampleOneOrMoreFilesWereLockedBySomeOtherApplicationSoTheyWereNotCompressed); break;
                case (int)SevenZipReturnCodes.FatalErrorsOccured: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.ArchivatorFatalError); break;
                case (int)SevenZipReturnCodes.InvalidArguments: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.PleaseReportThisBugToMeArchivatorCommandLineError); break;
                case (int)SevenZipReturnCodes.NotEnoughMemory: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.ArchivatorNotEnoughMemoryForOperation); break;
                case (int)SevenZipReturnCodes.ExternalTermination: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.UserStoppedTheProcess); break;
                default:
                    _log.WriteLine(LoggingEvent.Error, string.Format(CultureInfo.InvariantCulture, _packingParameter.ItemToCompress.Target + ": Abnormal 7-zip exit code: {0}. Please report this bug!", code.ToString(CultureInfo.InvariantCulture)));
                    break;
            }

            return result;
        }
    }
}
