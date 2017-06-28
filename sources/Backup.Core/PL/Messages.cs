using System;
using System.Windows.Forms;

using BULocalization;

namespace BUtil.Core.PL
{
	/// <summary>
	/// Provides abilities for showing information box and errors
	/// </summary>
	public static class Messages
	{
		public static void ShowInformationBox(string message)
		{
			MessageBox.Show(message, Translation.Current[61], MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
		}
		
		public static void ShowErrorBox(string message)
		{
			MessageBox.Show(message, Translation.Current[58], MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
		}
		
		public static bool ShowYesNoDialog(string question)
		{
			return MessageBox.Show(question, Translation.Current[618], MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, 0) == DialogResult.Yes;
		}
	}
}
