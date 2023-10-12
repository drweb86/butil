using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Controls;
using BUtil.Core.PL;
using BUtil.Core.Options;
using BUtil.Core.Localization;
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
            _views.Add(TaskEditorPageEnum.SourceItems, new WhatUserControl(_task));
            _views.Add(TaskEditorPageEnum.Storages, new WhereUserControl());
            foreach (KeyValuePair<TaskEditorPageEnum, BackUserControl> pair in _views)
            {
                pair.Value.HelpLabel = _toolStripStatusLabel;
            }

            ApplyOptionsToUi();
            UpdateAccessibilitiesView();
        }

        void ApplyLocalization()
        {
            Text = _task.Name;

            foreach (KeyValuePair<TaskEditorPageEnum, BackUserControl> pair in _views)
            {
                pair.Value.ApplyLocalization();
            }
            choosePanelUserControl.ApplyLocalization();
            cancelButton.Text = Resources.Button_Cancel;
            
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
            ((WhereUserControl)_views[TaskEditorPageEnum.Storages]).StorageSettings = ((IncrementalBackupModelOptionsV2)_task.Model).To;
        }

        private bool OnCanChangeView(TaskEditorPageEnum oldView)
        {
            return _views[oldView].ValidateUi();
        }
    }
}
