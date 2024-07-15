namespace BUtil.Core.TasksTree;

internal class Quota(long quota = 0)
{
    private long _quota = quota == 0 ? long.MaxValue : quota;
    private readonly object _lock = new();

    public bool TryQuota(long quotaDemand)
    {
        lock (_lock)
        {
            if (quotaDemand <= _quota)
            {
                _quota -= quotaDemand;
                return true;
            }
            return false;
        }
    }
}
