using System;
using System.Globalization;
using System.Reflection;

namespace BUtil.Core
{
	public static class CopyrightInfo
	{
	    public static string Copyright { get; }
        
	    public static Version Version { get; }

		static CopyrightInfo()
		{
			Version = Assembly
				.GetExecutingAssembly()
                .GetName()
				.Version;

            Copyright = string.Format(CultureInfo.CurrentUICulture, "BUtil {0} : Copyright (c) 2010-2023 Siarhei Kuchuk", Version);
        }
    }
}
