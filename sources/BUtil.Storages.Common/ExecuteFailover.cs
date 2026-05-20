using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace BUtil.Core.Misc;

public class ExecuteFailover
{
    public static void TryNTimes(Action<string> logError, Action func, int times = 10, int retryDelayMs = 30 * 1000)
    {
        Exception? lastError = null;
        while (times > 0)
        {
            try
            {
                func();
                return;
            }
            catch (Exception e)
            {
                if (!IsTransient(e))
                    throw;

                lastError = e;
                logError(e.ToString());
                if (--times <= 0)
                    throw new InvalidOperationException("No more attempts left", lastError);
                logError("Retrying in 30 seconds");
                if (retryDelayMs > 0)
                    Thread.Sleep(retryDelayMs);
            }
        }

        throw new InvalidOperationException("No more attempts left", lastError);
    }

    public static T TryNTimes<T>(Action<string> logError, Func<T> func, int times = 10, int retryDelayMs = 30 * 1000)
    {
        Exception? lastError = null;
        while (times > 0)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                if (!IsTransient(e))
                    throw;

                lastError = e;
                logError(e.ToString());
                if (--times <= 0)
                    throw new InvalidOperationException("No more attempts left", lastError);
                logError("Retrying in 30 seconds");
                if (retryDelayMs > 0)
                    Thread.Sleep(retryDelayMs);
            }
        }

        throw new InvalidOperationException("No more attempts left", lastError);
    }

    private static bool IsTransient(Exception e)
    {
        return e is IOException
            || e is TimeoutException
            || e is SocketException;
    }
}
