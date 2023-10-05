using System.IO;
using System;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Options
{
    public class SettingsStoreService
    {
        public SettingsStoreService()
        {
            if (!Directory.Exists(Directories.SettingsDir))
                Directory.CreateDirectory(Directories.SettingsDir);
        }

        public string Load(string name, string defaultValue)
        {
            var file = GetFileName(name);
            if (File.Exists(file))
                return File.ReadAllText(file);

            return defaultValue;
        }

        public void Save(string name, string value)
        {
            var file = GetFileName(name);
            if (File.Exists(file))
                File.Delete(file);
            File.WriteAllText(file, value);
        }

        private string GetFileName(string name)
        {
            if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
                throw new ArgumentException(nameof(name));

            return Path.Combine(Directories.SettingsDir, name);
        }
    }
}
