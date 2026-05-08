using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace BUtil.Storages.S3;

class S3Storage : StorageBase<S3StorageSettingsV2>
{
    private readonly AmazonS3Client _client;

    internal S3Storage(ILog log, S3StorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.AccessKey))
            throw new InvalidDataException("Access key is not specified.");
        if (string.IsNullOrWhiteSpace(Settings.SecretKey))
            throw new InvalidDataException("Secret key is not specified.");
        if (string.IsNullOrWhiteSpace(Settings.BucketName))
            throw new InvalidDataException("Bucket name is not specified.");

        _client = CreateClient();
    }

    private AmazonS3Client CreateClient()
    {
        if (Settings.Provider == "AWSS3" && string.IsNullOrWhiteSpace(Settings.ServiceUrl))
        {
            var region = string.IsNullOrWhiteSpace(Settings.Region) ? "us-east-1" : Settings.Region;
            var config = new AmazonS3Config { RegionEndpoint = RegionEndpoint.GetBySystemName(region) };
            return new AmazonS3Client(Settings.AccessKey, Settings.SecretKey, config);
        }
        else
        {
            // Alibaba OSS and Tencent COS use virtual-hosted style by default;
            // all other S3-compatible endpoints work with path-style.
            var forcePathStyle = Settings.Provider is not ("AlibabaCloudOSS" or "TencentCloudCOS");
            var config = new AmazonS3Config
            {
                ServiceURL = Settings.ServiceUrl,
                ForcePathStyle = forcePathStyle,
            };
            if (!string.IsNullOrWhiteSpace(Settings.Region))
                config.AuthenticationRegion = Settings.Region;
            return new AmazonS3Client(Settings.AccessKey, Settings.SecretKey, config);
        }
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
            _client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = Settings.BucketName,
                MaxKeys = 1,
            }, CancellationToken.None).GetAwaiter().GetResult();
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
            _client.GetObjectMetadataAsync(new GetObjectMetadataRequest
            {
                BucketName = Settings.BucketName,
                Key = GetKey(relativeFileName),
            }, CancellationToken.None).GetAwaiter().GetResult();
            return true;
        }
        catch (AmazonS3Exception e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        var key = GetKey(relativeFileName);
        _client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = Settings.BucketName,
            Key = key,
            FilePath = sourceFile,
        }, CancellationToken.None).GetAwaiter().GetResult();
        return new IStorageUploadResult
        {
            StorageFileName = key,
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
            var response = _client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = Settings.BucketName,
                Key = GetKey(relativeFileName),
            }, CancellationToken.None).GetAwaiter().GetResult();

            using (var fs = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
                response.ResponseStream.CopyTo(fs);

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
        _client.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = Settings.BucketName,
            Key = GetKey(relativeFileName),
        }, CancellationToken.None).GetAwaiter().GetResult();
    }

    public override void DeleteFolder(string relativeFolderName)
    {
        var prefix = GetKey(relativeFolderName) + "/";
        string? token = null;
        do
        {
            var list = _client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = Settings.BucketName,
                Prefix = prefix,
                ContinuationToken = token,
            }, CancellationToken.None).GetAwaiter().GetResult();

            if (list.S3Objects.Count > 0)
            {
                _client.DeleteObjectsAsync(new DeleteObjectsRequest
                {
                    BucketName = Settings.BucketName,
                    Objects = list.S3Objects.Select(o => new KeyVersion { Key = o.Key }).ToList(),
                }, CancellationToken.None).GetAwaiter().GetResult();
            }

            token = list.NextContinuationToken;
        }
        while (!string.IsNullOrEmpty(token));
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        _client.CopyObjectAsync(new CopyObjectRequest
        {
            SourceBucket = Settings.BucketName,
            SourceKey = GetKey(fromRelativeFileName),
            DestinationBucket = Settings.BucketName,
            DestinationKey = GetKey(toRelativeFileName),
        }, CancellationToken.None).GetAwaiter().GetResult();
        Delete(fromRelativeFileName);
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var meta = _client.GetObjectMetadataAsync(new GetObjectMetadataRequest
        {
            BucketName = Settings.BucketName,
            Key = GetKey(relativeFileName),
        }, CancellationToken.None).GetAwaiter().GetResult();
        return meta.LastModified.ToUniversalTime();
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        var prefix = GetKey(relativeFolderName);
        if (!string.IsNullOrEmpty(prefix)) prefix += "/";

        var delimiter = option == SearchOption.TopDirectoryOnly ? "/" : null;
        var files = new List<string>();
        string? token = null;

        do
        {
            var response = _client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = Settings.BucketName,
                Prefix = prefix,
                Delimiter = delimiter,
                ContinuationToken = token,
            }, CancellationToken.None).GetAwaiter().GetResult();

            foreach (var obj in response.S3Objects)
            {
                if (!obj.Key.EndsWith('/'))
                    files.Add(GetRelativePath(obj.Key));
            }

            if (option == SearchOption.AllDirectories)
            {
                foreach (var cp in response.CommonPrefixes)
                {
                    var subFiles = GetFiles(GetRelativePath(cp.TrimEnd('/')), option);
                    files.AddRange(subFiles);
                }
            }

            token = response.NextContinuationToken;
        }
        while (!string.IsNullOrEmpty(token));

        return [.. files];
    }

    public override string[] GetFolders(string? relativeFolderName, string? mask = null)
    {
        var prefix = GetKey(relativeFolderName);
        if (!string.IsNullOrEmpty(prefix)) prefix += "/";

        var folders = new List<string>();
        string? token = null;

        do
        {
            var response = _client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = Settings.BucketName,
                Prefix = prefix,
                Delimiter = "/",
                ContinuationToken = token,
            }, CancellationToken.None).GetAwaiter().GetResult();

            foreach (var cp in response.CommonPrefixes)
            {
                var name = GetRelativePath(cp.TrimEnd('/'));
                if (mask == null || LinuxFileHelper.FitsMask(System.IO.Path.GetFileName(name), mask))
                    folders.Add(name);
            }

            token = response.NextContinuationToken;
        }
        while (!string.IsNullOrEmpty(token));

        return [.. folders];
    }

    public override void Dispose()
    {
        _client.Dispose();
    }
}
