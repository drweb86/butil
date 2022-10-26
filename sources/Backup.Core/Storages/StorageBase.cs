using System;
using System.Collections.Generic;
using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{
	public abstract class StorageBase
	{
        private bool _demandsSecurity;
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
		
		public bool RequireSecurity
		{
            get { return _demandsSecurity; }
            protected set { _demandsSecurity = value; }
        }

        #endregion

        protected StorageBase(string storageName, bool shouldBeSecure)
		{
            StorageName = storageName;
            _demandsSecurity = shouldBeSecure;
		}

        /// <summary>
        /// Here you should set Log property
        /// </summary>
        /// <param name="log"></param>
        public abstract void Open(LogBase log);
        public abstract void Process(string file);
        public abstract void Test();
        
        public abstract Dictionary<string, string> SaveSettings();

        /// <summary>
        /// Gets hint for showing in Storage places in Configurator
        /// </summary>
        /// <returns>hint</returns>
        public abstract string Hint
        {
            get;
        }

	}
}
