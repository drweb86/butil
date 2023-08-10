
namespace BUtil.ConsoleBackup.UI {
    using Terminal.Gui;
    
    public partial class BackupDialog : Terminal.Gui.Dialog
    {
        private Terminal.Gui.ListView _listView;

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            _listView = new Terminal.Gui.ListView
            {
                X = 0,
                Y = 0,
                Height = Dim.Fill(),
                Width = Dim.Fill(),
            };
            _listView.RowRender += OnRowRender;
            Add(_listView);

            var closeButton = new Button
            {
                Text = "Close",
                IsDefault = true,
            };
            closeButton.Clicked += OnClose;
            AddButton(closeButton);
        }
    }
}
