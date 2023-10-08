using System.Collections.Generic;

namespace BUtil.Core
{
    public interface IMtpService
    {
        IEnumerable<string> GetItems();
    }
}
