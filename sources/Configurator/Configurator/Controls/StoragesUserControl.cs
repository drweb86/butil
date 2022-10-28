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
		bool _hddStorageIsTheOneStorage;
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

			foreach (var storageSettings in _task.Storages)
            {
				StorageEnum kind;

				switch (storageSettings.ProviderName.ToLowerInvariant())
                {
                    case "hdd":
						kind = StorageEnum.Hdd;
						break;

                    case "ftp":
						kind = StorageEnum.Ftp;
						break;

					case "samba":
						kind = StorageEnum.Network;
						break;

                	default:
                		throw new NotImplementedException(storageSettings.ProviderName);
                }

				AddStorageToListView(storageSettings, kind);
            }
		}
		
		public override void GetOptionsFromUi()
		{
			_task.Storages.Clear();

			foreach (ListViewItem item in storagesListView.Items)
			{
				_task.Storages.Add((StorageSettings)item.Tag);
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
			if (_hddStorageIsTheOneStorage)
			{
				AddStorage(StorageEnum.Hdd);
			}
			else
			{
				addStorageContextMenuStrip.Show(addStorageButton, addStorageButton.Width / 2, addStorageButton.Height / 2);
			}
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
			var storageSettings = (StorageSettings)storageToChangeListViewItem.Tag;
            IStorageConfigurationForm configForm;

            switch (storageSettings.ProviderName.ToLowerInvariant())
            {
                case "hdd":
                    configForm = new HddStorageForm(storageSettings);
					break;
                case "ftp":
                    configForm = new FtpStorageForm(storageSettings);
					break;
                case "samba":
                    configForm = new NetworkStorageForm(storageSettings);
					break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageSettings));
            }

            if (configForm.ShowDialog() == DialogResult.OK)
            {
                var updatedStorageSettings = configForm.GetStorageSettings();

				storageToChangeListViewItem.Text = updatedStorageSettings.Name;
				storageToChangeListViewItem.Tag = updatedStorageSettings;
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
				var updatedStorageSettings = configForm.GetStorageSettings();
				AddStorageToListView(updatedStorageSettings, kind);
			}

			configForm.Dispose();
		}
		
		void AddStorageToListView(StorageSettings storageSettings, StorageEnum kind)
		{
			var listValue = new ListViewItem
			                    {
			                        Tag = storageSettings,
			                        Group = storagesListView.Groups[(int) kind],
			                        Text = storageSettings.Name,
			                        ImageIndex = (int) kind,
			                        
			                    };

		    storagesListView.Items.Add(listValue);
		}
		
		public void TurnInternetAndLocalNetworkFunctions(bool on)
		{
			_hddStorageIsTheOneStorage = !on;
			ftpStorageToolStripMenuItem1.Visible = on;
			networkStorageToolStripMenuItem1.Visible = on;
			ftpStorageToolStripMenuItem.Visible = on;
			networkStorageToolStripMenuItem.Visible = on;
		}
	}
}
