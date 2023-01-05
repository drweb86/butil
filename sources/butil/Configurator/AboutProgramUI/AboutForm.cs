using BUtil.Configurator.Localization;
using System.Windows.Forms;


namespace BUtil.Configurator.Configurator.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            ApplyLocalization();
            _aboutProgramUserControl.SetOptionsToUi(null);
        }

        void ApplyLocalization()
        {
            Text = Resources.About;
            _aboutProgramUserControl.ApplyLocalization();
        }
    }
}
