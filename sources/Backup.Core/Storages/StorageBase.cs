using System;
using System.Collections.Generic;
using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{
    public abstract class StorageBase
	{
        private string _StorageName;
		private LogBase _Log;

        #region Properties

        protected LogBase Log
        {
            get { return _Log; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("Log");

                _Log = value;
            }
        }
        
		public string StorageName
		{
            get { return _StorageName; }
			set 
			{
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("StorageName");

                _StorageName = value;
			}
		}
		
        #endregion

        protected StorageBase(string storageName)
		{
            StorageName = storageName;
        }

        /// <summary>
        /// Here you should set Log property
        /// </summary>
        /// <param name="log"></param>
        public abstract void Open(LogBase log);
        public abstract void Put(string file, string directory = null);
        public abstract void StoreFiles(string sourceDir, List<string> sourceFiles, string directory = null);
        public abstract byte[] ReadFile(string file);
        public abstract void Test();

	}
}
