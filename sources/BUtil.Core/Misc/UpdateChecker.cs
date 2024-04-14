
using BUtil.Core.FileSystem;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BUtil.Core.Misc;

public static class UpdateChecker
{
    public static async Task<AppUpdateInfo> CheckForUpdate()
    {
        try
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

#if DEBUG
            var path = Path.Combine(Directories.BinariesDir, @"..\..\..\..\web\latest.json");
            using var releasesStream = File.OpenRead(path);
#else
            await using var releasesStream = await client.GetStreamAsync("https://drweb86.synology.me:88/latest.json");
#endif

            var release = await JsonSerializer.DeserializeAsync<AppRelease>(releasesStream);
            var noUpdate = new AppUpdateInfo(false, null, null);

            if (release == null || !Version.TryParse(release.Version, out var version))
                return noUpdate;

            return new AppUpdateInfo(CopyrightInfo.Version < version, release.Version, release.Changes
                .Replace("\\r", "\r")
                .Replace("\\n", "\n")
                .Replace("#", ""));
        }
        catch (Exception e) { throw new InvalidOperationException(e.Message, e); }
    }
}

internal record class AppRelease(
    string Version,
    string Changes
);

public record class AppUpdateInfo(
    bool HasUpdate,
    string? Version,
    string? Changes
);