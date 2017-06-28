using System;
using System.Windows.Forms;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BULocalization;

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
			logLevelLabel.Text = Translation.Current[284];
			int loggingLevelIndex = logLevelComboBox.SelectedIndex;
			logLevelComboBox.Items.Clear();
            logLevelComboBox.Items.AddRange(new [] {Translation.Current[281], Translation.Current[283]});
            logLevelComboBox.SelectedIndex = loggingLevelIndex;

            logsLocationLabel.Text = Translation.Current[613];
            chooseOtherLogsLocationLinkLabel.Text = Translation.Current[614];
            restoreDefaultLogsLocationLinkLabel.Text = Translation.Current[615];
            _manageLogsLinkLabel.Text = Translation.Current[642];
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
			helpAboutLogTypeLabel.Text = Translation.Current[558+logLevelComboBox.SelectedIndex];
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
