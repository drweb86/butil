using System;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;


namespace BUtil.Configurator.Configurator.Controls
{
	internal sealed partial class LogsUserControl : Core.PL.BackUserControl
    {
        public LogsUserControl()
		{
			InitializeComponent();
		}

        public override void ApplyLocalization() 
		{
		}
	
		public override void SetOptionsToUi(object obj)
		{
			logsLocationLinkLabel.Text = Directories.LogsFolder;
            logsListUserControl1.SetSettings();
		}
		
		public override void GetOptionsFromUi()
		{
		}
		
        private void OpenJournalsFolderLinkLabelLinkClicked(object sender, EventArgs e)
		{
            ProcessHelper.ShellExecute(Directories.LogsFolder);
		}
    }
}
