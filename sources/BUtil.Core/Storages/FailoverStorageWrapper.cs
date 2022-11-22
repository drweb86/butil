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
