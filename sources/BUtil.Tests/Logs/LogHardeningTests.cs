using BUtil.Core.Logs;
using System.Reflection;

namespace BUtil.Tests.Logs;

[TestClass]
public class LogHardeningTests
{
    [TestMethod]
    public void FileLog_Close_WhenRenameFails_DetachesWriterAndReleasesFileHandle()
    {
        var taskName = "log-hardening-" + Guid.NewGuid().ToString("N");
        var log = new FileLog(taskName);
        log.Open();
        log.WriteLine(LoggingEvent.Debug, "before-failure");

        var dateTime = ReadPrivateField<DateTime>(log, "_dateTime");
        var sourceFile = ReadPrivateField<string>(log, "_fileName");
        var occupiedTarget = LogService.GetFileName(taskName, dateTime, isSuccess: true);
        File.WriteAllText(occupiedTarget, "occupied");

        try
        {
            _ = ExpectThrows<LogException>(() => log.Close(true));

            // After failed close, logger must be detached and safe to call again.
            log.WriteLine(LoggingEvent.Error, "must-not-throw");
            log.Close(false);

            // Writer must be closed even when rename fails.
            File.Delete(sourceFile);
            Assert.IsFalse(File.Exists(sourceFile));
        }
        finally
        {
            if (File.Exists(sourceFile))
                File.Delete(sourceFile);
            if (File.Exists(occupiedTarget))
                File.Delete(occupiedTarget);
        }
    }

    [TestMethod]
    public void MemoryLog_ConcurrentWrites_PreservesAllEntries()
    {
        var log = new MemoryLog();
        const int writesCount = 4096;

        Parallel.For(0, writesCount, i => log.WriteLine(LoggingEvent.Debug, $"message-{i}"));

        var lines = log
            .ToString()
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Where(x => x.StartsWith("Debug ", StringComparison.Ordinal))
            .ToArray();

        Assert.AreEqual(writesCount, lines.Length);
    }

    private static T ReadPrivateField<T>(object instance, string fieldName)
    {
        var field = instance
            .GetType()
            .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.IsNotNull(field, $"Missing private field '{fieldName}'.");
        var value = field.GetValue(instance);
        Assert.IsNotNull(value, $"Field '{fieldName}' is null.");
        return (T)value;
    }

    private static TException ExpectThrows<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action();
            Assert.Fail("Exception expected.");
        }
        catch (TException ex)
        {
            return ex;
        }

        throw new InvalidOperationException("Exception expected.");
    }
}
