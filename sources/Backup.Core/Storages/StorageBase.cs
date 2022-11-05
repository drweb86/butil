using System;
using System.Collections.Generic;
using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{
    public abstract class StorageBase
	{
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
		
        #endregion

        public abstract void Open(LogBase log);
        public abstract string Upload(string sourceFile, string relativeFileName);
        public abstract string ReadAllText(string file);
        public abstract void Test();

	}
}
