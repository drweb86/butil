using BUtil.Configurator.Localization;
using System.Windows.Forms;


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
            Text = Resources.About;
            _aboutProgramUserControl.ApplyLocalization();
        }

        #endregion
    }
}
