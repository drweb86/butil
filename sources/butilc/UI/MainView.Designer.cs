
namespace BUtil.ConsoleBackup.UI {
    using Terminal.Gui;
    
    public partial class MainView : Terminal.Gui.Toplevel
    {
        private Terminal.Gui.FrameView itemsFrame;
        private Terminal.Gui.ListView itemsListView;
        
        private Terminal.Gui.MenuBar menuBar;

        private Terminal.Gui.MenuBarItem infoMenu;
        private Terminal.Gui.MenuBarItem runMenu;
        private Terminal.Gui.MenuBarItem createMenu;
        private Terminal.Gui.MenuBarItem editMenu;
        private Terminal.Gui.MenuBarItem deleteMenu;

        private void InitializeComponent() {
            this.menuBar = new Terminal.Gui.MenuBar();
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.X = 0;
            this.Y = 0;
            this.Modal = false;
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.menuBar.Width = Dim.Fill(0);
            this.menuBar.Height = 1;
            this.menuBar.X = 0;
            this.menuBar.Y = 0;
            this.menuBar.Data = "menuBar";
            this.menuBar.TextAlignment = Terminal.Gui.TextAlignment.Left;

            this.infoMenu = new Terminal.Gui.MenuBarItem();
            this.infoMenu.Title = "BUtil CLI";

            this.runMenu = new Terminal.Gui.MenuBarItem
            {
                Title = "_Run (F5, Enter)",
                Shortcut = Key.F5,
                Action = () => this.OnRunSelectedBackupTask(),
            };

            this.createMenu = new Terminal.Gui.MenuBarItem
            {
                Title = "_Create",
                Action = () => this.OnCreateBackupTask(),
            };

            this.editMenu = new Terminal.Gui.MenuBarItem
            {
                Title = "_Edit (F4)",
                Shortcut = Key.F4,
                Action = () => this.OnEditSelectedBackupTask(),
            };

            this.deleteMenu = new Terminal.Gui.MenuBarItem
            {
                Title = "_Delete (F8, Del)",
                Shortcut = Key.F8,
                Action = () => this.OnDeleteSelectedBackupTask(),
            };

            this.menuBar.Menus = new Terminal.Gui.MenuBarItem[] {
                this.infoMenu,
                    this.runMenu, this.createMenu, this.editMenu, this.deleteMenu};
            this.Add(this.menuBar);
            itemsFrame = new FrameView("Tasks")
            {
                X = 0,
                Y = 1, // for menu
                Width = Dim.Fill(),
                Height = Dim.Fill(1),
                CanFocus = true,
                Shortcut = Key.CtrlMask | Key.T
            };
            itemsFrame.Title = $"{itemsFrame.Title} ({itemsFrame.ShortcutTag})";
            itemsFrame.ShortcutAction = () => itemsFrame.SetFocus();

            itemsListView = new ListView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(0),
                Height = Dim.Fill(0),
                AllowsMarking = false,
                CanFocus = true,
            };
            itemsListView.KeyDown += OnListShortcutKeyDown;
            itemsListView.OpenSelectedItem += e => OnRunSelectedBackupTask();

            itemsFrame.Add(itemsListView);
            this.Add(this.itemsFrame);
        }
    }
}
