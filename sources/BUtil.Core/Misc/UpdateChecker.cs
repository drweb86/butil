
using System;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace BUtil.Core.Misc
{
	public static class UpdateChecker
	{
		public static async Task<GithubUpdateInfo> CheckForUpdate()
		{
            try
			{
                using HttpClient client = new();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
                
                await using var releasesStream = await client.GetStreamAsync("https://api.github.com/repos/drweb86/butil/releases");
                var releases = await JsonSerializer.DeserializeAsync<List<GithubRelease>>(releasesStream);
                var noUpdate = new GithubUpdateInfo(false, null, null);

                if (releases == null)
                    return noUpdate;

                var updatedRelease = releases
                    .Where(x => !x.Prerelease && !x.Draft)
                    .FirstOrDefault(x => Version.TryParse(x.Tag, out var version) && CopyrightInfo.Version < version);

                if (updatedRelease == null)
                    return noUpdate;

                await using var versionRelease = await client.GetStreamAsync(updatedRelease.ApiUrl);
                var release = await JsonSerializer.DeserializeAsync<GithubReleaseV2>(versionRelease);
                if (release == null)
                    return noUpdate;

                return new GithubUpdateInfo(true, updatedRelease.Tag, release.Markdown
                    .Replace("\\r", "\r")
                    .Replace("\\n", "\n")
                    .Replace("#", ""));
            }
            catch (ArgumentNullException e) { throw new InvalidOperationException(e.Message, e); }
            catch (ArgumentException e) { throw new InvalidOperationException(e.Message, e); }
            catch (FormatException e) { throw new InvalidOperationException(e.Message, e); }
            catch (OverflowException e) { throw new InvalidOperationException(e.Message, e); }
            catch (IOException e) { throw new InvalidOperationException(e.Message, e); }
			catch (System.Security.SecurityException e) { throw new InvalidOperationException(e.Message, e); }
		}
	}

    public record class GithubRelease(
        [property: JsonPropertyName("id")] long Id,
        [property: JsonPropertyName("url")] Uri ApiUrl,
        [property: JsonPropertyName("tag_name")] string Tag,
        [property: JsonPropertyName("draft")] bool Draft,
        [property: JsonPropertyName("prerelease")] bool Prerelease
    );

    public record class GithubReleaseV2(
       [property: JsonPropertyName("body")] string Markdown
   );

    public record class GithubUpdateInfo(
        bool HasUpdate,
        string? Version,
        string? Changes
    );
}