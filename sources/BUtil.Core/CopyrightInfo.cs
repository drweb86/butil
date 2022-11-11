using BUtil.Core.Misc;
using System;
using System.Globalization;
using System.Reflection;

namespace BUtil.Core
{
	public static class CopyrightInfo
	{
        readonly static Version _appVersion;
        static readonly string _copyRightNotice;

	    public static string Copyright
	    {
	    	get { return _copyRightNotice; }
	    }
        
	    public static Version Version
	    {
            get { return _appVersion; }
	    }

		static CopyrightInfo()
		{
            _appVersion = Assembly
				.GetExecutingAssembly()
                .GetName()
				.Version;

            _copyRightNotice = string.Format(CultureInfo.CurrentUICulture, "BUtil - {0}\n{1}\n", _appVersion, SupportManager.GetLink(SupportRequest.Homepage));
        }
    }
}
