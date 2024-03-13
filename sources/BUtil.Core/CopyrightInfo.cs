using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("BUtil.Tests")]

namespace BUtil.Core;

public static class CopyrightInfo
{
    public static string Copyright { get; }
        
    public static Version Version { get; }

	static CopyrightInfo()
	{
            Version = Assembly
			.GetExecutingAssembly()
                .GetName()
			.Version ?? throw new InvalidProgramException("Failed to get assembly from !");

            Copyright = string.Format(CultureInfo.CurrentUICulture, "BUtil {0} : Copyright (c) 2010-{1} Siarhei Kuchuk", Version, DateTime.Now.Year);
        }
    }
