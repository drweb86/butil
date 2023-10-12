using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;
using BUtil.Configurator.Configurator.Controls.Tasks;
using BUtil.Core.Localization;

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

        public delegate void ChangeViewEventHandler(TaskEditorPageEnum newView);
        public delegate bool CanChangeViewEventHandler(TaskEditorPageEnum oldView);

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

        public bool WhenVisible
        {
            get { return schedulerButton.Visible; }
            set
            {
                schedulerButton.Visible = value;
                _tableLayoutPanel.RowStyles[2].Height = value ? 80f : 0;
            }
        }
        public bool EncryptionVisi1ble
        {
            get { return encryptionButton.Visible; }
            set
            {
                encryptionButton.Visible = value;
                _tableLayoutPanel.RowStyles[4].Height = value ? 80f : 0;
            }
        }

        private TaskEditorPageEnum _currentView = TaskEditorPageEnum.Encryption;

        public TaskNavigationControl()
        {
            InitializeComponent();

            BackColor = SystemColors.Window;
            this.BorderStyle = BorderStyle.None;
            _selectedButton = itemsForBackupButton;
            changeView(itemsForBackupButton, TaskEditorPageEnum.Encryption);
            _selectedButtonColor = Color.FromArgb(_halfSelectedButtonColor.A,
                Math.Abs((_halfSelectedButtonColor.R + 5) % 256),
                Math.Abs((_halfSelectedButtonColor.G + 5) % 256),
                Math.Abs((_halfSelectedButtonColor.B + 10)) % 256);
            DrawAtractiveBorders = false;

            var buttons = new[] {_nameButton, itemsForBackupButton,
                storagesButton, schedulerButton, encryptionButton};

            _nameButton.Text = Resources.Name_Title;
            itemsForBackupButton.Text = Resources.LeftMenu_What;
            storagesButton.Text = Resources.LeftMenu_Where;
            schedulerButton.Text = Resources.LeftMenu_When;
            encryptionButton.Text = Resources.LeftMenu_Encryption;
            SetHintForControl(_nameButton, BUtil.Core.Localization.Resources.Name_Title);
            SetHintForControl(itemsForBackupButton, Resources.LeftMenu_What_Hint);
            SetHintForControl(storagesButton, Resources.LeftMenu_Where_Hint);
            SetHintForControl(schedulerButton, Resources.LeftMenu_When_Help);
            SetHintForControl(encryptionButton, string.Empty);

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

        public void UpdateView()
        {
            changeView(itemsForBackupButton, TaskEditorPageEnum.SourceItems);
        }

        [SupportedOSPlatform("windows")]
        static void setBoldFont(Button button)
        {
            button.Font = new Font(button.Font, FontStyle.Bold);
        }

        void registerVisualEffectsForButton(Button button)
        {
            button.MouseMove += new MouseEventHandler(controlMouseMove);
            button.GotFocus += new EventHandler(ControlGotFocus);
        }

        void ControlGotFocus(object sender, EventArgs e)
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

        void changeView(object sender, TaskEditorPageEnum newView)
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

        void ItemsForBackupButtonClick(object sender, EventArgs e)
        {
            changeView(sender, TaskEditorPageEnum.SourceItems);
        }

        void StoragesButtonClick(object sender, EventArgs e)
        {
            changeView(sender, TaskEditorPageEnum.Storages);
        }

        void SchedulerButtonClick(object sender, EventArgs e)
        {
            changeView(sender, TaskEditorPageEnum.Scheduler);
        }

        void EncryptionButtonClick(object sender, EventArgs e)
        {
            changeView(sender, TaskEditorPageEnum.Encryption);
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

        private void OnNameButtonClick(object sender, EventArgs e)
        {
            changeView(sender, TaskEditorPageEnum.Encryption);
        }
    }
}
