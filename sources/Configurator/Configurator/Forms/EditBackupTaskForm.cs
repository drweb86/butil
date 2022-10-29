using System;
using System.Collections.Generic;
using System.Windows.Forms;

using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Controls;
using BUtil.Core.PL;
using BUtil.Core.Options;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Configurator.Forms
{
    public partial class EditBackupTaskForm : Form
    {
        readonly Dictionary<BackupTaskViewsEnum, BackUserControl> _views;
        readonly BackupTask _task;
        readonly ScheduleInfo _scheduleInfo;
        readonly ProgramOptions _profileOptions;

        public bool ExecuteTask { get; private set; }

        public EditBackupTaskForm(ProgramOptions profileOptions, BackupTask task, ScheduleInfo scheduleInfo)
        {
            InitializeComponent();
            
            _task = task;
            _scheduleInfo = scheduleInfo;
            _profileOptions = profileOptions;
            _views = new Dictionary<BackupTaskViewsEnum, BackUserControl>();

            SetupUiComponents();
            ApplyLocalization();
        }

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
            Text = string.Format(Resources.BackupTask0, _task.Name);

            foreach (KeyValuePair<BackupTaskViewsEnum, BackUserControl> pair in _views)
            {
                pair.Value.ApplyLocalization();
            }
            choosePanelUserControl.ApplyLocalization();
            backupTaskTitleLabel_.Text = Resources.Title;
            _saveAndExecuteButton.Text = Resources.SaveAndRun;
            cancelButton.Text = Resources.Cancel;
            
            ViewChangeNotification(BackupTaskViewsEnum.SourceItems);
        }

        void UpdateAccessibilitiesView()
        {
            choosePanelUserControl.UpdateView(_profileOptions);

            if (_profileOptions.DontNeedScheduler)
            {
                ((SchedulerUserControl)_views[BackupTaskViewsEnum.Scheduler]).ResetScheduler();
            }
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
            _views[BackupTaskViewsEnum.Scheduler].SetOptionsToUi(_scheduleInfo);
            _views[BackupTaskViewsEnum.Encryption].SetOptionsToUi(new object[] { _profileOptions, _task });
            _views[BackupTaskViewsEnum.OtherOptions].SetOptionsToUi(new object[] { _task });
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

        private void OnNameChange(object sender, EventArgs e)
        {
            var trimmedText = TaskNameStringHelper.TrimIllegalChars(_taskTitleTextBox.Text);
            if (trimmedText != _taskTitleTextBox.Text)
            {
                _taskTitleTextBox.Text = trimmedText;
            }
        }
    }
}
