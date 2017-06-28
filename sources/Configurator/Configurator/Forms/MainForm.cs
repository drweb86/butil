using System;
using System.Windows.Forms;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Controls;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using System.Collections.Generic;
using BULocalization;
using BUtil.Core.PL;

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
            _controller.LocalManager.OnApplyLanguage += ApplyLocalization;
			_controller.LoadSettings();
			_controller.LoadLanguage(languageToolStripMenuItem1);

			ApplyOptionsToUi();
			ChoosePanelUserControlViewChanged(ConfiguratorViewsEnum.Tasks);
			UpdateAccessibilitiesView();
			
			restorationToolToolStripMenuItem.Enabled = !Program.PackageIsBroken && !Program.SevenZipIsBroken;
//            helpToolStripMenuItem.Enabled = Program.
		}

        #endregion

        void RunRestorationTool()
		{
			_controller.OpenRestorationMaster(string.Empty, false);
		}

		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_skipSavingOnExitRequest)
			{
				if (MessageBox.Show(Translation.Current[242], Translation.Current[243], MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,0) == DialogResult.Yes)
				{
					e.Cancel = !SaveOptions(); 
				}
			}
			else
			{
				e.Cancel = false;
			}
		}
		
        void ApplyLocalization(Translation translation)
        {
        	foreach (KeyValuePair<ConfiguratorViewsEnum, BackUserControl> pair in _views)
        	{
        		pair.Value.ApplyLocalization();
        	}
            choosePanelUserControl.ApplyLocalization();
            
            toolsToolStripMenuItem.Text = translation[148];
            Text = translation[153];
            cancelButton.Text = translation[359];
            restorationToolToolStripMenuItem.Text = translation[455];
            haveNoNetworkAndInternetToolStripMenuItem.Text = translation[536];
            dontNeedSchedulerToolStripMenuItem.Text = translation[537];
            dontCareAboutScheulerStartupToolStripMenuItem.Text = translation[538];
            hideAboutTabToolStripMenuItem.Text = translation[539];
            miscToolStripMenuItem.Text = translation[540];
            dontCareAboutPasswordLengthToolStripMenuItem.Text = translation[556];
            journalsToolStripMenuItem.Text = translation[557];
            helpToolStripMenuItem.Text = translation[294];
            _aboutToolStripMenuItem.Text = translation[141];

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
            if (!Program.SchedulerInstalled) 
            {
            	foreach (KeyValuePair<string, BackupTask> pair in profileOptions.BackupTasks)
            	{
	            	pair.Value.UnscheduleAllDays();
            	}
            }
            _views[ConfiguratorViewsEnum.Tasks].SetOptionsToUi( profileOptions);
            _views[ConfiguratorViewsEnum.Logging].SetOptionsToUi(profileOptions);
            _views[ConfiguratorViewsEnum.OtherOptions].SetOptionsToUi(profileOptions);
            
            haveNoNetworkAndInternetToolStripMenuItem.Checked = profileOptions.HaveNoNetworkAndInternet;
			dontNeedSchedulerToolStripMenuItem.Checked = profileOptions.DontNeedScheduler;
            if (!Program.SchedulerInstalled) 
            {
            	dontNeedSchedulerToolStripMenuItem.Enabled = false;
            	dontCareAboutScheulerStartupToolStripMenuItem.Enabled = false;
            }
			dontCareAboutScheulerStartupToolStripMenuItem.Checked = profileOptions.DontCareAboutSchedulerStartup;
			hideAboutTabToolStripMenuItem.Checked = profileOptions.HideAboutTab;
			dontCareAboutPasswordLengthToolStripMenuItem.Checked = profileOptions.DontCareAboutPasswordLength;
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
		
		void OkButtonClick(object sender, EventArgs e)
		{
			if (SaveOptions())
			{
				_skipSavingOnExitRequest = true;
				Close();
			}
		}
		
		void CancelButtonClick(object sender, EventArgs e)
		{
			_skipSavingOnExitRequest = true;
			Close();
		}
		
		void HaveNoNetworkAndInternetToolStripMenuItemClick(object sender, EventArgs e)
		{
			_controller.ProgramOptions.HaveNoNetworkAndInternet = haveNoNetworkAndInternetToolStripMenuItem.Checked;
			UpdateAccessibilitiesView();
		}
		
		void DontNeedSchedulerToolStripMenuItemClick(object sender, EventArgs e)
		{
			_controller.ProgramOptions.DontNeedScheduler = dontNeedSchedulerToolStripMenuItem.Checked;
			UpdateAccessibilitiesView();
		}
		
		void DontCareAboutScheulerStartupToolStripMenuItemClick(object sender, EventArgs e)
		{
			_controller.ProgramOptions.DontCareAboutSchedulerStartup = dontCareAboutScheulerStartupToolStripMenuItem.Checked;
			UpdateAccessibilitiesView();
		}
		
		void HideAboutTabToolStripMenuItemClick(object sender, EventArgs e)
		{
			_controller.ProgramOptions.HideAboutTab = hideAboutTabToolStripMenuItem.Checked;
	        UpdateAccessibilitiesView();
		}

		void DontCareAboutPasswordLengthToolStripMenuItemClick(object sender, EventArgs e)
		{
			_controller.ProgramOptions.DontCareAboutPasswordLength = dontCareAboutPasswordLengthToolStripMenuItem.Checked;
			UpdateAccessibilitiesView();
		}

		void UpdateAccessibilitiesView()
		{
            _beforeAboutToolStripSeparator.Visible =
                _aboutToolStripMenuItem.Visible =
                !_controller.ProgramOptions.HideAboutTab;
		}
	
		void JournalsToolStripMenuItemClick(object sender, EventArgs e)
		{
			_controller.OpenJournals(false);
		}

        private void OnHelpToolStripMenuItemClick(object sender, EventArgs e)
        {
            SupportManager.StartProcess(Files.HelpFile, true);
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
/* Placing of left panel requirements
 * (explorer like)
 * 
 * 0 pixels from top
 * 1 from left
 * 1 from bottom
 * 
 * Placing of right controls
 * (alchogol like)
 * 
 * 3 from left
 * 0 from right
 * 0 from top
 */