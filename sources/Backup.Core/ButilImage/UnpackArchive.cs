using System;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using BUtil.Core;
using BUtil.Core.Misc;
using BUtil.Core.ButilImage;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;

namespace BUtil.Core.ButilImage
{
	/// <summary>
	/// Description of ArchiveUnpacker.
	/// </summary>
	public class UnpackArchive
	{
		readonly UnpackParameters _parameters;
		
		public UnpackArchive(UnpackParameters parameters)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");

            _parameters = parameters;
		}
		
        /// <summary>
        /// Starts 7-zip and waits until it finishes its work
        /// </summary>
        /// <exception cref="FieldAccessException">Problems with 7-zip</exception>
		public void StartJob()
		{
			Process process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.FileName = Files.SevenZipGPacker;
            process.StartInfo.Arguments = _parameters.ToString();
			
			try
			{
				process.Start();
			}
			catch (System.ComponentModel.Win32Exception exc)
			{
				throw new FieldAccessException(string.Format(CultureInfo.InvariantCulture, Resources.CannotRun7ZgExeBecause0, exc.Message), exc);
			}

			process.WaitForExit();
		}
	}
}
