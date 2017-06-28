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
using BUtil.Kernel.Synchronization;
using BUtil.Kernel.Logs;
using BUtil.Common.FileSystem;
using BUtil.Scheduling;

namespace AutomaticTesting
{
	class MainClass
	{
		/*
		public static RunCPUUsageTest()
		{
			ConsoleKey key = ConsoleKey.Clear;
			while (key != ConsoleKey.Escape)
			{
				switch (key)
				{
					case ConsoleKey.NumPad0: break;
				
				}
			
			
				key = Console.ReadKey().Key;
			}
		
		}

		*/
		
		public static void TestSyncedFiles()
		{
			Console.WriteLine("Testing SyncedFiles...");
			string thesame = @"some\files";
			string thediff = @"some\2files";
			SyncedFiles sf = new SyncedFiles();
			if (sf.TryRegisterNewName(thesame) == false) Console.WriteLine("Error 1");
			if (sf.TryRegisterNewName(thesame) == false) Console.WriteLine("Passed 1(2) test");
			if (sf.TryRegisterNewName(thediff) == true) Console.WriteLine("Passed 2(2) test");
		
		}
		/*
		public static void TestFileLog()
		{
			Console.WriteLine("Testing FileLog...");
			
			FileLog filelog = new FileLog();
			filelog.Open();
			filelog.WriteLine(LoggingEvent.Error, "Это сообщение об ошибке, которое должно быть хорошо видно!");
			filelog.WriteLine(LoggingEvent.Information, "Это информационное сообщение!");
			filelog.WriteLine(LoggingEvent.ProcLevel, "Это отладочная информация");
			filelog.WriteLine(LoggingEvent.Warning, "Это предупреждение");
			filelog.Close();
			
		}*/
		
		public static void TestDirectories_BL()
		{
			try
			{
				Console.WriteLine("Testing Directories_BL...");
				Console.WriteLine("bin dir" + Directories.BinariesDir);
				Console.WriteLine("logs dir" + Directories.LogsFolder);
				Console.WriteLine("7-zip dir" + Directories.SevenZipFolder);
				Console.WriteLine("temp dir" + Directories.TempFolder);
			
			}
			catch (Exception ee)
			{
				Console.WriteLine("Dir: " + ee.Message);
			}
		}
		
		public static void actionfunc()
		{
			Console.WriteLine("In action func for 10 seconds");
			System.Threading.Thread.Sleep(10000);
			Console.WriteLine("Out action func");
		}
/*		
		public static void TestScheduler()
		{
			Console.WriteLine("Testing scheduler...");
			BUtilService.BL.Scheduler sc = Scheduler.GetInstance();
			
			ScheduleOptions schOptions = ScheduleOptionsManager.Default;
			schOptions.Hours = (byte)DateTime.Now.Hour;
			try {sc.Options = null; } catch {Console.WriteLine("1 - passed"); }

		
			
			sc.Resume();
			
			ConsoleKey key = ConsoleKey.Clear;
			while (key != ConsoleKey.Escape)
			{
				switch (key)
				{
					case ConsoleKey.NumPad0: break;
				
				}
			
			
				key = Console.ReadKey().Key;
			}
		
		}
		*/
		/*// found 2 bugs
		public static void RunCPUUsageTest()
		{
			
			ConsoleKey key = ConsoleKey.Clear;
			Console.WriteLine ("Testing arguments...");

            CpuUsage cpuusage = new CpuUsage();
			try
			{
				cpuusage.WaitUntilCpuLoadingLess(4);
			}
			catch
			{
				Console.WriteLine("1 - passed");
			}
			
			try
			{
                cpuusage.WaitUntilCpuLoadingLess(96);
			}
			catch
			{
				Console.WriteLine("2 - passed");
			}

            cpuusage.WaitUntilCpuLoadingLess(95);
			Console.WriteLine("3 - passed");

			try
			{
                cpuusage.GetNormalCpuUsage(0);
			}
			catch
			{
				Console.WriteLine("4 - passed");
			}
			try
			{
                cpuusage.GetNormalCpuUsage(61);
			}
			catch
			{
				Console.WriteLine("5 - passed");
			}

            cpuusage.GetNormalCpuUsage(10);
			Console.WriteLine("6 - passed");
			
			while (key != ConsoleKey.Escape)
			{
				switch (key)
				{
					case ConsoleKey.NumPad0: Console.WriteLine("> " + cpuusage.CpuActivity.ToString()); break;
					case ConsoleKey.NumPad1: 
					{
						Console.WriteLine("Period> "); 
						Console.WriteLine("> " + cpuusage.GetNormalCpuUsage(Int32.Parse(Console.ReadLine())).ToString());
						break;
					}
					
					case ConsoleKey.NumPad2: 
					{
						Console.WriteLine("Percents> ");
                        cpuusage.WaitUntilCpuLoadingLess(Int32.Parse(Console.ReadLine()));
						break;
					}

				
				}
				Console.WriteLine("CPU USAGE FUNCTIONS:");
				Console.WriteLine("0 - GetCurrentCPUActivity");
				Console.WriteLine("1 - GetNormalCPUUsage");
				Console.WriteLine("2 - WaitUntilCPULoadingLess");
				key = Console.ReadKey().Key;
			}
		
		}
		*/
		public static void Main(string[] args)
		{
			Console.WriteLine("(c) 2007 Cuchuk Sergey Alexandrovich \nproject BUTIL\ntool for automatic testing of application classes");
			
			#if RELEASE
			Console.WriteLine("To test please set compile mode to 'DEBUG' and recompile solution");
			return;
			#endif
			
			ConsoleKey key = ConsoleKey.Clear;
			while (key != ConsoleKey.Escape)
			{
				Console.WriteLine("Choose test:");
			
				Console.WriteLine("Namespace BUtilService.BL");
				Console.WriteLine("0 - class CPU USAGE");
				Console.WriteLine("1 - class Install_BL");
				Console.WriteLine("2 - class Scheduler");
				Console.WriteLine("Namespace BUtil.Common.DAL");
				Console.WriteLine("3 - class FileLog");
				Console.WriteLine("Namespace BUtil.Common.BL");
				Console.WriteLine("4 - class Directories_BL");
				Console.WriteLine("Namespace BUtil.Synchronization");
				Console.WriteLine("5 - class SyncedFiles");
				
				
				switch (key)
				{
					//case ConsoleKey.NumPad0: RunCPUUsageTest(); break;
					case ConsoleKey.NumPad1: Console.WriteLine("Does not require testing"); break;
					//case ConsoleKey.NumPad2: TestScheduler(); break;
					//case ConsoleKey.NumPad3: TestFileLog(); break;
					case ConsoleKey.NumPad4: TestDirectories_BL(); break;
					case ConsoleKey.NumPad5: TestSyncedFiles(); break;
				}
			
			
				key = Console.ReadKey().Key;
			}

		}
	}
}
