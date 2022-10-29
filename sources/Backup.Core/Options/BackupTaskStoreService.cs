using BUtil.Core.FileSystem;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using BUtil.Core.PL;

namespace BUtil.Core.Options
{
    public class BackupTaskStoreService
    {
        private readonly string _folder;
        private const string _extension = ".BUtil Backup Task v1";

        public BackupTaskStoreService()
        {
            _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks");
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);
        }

        public BackupTask Load(string name)
        {
            var fileName = GetFileName(name);
            if (!File.Exists(fileName))
                return null;
            var json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<BackupTask>(json);
        }

        public IEnumerable<BackupTask> LoadAll()
        {
            return GetNames()
                .Select(this.Load)
                .ToList();
        }

        public IEnumerable<BackupTask> Load(IEnumerable<string> taskNames, out IEnumerable<string> missingTasks)
        {
            var tasks = new List<BackupTask>();
            var missingTasksList = new List<string>();
            foreach (var taskName in taskNames)
            {
                var task = Load(taskName);
                if (task == null)
                    missingTasksList.Add(taskName);
                else
                    tasks.Add(task);
            }
            missingTasks = missingTasksList;
            return tasks;
        }

        public void Save(BackupTask task)
        {
            var fileName = GetFileName(task.Name);
            var json = JsonSerializer.Serialize(task);
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, json);
        }

        public void Delete(string name)
        {
            var fileName = GetFileName(name);
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        public IEnumerable<string> GetNames()
        {
            return Directory
                .GetFiles(_folder, $"*{_extension}")
                .Select(Path.GetFileNameWithoutExtension)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private string GetFileName(string name)
        {
            if (name.Contains("..") || name.Contains("/") || name.Contains("\\"))
                throw new ArgumentException(nameof(name));

            return Path.Combine(_folder, $"{name}{_extension}");
        }
    }
}
