using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using BUtil.Core.Localization;

namespace BUtil.BackupUiMaster.Controls
{
    internal sealed partial class TaskProgressUserControl : TitledBackUserControl
    {
        DateTime _start = DateTime.Now;

        public TaskProgressUserControl()
        {
            InitializeComponent();

            passedLabel.Text = Resources.Time_Field_Elapsed;
        }

        public void SetProgress(int ended, int total)
        {
            progressBar.Maximum = total;
            progressBar.Value = ended;
        }

        public void Start()
        {
            _start = DateTime.Now;
            clockTimer.Enabled = true;
            Title = Resources.Task_Status_InProgress;
        }

        public void Stop(string lastMinuteMessage, string generalStatus, bool isError)
        {
            clockTimer.Enabled = false;
            clockTimer.Stop();

            Title = $"{generalStatus} ({timeSpanToStringHelper(DateTime.Now.Subtract(_start))})";
            if (isError)
            {
                TitleBackground = Color.FromArgb(222, 98, 89);
            }
            else
            {
                TitleBackground = Color.FromArgb(5, 150, 14);
            }
            passedLabel.Visible = false;
            elapsedLabel.Text = lastMinuteMessage;
        }

        private void timerTick(object sender, EventArgs e)
        {
            TimeSpan span = DateTime.Now.Subtract(_start);
            elapsedLabel.Text = $"{timeSpanToStringHelper(span)} ({progressBar.Value}/{progressBar.Maximum})";
        }

        string timeSpanToStringHelper(TimeSpan timeSpan)
        {
            if (timeSpan.Days > 0)
                return $"{timeSpan.Days}:{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            else
            {
                if (timeSpan.Hours > 0)
                    return $"{timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
                else
                {
                    if (timeSpan.Minutes > 0)
                        return $"{timeSpan.Minutes}:{timeSpan.Seconds:00}";
                    else
                        return $"{timeSpan.Seconds}";
                }
            }
        }

        #region Locals

        public override void ApplyLocalization()
        {
            
        }

        #endregion
    }
}
