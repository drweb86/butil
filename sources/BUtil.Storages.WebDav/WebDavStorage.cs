using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WebDav;

namespace BUtil.Storages.WebDav;

class WebDavStorage : StorageBase<WebDavStorageSettingsV2>
{
    private readonly WebDavClient _client;
    private readonly string _basePath;

    internal WebDavStorage(ILog log, WebDavStorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.Host))
            throw new InvalidDataException("Host is not specified.");

        var isOAuth = Settings.Preset == "YandexDisk";
        if (!isOAuth && string.IsNullOrWhiteSpace(Settings.User))
            throw new InvalidDataException("User is not specified.");

        var scheme = Settings.UseHttps ? "https" : "http";
        var portPart = string.Empty;
        if (Settings.Port > 0)
        {
            bool isDefault = (Settings.UseHttps && Settings.Port == 443) || (!Settings.UseHttps && Settings.Port == 80);
            if (!isDefault)
                portPart = $":{Settings.Port}";
        }

        _basePath = "/" + Settings.BasePath.Trim('/');

        var httpClient = new HttpClient { BaseAddress = new Uri($"{scheme}://{Settings.Host}{portPart}") };
        if (isOAuth)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", Settings.Password);
        else
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Settings.User}:{Settings.Password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }

        _client = new WebDavClient(httpClient);
    }

    private string GetUri(string? relativePath)
    {
        var prefix = _basePath.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(relativePath))
            return prefix + "/";
        var normalized = relativePath.Replace('\\', '/').Trim('/');
        var encoded = string.Join("/", normalized.Split('/').Select(Uri.EscapeDataString));
        return $"{prefix}/{encoded}";
    }

    private string ToRelativePath(string? resourceUri)
    {
        if (string.IsNullOrEmpty(resourceUri)) return string.Empty;
        string path;
        if (Uri.TryCreate(resourceUri, UriKind.Absolute, out var uri))
            path = uri.AbsolutePath;
        else
            path = resourceUri;

        path = Uri.UnescapeDataString(path);

        var prefix = _basePath.TrimEnd('/');
        if (!string.IsNullOrEmpty(prefix) && path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            path = path[prefix.Length..];

        return path.Trim('/');
    }

    public override string? Test()
    {
        try
        {
            var result = _client.Propfind(GetUri(null), new PropfindParameters
            {
                ApplyTo = global::WebDav.ApplyTo.Propfind.ResourceOnly,
            }).GetAwaiter().GetResult();
            return result.IsSuccessful ? null : $"WebDAV test failed (HTTP {result.StatusCode})";
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public override bool Exists(string relativeFileName)
    {
        try
        {
            var result = _client.Propfind(GetUri(relativeFileName), new PropfindParameters
            {
                ApplyTo = global::WebDav.ApplyTo.Propfind.ResourceOnly,
            }).GetAwaiter().GetResult();
            return result.IsSuccessful && result.Resources.Count > 0;
        }
        catch
        {
            return false;
        }
    }

    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        var dirPart = relativeFileName.Replace('\\', '/').Contains('/')
            ? string.Join("/", relativeFileName.Replace('\\', '/').Split('/').SkipLast(1))
            : null;
        if (!string.IsNullOrEmpty(dirPart))
            EnsureDirectoryExists(dirPart);

        if (Exists(relativeFileName))
            Delete(relativeFileName);

        using var fs = File.OpenRead(sourceFile);
        var result = _client.PutFile(GetUri(relativeFileName), fs).GetAwaiter().GetResult();
        if (!result.IsSuccessful)
            throw new InvalidOperationException($"Upload failed (HTTP {result.StatusCode}): {relativeFileName}");

        return new IStorageUploadResult
        {
            StorageFileName = relativeFileName,
            StorageFileNameSize = new FileInfo(sourceFile).Length,
        };
    }

    public override void Download(string relativeFileName, string targetFileName)
    {
        var targetFolder = Path.GetDirectoryName(targetFileName);
        if (!string.IsNullOrWhiteSpace(targetFolder))
            Directory.CreateDirectory(targetFolder);

        var tmp = targetFileName + ".tmp." + Guid.NewGuid().ToString("N");
        try
        {
            var result = _client.GetRawFile(GetUri(relativeFileName)).GetAwaiter().GetResult();
            if (!result.IsSuccessful)
                throw new InvalidOperationException($"Download failed (HTTP {result.StatusCode}): {relativeFileName}");

            using (var fs = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
                result.Stream.CopyTo(fs);

            File.Move(tmp, targetFileName, true);
        }
        catch
        {
            if (File.Exists(tmp)) File.Delete(tmp);
            throw;
        }
    }

    public override void Delete(string relativeFileName)
    {
        _client.Delete(GetUri(relativeFileName)).GetAwaiter().GetResult();
    }

    public override void DeleteFolder(string relativeFolderName)
    {
        _client.Delete(GetUri(relativeFolderName)).GetAwaiter().GetResult();
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        var dirPart = toRelativeFileName.Replace('\\', '/').Contains('/')
            ? string.Join("/", toRelativeFileName.Replace('\\', '/').Split('/').SkipLast(1))
            : null;
        if (!string.IsNullOrEmpty(dirPart))
            EnsureDirectoryExists(dirPart);

        _client.Move(GetUri(fromRelativeFileName), GetUri(toRelativeFileName),
            new MoveParameters { Overwrite = true }).GetAwaiter().GetResult();
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var result = _client.Propfind(GetUri(relativeFileName), new PropfindParameters
        {
            ApplyTo = global::WebDav.ApplyTo.Propfind.ResourceOnly,
        }).GetAwaiter().GetResult();

        if (!result.IsSuccessful || result.Resources.Count == 0)
            throw new InvalidOperationException($"Cannot get metadata for: {relativeFileName}");

        return result.Resources.First().LastModifiedDate?.ToUniversalTime() ?? DateTime.UtcNow;
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        var files = new List<string>();
        CollectFiles(relativeFolderName, files, option);
        return [.. files];
    }

    private void CollectFiles(string? relativeFolderName, List<string> files, SearchOption option)
    {
        var result = _client.Propfind(GetUri(relativeFolderName), new PropfindParameters
        {
            ApplyTo = global::WebDav.ApplyTo.Propfind.ResourceAndChildren,
        }).GetAwaiter().GetResult();

        if (!result.IsSuccessful)
            return;

        foreach (var resource in result.Resources.Skip(1)) // skip self
        {
            var rel = ToRelativePath(resource.Uri);
            if (resource.IsCollection)
            {
                if (option == SearchOption.AllDirectories)
                    CollectFiles(rel, files, option);
            }
            else
            {
                files.Add(rel);
            }
        }
    }

    public override string[] GetFolders(string? relativeFolderName, string? mask = null)
    {
        var result = _client.Propfind(GetUri(relativeFolderName), new PropfindParameters
        {
            ApplyTo = global::WebDav.ApplyTo.Propfind.ResourceAndChildren,
        }).GetAwaiter().GetResult();

        if (!result.IsSuccessful)
            return [];

        return [.. result.Resources
            .Skip(1)
            .Where(r => r.IsCollection)
            .Select(r => ToRelativePath(r.Uri))
            .Where(p => mask == null || LinuxFileHelper.FitsMask(Path.GetFileName(p), mask))];
    }

    private void EnsureDirectoryExists(string relativePath)
    {
        var parts = relativePath.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var current = string.Empty;
        foreach (var part in parts)
        {
            current = string.IsNullOrEmpty(current) ? part : $"{current}/{part}";
            if (!Exists(current))
                _client.Mkcol(GetUri(current)).GetAwaiter().GetResult();
        }
    }

    public override void Dispose()
    {
        _client.Dispose();
    }
}
