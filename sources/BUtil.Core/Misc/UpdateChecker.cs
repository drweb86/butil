using System;
using System.IO;
using System.Net.Http;
using System.Linq;

namespace BUtil.Core.Misc
{
	public static class UpdateChecker
	{
		public static bool CheckForUpdate(out string newVersion, out string changes)
		{
            try
			{
                using (var client = new HttpClient())
                {
                    var url = SupportManager.GetLink(SupportRequest.UpdateInfo);
                    string updateInfoContent = client.GetStringAsync(url).Result; // TODO: async!

                    var items = updateInfoContent.Split('\n');
                    newVersion = items[0];
                    changes = string.Join('\n', items.Skip(1));

					var remoteVersion = Version.Parse(newVersion);
                    return CopyrightInfo.Version < remoteVersion;
                }
			}
            catch (ArgumentNullException e) { throw new InvalidOperationException(e.Message, e); }
            catch (ArgumentException e) { throw new InvalidOperationException(e.Message, e); }
            catch (FormatException e) { throw new InvalidOperationException(e.Message, e); }
            catch (OverflowException e) { throw new InvalidOperationException(e.Message, e); }
            catch (IOException e) { throw new InvalidOperationException(e.Message, e); }
			catch (System.Security.SecurityException e) { throw new InvalidOperationException(e.Message, e); }
		}
	}
}