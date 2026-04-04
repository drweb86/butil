using BUtil.Core.Options;
using System.Runtime.InteropServices;
using System.Text;

namespace BUtil.Linux.Services;

internal sealed class LinuxSecretService : SecretServiceBase
{
    private const string _libSecretTokenPrefix = "ls1:";

    protected override byte[] ProtectBytes(byte[] plainBytes)
    {
        var secretId = Guid.NewGuid().ToString("N");
        var secretValue = Convert.ToBase64String(plainBytes);
        LibSecretInterop.StoreSecret(secretId, secretValue);
        return Encoding.UTF8.GetBytes(_libSecretTokenPrefix + secretId);
    }

    protected override byte[] UnprotectBytes(byte[] encryptedBytes)
    {
        var token = Encoding.UTF8.GetString(encryptedBytes);
        if (!token.StartsWith(_libSecretTokenPrefix, StringComparison.Ordinal))
            throw new InvalidOperationException("Unsupported libsecret token format.");

        var secretId = token[_libSecretTokenPrefix.Length..];
        var secretValue = LibSecretInterop.LookupSecret(secretId)
            ?? throw new InvalidOperationException("Secret is missing in keyring.");
        return Convert.FromBase64String(secretValue);
    }

    private static class LibSecretInterop
    {
        private const string _libraryName = "libsecret-1.so.0";
        private const string _glibLibraryName = "libglib-2.0.so.0";
        private const string _schemaName = "org.butil.task-secrets";
        private const string _schemaAttribute = "id";
        private const string _labelPrefix = "BUtil Task Secret";

        private static readonly SecretSchema _schema = CreateSchema();

        private static readonly IntPtr GStrHashPtr;
        private static readonly IntPtr GStrEqualPtr;
        private static readonly IntPtr GFreePtr;

        static LibSecretInterop()
        {
            var glib = NativeLibrary.Load(_glibLibraryName);
            GStrHashPtr = NativeLibrary.GetExport(glib, "g_str_hash");
            GStrEqualPtr = NativeLibrary.GetExport(glib, "g_str_equal");
            GFreePtr = NativeLibrary.GetExport(glib, "g_free");
        }

        public static void StoreSecret(string id, string value)
        {
            using var attrs = CreateAttributesHashTable(id);
            var ok = secret_password_storev_sync(in _schema, null, $"{_labelPrefix} ({id})", value, IntPtr.Zero, out var error, attrs.Handle);
            if (!ok)
                ThrowLibSecretError("Failed to store secret in libsecret.", error);
            FreeError(error);
        }

        public static string? LookupSecret(string id)
        {
            using var attrs = CreateAttributesHashTable(id);
            var passwordPtr = secret_password_lookupv_sync(in _schema, IntPtr.Zero, out var error, attrs.Handle);
            if (error != IntPtr.Zero)
                ThrowLibSecretError("Failed to lookup secret in libsecret.", error);

            if (passwordPtr == IntPtr.Zero)
                return null;

            try
            {
                return Marshal.PtrToStringUTF8(passwordPtr);
            }
            finally
            {
                secret_password_free(passwordPtr);
            }
        }

        private static GHashTableHandle CreateAttributesHashTable(string id)
        {
            var attrs = g_hash_table_new_full(GStrHashPtr, GStrEqualPtr, GFreePtr, GFreePtr);
            if (attrs == IntPtr.Zero)
                throw new InvalidOperationException("Failed to allocate GHashTable for libsecret attributes.");

            g_hash_table_insert(attrs, g_strdup(_schemaAttribute), g_strdup(id));
            return new GHashTableHandle(attrs);
        }

        private readonly struct GHashTableHandle : IDisposable
        {
            private readonly IntPtr _handle;

            public GHashTableHandle(IntPtr handle) => _handle = handle;

            public IntPtr Handle => _handle;

            public void Dispose()
            {
                if (_handle != IntPtr.Zero)
                    g_hash_table_unref(_handle);
            }
        }

        [DllImport(_glibLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr g_hash_table_new_full(IntPtr hashFunc, IntPtr equalFunc, IntPtr keyDestroyFunc, IntPtr valueDestroyFunc);

        [DllImport(_glibLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void g_hash_table_insert(IntPtr hashTable, IntPtr key, IntPtr value);

        [DllImport(_glibLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void g_hash_table_unref(IntPtr hashTable);

        [DllImport(_glibLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr g_strdup([MarshalAs(UnmanagedType.LPUTF8Str)] string str);

        private static SecretSchema CreateSchema()
        {
            var attributes = new SecretSchemaAttribute[32];
            attributes[0] = new SecretSchemaAttribute
            {
                name = _schemaAttribute,
                type = SecretSchemaAttributeType.SECRET_SCHEMA_ATTRIBUTE_STRING,
            };

            return new SecretSchema
            {
                name = _schemaName,
                flags = SecretSchemaFlags.SECRET_SCHEMA_NONE,
                attributes = attributes,
            };
        }

        private static void ThrowLibSecretError(string message, IntPtr errorPtr)
        {
            var fullMessage = message;
            if (errorPtr != IntPtr.Zero)
            {
                try
                {
                    var error = Marshal.PtrToStructure<GError>(errorPtr);
                    var nativeMessage = Marshal.PtrToStringUTF8(error.message);
                    if (!string.IsNullOrWhiteSpace(nativeMessage))
                        fullMessage = $"{message} {nativeMessage}";
                }
                finally
                {
                    FreeError(errorPtr);
                }
            }

            throw new InvalidOperationException(fullMessage);
        }

        private static void FreeError(IntPtr errorPtr)
        {
            if (errorPtr != IntPtr.Zero)
                g_error_free(errorPtr);
        }

        // Must match libsecret's SecretSchema (secret-schema.h): gint reserved, then gpointer reserved1..7.
        [StructLayout(LayoutKind.Sequential)]
        private struct SecretSchema
        {
            [MarshalAs(UnmanagedType.LPUTF8Str)]
            public string name;

            public SecretSchemaFlags flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public SecretSchemaAttribute[] attributes;

            public int reserved;

            public IntPtr reserved1;
            public IntPtr reserved2;
            public IntPtr reserved3;
            public IntPtr reserved4;
            public IntPtr reserved5;
            public IntPtr reserved6;
            public IntPtr reserved7;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SecretSchemaAttribute
        {
            [MarshalAs(UnmanagedType.LPUTF8Str)]
            public string? name;

            public SecretSchemaAttributeType type;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct GError
        {
            public uint domain;
            public int code;
            public IntPtr message;
        }

        private enum SecretSchemaAttributeType
        {
            SECRET_SCHEMA_ATTRIBUTE_STRING = 0,
        }

        private enum SecretSchemaFlags
        {
            SECRET_SCHEMA_NONE = 0,
        }

        [DllImport(_libraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool secret_password_storev_sync(
            in SecretSchema schema,
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? collection,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string label,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string password,
#pragma warning restore CA2101 // Specify marshaling for P/Invoke string arguments
            IntPtr cancellable,
            out IntPtr error,
            IntPtr attributes);

        [DllImport(_libraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr secret_password_lookupv_sync(
            in SecretSchema schema,
            IntPtr cancellable,
            out IntPtr error,
            IntPtr attributes);

        [DllImport(_libraryName, CallingConvention = CallingConvention.Cdecl)]
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
        private static extern void secret_password_free(IntPtr password);
#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

        [DllImport("libglib-2.0.so.0", CallingConvention = CallingConvention.Cdecl)]
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
        private static extern void g_error_free(IntPtr error);
#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
    }
}
