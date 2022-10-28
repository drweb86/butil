using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// Setups scheduler
	/// </summary>
	internal sealed partial class SchedulerUserControl : BUtil.Core.PL.BackUserControl
	{
		BackupTask _task;
		
		public SchedulerUserControl()
		{
			InitializeComponent();
		}
		
		#region Overrides

		public override void ApplyLocalization() 
		{
			chooseDaysOfWeekLabel.Text = Resources.ChooseDaysOfWeek;
            scheduledDaysCheckedListBox.Items[2] = Resources.Wednesday;
            scheduledDaysCheckedListBox.Items[3] = Resources.Thursday;
            scheduledDaysCheckedListBox.Items[4] = Resources.Friday;
            scheduledDaysCheckedListBox.Items[5] = Resources.Saturday;
            scheduledDaysCheckedListBox.Items[1] = Resources.Tuesday;
            scheduledDaysCheckedListBox.Items[0] = Resources.Monday;
            scheduledDaysCheckedListBox.Items[6] = Resources.Sunday;
            minuteLabel.Text = Resources.Minute;
            hourLabel.Text = Resources.Hour;
		}
	
		public override void SetOptionsToUi(object settings)
		{
            _task = (BackupTask)settings;
			
			foreach(DayOfWeek enumItem in DayOfWeek.GetValues(typeof(DayOfWeek)))
			{
				scheduledDaysCheckedListBox.SetItemChecked((int) enumItem, _task.SchedulerDays.Contains(enumItem));
			}
            
            hourComboBox.SelectedIndex = _task.SchedulerTime.Hours;
            minuteComboBox.SelectedIndex = _task.SchedulerTime.Minutes;
            
		}
		
		public override void GetOptionsFromUi()
		{
			_task.SchedulerDays = Enum
				.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Where(x => scheduledDaysCheckedListBox.GetItemChecked((int)x))
				.ToList();

			_task.SchedulerTime = new TimeSpan(
				hourComboBox.SelectedIndex != -1 ? hourComboBox.SelectedIndex : 0,
                minuteComboBox.SelectedIndex != -1 ? minuteComboBox.SelectedIndex : 0,
				0);
		}
		
		#endregion
		
		public void ResetScheduler()
		{
			for (int i = 0; i < 7; i++)
			{
				scheduledDaysCheckedListBox.SetItemChecked(i, false);
			}
		}
	}
}
