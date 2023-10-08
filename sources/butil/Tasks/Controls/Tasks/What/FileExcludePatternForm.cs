using BUtil.Core.Misc;
using System;
using System.Windows.Forms;
using BUtil.Core.Localization;

namespace BUtil.Configurator.Configurator.Controls.Tasks.What
{
    public partial class FileExcludePatternForm : Form
    {
        public string Pattern
        {
            get => _patternTextBox.Text;
            private set => _patternTextBox.Text = value;
        }

        public FileExcludePatternForm(string pattern = null)
        {
            InitializeComponent();

            Pattern = pattern;
            this.Text = pattern == null ? BUtil.Core.Localization.Resources.StorageItem_ExcludePattern_Add : Resources.StorageItem_ExcludePattern_Edit;
            _patternLabel.Text = BUtil.Core.Localization.Resources.StorageItem_Field_ExcludePattern;
            _descriptionLabel.Text = BUtil.Core.Localization.Resources.StorageItem_ExcludePattern_Help;
            _formatLinkLabel.Text = BUtil.Core.Localization.Resources.StorageItem_ExcludePattern_GlobbingNetPatterns;
            _okButton.Text = Resources.Button_OK;
            _cancelButton.Text = Resources.Button_Cancel;
        
        }

        private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.ShellExecute("https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats");
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_patternTextBox.Text))
            {
                Messages.ShowErrorBox(BUtil.Core.Localization.Resources.StorageItem_Field_ExcludePattern_Validation);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
