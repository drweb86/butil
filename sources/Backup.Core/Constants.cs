using System;

namespace BUtil.Core
{
	/// <summary>
	/// Contains general information about program behaviour
	/// </summary>
	public static class Constants
	{
		public const int MinimumPasswordLength = 20;
		public const int MaximumPasswordLength = 255;
		public const string ConsoleBackupTitle = "Console backup - BUtil";
		
        // Defaults for scheduler options. If change - please update documentation
        public const int DefaultHours = 23;
        public const int DefaultMinutes = 50;
        public const int DefaultCpuLoading = 60;
        public const int MinimumCpuLoading = 5;
        public const int MaximumCpuLoading = 95;
        
        // Defaults for parralel processing
        public const int AmountOfStoragesToProcessSynchronouslyMinimum = 1;
        public const int AmountOfStoragesToProcessSynchronouslyDefault = 5;
        public const int AmountOfStoragesToProcessSynchronouslyMaximum = 10;
	}
	
	internal enum SevenZipReturnCodes
	{
		OK = 0,
		ErrorsOccured = 1,
		FatalErrorsOccured = 2,
		InvalidArguments = 7,
		NotEnoughMemory = 8,
		ExternalTermination = 255
	}
}
