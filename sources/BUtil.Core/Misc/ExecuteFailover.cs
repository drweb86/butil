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
                logError("Retrying in 30 seconds");
                Thread.Sleep(30 * 1000);
                if (--times <= 0)
                    throw;
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
                logError("Retrying in 30 seconds");
                Thread.Sleep(30 * 1000);
                if (--times <= 0)
                    throw;
            }
        }
        throw new NotImplementedException();
    }
}
