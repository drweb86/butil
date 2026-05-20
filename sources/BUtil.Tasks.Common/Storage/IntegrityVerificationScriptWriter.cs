using BUtil.Core.Storages;
using System;
using System.IO;

namespace BUtil.Tasks.Common.Storage;

internal static class IntegrityVerificationScriptWriter
{
    private const string FallbackPowershellFileName = "Integrity verification script.ps1";

    public static void WriteAndUploadPowershellScript(IStorage storage, string tempFolder, string scriptContent, Action<string> logDebug)
    {
        var localizedFileName = BUtil.Core.Localization.Resources.File_IntegrityVerificationScript_Ps1;
        try
        {
            WriteAndUpload(storage, tempFolder, scriptContent, localizedFileName);
            return;
        }
        catch (Exception ex) when (!string.Equals(localizedFileName, FallbackPowershellFileName, StringComparison.Ordinal))
        {
            logDebug($"Cannot save integrity verification script as \"{localizedFileName}\". Retrying as \"{FallbackPowershellFileName}\": {ex.Message}");
        }

        WriteAndUpload(storage, tempFolder, scriptContent, FallbackPowershellFileName);
    }

    private static void WriteAndUpload(IStorage storage, string tempFolder, string scriptContent, string fileName)
    {
        var powershellFile = Path.Combine(tempFolder, fileName);
        try
        {
            File.WriteAllText(powershellFile, scriptContent);
            _ = storage.Upload(powershellFile, fileName) ?? throw new Exception("Cannot save integrity verification scripts!");
        }
        finally
        {
            if (File.Exists(powershellFile))
                File.Delete(powershellFile);
        }
    }
}
