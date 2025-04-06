using BUtil.Core.Services;

namespace BUtil.Linux.Services;

internal class AndroidOsSleepPreventionService : IOsSleepPreventionService
{
    public void PreventSleep()
    {
        // search for systemd-inhibit
    }

    public void StopPreventSleep()
    {
        // search for systemd-inhibit
    }
}
