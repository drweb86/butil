using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BUtil.Core.Localization;
using BUtil.Configurator.Restore;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.RestorationMaster
{
    internal partial class VersionsViewerForm : Form
    {
        private readonly IncrementalBackupState _incrementalBackupState;
        private readonly IStorageSettingsV2 _storageSettings;

        public VersionsViewerForm(IStorageSettingsV2 storageSettings = null, IncrementalBackupState incrementalBackupState = null)
        {
            InitializeComponent();
            _incrementalBackupState = incrementalBackupState;
            _storageSettings = storageSettings;

            _selectedVersionToolStripLabel.Text = BUtil.Core.Localization.Resources.BackupVersion_Files_Title;
            _changesToolStripLabel.Text = BUtil.Core.Localization.Resources.BackupVersion_Changes_Title;
            _journalSelectedToolStripMenuItem.Text =
                _journalSelectedToolStripButton.Text =
                _treeViewJournalSelectedToolStripButton.Text =
                    _treeJournalSelectedToolStripMenuItem.Text =
                        BUtil.Core.Localization.Resources.BackupVersion_FileVersions_Open;
            _toolStripStatusLabel.Text = BUtil.Core.Localization.Resources.BackupVersion_Viewer_Help;
            recoverToolStripMenuItem.Text = _recoverToolStripButton.Text = BUtil.Core.Localization.Resources.Task_Restore;
        }



        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _filesTreeView.SelectedNode = _filesTreeView.GetNodeAt(e.X, e.Y);
        }

        private void OnJournalSelectedChangesView(object sender, EventArgs e)
        {
            if (_changesListView.SelectedItems.Count != 1)
                return;

            var blameForm = new BlameForm();
            blameForm.Init(_incrementalBackupState, _changesListView.SelectedItems[0].Tag.ToString(),
                versionDate =>
                {
                    _versionsListView.BeginUpdate();
                    _versionsListView.SelectedItems.Clear();
                    foreach (ListViewItem item in _versionsListView.Items)
                    {
                        if (((VersionState)item.Tag).BackupDateUtc == versionDate)
                            item.Selected = true;
                    }
                    _versionsListView.EndUpdate();
                }
                );
            blameForm.Show();
        }

        private void OnTreeJournalSelected(object sender, EventArgs e)
        {
            if (_filesTreeView.SelectedNode == null ||
                !(_filesTreeView.SelectedNode.Tag is StorageFile))
                return;

            var storageFile = (StorageFile)_filesTreeView.SelectedNode.Tag;

            var blameForm = new BlameForm();
            blameForm.Init(_incrementalBackupState, storageFile.FileState.FileName,
                versionDate =>
                {
                    _versionsListView.BeginUpdate();
                    _versionsListView.SelectedItems.Clear();
                    foreach (ListViewItem item in _versionsListView.Items)
                    {
                        if (((VersionState)item.Tag).BackupDateUtc == versionDate)
                            item.Selected = true;
                    }
                    _versionsListView.EndUpdate();
                }
                );
            blameForm.Show();
        }
    }
}
