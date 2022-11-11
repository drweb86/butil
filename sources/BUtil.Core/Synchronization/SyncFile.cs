using System;
using System.Threading;

namespace BUtil.Core.Synchronization
{
	/// <summary>
	/// Description of SyncFile.
	/// </summary>
	sealed public class SyncFile: IDisposable  
	{
        private const string _ALREADYSYNCED = "Already synced";
        private const string _NOTSETORNULL = "Not set or null";

		private Mutex _mutex = null;
        private string _mutexName = string.Empty;
        private string _fileName = string.Empty;
		
		public string FileName
		{
			get { return _fileName; }
		}

		/// <summary>
		/// Synccronizes temporary files and log files names between several copies of butil by FILE NAME(without path). In filename p-r '\' required
		/// </summary>
		/// <returns>true - if all's OK</returns>
		/// <exception cref="InvalidOperationException">mutex already exists</exception>
		/// <exception cref="ArgumentException"></exception>
		public bool TrySyncFile(string fileName)
		{
            if (_mutex != null) 
                throw new InvalidOperationException(_ALREADYSYNCED);

            if (string.IsNullOrEmpty(fileName))
				throw new ArgumentException(_NOTSETORNULL, "fileName");

            _fileName = fileName;
            _mutexName = fileName.Substring(FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
			
			try
			{
				_mutex = new Mutex(true, _mutexName);
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}

			return true;
		
		}
		
        ~SyncFile()
        {
			cleanResources();
        }

		private bool _disposed;
		public void Dispose ()
		{
			cleanResources();
			GC.SuppressFinalize(this);
		}

		private void cleanResources()
		{
			if (_mutex != null)
			{
				if (!_disposed)
				{
					_mutex.Close();
					_mutex = null;
					_disposed = true;
				}
			}
		}

	}
}
