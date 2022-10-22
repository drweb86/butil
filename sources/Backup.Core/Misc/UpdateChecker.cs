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
            try
			{
                using (WebClient client = new WebClient())
                {
                    var url = SupportManager.GetLink(SupportRequest.UpdateInfo);
                    string updateInfoContent = client.DownloadString(url);

                    var document = new XmlDocument();
                    document.LoadXml(updateInfoContent);

                    newVersion = document.SelectSingleNode("//LatestUpdate/Version").InnerText;
                    changes = document.SelectSingleNode("//LatestUpdate/Changes").InnerText.Replace(@"\n", Environment.NewLine);

					var remoteVersion = Version.Parse(newVersion);
                    return CopyrightInfo.Version < remoteVersion;
                }
			}
            catch (ArgumentNullException e) { throw new InvalidOperationException(e.Message, e); }
            catch (ArgumentException e) { throw new InvalidOperationException(e.Message, e); }
            catch (FormatException e) { throw new InvalidOperationException(e.Message, e); }
            catch (OverflowException e) { throw new InvalidOperationException(e.Message, e); }
            catch (IOException e) { throw new InvalidOperationException(e.Message, e); }
			catch (XmlException e) { throw new InvalidOperationException(e.Message, e); }
			catch (System.Security.SecurityException e) { throw new InvalidOperationException(e.Message, e); }
		}
	}
}