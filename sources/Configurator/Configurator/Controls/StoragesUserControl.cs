using System;
using System.ComponentModel;
using System.Windows.Forms;

using BUtil.Core.Storages;
using BUtil.Core.Options;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Configurator.Controls
{
	/// <summary>
	/// Manages target places for backup
	/// </summary>
	internal sealed partial class StoragesUserControl : Core.PL.BackUserControl
	{
		BackupTask _task;
		
		public StoragesUserControl()
		{
			InitializeComponent();
		}
		
		#region Overrides
		
		public override void ApplyLocalization() 
		{
			networkStorageToolStripMenuItem1.Text = Resources.NetworkStorage;
			ftpStorageToolStripMenuItem1.Text = Resources.FtpStorage;
			hardDriveStorageToolStripMenuItem1.Text = Resources.HardDriveStorage;
			storagesListView.Groups[0].Header = Resources.Hdd;
			storagesListView.Groups[1].Header = Resources.Ftp;
			storagesListView.Groups[2].Header = Resources.NetworkStorages;
			SetHintForControl(addStorageButton, Resources.Add);
			removeToolStripMenuItem.Text = Resources.RemoveFromList;
			modifyToolStripMenuItem.Text = Resources.Modify;
			networkStorageToolStripMenuItem.Text = Resources.NetworkStorage;
			ftpStorageToolStripMenuItem.Text = Resources.FtpStorage;
			hardDriveStorageToolStripMenuItem.Text = Resources.HardDriveStorage;
			addNewToolStripMenuItem.Text = Resources.Add;
			SetHintForControl(modifyStorageButton, Resources.Modify);
			SetHintForControl(removeStorageButton, Resources.RemoveFromList);
		}
	
		public override void SetOptionsToUi(object settings)
		{
            _task = (BackupTask)settings;

			foreach (StorageBase storage in _task.Storages)
            {
				StorageEnum kind;

				switch (storage.GetType().Name)
                {
                    case "HddStorage":
						kind = StorageEnum.Hdd;
						break;

                    case "FtpStorage":
						kind = StorageEnum.Ftp;
						break;

					case "NetworkStorage":
						kind = StorageEnum.Network;
						break;

                	default:
                		throw new NotImplementedException(storage.GetType().Name);
                }

				AddStorageToListView(storage, kind);
            }
		}
		
		public override void GetOptionsFromUi()
		{
			_task.Storages.Clear();

			foreach (ListViewItem item in storagesListView.Items)
			{
				_task.Storages.Add((StorageBase)item.Tag);
			}
		}
		#endregion
		
		void StoragesListViewItemActivate(object sender, EventArgs e)
		{
			ModifyStorage();
		}
		
		void StoragesListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			bool enabled = storagesListView.SelectedItems.Count != 0;
			modifyStorageButton.Enabled = enabled;
			removeStorageButton.Enabled = enabled;
		}
		
		void AddStorageButtonClick(object sender, EventArgs e)
		{
			addStorageContextMenuStrip.Show(addStorageButton, addStorageButton.Width / 2, addStorageButton.Height / 2);
		}
		
		void ModifyStorageButtonClick(object sender, EventArgs e)
		{
			ModifyStorage();
		}
		
		void RemoveStorageButtonClick(object sender, EventArgs e)
		{
			RemoveStorage();
		}
		
		void HardDriveStorageToolStripMenuItem1Click(object sender, EventArgs e)
		{
			AddStorage(StorageEnum.Hdd);
		}
		
		void FtpStorageToolStripMenuItem1Click(object sender, EventArgs e)
		{
			AddStorage(StorageEnum.Ftp);
		}
		
		void NetworkStorageToolStripMenuItem1Click(object sender, EventArgs e)
		{
			AddStorage(StorageEnum.Network);
		}
		
		void StoragesContextMenuStripOpening(object sender, CancelEventArgs e)
		{
			modifyToolStripMenuItem.Enabled = (storagesListView.SelectedItems.Count > 0);
            toolStripSeparator4.Enabled = (storagesListView.SelectedItems.Count > 0);
            removeToolStripMenuItem.Enabled = (storagesListView.SelectedItems.Count > 0);
		}

		void RemoveStorage()
		{
			storagesListView.Items.Remove(storagesListView.SelectedItems[0]);
		}	

		void ModifyStorage()
		{
			ListViewItem storageToChangeListViewItem = storagesListView.SelectedItems[0];
			var storageToChange = (StorageBase)storageToChangeListViewItem.Tag;
            IStorageConfigurationForm configForm;

            if (storageToChange is HddStorage) 
            {
            	configForm = new HddStorageForm((HddStorage)storageToChange);
            }
            else if (storageToChange is FtpStorage) 
            {
            	configForm = new FtpStorageForm((FtpStorage)storageToChange);
            }
            else if (storageToChange is NetworkStorage) 
            {
            	configForm = new NetworkStorageForm((NetworkStorage)storageToChange);
            }
            else
            {
            	throw new NotImplementedException(storageToChange.GetType().Name);
            }

            if (configForm.ShowDialog() == DialogResult.OK)
            {
                StorageBase storage = configForm.Storage;

				storageToChangeListViewItem.Text = storage.StorageName;
				storageToChangeListViewItem.ToolTipText = storage.Hint;
				storageToChangeListViewItem.Tag = storage;
            }
		}
		
		void AddStorage(StorageEnum kind)
		{
			IStorageConfigurationForm configForm;
				
			switch (kind)
			{
				case StorageEnum.Hdd: 
					configForm = new HddStorageForm(null);
					break;
				case StorageEnum.Ftp: 
					configForm = new FtpStorageForm(null);
					break;
				case StorageEnum.Network: 
					configForm = new NetworkStorageForm(null);
					break;
				default: 
					throw new NotImplementedException(kind.ToString());
			}

			if (configForm.ShowDialog() == DialogResult.OK)
			{ 
				StorageBase storage = configForm.Storage;
				AddStorageToListView(storage, kind);
			}

			configForm.Dispose();
		}
		
		void AddStorageToListView(StorageBase storage, StorageEnum kind)
		{
			var listValue = new ListViewItem
			                    {
			                        Tag = storage,
			                        Group = storagesListView.Groups[(int) kind],
			                        Text = storage.StorageName,
			                        ImageIndex = (int) kind,
			                        ToolTipText = storage.Hint
			                    };

		    storagesListView.Items.Add(listValue);
		}
	}
}
