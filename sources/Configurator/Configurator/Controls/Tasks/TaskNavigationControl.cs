using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;

using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;

namespace BUtil.Configurator.Controls
{
	internal sealed partial class TaskNavigationControl : BUtil.Core.PL.BackUserControl
    {
        #region Fields

        readonly Color _selectedButtonColor = Color.FromArgb(255, 157, 185, 235)/*SystemColors.GradientInactiveCaption*/;
		readonly Color _halfSelectedButtonColor = Color.FromArgb(255, 122, 150, 223)/*SystemColors.InactiveCaption*/;
		readonly Color _normalButtonColor = Color.FromArgb(255, 157, 185, 235)/*SystemColors.GradientInactiveCaption*/;
		Button _selectedButton;
		Button _halfSelectedButton;

        #endregion

        public delegate void ChangeViewEventHandler(BackupTaskViewsEnum newView);
        public delegate bool CanChangeViewEventHandler(BackupTaskViewsEnum oldView);

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

        [Description("Check if old view can be left")]
        [Browsable(true)]
        public event CanChangeViewEventHandler CanChangeView;

		private BackupTaskViewsEnum _currentView = BackupTaskViewsEnum.SourceItems;

        public TaskNavigationControl()
		{
			InitializeComponent();

            BackColor = SystemColors.Window;
            this.BorderStyle = BorderStyle.None;
			_selectedButton = itemsForBackupButton;
            changeView(itemsForBackupButton, BackupTaskViewsEnum.SourceItems);
			_selectedButtonColor = Color.FromArgb(_halfSelectedButtonColor.A, 
			    Math.Abs((_halfSelectedButtonColor.R + 5) % 256),
			    Math.Abs((_halfSelectedButtonColor.G + 5) % 256),
			    Math.Abs((_halfSelectedButtonColor.B + 10)) % 256);
			DrawAtractiveBorders = false;
		}
		
		public void UpdateView(ProgramOptions options)
		{
            changeView(itemsForBackupButton, BackupTaskViewsEnum.SourceItems);
		}
		
		override public void ApplyLocalization()
		{
		    var buttons = new[] {itemsForBackupButton, howButton, storagesButton, schedulerButton, encryptionButton, otherOptionsButton};

			itemsForBackupButton.Text = Resources.What;
			storagesButton.Text = Resources.Where;
			schedulerButton.Text = Resources.When;
			encryptionButton.Text = Resources.Encryption;
			otherOptionsButton.Text = Resources.OtherOptions;
            howButton.Text = Resources.How;
            SetHintForControl(itemsForBackupButton, Resources.HereYouMayAddFoldersAndFilesYouWantToBackup);
			SetHintForControl(storagesButton, Resources.InThisPlaceYouCanAddLocationsWhereYouWouldLikeYourBackupToBeCopiedAfterCompletionOfBackup);
			SetHintForControl(schedulerButton, Resources.HereYouCanSetUpASchedulerShcedulerCanHelpYouToAutomateCreationOfBackups);
			SetHintForControl(encryptionButton, Resources.ProtectionOfBackupWithPasswordItIsRequiredWhenYouWantYourDataToBeCopiedSomewhereOverTheNetwork);
			SetHintForControl(otherOptionsButton, Resources.MiscellaneousOptions);
            SetHintForControl(howButton, Resources.HowHint);

            foreach (var button in buttons)
		    {
                button.BackColor = _normalButtonColor;
                registerVisualEffectsForButton(button);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					setBoldFont(button);
				}
		    }
		}

        [SupportedOSPlatform("windows")]
        void setBoldFont(Button button)
		{
            button.Font = new Font(button.Font, FontStyle.Bold);
        }
		
		void registerVisualEffectsForButton(Button button)
		{
			button.MouseMove += new MouseEventHandler(controlMouseMove);
			button.GotFocus += new EventHandler(controlGotFocus);
		}
		
		void controlGotFocus(object sender, EventArgs e)
		{
			showHalfSelected(sender);
		}

		void controlMouseMove(object sender, MouseEventArgs e)
		{
			showHalfSelected(sender);
		}
		
		void showHalfSelected(object button)
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

        void changeView(object sender, BackupTaskViewsEnum newView)
		{
			if (_currentView != newView && !CanChangeView.Invoke(_currentView))
			{
				return;
			}

			_currentView = newView;

            if (_selectedButton != null)
			{
				_selectedButton.BackColor = _normalButtonColor;
			}

			Button newCurrentButton = ((Button)sender);
			newCurrentButton.BackColor = _selectedButtonColor;
			_selectedButton = newCurrentButton;

			if (ViewChanged != null)
			{				
				ViewChanged.Invoke(newView);
			}
		}
		
		void itemsForBackupButtonClick(object sender, EventArgs e)
		{
            changeView(sender, BackupTaskViewsEnum.SourceItems);
		}
		
		void storagesButtonClick(object sender, EventArgs e)
		{
            changeView(sender, BackupTaskViewsEnum.Storages);
		}
		
		void schedulerButtonClick(object sender, EventArgs e)
		{
            changeView(sender, BackupTaskViewsEnum.Scheduler);
		}
		
		void encryptionButtonClick(object sender, EventArgs e)
		{
            changeView(sender, BackupTaskViewsEnum.Encryption);
		}
		
		void otherOptionsButtonClick(object sender, EventArgs e)
		{
            changeView(sender, BackupTaskViewsEnum.OtherOptions);
		}
		
		void leftPanelUserControlLoad(object sender, EventArgs e)
		{
			foreach (var control in Controls)
			{
				var button = control as Button;
				if (button != null)
				{
                    button.BackColor = _normalButtonColor;
                    button.ForeColor = Color.White;
				}
			}
		}

		private void HowButtonClick(object sender, EventArgs e)
		{
            changeView(sender, BackupTaskViewsEnum.How);
        }
	}
}
