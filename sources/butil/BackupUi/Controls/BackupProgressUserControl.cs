using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using BUtil.Configurator.Localization;

namespace BUtil.BackupUiMaster.Controls
{
	/// <summary>
	/// Shows state of backup process
	/// </summary>
	internal sealed partial class BackupProgressUserControl : TitledBackUserControl
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
				return string.Format(CultureInfo.InstalledUICulture, Resources._0DayS1H2Min3Sec, timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			else
			{
				if (timeSpan.Hours > 0)
					//{0} h : {1} min : {2} sec
					return string.Format(CultureInfo.InstalledUICulture, Resources._0H1Min2Sec, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				else
				{
					if (timeSpan.Minutes > 0)
						//{0} min : {1} sec
						return string.Format(CultureInfo.InstalledUICulture, Resources._0Min1Sec, timeSpan.Minutes, timeSpan.Seconds);
					else
						//{0} sec
						return string.Format(CultureInfo.InstalledUICulture, Resources._0Sec, timeSpan.Seconds);
				}
			}
		}
		
		#region Locals
		
		public override void ApplyLocalization()
		{
			passedLabel.Text = Resources.Elapsed;
			Title = Resources.BackupIsInAProgress;
		}
		
		#endregion
	}
}
