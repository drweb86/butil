using System;
using System.Windows.Forms;
using BUtil.Configurator.AddBackupTaskWizard.Controller;
using BULocalization;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    public partial class AddBackupTaskWizardForm : Form
    {
        #region Fields

        readonly AddBackupTaskWizardController _controller;

        #endregion

        #region Constructors

        internal AddBackupTaskWizardForm(AddBackupTaskWizardController controller)
        {
            _controller = controller;

            InitializeComponent();

            _controller.AddBackupTaskWizardView.Configure();
            _controller.AddBackupTaskWizardView.ApplyLocalization();
            ApplyLocals();

            RefreshActivePage();
        }

        #endregion

        #region Private Methods

        private void ApplyLocals()
        {
            Text = Translation.Current[630];
            _nextButton.Text = Translation.Current[631];
            _previousButton.Text = Translation.Current[632];
            _cancelButton.Text = Translation.Current[186];
        }

        private void NextPageRequest(object sender, EventArgs e)
        {
            if (_controller.AddBackupTaskWizardView.CanGoNextPage)
            {
                _controller.AddBackupTaskWizardView.GoNext();
                RefreshActivePage();
            }
            else
            {
                _controller.AddBackupTaskWizardView.GetOptions();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void PreviousPageRequest(object sender, EventArgs e)
        {
            if (_controller.AddBackupTaskWizardView.CanGoPreviousPage)
            {
                _controller.AddBackupTaskWizardView.GoPrevious();
            }

            RefreshActivePage();
        }

        private void RefreshActivePage()
        {
            _nextButton.Text = _controller.AddBackupTaskWizardView.CanGoNextPage ? 
                Translation.Current[631] : Translation.Current[423];
            _previousButton.Enabled = _controller.AddBackupTaskWizardView.CanGoPreviousPage;

            _titleLabel.Text = _controller.AddBackupTaskWizardView.CurrentPage.Title;
            _descriptionLabel.Text = _controller.AddBackupTaskWizardView.CurrentPage.Description;
            _panel.BackgroundImageLayout = ImageLayout.None;
            _panel.BackgroundImage = _controller.AddBackupTaskWizardView.CurrentPage.Image;
            _containerPanel.Controls.Clear();
            
            var control = _controller.AddBackupTaskWizardView.CurrentPage.ControlToShow;

            if (control != null)
            {
                _containerPanel.Controls.Add(control);
                control.Dock = DockStyle.Fill;
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion
    }
}
