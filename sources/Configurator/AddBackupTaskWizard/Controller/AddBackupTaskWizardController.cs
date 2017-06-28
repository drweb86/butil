using BUtil.Configurator.AddBackupTaskWizard.View;
using BUtil.Core.Options;

namespace BUtil.Configurator.AddBackupTaskWizard.Controller
{
    class AddBackupTaskWizardController
    {
        #region Properties

        public AddBackupTaskWizardView AddBackupTaskWizardView { get; private set; }
        public BackupTask Task 
        {
            get { return AddBackupTaskWizardView.Task; }
        }

        #endregion

        #region Constructor

        public AddBackupTaskWizardController(ProgramOptions options)
        {
            AddBackupTaskWizardView = new AddBackupTaskWizardView(options);
        }

        #endregion
    }
}
