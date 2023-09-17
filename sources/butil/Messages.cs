using BUtil.Core.Localization;
using System.Windows.Forms;

namespace BUtil.Configurator
{
    public static class Messages
    {
        public static void ShowInformationBox(string message)
        {
            MessageBox.Show(message,  Resources.Messages_Header_Information, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
        }

        public static void ShowErrorBox(string message)
        {
            MessageBox.Show(message, Resources.Messages_Header_Error, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
        }

        public static bool ShowYesNoDialog(string question)
        {
            return MessageBox.Show(question, Resources.Messages_Header_Question, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, 0) == DialogResult.Yes;
        }
    }
}
