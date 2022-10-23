using System;
using System.ComponentModel;
using System.Drawing;
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
				scheduledDaysCheckedListBox.SetItemChecked((int) enumItem, _task.IsThisDayOfWeekScheduled(enumItem));
			}
            
            hourComboBox.SelectedIndex = _task.Hours;
            minuteComboBox.SelectedIndex = _task.Minutes;
            
		}
		
		public override void GetOptionsFromUi()
		{
			foreach(DayOfWeek enumItem in DayOfWeek.GetValues(typeof(DayOfWeek)))
			{
				_task.SetSchedulingStateOfDay(enumItem, scheduledDaysCheckedListBox.GetItemChecked((int) enumItem));
			}
            
            if (hourComboBox.SelectedIndex > -1)
            {
                _task.Hours = (byte)hourComboBox.SelectedIndex;
            }
            else
            {
                _task.Hours = 0;
            }

            if (minuteComboBox.SelectedIndex > -1)
            {
                _task.Minutes = (byte)minuteComboBox.SelectedIndex;
            }
            else
            {
                _task.Minutes = 0;
            }
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
