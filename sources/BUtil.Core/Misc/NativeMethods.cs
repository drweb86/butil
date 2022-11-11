using System;
using System.Runtime.InteropServices;

namespace BUtil.Core.Misc
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void SetWindowVisibility(bool visible, string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            IntPtr hWnd = NativeMethods.FindWindow(null, title);

            if (hWnd != IntPtr.Zero)
            {
                if (!visible)
                    //Hide the window                   
                    NativeMethods.ShowWindow(hWnd, 0); // 0 = SW_HIDE               
                else
                    //Show window again                   
                    NativeMethods.ShowWindow(hWnd, 1); //1 = SW_SHOWNORMA          
            }
        }
    }
}
