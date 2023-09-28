using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Core.Localization;
using System.IO;
using BUtil.Core.State;
using BUtil.Configurator;
using System.Linq;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.ConfigurationFileModels.V1;

namespace BUtil.RestorationMaster
{
    public partial class OpenBackupForm : Form
    {
        private TaskV2 storageStub = new TaskV2();

        public OpenBackupForm(TaskV2 backupTask = null)
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
            closeButton.Text = Resources.Button_Close;
            passwordLabel.Text = Resources.Password_Field;
            continueButton.Text = Resources.Button_Continue;
            continueButton.Enabled = true;
            this.Text = Resources.Task_Restore;
            _whereUserControl.ApplyLocalization();
            _whereUserControl.StorageSettings = ((IncrementalBackupModelOptionsV2)storageStub.Model).To;
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

            var options = (IncrementalBackupModelOptionsV2)storageStub.Model;
            options.To = _whereUserControl.StorageSettings;

            var storageSettings = options.To;

            string error = null;
            IncrementalBackupState state = null;

            using (var progressForm = new ProgressForm((Action<int> reportProgress) =>
            {
                reportProgress(10);
                var log = new StubLog();
                var storage = StorageFactory.Create(log, storageSettings);

                if (!storage.Exists(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile))
                {
                    error = string.Format(Resources.RestoreFrom_Field_Validation_NoStateFiles, IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);
                    return;
                }

                reportProgress(20);

                var commonServicesIoc = new CommonServicesIoc();
                var services = new StorageSpecificServicesIoc(log, storageSettings, commonServicesIoc.HashService);
                if (!services.IncrementalBackupStateService.TryRead(_passwordTextBox.Text, out state))
                {
                    error = Resources.RestoreFrom_Field_Validation_StateInvalid;
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
    }
}
