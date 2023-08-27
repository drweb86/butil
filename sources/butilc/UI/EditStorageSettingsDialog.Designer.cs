
namespace BUtil.ConsoleBackup.UI {
    using Terminal.Gui;
    
    public partial class EditStorageSettingsDialog : Terminal.Gui.Dialog
    {
        private Terminal.Gui.TextField _folderStorageFolderTextField;
        private Terminal.Gui.TabView _tabView;
        private Terminal.Gui.TabView.Tab _folderStorageTab;

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

            var folderStorageView = new View()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            _folderStorageTab = new TabView.Tab(BUtil.Core.Localization.Resources.FolderStorage, folderStorageView);
            _tabView.AddTab(_folderStorageTab, false);

            Add(_tabView);


            folderStorageView.Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 0,
                Text = BUtil.ConsoleBackup.Localization.Resources.DestinationFolder
            });

            _folderStorageFolderTextField = new TextField
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(0),
            };
            folderStorageView.Add(_folderStorageFolderTextField);


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
    }
}
