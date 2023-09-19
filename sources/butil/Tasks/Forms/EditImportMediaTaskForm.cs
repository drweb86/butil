using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Core.PL;
using BUtil.Core.Localization;
using BUtil.Configurator.AddBackupTaskWizard.View;
using BUtil.Configurator.Configurator.Controls.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.Configurator.Forms
{
    partial class EditImportMediaTaskForm : Form
    {
        private readonly Dictionary<TaskEditorPageEnum, BackUserControl> _pages = new Dictionary<TaskEditorPageEnum, BackUserControl>();
        private readonly TaskV2 _task;
        private TaskNameUserControl _taskNameUserControl = new TaskNameUserControl(Resources.ImportMediaTask_Help);
        private WhereUserControl _whereUserControl = new WhereUserControl();
        private ImportMediaTaskWhereUserControl _importMediaTaskWhereUserControl = new ImportMediaTaskWhereUserControl();

        public EditImportMediaTaskForm(TaskV2 task, TaskEditorPageEnum startPage)
        {
            InitializeComponent();
            choosePanelUserControl.WhenVisible = false;
            choosePanelUserControl.EncryptionVisi1ble = false;

            _task = task;
            _pages = new Dictionary<TaskEditorPageEnum, BackUserControl>();
            
            if (task.Name == Resources.Task_Field_Name_NewDefaultValue)
            {
                Text = Resources.ImportMediaTask_Create;
            }
            else
            {
                Text = string.Format(Resources.ImportMediaTask_Edit_Title, task.Name);
            }
            cancelButton.Text = Resources.Button_Cancel;

            SetupUiComponents();
            ViewChangeNotification(startPage);
        }

        private void SetupUiComponents()
        {
            _pages.Add(TaskEditorPageEnum.Name, _taskNameUserControl);
            _pages.Add(TaskEditorPageEnum.SourceItems, _whereUserControl);
            _pages.Add(TaskEditorPageEnum.Storages, _importMediaTaskWhereUserControl);

            foreach (var pagePair in _pages)
            {
                pagePair.Value.HelpLabel = _toolStripStatusLabel;
            }

            _taskNameUserControl.TaskName = _task.Name;
            var settings = (ImportMediaTaskModelOptionsV2)_task.Model;
            _whereUserControl.StorageSettings = settings.From;
            _importMediaTaskWhereUserControl.TransformFileName = settings.TransformFileName;
            _importMediaTaskWhereUserControl.DestinationFolder = settings.DestinationFolder;
            _importMediaTaskWhereUserControl.SkipAlreadyImportedFiles = settings.SkipAlreadyImportedFiles;
        }

        private bool SaveTask()
        {
            bool isValid = true;
            foreach (var pagePair in _pages)
            {
                isValid = isValid && pagePair.Value.ValidateUi();
            }

            _task.Name = _taskNameUserControl.TaskName;
            var settings = (ImportMediaTaskModelOptionsV2)_task.Model;
            settings.From = _whereUserControl.StorageSettings;
            settings.TransformFileName = _importMediaTaskWhereUserControl.TransformFileName;
            settings.DestinationFolder = _importMediaTaskWhereUserControl.DestinationFolder;
            settings.SkipAlreadyImportedFiles = _importMediaTaskWhereUserControl.SkipAlreadyImportedFiles;

            return isValid;
        }

        private void SaveRequest(object sender, EventArgs e)
        {
            if (!SaveTask())
                return;
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ViewChangeNotification(TaskEditorPageEnum newView)
        {
            nestingControlsPanel.Controls.Clear();
            nestingControlsPanel.Controls.Add(_pages[newView]);
            nestingControlsPanel.Controls[0].Dock = DockStyle.Fill;
            nestingControlsPanel.AutoScrollMinSize = new System.Drawing.Size(_pages[newView].MinimumSize.Width, _pages[newView].MinimumSize.Height);
            optionsHeader.Title = choosePanelUserControl.SelectedCategory;
        }

        private bool OnCanChangeView(TaskEditorPageEnum oldView)
        {
            return _pages[oldView].ValidateUi();
        }
    }
}
