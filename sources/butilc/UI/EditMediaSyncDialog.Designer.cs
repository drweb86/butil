
namespace BUtil.ConsoleBackup.UI {
    using System;
    using Terminal.Gui;
    
    public partial class EditMediaSyncDialog : Terminal.Gui.Dialog
    {
        private Terminal.Gui.TextField titleTextField;
        private Terminal.Gui.TextField mediaSourceTextField;
        private Terminal.Gui.TextField destinationTextField;
        private Terminal.Gui.TextField transformFileNameTextField;

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 0,
                Text = "Title"
            });

            titleTextField = new TextField
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(0),
            };
            Add(titleTextField);

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 3,
                Text = "DCIM folder on media device"
            });

            mediaSourceTextField = new TextField
            {
                Text = "c:\\",
                X = 0,
                Y = 4,
                Width = Dim.Fill(0),
            };
            Add(mediaSourceTextField);

            //Add(new Button
            //{
            //    Text = "Browse",
            //    X = 0,
            //    Y = 5,
            //    AutoSize = true,
            //});



            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 6,
                Text = "Library folder at PC"
            });

            destinationTextField = new TextField
            {
                Text = "c:\\",
                X = 0,
                Y = 7,
                Width = Dim.Fill(0),
            };
            Add(destinationTextField);

            //Add(new Button
            //{
            //    Text = "Browse",
            //    X = 0,
            //    Y = 9,
            //    AutoSize = true,
            //});



            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 9,
                Text = "Transform file name:"
            });

            transformFileNameTextField = new TextField
            {
                Text = "c:\\",
                X = 0,
                Y = 10,
                Width = Dim.Fill(0),
            };
            Add(transformFileNameTextField);
            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 11,
                Text = "Tokens:\n{INPUT NAME ONCE} - Inputs name once for all pictures\n{DATE:Format} - Inserts file modification date in the specified format.\nExample: {DATE:MMMM dd}.\nTo see all options google \"C# DateTime Format\""
            });


            var saveButton = new Button
            {
                Text = "Save",
                IsDefault = true,
            };
            saveButton.Clicked += OnSave;
            AddButton(saveButton);

            var cancelButton = new Button
            {
                Text = "Cancel",
            };
            cancelButton.Clicked += OnCancel;
            AddButton(cancelButton);
        }
    }
}
