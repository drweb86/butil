
namespace BUtil.ConsoleBackup.UI {
    using BUtil.Core.Localization;
    using System;
    using Terminal.Gui;
    
    public partial class TasksView : Terminal.Gui.Toplevel
    {
        private Terminal.Gui.ListView itemsListView;
        private Terminal.Gui.FrameView selectedItemInfoFrameView;

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

            var seeLogsMenuBarItem = new Terminal.Gui.MenuBarItem
            {
                Title = Resources.LogFile_OpenLogs,
                Action = () => this.OnOpenLogs(),
            };

            var createImportMediaTask = new Terminal.Gui.MenuBarItem
            {
                Title = Resources.ImportMediaTask_Create,
                Action = () => this.OnCreateImportMediaTask(),
            };

            var openRestorationAppMenuBarItem = new Terminal.Gui.MenuBarItem
            {
                Title = "⤾" + Resources.Task_Restore,
                Action = () => this.OnOpenRestorationApp(),
            };

            var goToHomePageTask = new Terminal.Gui.MenuBarItem
            {
                Title = "?",
                Action = () => this.OnOpenHomePage(),
            };

            var createMenu = new Terminal.Gui.MenuBarItem
            {
                Title = BUtil.Core.Localization.Resources.Task_Create,
                Children = new[] { createImportMediaTask },
            };
            
            menuBar.Menus = new Terminal.Gui.MenuBarItem[] { seeLogsMenuBarItem, createMenu, openRestorationAppMenuBarItem, goToHomePageTask };
            this.Add(menuBar);
            var tasksFrame = new FrameView(Resources.Task_List)
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


            var editButton = new Terminal.Gui.Button
            {
                Text = "⚒ " + BUtil.Core.Localization.Resources.Task_Edit + " (F4)",
                Shortcut = Key.F4,
                Y = 3,
                Width = Dim.Fill(),
            };
            editButton.Clicked += OnEditSelectedBackupTask;
            editButton.ShortcutAction += OnEditSelectedBackupTask;

            var deleteButton = new Terminal.Gui.Button
            {
                Text = "❌ " + BUtil.Core.Localization.Resources.Task_Delete + " (F8, Del)",
                Shortcut = Key.F8,
                Y = 5,
                Width = Dim.Fill(),
            };
            deleteButton.Clicked += OnDeleteSelectedBackupTask;
            deleteButton.ShortcutAction += OnDeleteSelectedBackupTask;

            var restoreButton = new Terminal.Gui.Button
            {
                Text = "⤾ " + BUtil.Core.Localization.Resources.Task_Restore,
                Y = 7,
                Width = Dim.Fill(),
            };
            restoreButton.Clicked += OnRestoreSelectedTask;

            selectedItemInfoFrameView.Add(editButton);
            selectedItemInfoFrameView.Add(deleteButton);
            selectedItemInfoFrameView.Add(restoreButton);
            this.Add(selectedItemInfoFrameView);
        }
    }
}
