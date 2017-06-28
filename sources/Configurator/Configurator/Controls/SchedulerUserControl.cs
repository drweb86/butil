using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BULocalization;
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
			chooseDaysOfWeekLabel.Text = Translation.Current[126];
            scheduledDaysCheckedListBox.Items[2] = Translation.Current[127];
            scheduledDaysCheckedListBox.Items[3] = Translation.Current[128];
            scheduledDaysCheckedListBox.Items[4] = Translation.Current[129];
            scheduledDaysCheckedListBox.Items[5] = Translation.Current[130];
            scheduledDaysCheckedListBox.Items[1] = Translation.Current[131];
            scheduledDaysCheckedListBox.Items[0] = Translation.Current[132];
            scheduledDaysCheckedListBox.Items[6] = Translation.Current[133];
            minuteLabel.Text = Translation.Current[134];
            hourLabel.Text = Translation.Current[135];
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
