using BUtil.Core;
using System.Runtime.CompilerServices;

namespace BUtil.Tests.Platform;

/// <summary>
/// Ensures <see cref="PlatformSpecificExperience.Instance"/> uses the fake implementation
/// before any test code touches types that trigger platform assembly loading.
/// </summary>
internal static class TestPlatformModuleInitializer
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        PlatformSpecificExperience.SetInstanceForTests(FakeCrossPlatformExperience.Instance);
    }
}
