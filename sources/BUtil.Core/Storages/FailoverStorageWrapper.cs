using BUtil.Core.Logs;
using BUtil.Core.Misc;

namespace BUtil.Core.Storages
{
    public class FailoverStorageWrapper : IStorage
    {
        private readonly ILog _log;
        private readonly IStorage _storage;
        private readonly IStorageSettings _settings;

        public FailoverStorageWrapper(ILog log, IStorage storage, IStorageSettings settings)
        {
            _log = log;
            _storage = storage;
            _settings = settings;
        }

        public void Delete(string file)
        {
            ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Delete \"{file}\""),
                () => _storage.Delete(file));
        }

        public void Dispose()
        {
            _storage.Dispose();
        }

        public void Download(string relativeFileName, string targetFileName)
        {
            ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Download \"{relativeFileName}\" to \"{targetFileName}\""),
                () => _storage.Download(relativeFileName, targetFileName));
        }

        public bool Exists(string relativeFileName)
        {
            return ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Exists \"{relativeFileName}\""),
                () => _storage.Exists(relativeFileName));
        }

        public void DeleteFolder(string relativeFolderName)
        {
            ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Delete folder \"{relativeFolderName}\""),
                () => _storage.DeleteFolder(relativeFolderName));
        }

        public string[] GetFolders(string relativeFolderName, string mask = null)
        {
            return ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Get folders \"{relativeFolderName}\" by mask \"{mask}\""),
                () => _storage.GetFolders(relativeFolderName, mask));
        }

        public string Test()
        {
            return _storage.Test();
        }

        public IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            return ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Upload \"{sourceFile}\" to \"{relativeFileName}\""),
                () => _storage.Upload(sourceFile, relativeFileName));
        }
    }
}
