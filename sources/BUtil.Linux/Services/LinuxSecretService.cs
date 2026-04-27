using BUtil.Core.Options;
using System.Security.Cryptography;
using System.Text;

namespace BUtil.Linux.Services;

/// <summary>
/// Linux secret service using AES encryption with a machine-derived key.
/// Similar to Windows DPAPI - secrets are tied to this machine.
/// </summary>
internal sealed class LinuxSecretService : SecretServiceBase
{
    private static readonly byte[] _encryptionKey = DeriveKeyFromMachine();

    protected override byte[] ProtectBytes(byte[] plainBytes)
    {
        using var aes = Aes.Create();
        aes.Key = _encryptionKey;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Prepend IV to encrypted data
        var result = new byte[aes.IV.Length + encrypted.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

        return result;
    }

    protected override byte[] UnprotectBytes(byte[] encryptedBytes)
    {
        using var aes = Aes.Create();
        aes.Key = _encryptionKey;

        // Extract IV from the beginning
        var iv = new byte[aes.BlockSize / 8];
        Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
        aes.IV = iv;

        var ciphertext = new byte[encryptedBytes.Length - iv.Length];
        Buffer.BlockCopy(encryptedBytes, iv.Length, ciphertext, 0, ciphertext.Length);

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
    }

    private static byte[] DeriveKeyFromMachine()
    {
        // Use machine-id as the basis for key derivation (like DPAPI uses machine key)
        // /etc/machine-id is a unique identifier created at OS install time
        var machineId = GetMachineId();
        var salt = Encoding.UTF8.GetBytes("BUtil.Linux.SecretService.v1");

        // Derive a 256-bit key using PBKDF2
#pragma warning disable SYSLIB0060 // Type or member is obsolete
        using var pbkdf2 = new Rfc2898DeriveBytes(machineId, salt, 100000, HashAlgorithmName.SHA256);
#pragma warning restore SYSLIB0060 // Type or member is obsolete
        return pbkdf2.GetBytes(32);
    }

    private static byte[] GetMachineId()
    {
        // Try /etc/machine-id first (systemd)
        const string machineIdPath = "/etc/machine-id";
        if (File.Exists(machineIdPath))
        {
            var content = File.ReadAllText(machineIdPath).Trim();
            if (!string.IsNullOrEmpty(content))
                return Encoding.UTF8.GetBytes(content);
        }

        // Fallback to /var/lib/dbus/machine-id
        const string dbusIdPath = "/var/lib/dbus/machine-id";
        if (File.Exists(dbusIdPath))
        {
            var content = File.ReadAllText(dbusIdPath).Trim();
            if (!string.IsNullOrEmpty(content))
                return Encoding.UTF8.GetBytes(content);
        }

        throw new InvalidOperationException(
            "Cannot find machine-id. Ensure /etc/machine-id or /var/lib/dbus/machine-id exists.");
    }
}
