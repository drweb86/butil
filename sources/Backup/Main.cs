using System;
using BUtil.Core;
using BUtil.Core.Misc;
using BUtil.ConsoleBackup.Controller;

[assembly: CLSCompliant(true)]
namespace BUtil.ConsoleBackup
{
	internal class MainClass
    {
        public static void Main(string[] args)
		{
            ImproveIt.InitInfrastructure(false);
            try
            {
                using (var controller = new ConsoleBackupController())
                {
                    if (controller.ParseCommandLineArguments(args))
                    {
                        if (controller.Prepare())
                        {
                            controller.Backup();
                        }
                    }
                }
                Console.WriteLine(CopyrightInfo.Copyright);
            }
            catch (Exception e)
            {
                ImproveIt.ProcessUnhandledException(e);
            }
		}
	}
}
