using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using BUtil.Core.Options;
using BUtil.Core.ButilImage;
using BUtil.Core;
using System.Diagnostics;
using BUtil.Configurator.Localization;
using System.Linq;

namespace BUtil.Configurator
{
	/// <summary>
	/// Class for separaring GUI logic and business logic in showing
	/// compression item components
	/// </summary>
	public class CompressionItemsKeeper
	{
		readonly ListView _listView;
		readonly List<SourceItem> _items;

		/// <summary>
		/// Warning: this class should be inited with locals before loading!!!
		/// </summary>
		public CompressionItemsKeeper(ListView listView, List<SourceItem> items)
		{
			if (listView == null)
				throw new ArgumentNullException("listView");

			if (items == null)
				throw new ArgumentNullException("items");
			
			_listView = listView;
			_items = items;
		}

		public void ApplyNewDegreesOfCompression()
		{
			_listView.BeginUpdate();

			foreach (CompressionDegree degree in CompressionDegree.GetValues(typeof(CompressionDegree)))
				_listView.Groups[(int)degree].Header = LocalsHelper.ToString(degree);

			_listView.EndUpdate();
		}

		public void InitWith()
		{
			foreach (SourceItem item in _items)
			{
				addItem(item, false);
			}
		}

		private void updateGuiControlHelper()
		{
			_listView.BeginUpdate();

			_listView.Items.Clear();
			foreach (SourceItem item in _items)
			{
				ListViewItem newlistViewItem = new ListViewItem(item.Target);
				newlistViewItem.ImageIndex = item.IsFolder ? 0 : 1;
				newlistViewItem.Group = _listView.Groups[(int)item.CompressionDegree];

				_listView.Items.Add(newlistViewItem);

			}
			_listView.EndUpdate();
		}

		public void UpdateCompressionLevelForSelectedItems(int newCompressionLevel)
		{
			foreach (int index in _listView.SelectedIndices)
			{
				_items[index].CompressionDegree = (CompressionDegree)newCompressionLevel;
			}
			updateGuiControlHelper();
		}

		void addItem(SourceItem newItem, bool add)
		{
			if (newItem.Target.StartsWith(@"\\", StringComparison.InvariantCulture))
			{
				// "Network places are not allowed to be added to the list of backup items!"
				MessageBox.Show(Resources.NetworkPlacesAreNotAllowedToBeAddedToTheListOfBackupItems, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
				return;
			}
			
			if (add)
			{
				_items.Add(newItem);
			}

			_listView.BeginUpdate();

			ListViewItem newlistViewItem = new ListViewItem(newItem.Target);
			if (newItem.IsFolder)
			{
				newlistViewItem.ImageIndex = 0;
			}
			else
			{
				newlistViewItem.ImageIndex = 1;
			}

			newlistViewItem.Group = _listView.Groups[(int)newItem.CompressionDegree];

			_listView.Items.Add(newlistViewItem);

			_listView.EndUpdate();
		}

        public void OpenItem(int itemIndex)
        {
            if (_items[itemIndex].IsFolder)
            {
                Process.Start(_items[itemIndex].Target);
            }
        }

		public void RemoveItems(ListView.SelectedIndexCollection indexes)
		{
            for (int i = (indexes.Count - 1); i > -1; i-- )
            {
                _items.RemoveAt(indexes[i]);
            }
			updateGuiControlHelper();
		}

		public void RemoveItem(int index)
		{
			_items.RemoveAt(index);
			updateGuiControlHelper();
		}

		public void VerifyNewItem(SourceItem newItem)
		{
			VerifyNewItems(new SourceItem[] {newItem});
		}
		/// <summary>
		/// Required: all files or folders should be added from one folder level.
		/// </summary>
		public void VerifyNewItems(SourceItem[] newItems)
		{
			bool AddFlag;
			string itemrepresentation;
			string fflist;

			if (newItems.Length == 0) return;

			if (_items.Count == 0)
			{
				for (int i = 0; i < newItems.Length; i++) addItem(newItems[i], true);
				return;
			}

			// Checking whether more global things added
			for (int itemid = 0; itemid < newItems.Length; itemid++)
			{
				itemrepresentation = newItems[itemid].Target;
				if (itemrepresentation.Length == 0) continue;

				int i = 0;
				while (i < _items.Count)
				{

					fflist = _items[i].Target;

					// Winfows
					if (fflist.StartsWith(itemrepresentation + "\\", StringComparison.InvariantCulture))
					{
						RemoveItem(i);
						continue;
					}

					// Linux, Unix, Networks
					if (fflist.StartsWith(itemrepresentation + "/", StringComparison.InvariantCulture))
					{
						RemoveItem(i);
						continue;
					}
					i++;
				}
			}

			for (int itemid = 0; itemid < newItems.Length; itemid++)
			{
				AddFlag = true;
				itemrepresentation = newItems[itemid].Target;
				if (string.IsNullOrEmpty(itemrepresentation)) 
                    continue;
				for (int i = 0; i < _items.Count; i++)
				{
					fflist = _items[i].Target;

					if (string.IsNullOrEmpty(fflist)) 
                        continue;
					// repeatings are not allowed
					if (fflist == itemrepresentation)
					{
						AddFlag = false;
						break;
					};
					// inside-folder files aten't allowed
					// win specific
					if (itemrepresentation.StartsWith(fflist + "\\", StringComparison.InvariantCulture))
					{
						AddFlag = false;
						break;
					};

					// inside-folder files aten't allowed
					// linux and networks specific
					if (itemrepresentation.StartsWith(fflist + "/", StringComparison.InvariantCulture))
					{
						AddFlag = false;
						break;
					};

					// D:\ d:/ and so on
					if ((fflist.EndsWith("\\", StringComparison.InvariantCulture) || fflist.EndsWith("/", StringComparison.InvariantCulture)) && itemrepresentation.StartsWith(fflist))
					{
						AddFlag = false;
						break;
					};
				}

				if (AddFlag) 
					addItem(newItems[itemid], true);
			}
		}
	}
}
