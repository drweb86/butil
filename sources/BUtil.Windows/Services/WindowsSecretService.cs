using BUtil.Core.Options;
using System.Security.Cryptography;

namespace BUtil.Windows.Services;

internal sealed class WindowsSecretService : SecretServiceBase
{
    protected override byte[] ProtectBytes(byte[] plainBytes)
    {
#pragma warning disable CA1416 // Validate platform compatibility
        return ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
#pragma warning restore CA1416 // Validate platform compatibility
    }

    protected override byte[] UnprotectBytes(byte[] encryptedBytes)
    {
#pragma warning disable CA1416 // Validate platform compatibility
        return ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
#pragma warning restore CA1416 // Validate platform compatibility
    }
}
