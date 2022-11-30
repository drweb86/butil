using System;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BUtil.RestorationMaster
{
	internal partial class ProgressForm : Form
	{
        private readonly Action<Action<int>> _action;

        public ProgressForm(Action<Action<int>> action)
		{
			InitializeComponent();
            this.Text = Resources.RestorationMaster;
            Height = _progressBar.Height + _progressBar.Location.Y + 50;
            Width = _progressBar.Width + _progressBar.Location.X * 2 + 20;
            _backgroundWorker.RunWorkerAsync();
            _action = action;
        }

        private void OnDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _action(percent => _backgroundWorker.ReportProgress(percent));
        }

        private void OnWorkCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            _progressBar.Value = e.ProgressPercentage;
        }
    }
}
