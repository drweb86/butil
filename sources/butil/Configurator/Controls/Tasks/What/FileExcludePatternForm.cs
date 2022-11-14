using BUtil.Core.Misc;
using System;
using System.Windows.Forms;

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
            this.Text = pattern == null ? BUtil.Configurator.Localization.Resources.AddFileExcludePattern : Localization.Resources.EditFileExcludePattern;
            _patternLabel.Text = BUtil.Configurator.Localization.Resources.FileExclusionPattern;
            _descriptionLabel.Text = BUtil.Configurator.Localization.Resources.HereYouCanSpecifyFileGlobbingExclusionPatternsItCanBeAFileNameOrFolderNameOrWildcardExamplesDTempDTempFileTxtDTempTxtOrBinSeeDetailedDescriptionInTheFollowingLink;
            _formatLinkLabel.Text = BUtil.Configurator.Localization.Resources.FileGlobbingInNETPatterns;
            _okButton.Text = Localization.Resources.Ok;
            _cancelButton.Text = Localization.Resources.Cancel;
        
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
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.PatternIsEmpty);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
