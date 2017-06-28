using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BULocalization;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Controls;
using BUtil.Core.PL;
using BUtil.Core.Options;

namespace BUtil.Configurator.Configurator.Forms
{
    public partial class BackupTaskEditForm : Form
    {
        #region Fields

        readonly Dictionary<BackupTaskViewsEnum, BackUserControl> _views;
        readonly BackupTask _task;
        readonly ProgramOptions _profileOptions;

        #endregion

        #region Properties

        public bool ExecuteTask { get; private set; }

        #endregion

        #region Constructors

        public BackupTaskEditForm(ProgramOptions profileOptions, BackupTask task)
        {
            InitializeComponent();
            
            _task = task;
            _profileOptions = profileOptions;
            _views = new Dictionary<BackupTaskViewsEnum, BackUserControl>();

            SetupUiComponents();
            ApplyLocalization();
        }

        #endregion

        #region Private Methods

        void SetupUiComponents()
        {
            _views.Add(BackupTaskViewsEnum.SourceItems, new SourceItemsUserControl());
            _views.Add(BackupTaskViewsEnum.Storages, new StoragesUserControl());
            _views.Add(BackupTaskViewsEnum.Scheduler, new SchedulerUserControl());
            _views.Add(BackupTaskViewsEnum.Encryption, new EncryptionUserControl());
            _views.Add(BackupTaskViewsEnum.OtherOptions, new TaskOtherOptionsUserControl());
            foreach (KeyValuePair<BackupTaskViewsEnum, BackUserControl> pair in _views)
            {
                pair.Value.HelpLabel = helpToolStripStatusLabel;
            }

            _taskTitleTextBox.Text = _task.Name;
            

            ApplyOptionsToUi();
            ViewChangeNotification(BackupTaskViewsEnum.SourceItems);
            UpdateAccessibilitiesView();
        }

        void ApplyLocalization()
        {
            Text = string.Format(Translation.Current[639], _task.Name);

            foreach (KeyValuePair<BackupTaskViewsEnum, BackUserControl> pair in _views)
            {
                pair.Value.ApplyLocalization();
            }
            choosePanelUserControl.ApplyLocalization();
            backupTaskTitleLabel_.Text = Translation.Current[633];
            _saveAndExecuteButton.Text = Translation.Current[640];
            cancelButton.Text = Translation.Current[4];
            
            ViewChangeNotification(BackupTaskViewsEnum.SourceItems);
        }

        void UpdateAccessibilitiesView()
        {
            choosePanelUserControl.UpdateView(_profileOptions);

            if (_profileOptions.DontNeedScheduler)
            {
                ((SchedulerUserControl)_views[BackupTaskViewsEnum.Scheduler]).ResetScheduler();
            }
            ((EncryptionUserControl)_views[BackupTaskViewsEnum.Encryption]).DontCareAboutPasswordLength = _profileOptions.DontCareAboutPasswordLength;
            ((StoragesUserControl)_views[BackupTaskViewsEnum.Storages]).TurnInternetAndLocalNetworkFunctions(!_profileOptions.HaveNoNetworkAndInternet);
        }

        void SaveTask()
        { 
            foreach (KeyValuePair<BackupTaskViewsEnum, BackUserControl> pair in _views)
            {
                pair.Value.GetOptionsFromUi();
            }
            _task.Name = _taskTitleTextBox.Text;        
        }

        void SaveRequest(object sender, EventArgs e)
        {
            ExecuteTask = false;
            SaveTask();
            DialogResult = DialogResult.OK;
            Close();
        }

        void ApplyOptionsToUi()
        {
            _views[BackupTaskViewsEnum.Storages].SetOptionsToUi(_task);
            _views[BackupTaskViewsEnum.SourceItems].SetOptionsToUi(_task);
            _views[BackupTaskViewsEnum.Scheduler].SetOptionsToUi(_task);
            _views[BackupTaskViewsEnum.Encryption].SetOptionsToUi(new object[] { _profileOptions, _task });
            _views[BackupTaskViewsEnum.OtherOptions].SetOptionsToUi(new object[] { _task });

            if (!Program.SchedulerInstalled)
            {
                _views[BackupTaskViewsEnum.Scheduler].Enabled = false;
            }
        }

        void ViewChangeNotification(BackupTaskViewsEnum newView)
        {
            nestingControlsPanel.Controls.Clear();
            nestingControlsPanel.Controls.Add(_views[newView]);
            nestingControlsPanel.Controls[0].Dock = DockStyle.Fill;
            nestingControlsPanel.AutoScrollMinSize = new System.Drawing.Size(_views[newView].MinimumSize.Width, _views[newView].MinimumSize.Height);
            optionsHeader.Title = choosePanelUserControl.SelectedCategory;
        }

        void SaveAndExecuteTaskButtonRequest(object sender, EventArgs e)
        {
            ExecuteTask = true;
            SaveTask();
            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion
    }
}
