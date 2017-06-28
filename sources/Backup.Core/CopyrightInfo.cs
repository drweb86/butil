using System.Globalization;

namespace BUtil.Core
{
	public static class CopyrightInfo
	{
        const string AppVersion = "5.0";// version can contain only 2 numbers due to implementation mechanizm for update
        static readonly string CopyRightNotice =
            string.Format(CultureInfo.CurrentCulture, "BUtil - {0}\nhttp://sourceforge.net/projects/butil/\nhttp://butil.codeplex.com\n", AppVersion);

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
