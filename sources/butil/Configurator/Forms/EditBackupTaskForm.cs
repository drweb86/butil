using System;
using System.Collections.Generic;
using System.Windows.Forms;

using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Controls;
using BUtil.Core.PL;
using BUtil.Core.Options;
using BUtil.Configurator.Localization;
using BUtil.Configurator.AddBackupTaskWizard.View;
using BUtil.Configurator.Configurator.Controls.Tasks;

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

        private void SetupUiComponents()
        {
            var encryptionControl = new EncryptionUserControl(_task);

            _views.Add(BackupTaskViewsEnum.Name, new TaskNameUserControl());
            _views.Add(BackupTaskViewsEnum.SourceItems, new WhatUserControl());
            _views.Add(BackupTaskViewsEnum.Storages, new WhereUserControl());
            _views.Add(BackupTaskViewsEnum.Scheduler, new WhenUserControl());
            _views.Add(BackupTaskViewsEnum.How, new HowUserControl(_task, () => encryptionControl.UpdateModel(_task)));
            _views.Add(BackupTaskViewsEnum.Encryption, encryptionControl);
            _views.Add(BackupTaskViewsEnum.OtherOptions, new OtherTaskOptionsUserControl());
            foreach (KeyValuePair<BackupTaskViewsEnum, BackUserControl> pair in _views)
            {
                pair.Value.HelpLabel = _toolStripStatusLabel;
            }

            ApplyOptionsToUi();
            ViewChangeNotification(BackupTaskViewsEnum.Name);
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
            _saveAndExecuteButton.Text = Resources.SaveAndRun;
            cancelButton.Text = Resources.Cancel;
            
            ViewChangeNotification(BackupTaskViewsEnum.SourceItems);
        }

        void UpdateAccessibilitiesView()
        {
            choosePanelUserControl.UpdateView(_profileOptions);
        }

        bool SaveTask()
        {
            bool isValid = true;
            foreach (KeyValuePair<BackupTaskViewsEnum, BackUserControl> pair in _views)
            {
                isValid = isValid && pair.Value.ValidateUi();
                pair.Value.GetOptionsFromUi();
            }
            return isValid;
        }

        void SaveRequest(object sender, EventArgs e)
        {
            if (!SaveTask())
                return;

            ExecuteTask = false;
            
            DialogResult = DialogResult.OK;
            Close();
        }

        void ApplyOptionsToUi()
        {
            _views[BackupTaskViewsEnum.Name].SetOptionsToUi(_task);
            _views[BackupTaskViewsEnum.SourceItems].SetOptionsToUi(_task);
            _views[BackupTaskViewsEnum.Storages].SetOptionsToUi(_task);
            _views[BackupTaskViewsEnum.Scheduler].SetOptionsToUi(_scheduleInfo);
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
            if (!SaveTask())
                return;

            ExecuteTask = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool OnCanChangeView(BackupTaskViewsEnum oldView)
        {
            return _views[oldView].ValidateUi();
        }
    }
}
