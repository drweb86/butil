using System;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;


namespace BUtil.Configurator.Configurator.Controls
{
	internal sealed partial class LoggingUserControl : Core.PL.BackUserControl
    {
        ProgramOptions _profileOptions;
	    readonly ConfiguratorController _controller;

        public LoggingUserControl(ConfiguratorController controller)
		{
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            _controller = controller;

			InitializeComponent();
		}

        #region Overrides

        public override void ApplyLocalization() 
		{
            logsLocationLabel.Text = Resources.LogsLocation;
            chooseOtherLogsLocationLinkLabel.Text = Resources.ChangeLogsLocation;
            restoreDefaultLogsLocationLinkLabel.Text = Resources.RestoreDefaultLogsLocation;
		}
	
		public override void SetOptionsToUi(object settings)
		{
			_profileOptions = (ProgramOptions)settings;
			logsLocationLinkLabel.Text = _profileOptions.LogsFolder;
			logsViewerUserControl1.SetSettings(_profileOptions);
		}
		
		public override void GetOptionsFromUi()
		{
		}
		
		#endregion

        #region Private Methods

        private void OpenJournalsFolderLinkLabelLinkClicked(object sender, EventArgs e)
		{
            ProcessHelper.ShellExecute(_profileOptions.LogsFolder);
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

        #endregion
    }
}
