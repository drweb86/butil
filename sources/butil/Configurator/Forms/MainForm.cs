using System;
using System.Windows.Forms;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Core.Misc;
using System.Collections.Generic;
using BUtil.Core.PL;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Configurator.Forms
{
	internal partial class MainForm
	{
		#region Fields

	    readonly Dictionary<ConfiguratorViewsEnum, BackUserControl> _views;
		
		#endregion

        #region Constructors

        public MainForm()
		{
			InitializeComponent();

		    var backupTasksControl = new BackupTasksUserControl();
			_views = new Dictionary<ConfiguratorViewsEnum, BackUserControl>
			             {
			                 {ConfiguratorViewsEnum.Tasks, backupTasksControl},
			                 {ConfiguratorViewsEnum.Logging, new LogsUserControl()}
			             };

		    backupTasksControl.OnRequestToSaveOptions += OnRequestToSaveOptions;

		    foreach (KeyValuePair<ConfiguratorViewsEnum, BackUserControl> pair in _views)
        	{
        		pair.Value.HelpLabel = helpToolStripStatusLabel;
        	}
			ApplyLocalization();

			ApplyOptionsToUi();
			ChoosePanelUserControlViewChanged(ConfiguratorViewsEnum.Tasks);
			
			restorationToolToolStripMenuItem.Enabled = !Program.PackageIsBroken;
		}

        #endregion

        private static void RunRestorationTool()
		{
            ConfiguratorController.OpenRestorationMaster(null, false);
		}

		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			GetOptionsFromUi();
            e.Cancel = false;
		}
		
        void ApplyLocalization()
        {
        	foreach (KeyValuePair<ConfiguratorViewsEnum, BackUserControl> pair in _views)
        	{
        		pair.Value.ApplyLocalization();
        	}
            choosePanelUserControl.ApplyLocalization();
            
            Text = Resources.Configurator;
            restorationToolToolStripMenuItem.Text = Resources.RestoreData;
            _helpToolStripMenuItem.Text= Resources.Help;
            helpToolStripMenuItem.Text = Resources.Documentation;
            _aboutToolStripMenuItem.Text = Resources.About;

            ChoosePanelUserControlViewChanged(ConfiguratorViewsEnum.Tasks);
        }

		private void GetOptionsFromUi()
		{
            foreach (KeyValuePair<ConfiguratorViewsEnum, BackUserControl> pair in _views)
            {
                pair.Value.GetOptionsFromUi();
            }
        }

		private bool OnRequestToSaveOptions()
		{
            GetOptionsFromUi();
			return true;
        }

        private void ApplyOptionsToUi()
        {
            _views[ConfiguratorViewsEnum.Tasks].SetOptionsToUi(null);
            _views[ConfiguratorViewsEnum.Logging].SetOptionsToUi(null);
        }

		void RestorationToolToolStripMenuItemClick(object sender, EventArgs e)
		{
			RunRestorationTool();
		}

		void ChoosePanelUserControlViewChanged(ConfiguratorViewsEnum newView)
		{
			nestingControlsPanel.Controls.Clear();
			nestingControlsPanel.Controls.Add(_views[newView]);
			nestingControlsPanel.Controls[0].Dock = DockStyle.Fill;
			nestingControlsPanel.AutoScrollMinSize = new System.Drawing.Size(_views[newView].MinimumSize.Width, _views[newView].MinimumSize.Height);
			optionsHeader.Title = choosePanelUserControl.SelectedCategory;
		}
		
		void CancelButtonClick(object sender, EventArgs e)
		{
			Close();
		}

        private void OnHelpToolStripMenuItemClick(object sender, EventArgs e)
        {
            SupportManager.DoSupport(SupportRequest.Documentation);
        }

        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
			using var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }
	}
}
