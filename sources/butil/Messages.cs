using BUtil.Core.Localization;
using System.Windows.Forms;

namespace BUtil.Configurator
{
    public static class Messages
    {
        public static void ShowInformationBox(string message)
        {
            MessageBox.Show(message,  Resources.InformationButil, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
        }

        public static void ShowErrorBox(string message)
        {
            MessageBox.Show(message, Resources.ErrorButil, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
        }

        public static bool ShowYesNoDialog(string question)
        {
            return MessageBox.Show(question, Resources.QuestionButil, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, 0) == DialogResult.Yes;
        }
    }
}
