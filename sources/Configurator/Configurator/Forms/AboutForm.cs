using System.Windows.Forms;
using BULocalization;

namespace BUtil.Configurator.Configurator.Forms
{
    public partial class AboutForm : Form
    {
        #region Constructors

        public AboutForm(ConfiguratorController controller)
        {
            InitializeComponent();

            ApplyLocalization();
            _aboutProgramUserControl.SetOptionsToUi(controller.ProgramOptions);
        }

        #endregion

        #region Private Methods

        void ApplyLocalization()
        {
            Text = Translation.Current[141];
            _aboutProgramUserControl.ApplyLocalization();
        }

        #endregion
    }
}
