using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using System.IO;
using BUtil.Core.State;
using BUtil.Configurator;
using System.Linq;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.RestorationMaster
{
    public partial class OpenBackupForm : Form
    {
        private BackupTaskV2 storageStub = new BackupTaskV2();

        public OpenBackupForm(BackupTaskV2 backupTask = null)
        {
            InitializeComponent();

            if (backupTask != null)
            {
                storageStub = backupTask;
                if (backupTask.Model is IncrementalBackupModelOptionsV2)
                {
                    _passwordTextBox.Text = ((IncrementalBackupModelOptionsV2)backupTask.Model).Password;
                }
            }

            ApplyLocals();

        }

        private void ApplyLocals()
        {
            closeButton.Text = Resources.Close;
            passwordLabel.Text = Resources.IfYourBackupIsPasswordProtectedPleaseTypePasswordHere;
            continueButton.Text = Resources.Continue;
            continueButton.Enabled = true;
            this.Text = Resources.RestorationMaster;
            _whereUserControl.ApplyLocalization();
            _whereUserControl.SetOptionsToUi(storageStub);
            continueButton.Left = closeButton.Left - continueButton.Width - 10;
        }

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnNextButtonClick(object sender, EventArgs e)
        {
            if (!_whereUserControl.ValidateUi())
            {
                return;
            }
            _whereUserControl.GetOptionsFromUi();

            var options = (IncrementalBackupModelOptionsV2)storageStub.Model;
            var storageSettings = options.To;

            string error = null;
            IncrementalBackupState state = null;

            using (var progressForm = new ProgressForm((Action<int> reportProgress) =>
            {
                reportProgress(10);
                var log = new StubLog();
                var storage = StorageFactory.Create(log, storageSettings);

                if (!IncrementalBackupModelConstants.Files.Any(x => storage.Exists(x)))
                {
                    var allowedFiles = string.Join(", ", IncrementalBackupModelConstants.Files);
                    error = string.Format(Resources.CannotLocateFile0InDirectoryPointToADirectoryContainingThisFile, allowedFiles);
                    return;
                }

                reportProgress(20);
                
                var commonServicesIoc = new CommonServicesIoc();
                var services = new StorageSpecificServicesIoc(log, storageSettings, commonServicesIoc.HashService);
                if (!services.IncrementalBackupStateService.TryRead(_passwordTextBox.Text, out state))
                {
                    error = Resources.CannotOpenBackupFolder;
                    return;
                }
                storage.Dispose();
                services.Dispose();
            }))
                progressForm.ShowDialog();

            if (!string.IsNullOrWhiteSpace(error))
            {
                Messages.ShowErrorBox(error);
                return;
            }

            Hide();
            using var restoreForm = new VersionsViewerForm(storageSettings, state);
            restoreForm.ShowDialog();
            
            Close();
        }

        private void OnHelpClick(object sender, EventArgs e)
        {
            SupportManager.DoSupport(SupportRequest.RestorationWizard);
        }
    }
}
