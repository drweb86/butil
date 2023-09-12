
namespace BUtil.ConsoleBackup.UI {
    using Terminal.Gui;
    
    public partial class EditStorageSettingsDialog : Terminal.Gui.Dialog
    {
        private Terminal.Gui.TabView _tabView;

        private Terminal.Gui.TabView.Tab _folderStorageTab;
        private Terminal.Gui.TextField _folderStorageFolderTextField;
        
        private Terminal.Gui.TabView.Tab _ftpsStorageTab;
        private Terminal.Gui.TextField _hostFtpsStorageFolderTextField;
        private Terminal.Gui.TextField _portFtpsStorageFolderTextField;
        private Terminal.Gui.TextField _userFtpsStorageFolderTextField;
        private Terminal.Gui.TextField _pwdFtpsStorageFolderTextField;
        private Terminal.Gui.TextField _folderFtpsStorageFolderTextField;

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            _tabView = new TabView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(0),
                Height = Dim.Fill(1),
            };
            Add(_tabView);

            var folderStorageView = new View()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var ftpsStorageView = new View()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = new ColorScheme()
            };

            _folderStorageTab = new TabView.Tab(BUtil.Core.Localization.Resources.FolderStorage, folderStorageView);
            _ftpsStorageTab = new TabView.Tab("FTPS", ftpsStorageView);
            _tabView.AddTab(_folderStorageTab, true);
            _tabView.AddTab(_ftpsStorageTab, false);

            _folderStorageFolderTextField = AddTextField(folderStorageView, BUtil.ConsoleBackup.Localization.Resources.DestinationFolder, 0);

            _hostFtpsStorageFolderTextField = AddTextField(ftpsStorageView, BUtil.ConsoleBackup.Localization.Resources.HostField, 0);
            _portFtpsStorageFolderTextField = AddTextField(ftpsStorageView, BUtil.ConsoleBackup.Localization.Resources.PortField, 3);
            _userFtpsStorageFolderTextField = AddTextField(ftpsStorageView, BUtil.ConsoleBackup.Localization.Resources.UserField, 6);
            _pwdFtpsStorageFolderTextField = AddTextField(ftpsStorageView, BUtil.ConsoleBackup.Localization.Resources.PasswordField, 9);
            _folderFtpsStorageFolderTextField = AddTextField(ftpsStorageView, BUtil.ConsoleBackup.Localization.Resources.FolderField, 12);

            var saveButton = new Button
            {
                Text = BUtil.ConsoleBackup.Localization.Resources.Save,
                IsDefault = true,
            };
            saveButton.Clicked += OnSave;
            AddButton(saveButton);

            var cancelButton = new Button
            {
                Text = BUtil.Core.Localization.Resources.Cancel,
            };
            cancelButton.Clicked += OnCancel;
            AddButton(cancelButton);
        }


        private TextField AddTextField(View view, string label, int y)
        {
            view.Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = y,
                Text = label,
                ColorScheme = view.ColorScheme, // bug in terminal.gui
            });
            var field = new TextField
            {
                X = 0,
                Y = y + 1,
                Width = Dim.Fill(0),
                ColorScheme = view.ColorScheme, // bug in terminal.gui
            };
            view.Add(field);
            return field;
        }
    }
}
