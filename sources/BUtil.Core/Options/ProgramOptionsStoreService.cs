using System.IO;
using System.Text.Json;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Options
{
    public class ProgramOptionsStoreService
    {
        public ProgramOptions Load()
        {
            var fileName = GetFileName();
            if (!File.Exists(fileName))
                return ProgramOptionsManager.Default;
            var json = File.ReadAllText(fileName);
            try
            {
                return JsonSerializer.Deserialize<ProgramOptions>(json);
            }
            catch
            {
                return ProgramOptionsManager.Default;
            }
        }


        public void Save(ProgramOptions programOptions)
        {
            var fileName = GetFileName();
            var json = JsonSerializer.Serialize(programOptions
                , new JsonSerializerOptions { WriteIndented = true });
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, json);
        }

        private string GetFileName()
        {
            return Path.Combine(Directories.UserDataFolder, "options.json");
        }
    }
}
