using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace BUtil.Core.Synchronization
{
	/// <summary>
	/// Synchronizes file names(without pathes) between applications and inside each application
	/// </summary>
	public class SyncedFiles: IDisposable
	{
		private Collection<SyncFile> _syncedFileList = new Collection<SyncFile>();

		#region Dispose

		private bool _isDisposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool normalMode)
		{
			if (!normalMode)
				if (_syncedFileList == null) return;

			if (!_isDisposed)
			{ 
				foreach (SyncFile file in _syncedFileList)
				{
					file.Dispose();
				}
				_isDisposed = true;
			}
		}

		~SyncedFiles()
		{
			Dispose(false);
		}

		#endregion

		public Collection<SyncFile> SynchronizedFiles
		{
            get { return _syncedFileList; }
		}
		
		private void addSyncedFile(SyncFile newSyncedFile)
		{
            _syncedFileList.Add(newSyncedFile);
		}
		
		public bool TryRegisterNewName(string fileName)
		{
            foreach (SyncFile file in _syncedFileList)
            {
                if (file.FileName == fileName) return false;
            }

			SyncFile sf = new SyncFile();

            if (sf.TrySyncFile(fileName)) addSyncedFile(sf);
                else return false;
			
			return true;
		}
		
	}
}
