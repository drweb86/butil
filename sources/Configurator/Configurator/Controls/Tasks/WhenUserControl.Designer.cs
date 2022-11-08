
namespace BUtil.Configurator.Controls
{
	partial class WhenUserControl
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.minuteComboBox = new System.Windows.Forms.ComboBox();
            this.chooseDaysOfWeekLabel = new System.Windows.Forms.Label();
            this.hourComboBox = new System.Windows.Forms.ComboBox();
            this.scheduledDaysCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.minuteLabel = new System.Windows.Forms.Label();
            this.hourLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // minuteComboBox
            // 
            this.minuteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.minuteComboBox.FormattingEnabled = true;
            this.minuteComboBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.minuteComboBox.Location = new System.Drawing.Point(222, 25);
            this.minuteComboBox.Name = "minuteComboBox";
            this.minuteComboBox.Size = new System.Drawing.Size(78, 21);
            this.minuteComboBox.TabIndex = 2;
            // 
            // chooseDaysOfWeekLabel
            // 
            this.chooseDaysOfWeekLabel.AutoSize = true;
            this.chooseDaysOfWeekLabel.Location = new System.Drawing.Point(12, 9);
            this.chooseDaysOfWeekLabel.Name = "chooseDaysOfWeekLabel";
            this.chooseDaysOfWeekLabel.Size = new System.Drawing.Size(109, 13);
            this.chooseDaysOfWeekLabel.TabIndex = 23;
            this.chooseDaysOfWeekLabel.Text = "Choose days of week";
            // 
            // hourComboBox
            // 
            this.hourComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hourComboBox.FormattingEnabled = true;
            this.hourComboBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.hourComboBox.Location = new System.Drawing.Point(139, 25);
            this.hourComboBox.Name = "hourComboBox";
            this.hourComboBox.Size = new System.Drawing.Size(74, 21);
            this.hourComboBox.TabIndex = 1;
            // 
            // scheduledDaysCheckedListBox
            // 
            this.scheduledDaysCheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scheduledDaysCheckedListBox.CheckOnClick = true;
            this.scheduledDaysCheckedListBox.FormattingEnabled = true;
            this.scheduledDaysCheckedListBox.Items.AddRange(new object[] {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"});
            this.scheduledDaysCheckedListBox.Location = new System.Drawing.Point(15, 25);
            this.scheduledDaysCheckedListBox.Name = "scheduledDaysCheckedListBox";
            this.scheduledDaysCheckedListBox.Size = new System.Drawing.Size(118, 107);
            this.scheduledDaysCheckedListBox.TabIndex = 0;
            // 
            // minuteLabel
            // 
            this.minuteLabel.AutoSize = true;
            this.minuteLabel.Location = new System.Drawing.Point(219, 9);
            this.minuteLabel.Name = "minuteLabel";
            this.minuteLabel.Size = new System.Drawing.Size(42, 13);
            this.minuteLabel.TabIndex = 20;
            this.minuteLabel.Text = "Minute:";
            // 
            // hourLabel
            // 
            this.hourLabel.AutoSize = true;
            this.hourLabel.Location = new System.Drawing.Point(139, 9);
            this.hourLabel.Name = "hourLabel";
            this.hourLabel.Size = new System.Drawing.Size(33, 13);
            this.hourLabel.TabIndex = 19;
            this.hourLabel.Text = "Hour:";
            // 
            // SchedulerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.minuteComboBox);
            this.Controls.Add(this.chooseDaysOfWeekLabel);
            this.Controls.Add(this.hourComboBox);
            this.Controls.Add(this.scheduledDaysCheckedListBox);
            this.Controls.Add(this.minuteLabel);
            this.Controls.Add(this.hourLabel);
            this.MinimumSize = new System.Drawing.Size(311, 167);
            this.Name = "SchedulerUserControl";
            this.Size = new System.Drawing.Size(311, 167);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		private System.Windows.Forms.Label hourLabel;
		private System.Windows.Forms.Label minuteLabel;
		private System.Windows.Forms.CheckedListBox scheduledDaysCheckedListBox;
		private System.Windows.Forms.ComboBox hourComboBox;
		private System.Windows.Forms.Label chooseDaysOfWeekLabel;
		private System.Windows.Forms.ComboBox minuteComboBox;
	}
}
