
namespace BUtil.ConsoleBackup.UI {
    using BUtil.Core.Localization;
    using BUtil.Core.Events;
    using System.ComponentModel;
    using Terminal.Gui;
    
    public partial class BackupDialog : Terminal.Gui.Dialog
    {
        private readonly BackupEvents _backupEvents = new();
        private Terminal.Gui.ListView _listView;
        private readonly BackgroundWorker _backgroundWorker = new() { WorkerSupportsCancellation = true };
        private readonly ListViewItemDataSource _dataSource = new();

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            _listView = new Terminal.Gui.ListView
            {
                X = 0,
                Y = 0,
                Height = Dim.Fill(1),
                Width = Dim.Fill(),
            };

            _listView.RowRender += OnRenderRow;
            _listView.SetSource(_dataSource);
            Add(_listView);

            var closeButton = new Button
            {
                Text = Resources.Close,
                IsDefault = true,
            };
            closeButton.Clicked += OnClickClose;
            AddButton(closeButton);

            _backgroundWorker.DoWork += OnBackgroundWorkerDoWork;
            _backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerCompleted;

            _backupEvents.OnTaskProgress += OnTaskProgress;
            _backupEvents.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;
            _backupEvents.OnMessage += OnAddLastMinuteMessageToUser;
        }
    }
}
