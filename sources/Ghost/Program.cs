using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using BUtil.Core;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.PL;
using BUtil.Core.FileSystem;
using BUtil.Ghost.Localization;

[assembly: CLSCompliant(true)]
namespace BUtil.Ghost
{
    public static class Program
    {
    	#region Private Fields
    	
    	static ProgramOptions _options;
 	    
        static readonly string _SchedulerStartupCmd = 
        	Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.Startup), 
        	             Constants.TrayApplicationProcessName + CopyrightInfo.Version + ".cmd");
    	
    	#endregion
    	
        #region Public Properties
        
        /// <summary>
        /// Shows if the application should create icon in tray and show messages about backup
        /// </summary>
        public static bool RunAsWinFormApp
        {
            get { return _options.ShowSchedulerInTray; }
        }

		#endregion
    	
    	#region Private Methods

    	/// <summary>
    	/// ThreadPool function for closing
    	/// </summary>
    	static void closingIn10Seconds(object obj)
    	{
    		Thread.Sleep(10000);
    		Environment.Exit(-1);
    	}

    	/// <summary>
    	/// ThreadPool function for showing the message during 10 seconds
    	/// </summary>
    	/// <param name="message">The string message to show</param>
    	static void showMessageFor10Seconds(object message)
    	{
    		Messages.ShowInformationBox((string)message);
    		Thread.Sleep(10000);
    		
    		//FIXME: Message should be closed after the time above, but it didn't closed; don't know how to implement it!
	   	}
    	
    	/// <summary>
    	/// Verifies the startup and fixes it
    	/// </summary>
    	static void verifyStartupScriptAndFixIt()
    	{
    		string contents = prepareStartupFileContents();
        	bool recreate = false;
        	
        	if (!File.Exists(_SchedulerStartupCmd))
        	{
        		recreate = true;
        	}
        	else
        	{
        		if (File.ReadAllText(_SchedulerStartupCmd) != contents)
        		{
        			recreate = true;
        		}
        	}
        	
        	if (recreate)
        	{
        		File.WriteAllText(_SchedulerStartupCmd, contents);
        	}
    	}
    	
    	/// <summary>
    	/// Prepares string contents of a cmd script for startup
    	/// </summary>
    	/// <returns>The script contents</returns>
    	static string prepareStartupFileContents()
    	{
    		return "start \" \" \"" + Files.Scheduler + "\"" + " \"" + SchedulerParameters.START_WITHOUT_MESSAGE + "\"";
    	}
    	
    	/// <summary>
    	/// Creates start up cmd script in Start\Programs\Startup
    	/// </summary>
    	static void createStartupScript()
    	{
    		verifyStartupScriptAndFixIt();
    	}
    	
    	/// <summary>
    	/// Removes startup cmd script if any
    	/// </summary>
    	static void removeStartupScriptIfAny()
    	{
    		if (File.Exists(_SchedulerStartupCmd))
			{
            	File.Delete(_SchedulerStartupCmd);
            }
    	}
    	
    	/// <summary>
    	/// Processes the CREATE_STARTUP_SCRIPT and REMOVE_STARTUP_SCRIPT_IF_ANY arguments
    	/// </summary>
    	/// <param name="args">The command line arguments list</param>
    	static void processInternalArguments(string[] args)
    	{
			if (args != null && args.Length == 1)
            {
            	if (args[0] == SchedulerParameters.CREATE_STARTUP_SCRIPT)
            	{
            		createStartupScript();
            		Environment.Exit(-1);
				}
            	else if (args[0] == SchedulerParameters.REMOVE_STARTUP_SCRIPT_IF_ANY)
            	{
            		removeStartupScriptIfAny();
            		Environment.Exit(-1);
            	}
            }
    	}
    	
    	/// <summary>
    	/// Loads the configuration
    	/// </summary>
		static void loadConfiguration()
        {
			_options = null;

			try
			{
        		_options = ProgramOptionsManager.LoadSettings();
			}
			catch(OptionsException)
			{
				showErrorAndCloseApplicationIn10Seconds(Resources.OptionsAreDamagedPleaseReconfigureThemInConfigurator);
			}

           	if (_options.DontNeedScheduler)
           	{
           		showErrorAndCloseApplicationIn10Seconds(Resources.SchedulerStartUpIsForbiddenNtoChangeItYouCanOpenTheConfiguratorNNapplicationWillNowClose);
           	}
           		//TODO:
            // if there's no scheduling
            if (!_options.BackupTasks["default"].EnableScheduling)
            {
                // we're exiting to free system resources
                showErrorAndCloseApplicationIn10Seconds(Resources.NoOneTaskUsesSchedulerNyoudOpenConfiguratorOnWhenTabNNapplicationWillNowClose);
            }
        }
		
    	/// <summary>
    	/// Plans in separate thread closing of application in 10 seconds
    	/// </summary>
		/// <param name="message">The message to show</param>
    	public static void showErrorAndCloseApplicationIn10Seconds(string message)
    	{
    		ThreadPool.QueueUserWorkItem(new WaitCallback(closingIn10Seconds));
           	Messages.ShowErrorBox(message);
			Environment.Exit(-1);
	   	}   
    	
    	#endregion
    	
    	#region Public Methods

        [STAThread]
        public static void Main(string[] args)
        {
            ImproveIt.InitInfrastructure(true);

			processInternalArguments(args);
			
            if (!File.Exists(Files.ProfileFile))
            {
            	showErrorAndCloseApplicationIn10Seconds(Resources.ThereAreNoOptionsFileNyouCanConfigureApplicationInConfiruratorNNapplicationWillNowClose);
            }
            
            if (!SingleInstance.FirstInstance)
            {
            	showErrorAndCloseApplicationIn10Seconds(Resources.TheSecondCopyOfButilSchedulerRanNNnowThisCopyWillTerminate);
            }
            
            loadConfiguration();
            Controller controller = new Controller(_options);
            
            if (args != null && args.Length > 0)
            {
            	if ( !(args.Length == 1 && args[0] == SchedulerParameters.START_WITHOUT_MESSAGE) )
            	{
            		showErrorAndCloseApplicationIn10Seconds(Resources.ParameterIsWrongNNforTheCompleteParametersListPleaseSeeDocumentation);
				}
            }
            else
            {
				ThreadPool.QueueUserWorkItem(new WaitCallback(showMessageFor10Seconds), Resources.ButilSchedulerIsRunningNNtoNotSeeThisMessageAgainYouShouldRunTheSchedulerWithParameterNpleaseSeeTheDocumentationForDetails);
            }
            
            try
            {
            	if (!_options.DontCareAboutSchedulerStartup && !_options.DontNeedScheduler)
            	{
	            	verifyStartupScriptAndFixIt();
            	}

                Application.SetCompatibleTextRenderingDefault(false);

                using (WithTray tray = new WithTray(controller))
                {
                  	if (!RunAsWinFormApp)
                  	{
                	   	tray.TurnIntoHiddenMode();
                  	}
                  	
                    Application.Run();
                }
            }
            catch (Exception unhandledException)
            {
                ImproveIt.ProcessUnhandledException(unhandledException);
            }
        }
    	
        #endregion
    }
}
