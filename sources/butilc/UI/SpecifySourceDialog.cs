using BUtil.Core.Storages;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{
    public partial class SpecifySourceDialog
    {
        public IStorageSettings Source { get; private set; }

        internal SpecifySourceDialog(IStorageSettings source) 
        {
            InitializeComponent();

            if (source == null)
            {
            }
            else
            {
            }
        }

        public bool Canceled { get; private set; } = true;

        private void OnCancel()
        {
            Canceled = true;
            Application.RequestStop();
        }

        private void OnSave()
        {
            //if (!Directory.Exists(this._sourceFolderTextField.Text.ToString()))
            //{
            //    MessageBox.ErrorQuery(string.Empty, Resources.SourceFolderDoesNotExist, Resources.Close);
            //    return;
            //}

            Canceled = false;
            Application.RequestStop();
        }
    }
}
