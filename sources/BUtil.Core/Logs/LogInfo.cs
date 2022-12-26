using System;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Logs
{
	public class LogInfo
	{
		public string File { get; }
		public BackupResult Status { get; }
        public DateTime CreatedAt { get; }

        public LogInfo(string file)
		{
            File = file;
            CreatedAt = System.IO.File.GetCreationTime(File);

			try
			{
				string contents = System.IO.File.ReadAllText(File);
				if (contents.Contains(Files.SuccesfullBackupMarkInHtmlLog))
				{
					Status = BackupResult.Successfull;
				}
				else if (contents.Contains(Files.ErroneousBackupMarkInHtmlLog))
				{
					Status = BackupResult.Erroneous;
				}
				else
				{
					Status = BackupResult.Unknown;
				}
			}
			catch
			{
				Status = BackupResult.Unknown;
			}
		}
	}
}
