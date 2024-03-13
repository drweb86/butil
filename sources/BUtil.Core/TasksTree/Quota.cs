namespace BUtil.Core.TasksTree;

internal class Quota
{
    private long _quota;
    private readonly object _lock = new();

    public Quota(long quota = 0)
    {
        _quota = quota == 0 ? long.MaxValue : quota;
    }

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
