#nullable disable
using BUtil.Core.Localization;
using BUtil.Core.State;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BUtil.Configurator.Restore
{
    public partial class BlameForm : Form
    {
        private Action<DateTime> _selectVersion;

        public BlameForm()
        {
            InitializeComponent();

            this._closeButton.Text = Resources.Button_Close;
            _versionsListToolStripLabel.Text = BUtil.Core.Localization.Resources.BackupVersion_Versions_Title;
            _openSelectedVersionToolStripButton.Text =
                _openSelectedVersionToolStripMenuItem.Text = BUtil.Core.Localization.Resources.BackupVersion_Select;
        }

        public void Init(
            IncrementalBackupState incrementalBackupState,
            string fileFullPath,
            Action<DateTime> selectVersion)
        {
            _selectVersion = selectVersion;
            this.Text = string.Format(BUtil.Core.Localization.Resources.BackupVersion_FileVersion_Title, fileFullPath);

            this._versionsListView.SuspendLayout();
            this._versionsListView.Items.Clear();

            var descendingVersions = incrementalBackupState.VersionStates
                .OrderByDescending(x => x.BackupDateUtc)
                .ToList();

            foreach (var versionState in descendingVersions)
            {
                foreach (var sourceItemChanges in versionState.SourceItemChanges)
                {
                    foreach (var deletedFile in sourceItemChanges.DeletedFiles)
                    {
                        if (deletedFile == fileFullPath)
                        {
                            _versionsListView.Items.Add(new ListViewItem(versionState.BackupDateUtc.ToString(), 1) { Tag = versionState.BackupDateUtc });
                            break;
                        }
                    }

                    foreach (var createdFile in sourceItemChanges.CreatedFiles)
                    {
                        if (createdFile.FileState.FileName == fileFullPath)
                        {
                            _versionsListView.Items.Add(new ListViewItem(versionState.BackupDateUtc.ToString(), 0) { Tag = versionState.BackupDateUtc });
                            break;
                        }
                    }

                    foreach (var updatedFile in sourceItemChanges.UpdatedFiles)
                    {
                        if (updatedFile.FileState.FileName == fileFullPath)
                        {
                            _versionsListView.Items.Add(new ListViewItem(versionState.BackupDateUtc.ToString(), 2) { Tag = versionState.BackupDateUtc });
                            break;
                        }
                    }
                }
            }

            this._versionsListView.ResumeLayout();
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnOpenSelectedVersion(object sender, EventArgs e)
        {
            if (_versionsListView.SelectedItems.Count != 1)
                return;

            _selectVersion((DateTime)_versionsListView.SelectedItems[0].Tag);
        }
    }
}
