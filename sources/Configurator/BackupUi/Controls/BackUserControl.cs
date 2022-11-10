using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using BUtil.Core.Options;

namespace BUtil.BackupUiMaster.Controls
{
	/// <summary>
	/// Default control for most others controls
	/// </summary>
	internal partial class BackUserControl : BUtil.Core.PL.BackUserControl
	{
		public string Title
		{
			get { return titleLabel.Text; }
			set { titleLabel.Text = value; }
		}

		public BackUserControl()
		{
			InitializeComponent();
			titleLabelResize(null, null);
		}
		
		void titleLabelResize(object sender, EventArgs e)
		{
			titleLabel.Width = this.Width - 2;
		}
	}
}
