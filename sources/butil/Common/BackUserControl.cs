using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BUtil.Core.PL
{
	public partial class BackUserControl : UserControl
	{
        private bool _drawAtractiveBorders = true;
		private readonly Dictionary<Control, string> _hints = new();

		[Category("Appearance")]
    	[DefaultValue(true)]
		public bool DrawAtractiveBorders
		{
			get { return _drawAtractiveBorders; }
			set { _drawAtractiveBorders = value; }
		}
		
		public ToolStripStatusLabel HelpLabel { get; set; }
		
		public void SetHintForControl(Control control, string hint)
		{
			if (_hints.ContainsKey(control))
			{
				_hints[control] = hint;
			}
			else
			{
				control.MouseMove += ControlMouseMove;
				control.GotFocus += ControlGotFocus;
				control.MouseLeave += ControlLostFocus;
                _hints.Add(control, hint);
			}
		}
		
		protected BackUserControl()
		{
			InitializeComponent();

			BackColor = SystemColors.Window;
		}

		#region Methods to Overrides

        public virtual void ApplyLocalization()	{ }

		public virtual void SetOptionsToUi(object settings) { }

		public virtual void GetOptionsFromUi()
		{
		}

		public virtual bool ValidateUi()
		{
			return true;
		}

        #endregion

        void ControlGotFocus(object sender, EventArgs e)
		{
			ShowHelp(sender);
		}

        private void ControlLostFocus(object sender, EventArgs e)
        {
            if (HelpLabel != null)
				HelpLabel.Text = null;
        }

        private void ControlMouseMove(object sender, MouseEventArgs e)
		{
			ShowHelp(sender);
		}

        private void ShowHelp(object sender)
		{
			if (HelpLabel != null && (sender != null))
			{
                HelpLabel.Text = _hints[(Control)sender];
			}
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			if (_drawAtractiveBorders)
			{
				// draws attractive border
				Rectangle Rect = ClientRectangle;
				
				using (Brush BackBrush = new SolidBrush(BackColor))
				{
					e.Graphics.FillRectangle(BackBrush, Rect);
				}

				using (Pen BorderPen = new(SystemColors.InactiveCaption))
				{
					e.Graphics.DrawLine(BorderPen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Right - 1, ClientRectangle.Top);
					Rect.X++;
	            	Rect.Width--;
					e.Graphics.DrawLine(BorderPen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Left, ClientRectangle.Bottom - 1);
		            Rect.Y++;
		            Rect.Height--;
					e.Graphics.DrawLine(BorderPen, ClientRectangle.Right - 1, ClientRectangle.Top, ClientRectangle.Right - 1, ClientRectangle.Bottom - 1);
					Rect.Width--;
					e.Graphics.DrawLine(BorderPen, ClientRectangle.Left, ClientRectangle.Bottom - 1, ClientRectangle.Right - 1, ClientRectangle.Bottom - 1);
					Rect.Height--;
				}
				
				using (Brush BackBrush = new SolidBrush(BackColor))
				{
					e.Graphics.FillRectangle(BackBrush, Rect);
				}
			}
			base.OnPaint(e);
		}
		
		private void BackUserControlResize(object sender, EventArgs e)
		{
			Invalidate();
		}
	}
}
