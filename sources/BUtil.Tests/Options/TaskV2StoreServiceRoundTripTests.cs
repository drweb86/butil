using BUtil.Interop.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Tasks.BUtilClient;
using BUtil.Tasks.BUtilServer;
using BUtil.Tasks.ImportMedia;
using BUtil.Tasks.IncrementalBackup;
using BUtil.Tasks.BUtilClient;
using BUtil.Tasks.BUtilServer;
using BUtil.Tasks.ImportMedia;
using BUtil.Tasks.Synchronization;
using BUtil.Core.FileSystem;
using BUtil.Core.Serialization;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Storages.AzureBlob;
using BUtil.Storages.Ftps;
using BUtil.Storages.Nfs;
using BUtil.Storages.S3;
using BUtil.Storages.Samba;
using BUtil.Storages.Sftp;
using BUtil.Storages.WebDav;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace BUtil.Tests.Options;

[TestClass]
public class TaskV2StoreServiceRoundTripTests
{
    private const string ExtV3 = ".v3.json";
    private static int _storagesRegistered;

    [ClassInitialize]
    public static void Initialize(TestContext _)
    {
        if (Interlocked.Exchange(ref _storagesRegistered, 1) != 0)
            return;

        StorageProviderRegistry.Register(
            "Folder",
            "Folder",
            new FolderStorageSettingsProvider(),
            typeof(FolderStorageSettingsV2),
            (_, _, _) => throw new NotSupportedException());
        SftpStoragePlugin.Register();
        FtpsStoragePlugin.Register();
        WebDavStoragePlugin.Register();
        S3StoragePlugin.Register();
        AzureBlobStoragePlugin.Register();

        StorageProviderRegistry.Register(
            "Samba",
            "SMB/CIFS",
            new SambaStorageSettingsProvider(),
            typeof(SambaStorageSettingsV2),
            (_, _, _) => throw new NotSupportedException());
        StorageProviderRegistry.Register(
            "Nfs",
            "NFS",
            new NfsStorageSettingsProvider(),
            typeof(NfsStorageSettingsV2),
            (_, _, _) => throw new NotSupportedException());

        IncrementalBackupTaskPlugin.Register();
        SynchronizationTaskPlugin.Register();
        ImportMediaTaskPlugin.Register();
        BUtilServerTaskPlugin.Register();
        BUtilClientTaskPlugin.Register();
    }

    public static IEnumerable<object[]> TaskOptionsCases()
    {
        yield return [new TaskV2
        {
            Name = "incremental-options",
            Model = new IncrementalBackupModelOptionsV2
            {
                To = FolderStorage("F:\\backup\\incremental"),
                Items =
                [
                    new SourceItemV2("C:\\Users\\test\\Documents", true)
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    },
                    new SourceItemV2("C:\\Users\\test\\file.txt", false)
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    },
                ],
                FileExcludePatterns = ["bin", "obj", "*.tmp"],
                Password = "incremental-task-password",
            },
        }];

        yield return [new TaskV2
        {
            Name = "synchronization-options",
            Model = new SynchronizationTaskModelOptionsV2
            {
                To = FolderStorage("F:\\backup\\sync"),
                LocalFolder = "C:\\sync-local",
                RepositorySubfolder = "remote/subfolder",
                SynchronizationMode = SynchronizationTaskModelMode.Read,
                Password = "sync-task-password",
            },
        }];

        yield return [new TaskV2
        {
            Name = "import-media-options",
            Model = new ImportMediaTaskModelOptionsV2
            {
                From = FolderStorage("E:\\camera"),
                DestinationFolder = "D:\\media",
                TransformFileName = "{DATE:yyyy}\\{DATE:MM}\\{SHA256}",
                SkipAlreadyImportedFiles = false,
                FileLastWriteTimeMin = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc),
            },
        }];

        yield return [new TaskV2
        {
            Name = "server-options",
            Model = new BUtilServerModelOptionsV2(
                port: 12001,
                username: "server-user",
                password: "server-task-password",
                folder: "D:\\server-drop",
                durationMinutes: 77),
        }];

        yield return [new TaskV2
        {
            Name = "client-options",
            Model = new BUtilClientModelOptionsV2(
                folder: "D:\\client-drop",
                direction: FileSenderDirection.ToServer,
                to: FolderStorage("F:\\backup\\client")),
        }];
    }

    public static IEnumerable<object[]> StorageOptionsCases()
    {
        yield return
        [
            new FolderStorageSettingsV2
            {
                DestinationFolder = "F:\\backup\\folder",
                SingleBackupQuotaGb = 11,
                MountPowershellScript = "mount-folder",
                UnmountPowershellScript = "unmount-folder",
            },
            Array.Empty<string>()
        ];

        yield return
        [
            new SambaStorageSettingsV2
            {
                Url = "\\\\server\\share\\backup",
                User = "samba-user",
                Password = "samba-password",
                SingleBackupQuotaGb = 12,
                MountPowershellScript = "mount-samba",
                UnmountPowershellScript = "unmount-samba",
            },
            new[] { "samba-password" }
        ];

        yield return
        [
            new SftpStorageSettingsV2
            {
                Host = "sftp.example.test",
                Port = 2222,
                Folder = "/backups",
                User = "sftp-user",
                Password = "sftp-password",
                KeyFile = "C:\\keys\\id_rsa",
                FingerPrintSHA256 = "SHA256:abc123",
                SingleBackupQuotaGb = 13,
                MountPowershellScript = "mount-sftp",
                UnmountPowershellScript = "unmount-sftp",
            },
            new[] { "sftp-password" }
        ];

        yield return
        [
            new FtpsStorageSettingsV2
            {
                Host = "ftps.example.test",
                Encryption = FtpsStorageEncryptionV2.Implicit,
                Port = 990,
                Folder = "/ftps-backups",
                User = "ftps-user",
                Password = "ftps-password",
                TrustedCertificate = "A1B2C3",
                SingleBackupQuotaGb = 14,
                MountPowershellScript = "mount-ftps",
                UnmountPowershellScript = "unmount-ftps",
            },
            new[] { "ftps-password" }
        ];

        yield return
        [
            new WebDavStorageSettingsV2
            {
                Preset = "Custom",
                Host = "webdav.example.test",
                Port = 8443,
                UseHttps = true,
                BasePath = "/remote.php/dav/files/user",
                User = "webdav-user",
                Password = "webdav-password",
                SingleBackupQuotaGb = 15,
                MountPowershellScript = "mount-webdav",
                UnmountPowershellScript = "unmount-webdav",
            },
            new[] { "webdav-password" }
        ];

        yield return
        [
            new S3StorageSettingsV2
            {
                Provider = "Custom",
                ServiceUrl = "https://s3.example.test",
                Region = "test-region-1",
                AccessKey = "s3-access-key",
                SecretKey = "s3-secret-key",
                BucketName = "backup-bucket",
                PathPrefix = "machine-a",
                SingleBackupQuotaGb = 16,
                MountPowershellScript = "mount-s3",
                UnmountPowershellScript = "unmount-s3",
            },
            new[] { "s3-secret-key" }
        ];

        yield return
        [
            new AzureBlobStorageSettingsV2
            {
                AccountName = "accountname",
                AccountKey = "azure-account-key",
                ContainerName = "backup-container",
                PathPrefix = "machine-b",
                SingleBackupQuotaGb = 17,
                MountPowershellScript = "mount-azure",
                UnmountPowershellScript = "unmount-azure",
            },
            new[] { "azure-account-key" }
        ];

        yield return
        [
            new NfsStorageSettingsV2
            {
                Host = "nfs.example.test",
                SharePath = "/export/backups",
                MountPoint = "Z:",
                MountOptions = "anon",
                SingleBackupQuotaGb = 18,
                MountPowershellScript = "mount-nfs",
                UnmountPowershellScript = "unmount-nfs",
            },
            Array.Empty<string>()
        ];
    }

    [TestMethod]
    [DynamicData(nameof(TaskOptionsCases))]
    public void SaveAndLoad_AllTaskOptions_RoundTrips(TaskV2 expected)
    {
        var fs = new InMemoryFileSystem();
        var store = new TaskStore(fs);

        store.Save(expected);
        var loaded = store.Load(expected.Name);

        Assert.IsNotNull(loaded);
        Assert.AreEqual(Serialize(expected), Serialize(loaded));
    }

    [TestMethod]
    [DynamicData(nameof(StorageOptionsCases))]
    public void SaveAndLoad_AllStorageOptions_RoundTripsAndProtectsProviderSecrets(
        IStorageSettingsV2 storageSettings,
        string[] secretValues)
    {
        var fs = new InMemoryFileSystem();
        var expected = new TaskV2
        {
            Name = $"storage-{storageSettings.GetType().Name}",
            Model = new IncrementalBackupModelOptionsV2
            {
                To = storageSettings,
                Items = [],
                FileExcludePatterns = ["*.cache"],
                Password = "storage-test-task-password",
            },
        };
        var store = new TaskStore(fs);

        store.Save(expected);
        var rawJson = fs.Files[expected.Name + ExtV3];
        var loaded = store.Load(expected.Name);

        Assert.IsNotNull(loaded);
        Assert.AreEqual(Serialize(expected), Serialize(loaded));
        AssertProtected(rawJson, "storage-test-task-password");
        foreach (var secretValue in secretValues)
            AssertProtected(rawJson, secretValue);
    }

    private static FolderStorageSettingsV2 FolderStorage(string destinationFolder) => new()
    {
        DestinationFolder = destinationFolder,
        SingleBackupQuotaGb = 10,
        MountPowershellScript = "mount-folder",
        UnmountPowershellScript = "unmount-folder",
    };

    private static string Serialize(TaskV2 task) =>
        JsonSerializer.Serialize(task, JsonOptions.TaskSerialization);

    private static void AssertProtected(string rawJson, string secretValue)
    {
        Assert.IsFalse(
            rawJson.Contains(secretValue, StringComparison.Ordinal),
            $"Stored JSON should not contain the raw secret '{secretValue}'.");
        Assert.IsTrue(rawJson.Contains("enc::", StringComparison.Ordinal));
    }

    private sealed class InMemoryFileSystem : ILocalFileSystem
    {
        public Dictionary<string, string> Files { get; } = new(StringComparer.OrdinalIgnoreCase);

        public void EnsureFolderCreated(string folder) { }

        public bool FileExists(string path) =>
            Files.ContainsKey(Path.GetFileName(path));

        public string ReadAllText(string path) =>
            Files[Path.GetFileName(path)];

        public void WriteAllText(string path, string content) =>
            Files[Path.GetFileName(path)] = content;

        public void DeleteFile(string path) =>
            Files.Remove(Path.GetFileName(path));

        public string[] GetFiles(string folder, string pattern)
        {
            var ext = pattern.TrimStart('*');
            return [.. Files.Keys
                .Where(file => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                .Select(file => Path.Combine(folder, file))];
        }
    }
}
