namespace BUtil.RestorationMaster
{
	internal class RestoreController
	{
		public void OpenImage(string folder, string password)
		{
			// TBD: load versions.

			using var restoreForm = new RestoreForm(this);
			restoreForm.ShowDialog();
		}
	}
}
