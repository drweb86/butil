using MediaDevices;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core
{
    public class MtpService
    {
        public IEnumerable<string> GetItems()
        {
            return MediaDevice.GetDevices()
                .Select(x => x.FriendlyName)
                .Order()
                .ToList();
        }
    }
}
