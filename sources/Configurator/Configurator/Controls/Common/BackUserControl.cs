using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BUtil.Core.PL
{
	public partial class BackUserControl : UserControl
	{
		bool _drawAtractiveBorders = true;
		Dictionary<Control, string> _hints = new Dictionary<Control, string>();

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
				control.MouseMove += controlMouseMove;
				control.GotFocus += controlGotFocus;
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

        public virtual void ApplyLocalization()
		{
			throw new NotImplementedException("ApplyLocalization");
		}

		public virtual void SetOptionsToUi(object settings)
		{
			throw new NotImplementedException("SetOptionsToUi");
		}

		public virtual void GetOptionsFromUi()
		{
			throw new NotImplementedException("GetOptionsFromUi");
		}

		public virtual bool ValidateUi()
		{
			return true;
		}

        #endregion

        void controlGotFocus(object sender, EventArgs e)
		{
			showHelp(sender);
		}

        private void ControlLostFocus(object sender, EventArgs e)
        {
            if (HelpLabel != null)
				HelpLabel.Text = null;
        }

        void controlMouseMove(object sender, MouseEventArgs e)
		{
			showHelp(sender);
		}
		
		void showHelp(object sender)
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

				using (Pen BorderPen = new Pen(SystemColors.InactiveCaption))
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
		
		void backUserControlResize(object sender, EventArgs e)
		{
			Invalidate();
		}
	}
}
