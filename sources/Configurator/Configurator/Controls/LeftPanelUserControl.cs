using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BULocalization;

namespace BUtil.Configurator.Configurator.Controls
{
	/// <summary>
	/// Switcher between views
	/// </summary>
	internal sealed partial class LeftPanelUserControl : Core.PL.BackUserControl
    {
        #region Fields

        readonly Color _selectedButtonColor = Color.FromArgb(255, 157, 185, 235)/*SystemColors.GradientInactiveCaption*/;
		readonly Color _halfSelectedButtonColor = Color.FromArgb(255, 122, 150, 223)/*SystemColors.InactiveCaption*/;
		readonly Color _normalButtonColor = Color.FromArgb(255, 157, 185, 235)/*SystemColors.GradientInactiveCaption*/;
		Button _selectedButton;
		Button _halfSelectedButton;

        #endregion

        public delegate void ChangeViewEventHandler(ConfiguratorViewsEnum newView);
		
		/// <summary>
		/// Returns localized name of selected category
		/// </summary>
		public string SelectedCategory
		{
			get { return _selectedButton.Text; }
		}
		
		[Description("Occurs when user selects the other tab")]
		[Browsable(true)]
		public event ChangeViewEventHandler ViewChanged;

		public LeftPanelUserControl()
		{
			InitializeComponent();
			
			BorderStyle = BorderStyle.None;
			_selectedButton = _tasksButton;
            ChangeView(_tasksButton, ConfiguratorViewsEnum.Tasks);
			_selectedButtonColor = Color.FromArgb(_halfSelectedButtonColor.A, 
			    Math.Abs((_halfSelectedButtonColor.R + 5) % 256),
			    Math.Abs((_halfSelectedButtonColor.G + 5) % 256),
			    Math.Abs((_halfSelectedButtonColor.B + 10)) % 256);
			DrawAtractiveBorders = false;
		}
		
		override public void ApplyLocalization()
		{
//TODO: refactor to make as input a special structure - with localization

		    var buttons = new[] {_otherOptionsButton, _loggingButton, _tasksButton};

			_otherOptionsButton.Text = Translation.Current[96];
			_loggingButton.Text = Translation.Current[114];
            _tasksButton.Text = Translation.Current[637];
			SetHintForControl(_otherOptionsButton, Translation.Current[528]);
			SetHintForControl(_loggingButton, Translation.Current[529]);
            SetHintForControl(_tasksButton, Translation.Current[637]);
            
		    foreach (var button in buttons)
		    {
		        RegisterVisualEffectsForButton(button);
		        button.Font = new Font(button.Font, FontStyle.Bold);
		    }
		}
		
		void RegisterVisualEffectsForButton(Button button)
		{
			button.MouseMove += ControlMouseMove;
			button.GotFocus += ControlGotFocus;
		}
		
		void ControlGotFocus(object sender, EventArgs e)
		{
			ShowHalfSelected(sender);
		}

		void ControlMouseMove(object sender, MouseEventArgs e)
		{
			ShowHalfSelected(sender);
		}
		
		void ShowHalfSelected(object button)
		{
			if (_halfSelectedButton != null && _halfSelectedButton != _selectedButton)
			{
				_halfSelectedButton.BackColor = _normalButtonColor;
			}

			if (_selectedButton != button)
			{
				((Button)button).BackColor = _halfSelectedButtonColor;
			}

			_halfSelectedButton = (Button)button;
		}
		
		void ChangeView(object sender, ConfiguratorViewsEnum newView)
		{
			if (_selectedButton != null)
			{
				_selectedButton.BackColor = _normalButtonColor;
			}

			var newCurrentButton = ((Button)sender);
			newCurrentButton.BackColor = _selectedButtonColor;
			_selectedButton = newCurrentButton;

			if (ViewChanged != null)
			{				
				ViewChanged.Invoke(newView);
			}
		}
		
		void OtherOptionsButtonClick(object sender, EventArgs e)
		{
			ChangeView(sender, ConfiguratorViewsEnum.OtherOptions);
		}
		
		void LoggingButtonClick(object sender, EventArgs e)
		{
			ChangeView(sender, ConfiguratorViewsEnum.Logging);
		}
		
		void LeftPanelUserControlLoad(object sender, EventArgs e)
		{
			foreach (Button control in Controls)
			{
				control.BackColor = _normalButtonColor;
				control.ForeColor = Color.White;
			}
		}

        private void TasksShowRequest(object sender, EventArgs e)
        {
            ChangeView(sender, ConfiguratorViewsEnum.Tasks);
        }
	}
}
