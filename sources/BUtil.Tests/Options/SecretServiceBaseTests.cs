using BUtil.Interop.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Tasks.BUtilServer;
using BUtil.Tasks.IncrementalBackup;
using BUtil.Core.Options;
using BUtil.Core.Serialization;
using BUtil.Core.Storages;
using BUtil.Interop.Logs;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace BUtil.Tests.Options;

[TestClass]
public class SecretServiceBaseTests
{
    private const string EncPrefix = "enc::";
    private static int _providerRegistered;
    private readonly TestSecretService _secretService = new();

    [ClassInitialize]
    public static void Initialize(TestContext _)
    {
        if (Interlocked.Exchange(ref _providerRegistered, 1) != 0)
            return;

        StorageProviderRegistry.Register(
            "SecretServiceBaseTest",
            "Secret Service Base Test",
            new TestStorageSettingsProvider(),
            typeof(TestStorageSettings),
            (_, _, _) => throw new NotSupportedException());
    }

    [TestMethod]
    public void CreateProtectedClone_TaskPassword_ProtectsCloneOnly()
    {
        var task = new TaskV2
        {
            Name = "server-secret",
            Model = new BUtilServerModelOptionsV2(
                port: 10999,
                username: "server-user",
                password: "task-password",
                folder: "D:\\drop",
                durationMinutes: 30),
        };

        var protectedTask = _secretService.CreateProtectedClone(task);

        var originalModel = (BUtilServerModelOptionsV2)task.Model;
        var protectedModel = (BUtilServerModelOptionsV2)protectedTask.Model;
        Assert.AreEqual("task-password", originalModel.Password);
        AssertProtected("task-password", protectedModel.Password);
        Assert.AreEqual(originalModel.Username, protectedModel.Username);
        Assert.AreEqual(originalModel.Folder, protectedModel.Folder);
    }

    [TestMethod]
    public void CreateProtectedClone_StorageProviderSecretSettingsProperties_ProtectsMultipleFields()
    {
        var task = CreateTaskWithTestStorage();

        var protectedTask = _secretService.CreateProtectedClone(task);

        var settings = GetStorageSettings(protectedTask);
        AssertProtected("secret-one", settings.SecretOne);
        AssertProtected("secret-two", settings.SecretTwo);
        Assert.AreEqual("password-field-not-listed", settings.UnlistedPassword);
        Assert.AreEqual("public-value", settings.PublicValue);
    }

    [TestMethod]
    public void UnprotectInPlace_ProtectedTask_RestoresTaskAndProviderProtectedFields()
    {
        var task = CreateTaskWithTestStorage();
        var expectedJson = Serialize(task);
        var protectedTask = _secretService.CreateProtectedClone(task);

        _secretService.UnprotectInPlace(protectedTask);

        Assert.AreEqual(expectedJson, Serialize(protectedTask));
    }

    [TestMethod]
    public void CreateProtectedClone_EmptyWhitespaceAndAlreadyProtectedValues_AreLeftAsIs()
    {
        var alreadyProtected = EncPrefix + Convert.ToBase64String(Encoding.UTF8.GetBytes("already-protected"));
        var task = new TaskV2
        {
            Name = "skip-empty-secrets",
            Model = new IncrementalBackupModelOptionsV2
            {
                Items = [],
                FileExcludePatterns = [],
                Password = "   ",
                To = new TestStorageSettings
                {
                    SecretOne = string.Empty,
                    SecretTwo = alreadyProtected,
                    UnlistedPassword = "unlisted",
                    PublicValue = "public",
                },
            },
        };

        var protectedTask = _secretService.CreateProtectedClone(task);

        var model = (IncrementalBackupModelOptionsV2)protectedTask.Model;
        var settings = (TestStorageSettings)model.To;
        Assert.AreEqual("   ", model.Password);
        Assert.AreEqual(string.Empty, settings.SecretOne);
        Assert.AreEqual(alreadyProtected, settings.SecretTwo);
    }

    private static TaskV2 CreateTaskWithTestStorage() => new()
    {
        Name = "storage-secret",
        Model = new IncrementalBackupModelOptionsV2
        {
            Items = [],
            FileExcludePatterns = [],
            Password = "task-password",
            To = new TestStorageSettings
            {
                SingleBackupQuotaGb = 123,
                MountPowershellScript = "mount",
                UnmountPowershellScript = "unmount",
                SecretOne = "secret-one",
                SecretTwo = "secret-two",
                UnlistedPassword = "password-field-not-listed",
                PublicValue = "public-value",
            },
        },
    };

    private static TestStorageSettings GetStorageSettings(TaskV2 task)
    {
        var model = (IncrementalBackupModelOptionsV2)task.Model;
        return (TestStorageSettings)model.To;
    }

    private static string Serialize(TaskV2 task) =>
        JsonSerializer.Serialize(task, JsonOptions.TaskSerialization);

    private static void AssertProtected(string plainText, string? protectedText)
    {
        Assert.IsNotNull(protectedText);
        Assert.AreNotEqual(plainText, protectedText);
        Assert.IsTrue(protectedText.StartsWith(EncPrefix, StringComparison.Ordinal));
    }

    private sealed class TestSecretService : SecretServiceBase
    {
        protected override byte[] ProtectBytes(byte[] plainBytes) => plainBytes;

        protected override byte[] UnprotectBytes(byte[] encryptedBytes) => encryptedBytes;
    }

    private sealed class TestStorageSettings : IStorageSettingsV2
    {
        public long SingleBackupQuotaGb { get; set; }
        public string? MountPowershellScript { get; set; }
        public string? UnmountPowershellScript { get; set; }
        public string SecretOne { get; set; } = string.Empty;
        public string SecretTwo { get; set; } = string.Empty;
        public string UnlistedPassword { get; set; } = string.Empty;
        public string PublicValue { get; set; } = string.Empty;
    }

    private sealed class TestStorageSettingsProvider : IStorageSettingsProvider
    {
        public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
        [
            new StorageFieldDescriptor { Key = "secretOne", Label = "Secret One", Type = StorageFieldType.Password },
            new StorageFieldDescriptor { Key = "secretTwo", Label = "Secret Two", Type = StorageFieldType.Text },
            new StorageFieldDescriptor { Key = "unlistedPassword", Label = "Unlisted Password", Type = StorageFieldType.Password },
            new StorageFieldDescriptor { Key = "publicValue", Label = "Public Value", Type = StorageFieldType.Text },
        ];

        public IReadOnlyList<string> SecretSettingsProperties { get; } = ["secretOne", "secretTwo"];


        public IStorageSettingsV2 GetSettings(
            IReadOnlyDictionary<string, string?> fieldValues,
            long quota,
            string? mountScript,
            string? unmountScript) =>
            new TestStorageSettings
            {
                SingleBackupQuotaGb = quota,
                MountPowershellScript = mountScript,
                UnmountPowershellScript = unmountScript,
                SecretOne = fieldValues.GetValueOrDefault("secretOne") ?? string.Empty,
                SecretTwo = fieldValues.GetValueOrDefault("secretTwo") ?? string.Empty,
                UnlistedPassword = fieldValues.GetValueOrDefault("unlistedPassword") ?? string.Empty,
                PublicValue = fieldValues.GetValueOrDefault("publicValue") ?? string.Empty,
            };

        public IReadOnlyDictionary<string, string?> GetFieldValues(IStorageSettingsV2 settings)
        {
            var testSettings = (TestStorageSettings)settings;
            return new Dictionary<string, string?>
            {
                ["secretOne"] = testSettings.SecretOne,
                ["secretTwo"] = testSettings.SecretTwo,
                ["unlistedPassword"] = testSettings.UnlistedPassword,
                ["publicValue"] = testSettings.PublicValue,
            };
        }

        public string? TryApplyDetectedTrust(
            IStorageSettingsV2 testedSettings,
            IReadOnlyDictionary<string, string?> currentValues,
            out IReadOnlyDictionary<string, string?>? updatedValues)
        {
            updatedValues = null;
            return null;
        }
    }
}
