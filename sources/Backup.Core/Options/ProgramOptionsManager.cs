using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using BUtil.Core.Logs;
using BUtil.Core.FileSystem;
using BUtil.Core.Storages;
using BUtil.Core.ButilImage;

//									RELOCALIZE!
namespace BUtil.Core.Options
{
	/// <summary>
	/// Manages the profile options
	/// </summary>
    public static class ProgramOptionsManager
	{
        /// <summary>
        /// Validates password and throws exception if something is bad
        /// </summary>
        /// <param name="lengthCheck">Do the check on length or not</param>
        /// <param name="password">The password to check</param>
        /// <exception cref="ArgumentException">The password is invalid</exception>
        public static void ValidatePassword(bool lengthCheck, string password)
        {
            if (string.IsNullOrEmpty(password))
				throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, "Password is empty"));
				
            if (password.Contains(" "))
				throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, "Password contains blank"));
      
			if (lengthCheck && ( password.Length < Constants.MinimumPasswordLength || password.Length > Constants.MaximumPasswordLength))
                throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, "Password size is out of range [{0}..{1}]", Constants.MinimumPasswordLength, Constants.MaximumPasswordLength));
        }
		
		/// <summary>
		/// Stores the settings
		/// </summary>
		/// <param name="options">The object to store</param>
		public static void StoreSettings(ProgramOptions options)
		{
			ProgramOptionsKeeper.StoreSettings(options);
		}
	
		public static ProgramOptions LoadSettings()
		{
            return ProgramOptionsKeeper.LoadSettings(); 
		}

        public static BackupTask GetDefaultBackupTask(string name)
        {
            BackupTask task = new BackupTask();
            task.Name = name;

            SourceItem item = new SourceItem();
            item.CompressionDegree = CompressionDegree.Normal;
            item.IsFolder = true;
            item.Target = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            task.Items.Add(item);

            item = new SourceItem();
            item.CompressionDegree = CompressionDegree.Normal;
            item.IsFolder = true;
            item.Target = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            task.Items.Add(item);

            return task;
        }

		public static ProgramOptions Default
		{
			get 
			{
				return new ProgramOptions
				{
					Priority = System.Threading.ThreadPriority.BelowNormal,
					LogsFolder = Directories.LogsFolder,
					PuttingOffBackupCpuLoading = Constants.DefaultCpuLoading,
					LoggingLevel = LogLevel.Normal,
					AmountOfStoragesToProcessSynchronously = Constants.AmountOfStoragesToProcessSynchronouslyDefault,
					AmountOf7ZipProcessesToProcessSynchronously = Environment.ProcessorCount
				};
			}
		}
	}
}
