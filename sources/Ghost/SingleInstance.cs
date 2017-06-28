using System;
using System.Threading;
using BUtil.Core;

namespace BUtil.Ghost
{
	/// <summary>
	/// Enables single instance functional in scheduler for each user
	/// </summary>
    internal static class SingleInstance
    {
    	#region Internal Fields
    	
        static readonly string _name = "Backup.Ghost" + CopyrightInfo.Version + Environment.UserDomainName;
        static bool _firstInstance;
        static Mutex _mutex;
        
        #endregion

        #region Public Properties
        
        /// <summary>
        /// Shows if the current instance of ghost is the first
        /// </summary>
        public static bool FirstInstance
        {
            get { return _firstInstance; }
        }

		#endregion
        
		#region Public Methods
		
		/// <summary>
		/// Static constructor
		/// </summary>
        static SingleInstance()
        {
            try
            {
                //Grab mutex if it exists
                _mutex = Mutex.OpenExisting(_name);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                _firstInstance = true;
            }

            if (_mutex == null)
            {
                _mutex = new Mutex(false, _name);
            }
        }
        
        #endregion
    }
}
