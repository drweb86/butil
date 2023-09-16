
namespace BUtil.ConsoleBackup.UI {
    using BUtil.Core.Localization;
    using System;
    using Terminal.Gui;
    
    public partial class MainView : Terminal.Gui.Toplevel
    {
        private Terminal.Gui.ListView itemsListView;
        private Terminal.Gui.FrameView selectedItemInfoFrameView;

        private Terminal.Gui.MenuBarItem createMenu;

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.X = 0;
            this.Y = 0;
            this.Modal = false;
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            var menuBar = new Terminal.Gui.MenuBar();
            menuBar.Width = Dim.Fill(0);
            menuBar.Height = 1;
            menuBar.X = 0;
            menuBar.Y = 0;
            menuBar.Data = "menuBar";
            menuBar.TextAlignment = Terminal.Gui.TextAlignment.Left;

            

            var createImportMediaTask = new Terminal.Gui.MenuBarItem
            {
                Title = Resources.CreateImportMediaTask,
                Action = () => this.OnCreateImportMediaTask(),
            };

            this.createMenu = new Terminal.Gui.MenuBarItem
            {
                Title = BUtil.Core.Localization.Resources._Create,
                Children = new[] { createImportMediaTask },
            };

            
            menuBar.Menus = new Terminal.Gui.MenuBarItem[] {this.createMenu};
            this.Add(menuBar);
            var tasksFrame = new FrameView(Resources.Tasks)
            {
                X = 0,
                Y = 1, // for menu
                Width = Dim.Percent(70),
                Height = Dim.Fill(1),
                CanFocus = true,
            };
            tasksFrame.ShortcutAction = () => tasksFrame.SetFocus();

            itemsListView = new ListView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(0),
                Height = Dim.Fill(0),
                AllowsMarking = false,
                CanFocus = true,
            };
            itemsListView.SelectedItemChanged += OnSelectedItemChanged;
            itemsListView.KeyDown += OnListShortcutKeyDown;
            itemsListView.OpenSelectedItem += e => OnRunSelectedBackupTask();

            tasksFrame.Add(itemsListView);
            this.Add(tasksFrame);


            selectedItemInfoFrameView = new FrameView()
            {
                X = Pos.Percent(70),
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(1),
                CanFocus = true,
            };

            var runButton = new Terminal.Gui.Button
            {
                Text = "► " + BUtil.Core.Localization.Resources._RunF5Enter,
                Shortcut = Key.F5,
                Y = 1,
                Width = Dim.Fill(),
            };
            runButton.Clicked += OnRunSelectedBackupTask;
            runButton.ShortcutAction += OnRunSelectedBackupTask;

            var editButton = new Terminal.Gui.Button
            {
                Text = "⚒ " + BUtil.Core.Localization.Resources._EditF4,
                Shortcut = Key.F4,
                Y = 3,
                Width = Dim.Fill(),
            };
            editButton.Clicked += OnEditSelectedBackupTask;
            editButton.ShortcutAction += OnEditSelectedBackupTask;

            var deleteButton = new Terminal.Gui.Button
            {
                Text = "❌ " + BUtil.Core.Localization.Resources._DeleteF8Del,
                Shortcut = Key.F8,
                Y = 5,
                Width = Dim.Fill(),
            };
            deleteButton.Clicked += OnDeleteSelectedBackupTask;
            deleteButton.ShortcutAction += OnDeleteSelectedBackupTask;

            selectedItemInfoFrameView.Add(runButton);
            selectedItemInfoFrameView.Add(editButton);
            selectedItemInfoFrameView.Add(deleteButton);
            this.Add(selectedItemInfoFrameView);
        }
    }
}
