using BUtil.Core.Misc;
using System.Globalization;

namespace BUtil.Core
{
	public static class CopyrightInfo
	{
        const string AppVersion = "5.0";
        static readonly string CopyRightNotice =
            string.Format(CultureInfo.CurrentCulture, "BUtil - {0}\n{1}\n", AppVersion, SupportManager.GetLink(SupportRequest.Homepage));

	    public static string Copyright
	    {
	    	get { return CopyRightNotice; }
	    }
        
	    public static string Version
	    {
            get { return AppVersion; }
	    }
    }
}
