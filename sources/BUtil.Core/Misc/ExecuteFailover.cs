using System;
using System.Threading;

namespace BUtil.Core.Misc;

public class ExecuteFailover
{
    public static void TryNTimes(Action<string> logError, Action func, int times = 10)
    {
        while (times > 0)
        {
            try
            {
                func();
                return;
            }
            catch (Exception e)
            {
                logError(e.ToString());
                if (--times <= 0)
                    throw new InvalidOperationException("No more attempts left");
                logError("Retrying in 30 seconds");
                Thread.Sleep(30 * 1000);
            }
        }
    }

    public static T TryNTimes<T>(Action<string> logError, Func<T> func, int times = 10)
    {
        while (times > 0)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                logError(e.ToString());
                if (--times <= 0)
                    throw new InvalidOperationException("No more attempts left");
                logError("Retrying in 30 seconds");
                Thread.Sleep(30 * 1000);
            }
        }
        throw new NotImplementedException();
    }
}
