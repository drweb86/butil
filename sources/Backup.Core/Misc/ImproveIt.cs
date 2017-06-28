using System;
using System.IO;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using BULocalization;
using BUtil.Core.FileSystem;
using BUtil.Core.PL;

namespace BUtil.Core.Misc
{
    /// <summary>
    /// This class is for handling exceptions
    /// and improving logic of program
    /// </summary>
    public static class ImproveIt
    {
        static bool _showMessageBox = true;

        public static void ProcessUnhandledException(Exception exception)
        { 
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine("BUtil " + CopyrightInfo.Version + " - Bug report (" + DateTime.Now.ToString("g", CultureInfo.InvariantCulture) + ")");
                builder.AppendLine("Please report about it here: ");
                builder.AppendLine("https://sourceforge.net/tracker/?func=add&group_id=195114&atid=952141");
                builder.AppendLine(exception.Message);
                builder.AppendLine(exception.StackTrace);
                builder.AppendLine(exception.Source);
                
                Exception inner = exception.InnerException;
                if (inner != null)
                {
                    builder.AppendLine(inner.Message);
                    builder.AppendLine(inner.StackTrace);
                    builder.AppendLine(inner.Source);
                }
                
                File.AppendAllText(Files.BugReportFile, builder.ToString());
                
                if (_showMessageBox)
                	Messages.ShowErrorBox(string.Format(Translation.Current[590], Files.BugReportFile));
            }
            finally
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Inits infrastructure for improving console or winForms tool. If any unhandled exception occurs
        /// an error report will be generated and stored on desktop for user
        /// </summary>
        /// <param name="showMessageBox"></param>
        /// <remarks>Due to MS bugs in implementation 
        /// of catching unhandled exceptions in console applications
        /// please use the next pattern 
        /// <code>ImproveIt.InitInfrastructure(false); 
        /// try {...} 
        /// catch (Exception unhandledException) 
        /// {
        ///   ImproveIt.ProcessUnhandledException(unhandledException);
        /// }</code>
        /// </remarks>
        public static void InitInfrastructure(bool showMessageBox)
        {
            _showMessageBox = showMessageBox;
            
             AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(
                    unhandledException);
            
        }

        private static void unhandledException(object sender, UnhandledExceptionEventArgs exception)
        {
            ProcessUnhandledException(exception.ExceptionObject as Exception);
        }
    }
}
