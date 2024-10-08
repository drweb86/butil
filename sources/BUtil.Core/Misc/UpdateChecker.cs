
using BUtil.Core.FileSystem;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Core.Misc;

public static class UpdateChecker
{
    private static async Task<string> GetResponse(string url)
    {
        using HttpClient client = new();
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
        request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*");
        request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
        request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
        request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

        using var response = await client.SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        await using var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
        using var streamReader = new StreamReader(decompressedStream);
        return await streamReader.ReadToEndAsync().ConfigureAwait(false);
    }

    public static async Task<AppUpdateInfo> CheckForUpdateGithub()
    {
        try
        {
            var apiResponse = await GetResponse(ApplicationLinks.ApiLatestRelease);
            var release = JsonSerializer.Deserialize<GithubRelease>(apiResponse);
            var noUpdate = new AppUpdateInfo(false, null, null);

            if (release == null || !Version.TryParse(release.Version, out var version))
                return noUpdate;

            return new AppUpdateInfo(CopyrightInfo.Version < version, release.Version, release.Body
                .Replace("\\r", "\r")
                .Replace("\\n", "\n")
                .Replace("#", ""));
        }
        catch
        {
            return new AppUpdateInfo(false, null, null);
        }
    }

    public static async Task<AppUpdateInfo> CheckForUpdateSynology()
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
        catch 
        { 
            return new AppUpdateInfo(false, null, null); ;
        }
    }
}

class GithubRelease
{
    [JsonPropertyName("tag_name")]
    public string Version { get; set; } = null!;

    [JsonPropertyName("body")]
    public string Body { get; set; } = null!;
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