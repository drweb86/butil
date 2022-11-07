using System;
using System.Windows.Forms;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Controls;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using System.Collections.Generic;

using BUtil.Core.PL;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Configurator.Forms
{
	internal partial class MainForm
	{
		#region Fields

		bool _skipSavingOnExitRequest;
        readonly ConfiguratorController _controller;
	    readonly Dictionary<ConfiguratorViewsEnum, BackUserControl> _views;
		
		#endregion

        #region Constructors

        public MainForm(ConfiguratorController controller)
		{
			_controller = controller;
			
			InitializeComponent();

		    var backupTasksControl = new BackupTasksUserControl(_controller);
			_views = new Dictionary<ConfiguratorViewsEnum, BackUserControl>
			             {
			                 {ConfiguratorViewsEnum.Tasks, backupTasksControl},
			                 {ConfiguratorViewsEnum.OtherOptions, new OtherOptionsUserControl()},
			                 {ConfiguratorViewsEnum.Logging, new LoggingUserControl(controller)}
			             };

		    backupTasksControl.OnRequestToSaveOptions += SaveOptions;

		    foreach (KeyValuePair<ConfiguratorViewsEnum, BackUserControl> pair in _views)
        	{
        		pair.Value.HelpLabel = helpToolStripStatusLabel;
        	}
			ApplyLocalization();
			_controller.LoadSettings();

			ApplyOptionsToUi();
			ChoosePanelUserControlViewChanged(ConfiguratorViewsEnum.Tasks);
			
			restorationToolToolStripMenuItem.Enabled = !Program.PackageIsBroken;
		}

        #endregion

        void RunRestorationTool()
		{
			_controller.OpenRestorationMaster(null, false);
		}

		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_skipSavingOnExitRequest)
			{
				if (MessageBox.Show(Resources.WouldYouLikeToApplyModifiedSettings, Resources.Exiting, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,0) == DialogResult.Yes)
				{
					e.Cancel = !SaveOptions(); 
				}
			}
			else
			{
				e.Cancel = false;
			}
		}
		
        void ApplyLocalization()
        {
        	foreach (KeyValuePair<ConfiguratorViewsEnum, BackUserControl> pair in _views)
        	{
        		pair.Value.ApplyLocalization();
        	}
            choosePanelUserControl.ApplyLocalization();
            
            toolsToolStripMenuItem.Text = Resources.Tasks;
            Text = Resources.Configurator;
            restorationToolToolStripMenuItem.Text = Resources.RestoreData;
            _helpToolStripMenuItem.Text= Resources.Help;
            journalsToolStripMenuItem.Text = Resources.BackupJournals;
            helpToolStripMenuItem.Text = Resources.Documentation;
            _aboutToolStripMenuItem.Text = Resources.About;

            ChoosePanelUserControlViewChanged(ConfiguratorViewsEnum.Tasks);
        }

        private bool SaveOptions()
        {
        	foreach (KeyValuePair<ConfiguratorViewsEnum, BackUserControl> pair in _views)
        	{
        		pair.Value.GetOptionsFromUi();
        	}
            
            return _controller.StoreSettings();
        }

        private void ApplyOptionsToUi()
        {
            ProgramOptions profileOptions = _controller.ProgramOptions;

            _views[ConfiguratorViewsEnum.Tasks].SetOptionsToUi( profileOptions);
            _views[ConfiguratorViewsEnum.Logging].SetOptionsToUi(profileOptions);
            _views[ConfiguratorViewsEnum.OtherOptions].SetOptionsToUi(profileOptions);
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
			_skipSavingOnExitRequest = true;
			Close();
		}
			
		void JournalsToolStripMenuItemClick(object sender, EventArgs e)
		{
			_controller.OpenJournals(false);
		}

        private void OnHelpToolStripMenuItemClick(object sender, EventArgs e)
        {
            SupportManager.DoSupport(SupportRequest.Documentation);
        }

        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            using (var aboutForm = new AboutForm(_controller))
            {
                aboutForm.ShowDialog();
            }
        }
	}
}
