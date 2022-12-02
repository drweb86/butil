using System;
using System.ComponentModel;
using System.Windows.Forms;
using BUtil.Core.Storages;
using BUtil.Core.Options;
using BUtil.Configurator.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Configurator.Configurator.Controls
{
    internal sealed partial class WhereUserControl : Core.PL.BackUserControl
	{
		BackupTask _task;
		
		public WhereUserControl()
		{
			InitializeComponent();
		}
		
		#region Overrides
		
		public override void ApplyLocalization() 
		{
			hardDriveStorageToolStripMenuItem1.Text = Resources.HardDriveStorage;
			storagesListView.Groups[0].Header = Resources.Folder;
            storagesListView.Groups[2].Header = Resources.Ftp;
            SetHintForControl(addStorageButton, Resources.Add);
			removeToolStripMenuItem.Text = Resources.RemoveFromList;
			modifyToolStripMenuItem.Text = Resources.Modify;
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

                if (storageSettings is FolderStorageSettings)
                    kind = StorageEnum.Folder;
				else if (storageSettings is SambaStorageSettings)
                    kind = StorageEnum.Samba;
                else
                    throw new ArgumentOutOfRangeException(nameof(storageSettings));

				AddStorageToListView(storageSettings, kind);
            }
		}
		
		public override void GetOptionsFromUi()
		{
			_task.Storages.Clear();

			foreach (ListViewItem item in storagesListView.Items)
			{
				_task.Storages.Add((IStorageSettings)item.Tag);
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
			AddStorage(StorageEnum.Folder);
		}

		public override bool ValidateUi()
		{
			if (storagesListView.Items.Count == 0)
			{
				Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.PleaseAddAtLeastOneDestinationPlace);
				return false;
			}

			return true;
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
			var storageSettings = (IStorageSettings)storageToChangeListViewItem.Tag;
            IStorageConfigurationForm configForm;

			var forbiddenNames = GetNames()
							.Except(new List<string> { storageSettings.Name })
							.ToList();

            if (storageSettings is FolderStorageSettings)
                configForm = new FolderStorageForm(
                        storageSettings as FolderStorageSettings,
                        forbiddenNames);
			else if (storageSettings is SambaStorageSettings)
                configForm = new SambaStorageForm(
                        storageSettings as SambaStorageSettings,
                        forbiddenNames);
            else
                throw new ArgumentOutOfRangeException(nameof(storageSettings));

            if (configForm.ShowDialog() == DialogResult.OK)
            {
                var updatedStorageSettings = configForm.GetStorageSettings();

				storageToChangeListViewItem.Text = updatedStorageSettings.Name;
				storageToChangeListViewItem.Tag = updatedStorageSettings;
            }
		}

        private IEnumerable<string> GetNames()
        {
            var names = new List<string>();
            foreach (ListViewItem task in storagesListView.Items)
            {
                names.Add(task.Text);
            }
            return names;
        }

        void AddStorage(StorageEnum kind)
		{
			IStorageConfigurationForm configForm;
				
			switch (kind)
			{
				case StorageEnum.Folder: 
					configForm = new FolderStorageForm(null, GetNames());
					break;
                case StorageEnum.Samba:
                    configForm = new SambaStorageForm(null, GetNames());
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
		
		void AddStorageToListView(IStorageSettings storageSettings, StorageEnum kind)
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

		private void OnAddSamba(object sender, EventArgs e)
		{
            AddStorage(StorageEnum.Samba);
		}
    }
}
