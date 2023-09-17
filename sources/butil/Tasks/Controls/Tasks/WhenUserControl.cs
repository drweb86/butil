using System;
using System.Linq;
using BUtil.Core.Localization;
using BUtil.Core.Options;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// Setups scheduler
	/// </summary>
	internal sealed partial class WhenUserControl : BUtil.Core.PL.BackUserControl
	{
		private ScheduleInfo _scheduleInfo;

		public WhenUserControl()
		{
			InitializeComponent();
		}
		
		#region Overrides

		public override void ApplyLocalization() 
		{
			chooseDaysOfWeekLabel.Text = Resources.Days_Field_Choose;
            scheduledDaysCheckedListBox.Items[2] = Resources.Days_Wednesday;
            scheduledDaysCheckedListBox.Items[3] = Resources.Days_Thursday;
            scheduledDaysCheckedListBox.Items[4] = Resources.Days_Friday;
            scheduledDaysCheckedListBox.Items[5] = Resources.Days_Saturday;
            scheduledDaysCheckedListBox.Items[1] = Resources.Days_Tuesday;
            scheduledDaysCheckedListBox.Items[0] = Resources.Days_Monday;
            scheduledDaysCheckedListBox.Items[6] = Resources.Days_Sunday;
            hourLabel.Text = Resources.Time_Field_Hour;
            minuteLabel.Text = Resources.Time_Field_Minute;
		}
	
		public override void SetOptionsToUi(object settings)
		{
			_scheduleInfo = (ScheduleInfo)settings;

            foreach (DayOfWeek enumItem in DayOfWeek.GetValues(typeof(DayOfWeek)))
			{
				scheduledDaysCheckedListBox.SetItemChecked((int) enumItem, _scheduleInfo.Days.Contains(enumItem));
			}
            
            hourComboBox.SelectedIndex = _scheduleInfo.Time.Hours;
            minuteComboBox.SelectedIndex = _scheduleInfo.Time.Minutes;
            
		}
		
		public override void GetOptionsFromUi()
		{
            _scheduleInfo.Days = Enum
				.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Where(x => scheduledDaysCheckedListBox.GetItemChecked((int)x))
				.ToList();

            _scheduleInfo.Time = new TimeSpan(
				hourComboBox.SelectedIndex != -1 ? hourComboBox.SelectedIndex : 0,
                minuteComboBox.SelectedIndex != -1 ? minuteComboBox.SelectedIndex : 0,
				0);
		}
		
		#endregion
	}
}
