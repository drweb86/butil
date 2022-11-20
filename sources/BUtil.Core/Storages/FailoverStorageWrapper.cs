using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;

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

        public void Download(StorageFile storageFile, string targetFileName)
        {
            ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Download \"{storageFile.StorageRelativeFileName}\" to \"{targetFileName}\""),
                () => _storage.Download(storageFile, targetFileName));
        }

        public byte[] ReadAllBytes(string file)
        {
            return ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": ReadAllBytes \"{file}\""),
                () => _storage.ReadAllBytes(file));
        }

        public string ReadAllText(string file)
        {
            return ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": ReadAllText \"{file}\""),
                () => _storage.ReadAllText(file));
        }

        public string Test()
        {
            return ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Test"),
                () => _storage.Test());
        }

        public IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            return ExecuteFailover.TryNTimes(
                error => _log.WriteLine(LoggingEvent.Error, $"Storage \"{_settings.Name}\": Upload \"{sourceFile}\" to \"{relativeFileName}\""),
                () => _storage.Upload(sourceFile, relativeFileName));
        }
    }
}
