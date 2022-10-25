using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Core.FileSystem;
using BUtil.Core;
using BUtil.Core.Misc;
using BUtil.Core.PL;
using BUtil.Configurator.Localization;

namespace BUtil.RestorationMaster
{
	/// <summary>
	/// Main form of an application
	/// </summary>
	public partial class RestoreMasterMainForm : Form
	{
		const string _oneParameterExpected = "Only one parameter - image file is excepted";

		RestorationMasterController _controller = new RestorationMasterController();
		
		public string ImageFileToShow
		{
			get { return imageLocationTextBox.Text; }
			set { setImageLocationHelper(value); }
		}
		
		public RestoreMasterMainForm()
		{
			InitializeComponent();
            
			if (BUtil.Configurator.Program.PackageIsBroken)
			{
				throw new InvalidOperationException("Tried to perform operation that requires package state is ok.");
			}

            applyLocals();
            setImageLocationHelper(string.Empty);
		}

        void applyLocals()
        { 
            openImageButton.Text = Resources.OpenAnImage;
			ofd.Filter = Resources.ButilImagesButilButil;
			closeButton.Text = Resources.Close;
			passwordGroupBox.Text = Resources.Password;
			passwordLabel.Text = Resources.IfYourBackupIsPasswordProtectedPleaseTypePasswordHere;
			continueButton.Text = Resources.Continue;
			imageLocationLabel.Text = Resources.ImageLocation;
			passwordHintLabel.Text = Resources.NoteForRestorationOfDataAsArchivesPasswordIsNotNeeded;
			this.Text = Resources.RestorationMaster;
			continueButton.Left = closeButton.Left - continueButton.Width - 10;
        }

		void setImageLocationHelper(string imageLocation)
		{
			imageLocationTextBox.Text = string.Empty;
			bool enabled = !string.IsNullOrEmpty(imageLocation);

			continueButton.Enabled = enabled;
			passwordGroupBox.Enabled = enabled;

			if (enabled)
			{
				imageLocationTextBox.Text = imageLocation;
			}
		}

		void openImageButtonClick(object sender, EventArgs e)
		{
			if (ofd.ShowDialog() == DialogResult.OK)
				setImageLocationHelper(ofd.FileName);
		}
		
        void closeButtonClick(object sender, EventArgs e)
        {
        	DialogResult = DialogResult.OK;
        	Close();
        }

		void nextButtonClick(object sender, EventArgs e)
		{
			_controller.Password = passwordMaskedTextBox.Text;
			_controller.ImageLocation = imageLocationTextBox.Text;

			_controller.OpenImage();
		}

		void helpButtonClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.RestorationWizard);
        }
	}
}
