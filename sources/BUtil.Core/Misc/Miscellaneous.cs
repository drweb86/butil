using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Misc
{
    public static class Miscellaneous
    {
        /// <summary>
        /// Outputs sound to console
        /// </summary>
        public static void DoBeeps()
        {
            // Microwave - your backup tost is ready!
            Console.Beep(1000, 400);
            System.Threading.Thread.Sleep(500);
            Console.Beep(1000, 400);
            System.Threading.Thread.Sleep(500);
            Console.Beep(1000, 400);
            System.Threading.Thread.Sleep(500);
            Console.Beep(1000, 400);
            System.Threading.Thread.Sleep(500);
            Console.Beep(1000, 400);
            System.Threading.Thread.Sleep(500);
        }

	}
}
