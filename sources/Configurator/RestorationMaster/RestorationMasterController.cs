using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BUtil.Core.ButilImage;
using BUtil.Core.Synchronization;
using BUtil.Core;
using BUtil.Core.FileSystem;


namespace BUtil.RestorationMaster
{
	internal class RestorationMasterController
	{
		private const string _IMAGELOCATION = "ImageLocation";

		private string _password = string.Empty;
		private string _imageLocation = string.Empty;
		
		private ImageHeader _metaData;

		private ImageReader _imageReader = new();

		private bool ImageDataIsPasswordProtected
		{
			get { return !string.IsNullOrEmpty(_password); }
		}

		public Collection<MetaRecord> MetaRecords
		{
			get { return _metaData.Records; }
		}
		/// <summary>
		/// Password. Can be null if image is not password protected
		/// </summary>
		public string Password
		{
			set { _password = value; }
			private get { return _password; }
		}

		/// <summary>
		/// Should never be null or empty
		/// </summary>
		public string ImageLocation
		{
			get { return _imageLocation; }
			set 
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException(_IMAGELOCATION);

				_imageLocation = value; 
			}
		}

		public void OpenImage()
		{
			_imageReader = new ImageReader
			{
				FileName = ImageLocation
			};

			try
            {
                _metaData = _imageReader.Open();
            }
            catch (InvalidDataException e)
            {
                MessageBox.Show(e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
                return;
            }

			using var restoreForm = new RestoreForm(this);
			restoreForm.ShowDialog();
		}

        public void RunRecovering(RestoreTaskInfo task)
        {
        	switch (task.RestorationType)
			{
				case RestoreType.As7ZipArchive: 
        			RecoveryManager.RestoreAs7ZipArchiveHelper(_imageReader, task.Record, task.Parameter); 
        			break;
				case RestoreType.ToPointedFolder: 
        			RecoveryManager.RestoreToPointedFolderHelper(_imageReader, task.Record, task.Parameter, this.ImageDataIsPasswordProtected, this.Password); 
        			break;
				case RestoreType.ToOriginal: 
        			RecoveryManager.RestoreToOriginalLocation(_imageReader, task.Record, this.ImageDataIsPasswordProtected, this.Password); 
        			break;
			}
        }
	}
}
