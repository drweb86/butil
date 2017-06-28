using System;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Core.ButilImage;
using BULocalization;

namespace BUtil.RestorationMaster
{
	/// <summary>
	/// In this form user chooses the way how to restore selected data
	/// </summary>
	internal partial class HowToRestoreForm : Form
	{
		readonly MetaRecord _record;
		readonly RestorationMasterController _controller;

		public HowToRestoreForm(MetaRecord record, RestorationMasterController controller)
		{
			InitializeComponent();

			_record = record;
			_controller = controller;

			recoverItemTextBox.Text = _record.InitialTarget;
			this.Text = _record.InitialTarget;

			// locals
			restoreButton.Text = Translation.Current[438];
			HowToRestoregroupBox.Text = Translation.Current[439];
			toFolderRadioButton.Text = Translation.Current[440];
            helpToolTip.SetToolTip(toOriginalLocationRadioButton, Translation.Current[441]);
			toOriginalLocationRadioButton.Text = Translation.Current[442];
			helpToolTip.SetToolTip(to7zipArchiveRadioButton, Translation.Current[443]);
			to7zipArchiveRadioButton.Text = Translation.Current[444];
			skipButton.Text = Translation.Current[445];
			save7zipArchiveDialog.Filter = Translation.Current[446];
			fbDialog.Description = Translation.Current[447];
			this.Text = string.Format(Translation.Current[427], record.InitialTarget);
			restoreButton.Left = skipButton.Left - restoreButton.Width - 10;
			
			refreshRestoreButton();
			refreshUiOnSelectedRestorationType(RestoreType.ToPointedFolder);
		}
		
		void restoreButtonClick(object sender, EventArgs e)
		{
			if (to7zipArchiveRadioButton.Checked)
			{
				if (!string.IsNullOrEmpty(sevenZipDestinationArchiveLocationTextBox.Text))
				{
					restore(sevenZipDestinationArchiveLocationTextBox.Text, RestoreType.As7ZipArchive);
				}
			}
			else if (toFolderRadioButton.Checked)
			{
                if (!string.IsNullOrEmpty(specifiedFolderTextBox.Text))
                {
                	restore(specifiedFolderTextBox.Text, RestoreType.ToPointedFolder);
                }
			}
			else if (toOriginalLocationRadioButton.Checked)
			{
				restore(string.Empty, RestoreType.ToOriginal);
			}
		}
		
		void restore(string parameter, RestoreType restoreType)
		{
			restoreButton.Enabled = false;
			skipButton.Enabled = false;
			restorationProgressBar.Visible = true;
			
			restorationBackgroundWorker.RunWorkerAsync(new RestoreTaskInfo(_record, restoreType, parameter));
		}
		
		void refreshRestoreButton()
		{
			bool enabled = false;
			if (to7zipArchiveRadioButton.Checked)
			{
				if (!string.IsNullOrEmpty(sevenZipDestinationArchiveLocationTextBox.Text))
				{
					enabled = true;
				}
			}
			else if (toFolderRadioButton.Checked)
			{
                if (!string.IsNullOrEmpty(specifiedFolderTextBox.Text))
                {
                    enabled = true;
                }
			}
			else if (toOriginalLocationRadioButton.Checked)
			{
				enabled = true;
			}
			
			restoreButton.Enabled = enabled;
		}

        void chooseTargetFolderButtonClick(object sender, EventArgs e)
        {
            if (fbDialog.ShowDialog() == DialogResult.OK)
            {
                specifiedFolderTextBox.Text = fbDialog.SelectedPath;
                refreshRestoreButton();
            }
        }

        void refreshUiOnSelectedRestorationType(RestoreType kind)
        {
        	refreshRestoreButton();
        	specifiedFolderTextBox.Enabled = (kind == RestoreType.ToPointedFolder);
        	chooseFolderButton.Enabled = (kind == RestoreType.ToPointedFolder);
            
        	sevenZipDestinationArchiveLocationTextBox.Enabled = (kind == RestoreType.As7ZipArchive);
        	choose7zipArchiveDestinationLocationButton.Enabled = (kind == RestoreType.As7ZipArchive);
        }

        void to7zipArchiveRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            refreshUiOnSelectedRestorationType(RestoreType.As7ZipArchive);
        }

        void toFolderRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            refreshUiOnSelectedRestorationType(RestoreType.ToPointedFolder);
        }

        void ToOriginalLocationRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            refreshUiOnSelectedRestorationType(RestoreType.ToOriginal);
        }
		
		void chooseTarget7zipArchiveButtonClick(object sender, EventArgs e)
		{
			if (save7zipArchiveDialog.ShowDialog() == DialogResult.OK)
			{
				sevenZipDestinationArchiveLocationTextBox.Text = save7zipArchiveDialog.FileName;
				refreshRestoreButton();
			}
		}
		
		void RestorationBackgroundWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			_controller.RunRecovering((RestoreTaskInfo)e.Argument);
		}
		
		void RestorationBackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
