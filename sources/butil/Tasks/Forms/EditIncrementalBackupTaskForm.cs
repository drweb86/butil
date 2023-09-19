using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Controls;
using BUtil.Core.PL;
using BUtil.Core.Options;
using BUtil.Core.Localization;
using BUtil.Configurator.AddBackupTaskWizard.View;
using BUtil.Configurator.Configurator.Controls.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.Configurator.Forms
{
    partial class EditIncrementalBackupTaskForm : Form
    {
        readonly Dictionary<TaskEditorPageEnum, BackUserControl> _views;
        readonly TaskV2 _task;
        readonly ScheduleInfo _scheduleInfo;
        private readonly TaskEditorPageEnum _initialView;

        public EditIncrementalBackupTaskForm(TaskV2 task, ScheduleInfo scheduleInfo, TaskEditorPageEnum initialView)
        {
            InitializeComponent();
            
            _task = task;
            _scheduleInfo = scheduleInfo;
            _initialView = initialView;
            _views = new Dictionary<TaskEditorPageEnum, BackUserControl>();

            SetupUiComponents();
            ApplyLocalization();
        }

        private void SetupUiComponents()
        {
            var encryptionControl = new EncryptionUserControl(_task);

            _views.Add(TaskEditorPageEnum.Name, new TaskNameUserControl(Resources.IncrementalBackup_Help));
            _views.Add(TaskEditorPageEnum.SourceItems, new WhatUserControl(_task));
            _views.Add(TaskEditorPageEnum.Storages, new WhereUserControl());
            _views.Add(TaskEditorPageEnum.Scheduler, new WhenUserControl());
            _views.Add(TaskEditorPageEnum.Encryption, encryptionControl);
            foreach (KeyValuePair<TaskEditorPageEnum, BackUserControl> pair in _views)
            {
                pair.Value.HelpLabel = _toolStripStatusLabel;
            }

            ApplyOptionsToUi();
            ViewChangeNotification(TaskEditorPageEnum.Name);
            UpdateAccessibilitiesView();
        }

        void ApplyLocalization()
        {
            Text = $"{_task.Name} - {Resources.ApplicationName_Tasks}";

            foreach (KeyValuePair<TaskEditorPageEnum, BackUserControl> pair in _views)
            {
                pair.Value.ApplyLocalization();
            }
            choosePanelUserControl.ApplyLocalization();
            cancelButton.Text = Resources.Button_Cancel;
            
            ViewChangeNotification(_initialView);
        }

        private void UpdateAccessibilitiesView()
        {
            choosePanelUserControl.UpdateView();
        }

        private bool SaveTask()
        {
            bool isValid = true;
            foreach (KeyValuePair<TaskEditorPageEnum, BackUserControl> pair in _views)
            {
                isValid = isValid && pair.Value.ValidateUi();
                pair.Value.GetOptionsFromUi();
            }

            _task.Name = ((TaskNameUserControl)_views[TaskEditorPageEnum.Name]).TaskName;
            ((IncrementalBackupModelOptionsV2)_task.Model).To = ((WhereUserControl)_views[TaskEditorPageEnum.Storages]).StorageSettings;

            return isValid;
        }

        private void SaveRequest(object sender, EventArgs e)
        {
            if (!SaveTask())
                return;
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ApplyOptionsToUi()
        {
            ((TaskNameUserControl)_views[TaskEditorPageEnum.Name]).TaskName = _task.Name;
            ((WhereUserControl)_views[TaskEditorPageEnum.Storages]).StorageSettings = ((IncrementalBackupModelOptionsV2)_task.Model).To;
            _views[TaskEditorPageEnum.Scheduler].SetOptionsToUi(_scheduleInfo);
        }

        private void ViewChangeNotification(TaskEditorPageEnum newView)
        {
            nestingControlsPanel.Controls.Clear();
            nestingControlsPanel.Controls.Add(_views[newView]);
            nestingControlsPanel.Controls[0].Dock = DockStyle.Fill;
            nestingControlsPanel.AutoScrollMinSize = new System.Drawing.Size(_views[newView].MinimumSize.Width, _views[newView].MinimumSize.Height);
            optionsHeader.Title = choosePanelUserControl.SelectedCategory;
        }

        private bool OnCanChangeView(TaskEditorPageEnum oldView)
        {
            return _views[oldView].ValidateUi();
        }
    }
}
