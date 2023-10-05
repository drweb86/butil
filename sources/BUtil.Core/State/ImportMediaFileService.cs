#nullable disable
using System;
using System.IO;
using System.Text.Json;

namespace BUtil.Core.State
{
    public class ImportMediaFileService
    {
        private readonly string _folder;
        private const string _extension = ".json";

        public ImportMediaFileService()
        {
#if DEBUG
            _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - DEBUG - States");
#else
            _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - States");
#endif
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);
        }

        public ImportMediaState Load(string name)
        {
            var fileName = GetFileName(name);
            if (!File.Exists(fileName))
                return null;
            var json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<ImportMediaState>(json);
        }


        public void Save(ImportMediaState task, string name)
        {
            var fileName = GetFileName(name);
            var json = JsonSerializer.Serialize(task, new JsonSerializerOptions { WriteIndented = true });
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, json);
        }

        private string GetFileName(string name)
        {
            if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
                throw new ArgumentException(nameof(name));

            return Path.Combine(_folder, $"{name}{_extension}");
        }
    }
}
