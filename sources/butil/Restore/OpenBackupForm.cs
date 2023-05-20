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
using BUtil.Core.Options;

namespace BUtil.RestorationMaster
{
    public partial class OpenBackupForm : Form
    {
        private BackupTask storageStub = new BackupTask();

        public OpenBackupForm(string folderStorage = null)
        {
            InitializeComponent();
            
            if (folderStorage != null)
            {
                storageStub.Storages.Add(new FolderStorageSettings() { DestinationFolder = folderStorage });
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
            var storageSettings = storageStub.Storages.First();

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
