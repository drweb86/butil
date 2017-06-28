using System;
using System.Xml;
using System.Globalization;
using System.IO;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Misc
{
	/// <summary>
	/// Checks for updates this tool
	/// </summary>
	public static class UpdateChecker
	{
		/// <summary>
		/// Connects with internet and searches for the new version
		/// </summary>
		/// <param name="newVersion">latest version</param>
		/// <param name="changes">latest changes</param>
		/// <returns>true if new version is available</returns>
		/// <exception cref="InvalidOperationException">Any problems</exception>
		public static bool CheckForUpdate(out string newVersion, out string changes)
		{
			try
			{
				XmlDocument document = new XmlDocument();
        
				document.Load(Files.UpdateUrlXml);

				newVersion = document.SelectSingleNode("//xml/version").InnerText;
				changes = document.SelectSingleNode("//xml/changes").InnerText.Replace(@"\n", Environment.NewLine);
		
				double versionCurrent = double.Parse(CopyrightInfo.Version, CultureInfo.InvariantCulture);
				double thatVersion = double.Parse(newVersion, CultureInfo.InvariantCulture);
				return versionCurrent < thatVersion;
			}
			catch(IOException e) { throw new InvalidOperationException(e.Message, e); }
			catch(XmlException e) { throw new InvalidOperationException(e.Message, e); }
			catch(System.Security.SecurityException e) { throw new InvalidOperationException(e.Message, e); }
		}
	}
}