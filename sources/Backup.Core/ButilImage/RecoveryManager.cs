using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BUtil.Core.Synchronization;
using BUtil.Core.FileSystem;

namespace BUtil.Core.ButilImage
{
	public static class RecoveryManager
	{
		public static void RestoreAs7ZipArchiveHelper(ImageReader openedReader, MetaRecord record, string targetFilename)
		{
			try
			{
				openedReader.Extract(record, targetFilename);
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
			}
		}

		public static void RestoreToPointedFolderHelper(ImageReader openedReader, MetaRecord record, string pointedFolder, bool imageDataIsPasswordProtected, string password)
		{
			try
			{
				string tempFile = Path.GetTempFileName();

					UnpackParameters unpackParameters;
					if (imageDataIsPasswordProtected)
						unpackParameters = new UnpackParameters(password, tempFile, pointedFolder);
					else
						unpackParameters = new UnpackParameters(tempFile, pointedFolder);

					try
					{
						openedReader.Extract(record, tempFile);

						UnpackArchive aup = new UnpackArchive(unpackParameters);
						aup.StartJob();
					}
					finally
					{
						if (imageDataIsPasswordProtected)
							BackupProcess.OverwriteFileWithZerosWithoutLogging(tempFile);

						File.Delete(tempFile);
					}
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
			}
		}

		public static void RestoreToOriginalLocation(ImageReader openedReader, MetaRecord record, bool imageDataIsPasswordProtected, string password)
		{
            string folderThatHasThisFile = record.InitialTarget.Substring(0, record.InitialTarget.LastIndexOf("\\", StringComparison.InvariantCulture));
			RestoreToPointedFolderHelper(openedReader, record, folderThatHasThisFile, imageDataIsPasswordProtected, password);
		}
	}
}
