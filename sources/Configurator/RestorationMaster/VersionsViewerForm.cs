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
	}
}
