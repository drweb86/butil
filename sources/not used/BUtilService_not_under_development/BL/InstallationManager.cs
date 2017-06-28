#region Copyright
/*
 * Copyright (c)Cuchuk Sergey Alexandrovich, 2007-2008. All rights reserved
 * Project: BUtil
 * Link: http://www.sourceforge.net/projects/butil
 * License: GNU GPL or SPL with limitations
 * E-mail:
 * Cuchuk.Sergey@gmail.com
 * toCuchukSergey@yandex.ru
 */
#endregion


using System;
using System.Reflection;
using System.Configuration.Install;
using System.Collections.Specialized;
using System.Collections;
using BUtil.Common.FileSystem;
using System.IO;

namespace BUutilService.BL
{
    public static class InstallationManager
    {
        private static readonly string _logPath = Directories.TempFolder + @"\BUtil.tmp";
        private static readonly IDictionary _SavedState = new Hashtable();
        private static readonly string[] _commandLineOptions = new string[1] { "/LogFile=" + _logPath };
        private static readonly AssemblyInstaller _AssemblyInstaller;

        static InstallationManager()
        { 
            _AssemblyInstaller = new
                AssemblyInstaller(Assembly.GetExecutingAssembly(), _commandLineOptions);
            _AssemblyInstaller.UseNewContext = true;
        }

        public static void InstallMe()
        {
            int result = 0;
            try
            {
                _AssemblyInstaller.Install(_SavedState);
                _AssemblyInstaller.Commit(_SavedState);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = -1;
            }
            finally
            {
                File.Delete(_logPath);
                Environment.Exit(result);
            }
        }

        public static void UninstallMe()
        {
            int result = 0;
            try
            {
                _AssemblyInstaller.Uninstall(_SavedState);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = -1;
            }
            finally
            {
                File.Delete(_logPath);
                Environment.Exit(result);
            }
        }
    }
}
