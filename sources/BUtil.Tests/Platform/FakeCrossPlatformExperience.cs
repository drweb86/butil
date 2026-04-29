using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using System.IO;

namespace BUtil.Tests.Platform;

/// <summary>
/// Minimal <see cref="CrossPlatformExperience"/> for unit tests (no BUtil.Windows / BUtil.Linux).
/// </summary>
internal sealed class FakeCrossPlatformExperience : CrossPlatformExperience
{
    public static readonly FakeCrossPlatformExperience Instance = new();

    private FakeCrossPlatformExperience()
    {
    }

    public override int MinimumListenerPort => 1024;

    public override ISecretService SecretService { get; } = new FakeSecretService();

    public override ISessionService SessionService { get; } = new FakeSessionService();

    public override ISupportManager SupportManager { get; } = new FakeSupportManager();

    public override IFolderService GetFolderService() => FakeFolderService.Instance;

    public override bool IsSmbCifsSupported => false;

    public override IStorage GetSmbCifsStorage(ILog log, SambaStorageSettingsV2 settings) =>
        throw new NotSupportedException("Tests do not provide SMB storage.");

    public override ITaskSchedulerService GetTaskSchedulerService() => FakeTaskSchedulerService.Instance;

    public override IUiService UiService { get; } = new FakeUiService();

    public override IOsSleepPreventionService OsSleepPreventionService { get; } = new FakeOsSleepPreventionService();

    private sealed class FakeSecretService : SecretServiceBase
    {
        protected override byte[] ProtectBytes(byte[] plainBytes) => plainBytes;

        protected override byte[] UnprotectBytes(byte[] encryptedBytes) => encryptedBytes;
    }

    private sealed class FakeSessionService : ISessionService
    {
        public void DoTask(PowerTask task)
        {
        }
    }

    private sealed class FakeSupportManager : ISupportManager
    {
        public string ScriptEngineName => "none";

        public bool CanLaunchScripts => false;

        public bool CanOpenLink => false;

        public bool LaunchScript(ILog log, string script, string forbiddenForLogs) => false;

        public void LaunchTasksAppOrExit()
        {
        }

        public void OpenHomePage()
        {
        }

        public void OpenIcons()
        {
        }

        public void OpenLatestRelease()
        {
        }
    }

    private sealed class FakeFolderService : IFolderService
    {
        public static readonly FakeFolderService Instance = new();

        public IEnumerable<string> GetDefaultBackupFolders() => [];

        public string GetDefaultSynchronizationFolder() => string.Empty;

        public string GetDefaultMediaImportFolder() => string.Empty;

        public string GetStorageItemExcludePatternHelp() => string.Empty;

        public void OpenFileInShell(string file)
        {
        }

        public void OpenFolderInShell(string folder)
        {
        }
    }

    private sealed class FakeTaskSchedulerService : ITaskSchedulerService
    {
        public static readonly FakeTaskSchedulerService Instance = new();

        public ScheduleInfo GetSchedule(string taskName) => new();

        public void Schedule(string taskName, ScheduleInfo scheduleInfo)
        {
        }

        public void Unschedule(string taskName)
        {
        }
    }

    private sealed class FakeUiService : IUiService
    {
        public bool CanExtendClientAreaToDecorationsHint => false;

        public void Blink()
        {
        }
    }

    private sealed class FakeOsSleepPreventionService : IOsSleepPreventionService
    {
        public void PreventSleep()
        {
        }

        public void StopPreventSleep()
        {
        }
    }
}
