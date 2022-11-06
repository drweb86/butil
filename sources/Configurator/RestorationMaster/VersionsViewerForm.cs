using System.Linq;
using System.Windows.Forms;
using BUtil.Core.State;

namespace BUtil.RestorationMaster
{
	internal partial class VersionsViewerForm : Form
	{
		private readonly IncrementalBackupState _incrementalBackupState;

		public VersionsViewerForm(IncrementalBackupState incrementalBackupState = null)
		{
			InitializeComponent();
			_incrementalBackupState = incrementalBackupState;

            //Resources.Recover;
			//Resources.UseRightClickMouseOnSelectedItemToRestoreIt;
		}
		
		private void Restore()
		{
				//using (HowToRestoreForm form = new HowToRestoreForm(_records[index], _controller))
				//{
				//	form.ShowDialog();
				//}
		}

		private void OnLoad(object sender, System.EventArgs e)
		{
			_versionsListBox.BeginUpdate();

			var versionsDesc = _incrementalBackupState.VersionStates
				.OrderByDescending(x => x.BackupDateUtc)
				.ToList();

			_versionsListBox.DataSource = versionsDesc;
            _versionsListBox.DisplayMember = nameof(VersionState.BackupDateUtc);
            _versionsListBox.EndUpdate();

            _versionsListBox.SelectedItem = versionsDesc.First();
            this.OnVersionListChange(sender, e);
        }

		private void OnVersionListChange(object sender, System.EventArgs e)
		{
			var selectedVersion = _versionsListBox.SelectedItem as VersionState;

		}
	}
}
