using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Core.PL;
using BUtil.Core.Localization;
using BUtil.Configurator.Configurator.Controls.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.Configurator.Forms
{
    partial class EditImportMediaTaskForm : Form
    {
        private readonly Dictionary<TaskEditorPageEnum, BackUserControl> _pages = new Dictionary<TaskEditorPageEnum, BackUserControl>();
        private readonly TaskV2 _task;
        private WhereUserControl _whereUserControl = new WhereUserControl();

        public EditImportMediaTaskForm(TaskV2 task, TaskEditorPageEnum startPage, bool isNewTask)
        {
            InitializeComponent();
            choosePanelUserControl.WhenVisible = false;
            choosePanelUserControl.EncryptionVisi1ble = false;

            _task = task;
            _pages = new Dictionary<TaskEditorPageEnum, BackUserControl>();

            Text = task.Name;

            cancelButton.Text = Resources.Button_Cancel;

            SetupUiComponents();
            ViewChangeNotification(startPage);
        }

        private void SetupUiComponents()
        {
            _pages.Add(TaskEditorPageEnum.SourceItems, _whereUserControl);

            foreach (var pagePair in _pages)
            {
                pagePair.Value.HelpLabel = _toolStripStatusLabel;
            }

            var settings = (ImportMediaTaskModelOptionsV2)_task.Model;
            _whereUserControl.StorageSettings = settings.From;
        }

        private bool SaveTask()
        {
            bool isValid = true;
            foreach (var pagePair in _pages)
            {
                isValid = isValid && pagePair.Value.ValidateUi();
            }

            var settings = (ImportMediaTaskModelOptionsV2)_task.Model;
            settings.From = _whereUserControl.StorageSettings;

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
