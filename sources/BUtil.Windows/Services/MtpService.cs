using BUtil.Core;
using MediaDevices;

namespace BUtil.Windows.Services;

class MtpService : IMtpService
{
    public IEnumerable<string> GetItems()
    {
        return [.. MediaDevice.GetDevices()
            .Select(x => x.FriendlyName)
            .Order()];
    }
}
