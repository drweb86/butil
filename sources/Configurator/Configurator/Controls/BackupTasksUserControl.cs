using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BULocalization;
using BUtil.Core.Options;
using BUtil.Configurator.Configurator.Forms;

namespace BUtil.Configurator.Configurator.Controls
{
    internal delegate bool RequestToSaveOptions();

    /// <summary>
    /// Manages the tasks
    /// </summary>
    public partial class BackupTasksUserControl : Core.PL.BackUserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupTasksUserControl"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public BackupTasksUserControl(ConfiguratorController controller)
        {
            InitializeComponent();

            _controller = controller;

            OnTasksListViewResize(this, null);
        }

        #endregion

        #region Properties

        internal RequestToSaveOptions OnRequestToSaveOptions;

        #endregion

        #region Public Methods

        public override void ApplyLocalization()
        {
            SetHintForControl(_addButton, Translation.Current[144]);
            SetHintForControl(_removeButton, Translation.Current[634]);
            SetHintForControl(_editButton, Translation.Current[635]);
            SetHintForControl(_executeButton, Translation.Current[150]);

            _addToolStripMenuItem.Text =Translation.Current[365];
            _removeToolStripMenuItem.Text = Translation.Current[77];
            _editToolStripMenuItem.Text=Translation.Current[364];
            _executeToolStripMenuItem.Text = Translation.Current[638];

            titleColumnHeader.Text = Translation.Current[360];
        }

        public override void SetOptionsToUi(object settings)
        {
            _profileOptions = (ProgramOptions)settings;

            foreach (var taskPair in _profileOptions.BackupTasks)
            {
                AddTask(taskPair.Value);
            }

            RefreshTaskControls(this, null);
        }

        public override void GetOptionsFromUi()
        {
            _profileOptions.BackupTasks.Clear();
            
            foreach (ListViewItem item in _tasksListView.Items)
            {
                _profileOptions.BackupTasks.Add(item.Text, (BackupTask)item.Tag);
            }
        }

        #endregion

        #region Private Methods

        void AddTask(BackupTask task)
        {
            var item = new ListViewItem(task.Name, 0) {Tag = task};
            _tasksListView.Items.Add(item);
        }

        void AddTaskRequest(object sender, EventArgs e)
        {
            var newTask = AddBackupTaskWizard.AddBackupTaskWizard.OpenAddBackupTaskWizard(_profileOptions, false);
            if (newTask != null)
            {
                AddTask(newTask);
            }

            RefreshTaskControls(this, e);
        }

        void ChangeTaskRequest(object sender, EventArgs e)
        {
            if (_tasksListView.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem item = _tasksListView.SelectedItems[0];
            var task = (BackupTask)item.Tag;
            using (var form = new BackupTaskEditForm(_profileOptions, task))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Text = task.Name;

                    if (form.ExecuteTask)
                    {
                        ExecuteRequest(this, e);
                    }
                }
            }
        }

        void RemoveTaskRequest(object sender, EventArgs e)
        {
            var selectedTasks = new List<ListViewItem>();
            foreach (ListViewItem taskToRemove in _tasksListView.SelectedItems)
            {
                selectedTasks.Add(taskToRemove);
            }

            foreach (var selectedTask in selectedTasks)
            {
                if (MessageBox.Show(string.Format(Translation.Current[636], selectedTask.Text), Translation.Current[618], MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    _tasksListView.Items.Remove(selectedTask);
                }
            }
            
            RefreshTaskControls(this, e);
        }

        void ExecuteRequest(object sender, EventArgs e)
        {
            if (!OnRequestToSaveOptions())
            {
                return;
            }

            var selectedTasks = new List<string>();
            foreach (ListViewItem taskToExecute in _tasksListView.SelectedItems)
            {
                selectedTasks.Add(((BackupTask)taskToExecute.Tag).Name);
            }

            _controller.OpenBackupUiMaster(selectedTasks.ToArray(), false);
        }

        void RefreshTaskControls(object sender, EventArgs e)
        {
            _removeButton.Enabled = 
                _removeToolStripMenuItem.Enabled = 
                _editButton.Enabled = 
                _editToolStripMenuItem.Enabled = 
                _executeButton.Enabled =
                _executeToolStripMenuItem.Enabled =
                    _tasksListView.SelectedItems.Count > 0;

            _executeToolStripMenuItem.Enabled =
                _executeButton.Enabled = _executeButton.Enabled && !Program.PackageIsBroken && !Program.SevenZipIsBroken;
        }

        private void OnTasksListViewResize(object sender, EventArgs e)
        {
            titleColumnHeader.Width = _tasksListView.Width > DisplacementToBorder ? _tasksListView.Width - DisplacementToBorder : DisplacementToBorder;
        }

        #endregion

        #region Fields

        ProgramOptions _profileOptions;
        readonly ConfiguratorController _controller;

        #endregion

        #region Constants

        const int DisplacementToBorder = 40;

        #endregion
    }
}
