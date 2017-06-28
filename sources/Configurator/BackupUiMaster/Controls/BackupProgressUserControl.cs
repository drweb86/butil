using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

using BULocalization;

namespace BUtil.BackupUiMaster.Controls
{
	/// <summary>
	/// Shows state of backup process
	/// </summary>
	internal sealed partial class BackupProgressUserControl : BackUserControl
	{
		DateTime _start = DateTime.Now;
		
		public BackupProgressUserControl()
		{
			InitializeComponent();
		}
		
		public void Stop()
		{
			_start = DateTime.Now;
			clockTimer.Enabled = false;
			progressBar.Style = ProgressBarStyle.Blocks;
		}
		
		void timerTick(object sender, EventArgs e)
		{
			TimeSpan span = DateTime.Now.Subtract(_start);
			elapsedLabel.Text = timeSpanToStringHelper(span);
		}
		
		string timeSpanToStringHelper(TimeSpan timeSpan)
		{
			if (timeSpan.Days > 0)
				//{0} day(s) {1} h : {2} min : {3} sec
				return string.Format(CultureInfo.InstalledUICulture, Translation.Current[505], timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			else
			{
				if (timeSpan.Hours > 0)
					//{0} h : {1} min : {2} sec
					return string.Format(CultureInfo.InstalledUICulture, Translation.Current[506], timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				else
				{
					if (timeSpan.Minutes > 0)
						//{0} min : {1} sec
						return string.Format(CultureInfo.InstalledUICulture, Translation.Current[507], timeSpan.Minutes, timeSpan.Seconds);
					else
						//{0} sec
						return string.Format(CultureInfo.InstalledUICulture, Translation.Current[508], timeSpan.Seconds);
				}
			}
		}
		
		#region Locals
		
		public override void ApplyLocalization()
		{
			passedLabel.Text = Translation.Current[191];
			Title = Translation.Current[193];
		}
		
		#endregion
	}
}
