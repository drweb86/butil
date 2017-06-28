#region Copyright
/*
 * Copyright (c)Cuchuk Sergey Alexandrovich, 2007-2008. All rights reserved
 * Project: BUtil
 * Link: http://www.sourceforge.net/projects/butil
 * License: GNU GPL or SPL with limitations
 * E-mail:
 * Cuchuk.Sergey@gmail.com
 * toCuchukSergey@yandex.ru
 */
#endregion


using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Configuration.Install;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Threading;

using BUutilService.BL;
using BUtil.Scheduling;
using BUtil.Common.FileSystem;
using BUtilService.BL;
using BUtil.Common;
using System.Globalization;
using System.Text;

namespace BUutilService
{
    public class BackupUtilityService : System.ServiceProcess.ServiceBase
    {
        private const string _7ZIPBINARIESWEREDAMAGED = "7-zip binaries were damaged by virus!!! Please reinstall software";
        private const string _InvalidOptions = "Options are damaged. Please reconfigure them in Configurator";
        private const string _SERVICENAME = "BUtil service";

        private readonly Scheduler _scheduler = new Scheduler();
        private Process _process = null;

        #region Security validation

        /// <summary>
        /// Checks integrity of 7-zip software. In case it's incorrect,
        /// writes event to program log and halts application
        /// Does not throw any exceptons
        /// </summary>
        private static void check7zipIntegrity()
        {
            try
            {
                MD5Class.Verify7ZipBinaries();
            }
            catch (InvalidSignException)
            {
                // there's no need in service - because installation was damaged by virus!
                seriousBugHelper(_7ZIPBINARIESWEREDAMAGED, true);
            }
            catch (Exception e)
            {
                ImproveIt.ProcessUnhandledException(e);
            }
        }
        #endregion

        public BackupUtilityService()
        {
            this.ServiceName = _SERVICENAME;
            this.CanStop = true;
            this.CanShutdown = true;
            this.CanHandlePowerEvent = true;
            _scheduler.DoAction += DoBackup;

            check7zipIntegrity();
        }

        void DoBackup(object sender, EventArgs e)
        {
            check7zipIntegrity();

            using (CpuUsage cpu = new CpuUsage())
            {
                cpu.WaitUntilCpuLoadingLess(_scheduler.Options.PuttingOffBackupCpuLoading);
            }

            _process = new Process();
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.Arguments = "-DoOutputInLogs";
            _process.StartInfo.FileName = Files.ConsoleBackupTool;
            _process.StartInfo.WorkingDirectory = Directories.BinariesDir;

            try
            {
                _process.Start();
            }
            catch (Exception ee)
            {
                ImproveIt.ProcessUnhandledException(ee);
            }
        }


        private static void seriousBugHelper(string shortDescription, bool exit)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("BUtil");
            builder.AppendLine(DateTime.Now.ToLongTimeString());
            
            builder.AppendLine("During running of service an error occured:");
            builder.AppendLine(shortDescription);

            File.AppendAllText(Files.BugReportFile, builder.ToString());

            if (exit)
                Environment.Exit(-1);
        }

        /// <summary>
        /// Starts the machinery
        /// </summary>
        void LoadConfiguration()
        {
            ScheduleOptions options = null;

            // fixed bug with restoring from sleep mode(do not access disk drive)
            try
            {
                options = ScheduleOptionsManager.Load();
                ScheduleOptionsManager.Validate(options);
            }
            catch (FileNotFoundException)
            {
                // but no config was created
                // it's normal situation after installation
                // so we're exiting
                Environment.Exit(0);
            }
            catch (ScheduleOptionsInvalidException)
            {
                seriousBugHelper(_InvalidOptions, true);
            }
            catch (Exception e)
            {
                ImproveIt.ProcessUnhandledException(e);
            }

            // if there's no scheduling
            if (!options.EnableScheduling)
                // we're exiting to free system resources
                Environment.Exit(0);
            else
            {
                _scheduler.Options = options;
                Resume();
            }
        }

        void Resume()
        {
            _scheduler.Resume();
        }

        protected override void OnStart(string[] args)
        {
            LoadConfiguration();
            base.OnStart(args);
        }

        /// <summary>
        /// Stops time planning and kills backup process if any
        /// </summary>
        void RequiresStop()
        {
#if DEBUG
            Console.WriteLine("RequiresStop: ");
#endif

            _scheduler.StopTimePlanning();
            if (_process != null)
            {
                if (!_process.HasExited)
                    try
                    {
                        _process.Kill();
                    }
                    catch (Exception e)
                    {
                        ImproveIt.ProcessUnhandledException(e);
                    }
                    finally 
                    { 
                        _process = null; 
                    }
            }

        }

        protected override void OnStop()
        {
            RequiresStop();
            this.ExitCode = 0;
        }

        protected override void OnShutdown()
        {
            OnStop();
        }


        /// <summary>
        /// Handles and processes all power events
        /// == not tested == BETA
        /// designed for supporting notebooks
        /// </summary>
        void onPowerStatusChanged()
        {
            PowerStatus power = SystemInformation.PowerStatus;
#if DEBUG
            Console.WriteLine("Battery state: " + power.BatteryChargeStatus.ToString());
#endif
            switch (power.BatteryChargeStatus)
            {
                // if we have no battery or unknown its state we're doing nothing!
                case BatteryChargeStatus.Unknown:
                case BatteryChargeStatus.NoSystemBattery:
                    {
                        // just do not carry about this
#if DEBUG
                        Console.WriteLine("just do not carry about this");
#endif
                        break;
                    }

                // power is too low - stopping any activity
                case BatteryChargeStatus.Critical:
                    {
#if DEBUG
                        Console.WriteLine("turning on economy mode");
#endif
                        RequiresStop();
                        break;
                    }

                // having enough power - continuing
                case BatteryChargeStatus.Charging:
                case BatteryChargeStatus.High:
                    {
#if DEBUG
                        Console.WriteLine("turning off economy");
#endif
                        Resume();
                        break;
                    }
            }

            double powerLifeTimeInHours = 1.0 * power.BatteryLifeRemaining / 3600;

            if (powerLifeTimeInHours > 0)// is known
            {
#if DEBUG
                Console.WriteLine("checking remained time of battery in hours: " + powerLifeTimeInHours.ToString(CultureInfo.InvariantCulture));
#endif

                if (powerLifeTimeInHours < 0.5)// half an hour is not enough to make backup
                    RequiresStop();
                else
                    Resume();

            }

        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
#if DEBUG
            Console.WriteLine("OnPowerEvent: " + powerStatus.ToString());
#endif

            switch (powerStatus)
            {
                case PowerBroadcastStatus.BatteryLow: 
                    RequiresStop(); 
                    break;

                case PowerBroadcastStatus.PowerStatusChange:
                    onPowerStatusChanged();
                    break;

                // required to suspend
                case PowerBroadcastStatus.QuerySuspend:
                    RequiresStop();
                    break;

                // some apps rejected this querry mode
                case PowerBroadcastStatus.QuerySuspendFailed:
                // or we're waking up
                case PowerBroadcastStatus.ResumeAutomatic:
                // and continuing our work
                case PowerBroadcastStatus.ResumeSuspend:
                    {
                        Resume();
                        break;
                    }

                // this event we doesn't process because we did it above
                case PowerBroadcastStatus.Suspend: ; break;

                // oem events are not handled because i cannot get process them - no information
                // TODO: разобраться с APM OEM событиями
                //case PowerBroadcastStatus.OemEvent: PowerStop(); break;

                // we didn't stop it so processing 2 events
                case PowerBroadcastStatus.ResumeCritical:
                    {
                        RequiresStop();
                        Resume();
                        break;
                    }
            }

            // Allowing all
            return true;
        }
    }
}
