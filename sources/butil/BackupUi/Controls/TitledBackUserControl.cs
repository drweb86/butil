using System;

namespace BUtil.BackupUiMaster.Controls;

internal partial class TitledBackUserControl : BUtil.Core.PL.BackUserControl
{
	public string Title
	{
		get { return titleLabel.Text; }
		set { titleLabel.Text = value; }
	}

	public TitledBackUserControl()
	{
		InitializeComponent();
		titleLabelResize(null, null);
	}
	
	void titleLabelResize(object sender, EventArgs e)
	{
		titleLabel.Width = this.Width - 2;
	}
}
