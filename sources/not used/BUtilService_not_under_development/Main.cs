using System;
using System.Collections.Generic;
using System.ServiceProcess;
//using System.Windows.Forms;
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

[assembly: CLSCompliant(true)]
namespace BUutilService
{
	class MainClass
	{
		private const string _USAGE = "Usage:\n" + 
			"BUtilService.exe\n" + 
			" Starts service\n\n" +
			"BUtilService.exe -Help\n" + 
			" Shows this help\n\n" +
			"BUtilService.exe -i\n" + 
			"BUtilService.exe -Install\n" + 			
			" Installs service in the system. Admin privileges required\n\n" +
			"BUtilService.exe -u\n" + 
			"BUtilService.exe -uninstall\n" + 
			" Uninstalls service in the system. Admin privileges required\n\n";

        private const string _ABOUT = "Backup service of Backup Utility(Butil)";
        private const string _HELP = "HELP";
        private const string _INSTALL = "INSTALL";
        private const string _DEINSTALL = "UNINSTALL";
        
        private const string _INVALIDARGUMENTS = "Invalid agument passed";
        private const string _INVALIDNUMBEROFARGUMENTS = "Invalid number of arguments";

        private static void runServiceHelper()
        {
            System.ServiceProcess.ServiceBase.Run(new BackupUtilityService());
        }

        private static void installHelper(bool install)
        {
                if (install)
                    InstallationManager.InstallMe();
                else
                    InstallationManager.UninstallMe();
        }

        [STAThread]
		public static void Main(string[] args)
		{
            ImproveIt.InitInfrastructure(false);
            try
            {
                Console.WriteLine(_ABOUT);
                if (args.Length == 0)
                    runServiceHelper();
                else
                    switch (args.Length)
                    {
                        case 1:
                            string command = args[0].Substring(1).ToUpperInvariant();
                            switch (command)
                            {
                                case _HELP:
                                    Console.WriteLine(_USAGE);
                                    return;
                                case "I":
                                case _INSTALL:
                                    installHelper(true);
                                    return;
                                case "U":
                                case _DEINSTALL:
                                    installHelper(false);
                                    return;
                                default:
                                    Console.WriteLine(_INVALIDARGUMENTS);
                                    Console.WriteLine(_USAGE);
                                    break;
                            }
                            break;

                        default:
                            Console.WriteLine(_INVALIDNUMBEROFARGUMENTS);
                            Console.WriteLine(_USAGE);
                            break;
                    }
            }
            catch (Exception e)
            {
                ImproveIt.ProcessUnhandledException(e);
            }
		}
	}
}
