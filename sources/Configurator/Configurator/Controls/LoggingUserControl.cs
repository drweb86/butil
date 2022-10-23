using System;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Options;


namespace BUtil.Configurator.Configurator.Controls
{
	/// <summary>
	/// Configures logging and provides access to logs
	/// Related document FD-2, FD-11
	/// </summary>
	internal sealed partial class LoggingUserControl : Core.PL.BackUserControl
    {
        #region Fields

        ProgramOptions _profileOptions;
	    readonly ConfiguratorController _controller;

        #endregion

        #region Constructors

        public LoggingUserControl(ConfiguratorController controller)
		{
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            _controller = controller;

			InitializeComponent();
		}

        #endregion

        #region Overrides

        public override void ApplyLocalization() 
		{
			logLevelLabel.Text = Resources.ChooseLoggingLevel;
			int loggingLevelIndex = logLevelComboBox.SelectedIndex;
			logLevelComboBox.Items.Clear();
            logLevelComboBox.Items.AddRange(new [] {Resources.Normal, Resources.Support});
            logLevelComboBox.SelectedIndex = loggingLevelIndex;

            logsLocationLabel.Text = Resources.LogsLocation;
            chooseOtherLogsLocationLinkLabel.Text = Resources.ChangeLogsLocation;
            restoreDefaultLogsLocationLinkLabel.Text = Resources.RestoreDefaultLogsLocation;
            _manageLogsLinkLabel.Text = Resources.OpenLogs;
		}
	
		public override void SetOptionsToUi(object settings)
		{
			_profileOptions = (ProgramOptions)settings;
			
			logLevelComboBox.SelectedIndex = (int)_profileOptions.LoggingLevel; 
			logsLocationLinkLabel.Text = _profileOptions.LogsFolder;
		}
		
		public override void GetOptionsFromUi()
		{
			if (logLevelComboBox.SelectedIndex < 0)
            {
				logLevelComboBox.SelectedIndex = (int)LogLevel.Normal;
            }
            _profileOptions.LoggingLevel = (LogLevel)logLevelComboBox.SelectedIndex;
		}
		
		#endregion

        #region Private Methods

        void OpenJournalsFolderLinkLabelLinkClicked(object sender, EventArgs e)
		{
			var operations = new LogOperations(_profileOptions.LogsFolder);
			operations.OpenLogsFolderInExplorer();
		}
		
		void LogLevelComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			helpAboutLogTypeLabel.Text = logLevelComboBox.SelectedIndex == 0 ? Resources.ErrorsAndWarningsWillBeSavedInLogs : Resources.ThisModeIsForGettingSupport;
		}
		
		void RestoreDefaultLogsLocationLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_profileOptions.LogsFolder = Directories.LogsFolder;
			SetOptionsToUi(_profileOptions);
		}
		
		void ChooseOtherLogsLocationLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				_profileOptions.LogsFolder = fbd.SelectedPath;
				SetOptionsToUi(_profileOptions);
			}
        }

        private void OnManageLogsLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _controller.OpenJournals(false);
        }

        #endregion
    }
}
