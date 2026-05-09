using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BUtil.Core.FileSystem;
using BUtil.Interop.Logs;
using BUtil.Core.Storages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BUtil.Storages.AzureBlob;

class AzureBlobStorage : StorageBase<AzureBlobStorageSettingsV2>
{
    private readonly BlobContainerClient _container;

    internal AzureBlobStorage(ILog log, AzureBlobStorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.AccountName))
            throw new InvalidDataException("Account name is not specified.");
        if (string.IsNullOrWhiteSpace(Settings.AccountKey))
            throw new InvalidDataException("Account key is not specified.");
        if (string.IsNullOrWhiteSpace(Settings.ContainerName))
            throw new InvalidDataException("Container name is not specified.");

        var credential = new StorageSharedKeyCredential(Settings.AccountName, Settings.AccountKey);
        var serviceUri = new Uri($"https://{Settings.AccountName}.blob.core.windows.net");
        _container = new BlobServiceClient(serviceUri, credential)
            .GetBlobContainerClient(Settings.ContainerName);
    }

    private string GetKey(string? relativePath)
    {
        var normalized = relativePath?.Replace('\\', '/').Trim('/') ?? string.Empty;
        if (string.IsNullOrWhiteSpace(Settings.PathPrefix))
            return normalized;
        var prefix = Settings.PathPrefix.Trim('/');
        return string.IsNullOrEmpty(normalized) ? prefix : $"{prefix}/{normalized}";
    }

    private string GetRelativePath(string key)
    {
        var prefix = string.IsNullOrWhiteSpace(Settings.PathPrefix)
            ? string.Empty
            : Settings.PathPrefix.Trim('/') + "/";
        if (!string.IsNullOrEmpty(prefix) && key.StartsWith(prefix, StringComparison.Ordinal))
            key = key[prefix.Length..];
        return key;
    }

    public override string? Test()
    {
        try
        {
            _ = _container.Exists().Value;
            return null;
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
            return _container.GetBlobClient(GetKey(relativeFileName)).Exists().Value;
        }
        catch
        {
            return false;
        }
    }

    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        _container.GetBlobClient(GetKey(relativeFileName)).Upload(sourceFile, overwrite: true);
        return new IStorageUploadResult
        {
            StorageFileName = GetKey(relativeFileName),
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
            _container.GetBlobClient(GetKey(relativeFileName)).DownloadTo(tmp);
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
        _container.GetBlobClient(GetKey(relativeFileName)).DeleteIfExists();
    }

    public override void DeleteFolder(string relativeFolderName)
    {
        var prefix = GetKey(relativeFolderName) + "/";
        foreach (var item in _container.GetBlobs(BlobTraits.None, BlobStates.None, prefix, CancellationToken.None))
            _container.GetBlobClient(item.Name).DeleteIfExists();
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        var fromBlob = _container.GetBlobClient(GetKey(fromRelativeFileName));
        var toBlob = _container.GetBlobClient(GetKey(toRelativeFileName));

        var tmp = Path.GetTempFileName();
        try
        {
            fromBlob.DownloadTo(tmp);
            toBlob.Upload(tmp, overwrite: true);
        }
        finally
        {
            File.Delete(tmp);
        }
        fromBlob.DeleteIfExists();
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var props = _container.GetBlobClient(GetKey(relativeFileName)).GetProperties().Value;
        return props.LastModified.UtcDateTime;
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        var prefix = GetKey(relativeFolderName);
        if (!string.IsNullOrEmpty(prefix)) prefix += "/";

        if (option == SearchOption.AllDirectories)
        {
            return [.. _container
                .GetBlobs(BlobTraits.None, BlobStates.None, prefix, CancellationToken.None)
                .Where(b => !b.Name.EndsWith('/'))
                .Select(b => GetRelativePath(b.Name))];
        }

        return [.. _container
            .GetBlobsByHierarchy(BlobTraits.None, BlobStates.None, "/", prefix)
            .Where(item => item.IsBlob && !item.Blob.Name.EndsWith('/'))
            .Select(item => GetRelativePath(item.Blob.Name))];
    }

    public override string[] GetFolders(string? relativeFolderName, string? mask = null)
    {
        var prefix = GetKey(relativeFolderName);
        if (!string.IsNullOrEmpty(prefix)) prefix += "/";

        return [.. _container
            .GetBlobsByHierarchy(BlobTraits.None, BlobStates.None, "/", prefix)
            .Where(item => item.IsPrefix)
            .Select(item => GetRelativePath(item.Prefix.TrimEnd('/')))
            .Where(name => mask == null || LinuxFileHelper.FitsMask(Path.GetFileName(name), mask))];
    }

    public override void Dispose()
    {
    }
}
