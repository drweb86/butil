using System;
using System.Xml;
using System.Globalization;
using System.IO;
using BUtil.Core.FileSystem;
using System.Net;

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
            var previous = ServicePointManager.SecurityProtocol;
            
            try
			{
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;

                using (WebClient client = new WebClient())
                {
                    var url = SupportManager.GetLink(SupportRequest.UpdateInfo);
                    string updateInfoContent = client.DownloadString(url);

                    var document = new XmlDocument();
                    document.LoadXml(updateInfoContent);

                    newVersion = document.SelectSingleNode("//LatestUpdate/Version").InnerText;
                    changes = document.SelectSingleNode("//LatestUpdate/Changes").InnerText.Replace(@"\n", Environment.NewLine);

                    double versionCurrent = double.Parse(CopyrightInfo.Version, CultureInfo.InvariantCulture);
                    double thatVersion = double.Parse(newVersion, CultureInfo.InvariantCulture);
                    return versionCurrent < thatVersion;
                }
			}
			catch(IOException e) { throw new InvalidOperationException(e.Message, e); }
			catch(XmlException e) { throw new InvalidOperationException(e.Message, e); }
			catch(System.Security.SecurityException e) { throw new InvalidOperationException(e.Message, e); }
			finally
			{
				ServicePointManager.SecurityProtocol = previous;

            }
		}
	}
}