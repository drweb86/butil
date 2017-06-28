using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// Visually shows the selected category
	/// </summary>
	internal sealed partial class OptionsHeader : UserControl
	{
		[Description("Text on a label")]
		[Browsable(true)]
		public string Title
		{
			get { return titleLabel.Text; }
			set { titleLabel.Text = value; }
		}
		
		public OptionsHeader()
		{
			InitializeComponent();
		}
	}
}
