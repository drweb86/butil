
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
                
                // releases endpoint is broken in github now. it returns invalid result (previous release).
                await using var releasesStream = await client.GetStreamAsync("https://api.github.com/repos/drweb86/butil/releases/latest");
                var release = await JsonSerializer.DeserializeAsync<GithubRelease>(releasesStream);
                var noUpdate = new GithubUpdateInfo(false, null, null);

                if (release == null || release.Prerelease || release.Draft || !Version.TryParse(release.Tag, out var version))
                    return noUpdate;

                return new GithubUpdateInfo(true, release.Tag, release.Markdown
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
        [property: JsonPropertyName("tag_name")] string Tag,
        [property: JsonPropertyName("draft")] bool Draft,
        [property: JsonPropertyName("prerelease")] bool Prerelease,
        [property: JsonPropertyName("body")] string Markdown
    );

    public record class GithubUpdateInfo(
        bool HasUpdate,
        string? Version,
        string? Changes
    );
}