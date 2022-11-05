using System;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.ButilImage;


namespace BUtil.RestorationMaster
{
	/// <summary>
	/// In this form user chooses the way how to restore selected data
	/// </summary>
	internal partial class HowToRestoreForm : Form
	{
		readonly MetaRecord _record;
		readonly RestoreController _controller;

		public HowToRestoreForm(MetaRecord record, RestoreController controller)
		{
			InitializeComponent();

			_record = record;
			_controller = controller;

			recoverItemTextBox.Text = _record.InitialTarget;
			this.Text = _record.InitialTarget;

			// locals
			restoreButton.Text = Resources.Restore;
			HowToRestoregroupBox.Text = Resources.HowToRestoreYourInformation;
			toFolderRadioButton.Text = Resources.ToSpecifiedFolder;
            helpToolTip.SetToolTip(toOriginalLocationRadioButton, Resources.RestoresAllDataToTheSourceFolderItWillRestorePreviousVersionOfFilesFromABackupArchiveUseItWhenNOriginalLocationEntirelyDamagedAndNThereWereNoErrorsDuringCreatingThisBackupNNnotRecommendedCanDamageExistingData);
			toOriginalLocationRadioButton.Text = Resources.ToOriginalLocation;
			helpToolTip.SetToolTip(to7zipArchiveRadioButton, Resources.RestorationOfInformationAsArchiveIsAnEfficientWayWhenYouWantToNRestoreSmallPiecesOfInformationFromALargeBackuppedFolderSavesTimeNAllDataInArchiveIsEncryptedIfYouUsedPasswordMoreSecureNhoweverFresh7ZipInstallationPackageIsRequiredToOpenExtractedArchive);
			to7zipArchiveRadioButton.Text = Resources.RestoreAs7ZipArchive;
			skipButton.Text = Resources.Skip;
			save7zipArchiveDialog.Filter = Resources._7ZipArchive7Z;
			fbDialog.Description = Resources.ChooseDestinationLocation;
			this.Text = string.Format(Resources._0Restoration, record.InitialTarget);
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
