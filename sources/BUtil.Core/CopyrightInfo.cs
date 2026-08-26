using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BUtil.Tests")]

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

        Copyright = string.Format(CultureInfo.CurrentUICulture, "BUtil {0} : CC0 1.0 Universal (Siarhei Kuchuk, 2010-{1})", Version, DateTime.Now.Year);
    }
}
