using System;
using System.Drawing;

namespace BUtil.BackupUiMaster.Controls;

internal partial class TitledBackUserControl : BUtil.Core.PL.BackUserControl
{
	public string Title
	{
		get { return titleLabel.Text; }
		set { titleLabel.Text = value; }
	}

	public Color TitleBackground
	{
		get => titleLabel.BackColor;
		set => titleLabel.BackColor = value;
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
