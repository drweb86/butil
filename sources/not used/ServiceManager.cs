using System;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.ServiceProcess;
using System.ComponentModel;
using System.Threading;
using BUtil.Common.FileSystem;
using BUtil.Kernel.Misc;
using BUtil.Common;

//DO NOT LOCALIZE. ALL ERRORS SHOULD BE HANDLED OUTSIDE
// THIS IS USED BY INSTALLER AND UNISTALLER
namespace BUtil.Kernel.BL
{
	public static class ServiceManager
	{
		private const string _cannotRemoveServiceFromSystem = 
			"Could not install/unistall service from system:\n{0}";
		private const string _couldNotPerformOperationFormatString =
			"Could not perform operation: {0}";

		static ServiceController BUtilServiceController//? а если не существует?
		{
			get {return new ServiceController(CopyrightInfo.ServiceName,"."); }
		}
		
		/// <summary>
		/// Checks if service installed in the system
		/// </summary>
		/// <returns>true - if installed</returns>
		/// <exception cref="InvalidOperationException">Windows error occured - do not handle</exception>
		public static bool CheckIfInstalled()
		{
			ServiceController[] services = new ServiceController[0];
			try
			{
				services = System.ServiceProcess.ServiceController.GetServices();
			}
			catch (Win32Exception e)
			{ throwInvalidOperationException(e.Message); }

			foreach (ServiceController service in services)
			{
				if (service.ServiceName == CopyrightInfo.ServiceName)
					return true;
			}
	
			return false;
		}

		/// <summary>
		/// Performs common service operations
		/// </summary>
		/// <param name="serviceOperation">Set of operations</param>
		/// <exception cref="InvalidOperationException">if not success</exception>
		public static void Perform(ServiceOperation serviceOperation)
		{ 
			switch (serviceOperation)
			{
				case ServiceOperation.Restart: restart(); break;
				case ServiceOperation.Start: start(); break;
				case ServiceOperation.Stop: stop(); break;
			}
		}

		#region Common service operations

		/// <summary>
		/// Stops the service
		/// </summary>
		/// <exception cref="InvalidOperationException">If not success</exception>
		private static void stop()
		{
			if (BUtilServiceController.Status == ServiceControllerStatus.Running)
			{
				try
				{
					BUtilServiceController.Stop();
				}
				catch (Win32Exception e)
				{ throwInvalidOperationException(e.Message); }
			}
		}
		
		/// <summary>
		/// Starts the service
		/// </summary>
		/// <exception cref="InvalidOperationException">If not success</exception>
		private static void start()
		{
            Thread.Sleep(1000);
			try
			{
				BUtilServiceController.Start();
			}
			catch (Win32Exception e)
			{ throwInvalidOperationException(e.Message); }
		}

		/// <summary>
		/// Restarts the service
		/// </summary>
		/// <exception cref="InvalidOperationException">Running failled</exception>
		private static void restart()
		{
			try
			{
				stop();
			}
			catch (InvalidOperationException)
			{ ; }

			start();
		}

		#endregion
		
		/// <summary>
		/// Checks wheather the state of installed service is 'Running'
		/// </summary>
		/// <exception cref="Win32Exception"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static bool IsRunning
		{
			get { return (BUtilServiceController.Status == ServiceControllerStatus.Running); }
		}

		#region Install / Uninstall of service
		/// <summary>
		/// Installs the service
		/// </summary>
		/// <exception cref="InvalidOperationException">Not an admin, something bad occured</exception>
		public static void Install()
		{
			runInstallWithParameter("-i");
		}

		/// <summary>
		/// Unistalls the service
		/// </summary>
		/// <exception cref="InvalidOperationException">Not an admin, something bad occured</exception>
		public static void Uninstall()
		{
			runInstallWithParameter("-u");
		}

		/// <summary>
		/// Starts service with special parameters and grabs its output
		/// </summary>
		/// <param name="parameter">Parameter to pass to the service</param>
		private static void runInstallWithParameter(string parameter)
		{
			if (string.IsNullOrEmpty(parameter))
				throw new ArgumentNullException("parameter");

            
			Process installProcess = new Process();
			installProcess.StartInfo.RedirectStandardOutput = true;
			installProcess.StartInfo.RedirectStandardError = true;
			installProcess.StartInfo.UseShellExecute = false;
			installProcess.StartInfo.CreateNoWindow = true;
			installProcess.StartInfo.FileName = Files.Service;
			installProcess.StartInfo.Arguments = parameter;

			try
			{
				installProcess.Start();
			}
			catch (InvalidOperationException e)
			{ throwInvalidOperationException(e.Message); }
			catch (Win32Exception e)
			{ throwInvalidOperationException(e.Message); }

			string output = installProcess.StandardOutput.ReadToEnd();

			try
			{
				// uninstallation/installation of service is a long time operation - 
				// so does not handling exceptions
				installProcess.WaitForExit();
			}
			catch (Win32Exception e)
			{ throwInvalidOperationException(e.Message); }
			
			if (installProcess.ExitCode != 0)
				throwInvalidOperationException(
					string.Format(CultureInfo.CurrentCulture, _cannotRemoveServiceFromSystem, output));
		
		}
		#endregion

		private static void throwInvalidOperationException(string message)
		{
			throw new InvalidOperationException(
						string.Format(CultureInfo.CurrentCulture,
							_couldNotPerformOperationFormatString,
							message));
		}

	}
}
