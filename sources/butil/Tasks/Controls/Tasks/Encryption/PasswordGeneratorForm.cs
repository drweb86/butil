using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Globalization;
using BUtil.Core.Options;
using BUtil.Core.Localization;

namespace BUtil.Configurator.Tasks.Controls.Tasks.Encryption
{
    public partial class PasswordGeneratorForm : Form
    {
        NumericUpDown passwordLengthNumericUpDown;

        public string Password
        {
            get { return passwordTextBox.Text; }
        }

        public PasswordGeneratorForm()
        {
            InitializeComponent();

            passwordLengthNumericUpDown.Value = 50;
            passwordLengthNumericUpDown.Minimum = 1;
            passwordLengthNumericUpDown.Maximum = 8000;

            // localization initialization ("Password generator Program Interface" namespace);
            Text = Resources.Password_Generator_Header;
            cancelButton.Text = Resources.Button_Cancel;
            copyToClipboardButton.Text = Resources.Button_CopyToClipboard;
            generateButton.Text = Resources.Password_Generate;
            passwordLengthLabel.Text = Resources.Password_Field_Length;
            useButton.Text = Resources.Password_Use;
            generatePasswordButtonClick(this, null);
        }

        void generatePasswordButtonClick(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(passwordLengthNumericUpDown.Value, CultureInfo.InvariantCulture);
            string temp = string.Empty;
            bool suit = false;
            char ch;

            byte[] resultentropy = new byte[count];
            byte[] tempentropy = new byte[count];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(resultentropy);
                randomNumberGenerator.GetBytes(tempentropy);

                for (int i = 0; i < count; i++)
                {
                    suit = false;
                    do
                    {
                        randomNumberGenerator.GetBytes(tempentropy);
                        ch = (char)tempentropy[i];

                        if (ch >= 'a' && ch <= 'z' || ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z') suit = true;
                    }
                    while (!suit);

                    resultentropy[i] = tempentropy[i];
                }
            }

            for (int i = 0; i < count; i++) temp += Convert.ToChar(resultentropy[i]);

            passwordTextBox.Text = temp.ToString();
            useButton.Enabled = true;
            copyToClipboardButton.Enabled = true;
        }

        void cancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        void copyToClipboardButtonClick(object sender, EventArgs e)
        {
            passwordTextBox.SelectAll();
            passwordTextBox.Copy();
            passwordTextBox.Select(0, 0);
        }

        private TableLayoutPanel _tableLayoutPanel;
        private TableLayoutPanel _mainTableLayoutPanel;
        private TableLayoutPanel _bottomTableLayoutPanel;
        private Button useButton;
        private TableLayoutPanel _contentTableLayoutPanel;
        private Button copyToClipboardButton;
        private Button cancelButton;
        #region designer
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordGeneratorForm));
            _tableLayoutPanel = new TableLayoutPanel();
            passwordLengthLabel = new Label();
            passwordLengthNumericUpDown = new NumericUpDown();
            passwordTextBox = new TextBox();
            generateButton = new Button();
            _mainTableLayoutPanel = new TableLayoutPanel();
            cancelButton = new Button();
            copyToClipboardButton = new Button();
            _contentTableLayoutPanel = new TableLayoutPanel();
            useButton = new Button();
            _bottomTableLayoutPanel = new TableLayoutPanel();
            _tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)passwordLengthNumericUpDown).BeginInit();
            _mainTableLayoutPanel.SuspendLayout();
            _bottomTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.ColumnCount = 3;
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _tableLayoutPanel.Controls.Add(passwordLengthLabel, 0, 0);
            _tableLayoutPanel.Controls.Add(generateButton, 2, 0);
            _tableLayoutPanel.Controls.Add(passwordLengthNumericUpDown, 1, 0);
            _tableLayoutPanel.Location = new System.Drawing.Point(13, 14);
            _tableLayoutPanel.Margin = new Padding(5, 6, 5, 6);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.RowCount = 1;
            _tableLayoutPanel.RowStyles.Add(new RowStyle());
            _tableLayoutPanel.Size = new System.Drawing.Size(959, 60);
            _tableLayoutPanel.TabIndex = 4;
            // 
            // passwordLengthLabel
            // 
            passwordLengthLabel.AutoSize = true;
            passwordLengthLabel.Dock = DockStyle.Fill;
            passwordLengthLabel.Location = new System.Drawing.Point(7, 0);
            passwordLengthLabel.Margin = new Padding(7, 0, 7, 0);
            passwordLengthLabel.Name = "passwordLengthLabel";
            passwordLengthLabel.Size = new System.Drawing.Size(169, 60);
            passwordLengthLabel.TabIndex = 0;
            passwordLengthLabel.Text = "Password length:";
            passwordLengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordLengthNumericUpDown
            // 
            passwordLengthNumericUpDown.Anchor = AnchorStyles.Left;
            passwordLengthNumericUpDown.Location = new System.Drawing.Point(191, 12);
            passwordLengthNumericUpDown.Margin = new Padding(8, 0, 0, 0);
            passwordLengthNumericUpDown.Maximum = new decimal(new int[] { 256, 0, 0, 0 });
            passwordLengthNumericUpDown.Minimum = new decimal(new int[] { 12, 0, 0, 0 });
            passwordLengthNumericUpDown.Name = "passwordLengthNumericUpDown";
            passwordLengthNumericUpDown.Size = new System.Drawing.Size(646, 35);
            passwordLengthNumericUpDown.TabIndex = 3;
            passwordLengthNumericUpDown.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // passwordTextBox
            // 
            passwordTextBox.Dock = DockStyle.Fill;
            passwordTextBox.Location = new System.Drawing.Point(15, 86);
            passwordTextBox.Margin = new Padding(7, 6, 7, 6);
            passwordTextBox.Multiline = true;
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.ReadOnly = true;
            passwordTextBox.ScrollBars = ScrollBars.Vertical;
            passwordTextBox.Size = new System.Drawing.Size(955, 267);
            passwordTextBox.TabIndex = 2;
            passwordTextBox.Text = "<Here is your password>";
            // 
            // generateButton
            // 
            generateButton.Anchor = AnchorStyles.Left;
            generateButton.AutoSize = true;
            generateButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            generateButton.Location = new System.Drawing.Point(845, 10);
            generateButton.Margin = new Padding(8, 6, 7, 6);
            generateButton.Name = "generateButton";
            generateButton.Size = new System.Drawing.Size(107, 40);
            generateButton.TabIndex = 3;
            generateButton.Text = "Generate";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += generatePasswordButtonClick;
            // 
            // _mainTableLayoutPanel
            // 
            _mainTableLayoutPanel.ColumnCount = 1;
            _mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _mainTableLayoutPanel.Controls.Add(_tableLayoutPanel, 0, 0);
            _mainTableLayoutPanel.Controls.Add(_bottomTableLayoutPanel, 0, 2);
            _mainTableLayoutPanel.Controls.Add(passwordTextBox, 0, 1);
            _mainTableLayoutPanel.Dock = DockStyle.Fill;
            _mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _mainTableLayoutPanel.Margin = new Padding(8);
            _mainTableLayoutPanel.Name = "_mainTableLayoutPanel";
            _mainTableLayoutPanel.Padding = new Padding(8);
            _mainTableLayoutPanel.RowCount = 3;
            _mainTableLayoutPanel.RowStyles.Add(new RowStyle());
            _mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _mainTableLayoutPanel.RowStyles.Add(new RowStyle());
            _mainTableLayoutPanel.Size = new System.Drawing.Size(985, 425);
            _mainTableLayoutPanel.TabIndex = 9;
            // 
            // cancelButton
            // 
            cancelButton.AutoSize = true;
            cancelButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Dock = DockStyle.Fill;
            cancelButton.Location = new System.Drawing.Point(800, 6);
            cancelButton.Margin = new Padding(7, 6, 7, 6);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(85, 40);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButtonClick;
            // 
            // copyToClipboardButton
            // 
            copyToClipboardButton.AutoSize = true;
            copyToClipboardButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            copyToClipboardButton.Dock = DockStyle.Fill;
            copyToClipboardButton.Enabled = false;
            copyToClipboardButton.Location = new System.Drawing.Point(602, 6);
            copyToClipboardButton.Margin = new Padding(8, 6, 7, 6);
            copyToClipboardButton.Name = "copyToClipboardButton";
            copyToClipboardButton.Size = new System.Drawing.Size(184, 40);
            copyToClipboardButton.TabIndex = 4;
            copyToClipboardButton.Text = "copy to clipboard";
            copyToClipboardButton.UseVisualStyleBackColor = true;
            copyToClipboardButton.Click += copyToClipboardButtonClick;
            // 
            // _contentTableLayoutPanel
            // 
            _contentTableLayoutPanel.AutoSize = true;
            _contentTableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _contentTableLayoutPanel.ColumnCount = 2;
            _contentTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _contentTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _contentTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            _contentTableLayoutPanel.Name = "_contentTableLayoutPanel";
            _contentTableLayoutPanel.RowCount = 2;
            _contentTableLayoutPanel.RowStyles.Add(new RowStyle());
            _contentTableLayoutPanel.RowStyles.Add(new RowStyle());
            _contentTableLayoutPanel.Size = new System.Drawing.Size(0, 0);
            _contentTableLayoutPanel.TabIndex = 7;
            // 
            // useButton
            // 
            useButton.AutoSize = true;
            useButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            useButton.Dock = DockStyle.Fill;
            useButton.Enabled = false;
            useButton.Location = new System.Drawing.Point(899, 6);
            useButton.Margin = new Padding(7, 6, 7, 6);
            useButton.Name = "useButton";
            useButton.Size = new System.Drawing.Size(57, 40);
            useButton.TabIndex = 5;
            useButton.Text = "Use";
            useButton.UseVisualStyleBackColor = true;
            useButton.Click += useButtonClick;
            // 
            // _bottomTableLayoutPanel
            // 
            _bottomTableLayoutPanel.AutoSize = true;
            _bottomTableLayoutPanel.ColumnCount = 4;
            _bottomTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bottomTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _bottomTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _bottomTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _bottomTableLayoutPanel.Controls.Add(useButton, 3, 0);
            _bottomTableLayoutPanel.Controls.Add(_contentTableLayoutPanel, 0, 0);
            _bottomTableLayoutPanel.Controls.Add(copyToClipboardButton, 1, 0);
            _bottomTableLayoutPanel.Controls.Add(cancelButton, 2, 0);
            _bottomTableLayoutPanel.Dock = DockStyle.Fill;
            _bottomTableLayoutPanel.Location = new System.Drawing.Point(11, 362);
            _bottomTableLayoutPanel.Name = "_bottomTableLayoutPanel";
            _bottomTableLayoutPanel.RowCount = 1;
            _bottomTableLayoutPanel.RowStyles.Add(new RowStyle());
            _bottomTableLayoutPanel.Size = new System.Drawing.Size(963, 52);
            _bottomTableLayoutPanel.TabIndex = 8;
            // 
            // PasswordGeneratorForm
            // 
            AcceptButton = generateButton;
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(985, 425);
            Controls.Add(_mainTableLayoutPanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(7, 6, 7, 6);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(1009, 489);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(690, 489);
            Name = "PasswordGeneratorForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Password generator";
            _tableLayoutPanel.ResumeLayout(false);
            _tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)passwordLengthNumericUpDown).EndInit();
            _mainTableLayoutPanel.ResumeLayout(false);
            _mainTableLayoutPanel.PerformLayout();
            _bottomTableLayoutPanel.ResumeLayout(false);
            _bottomTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }

        private Label passwordLengthLabel;
        private Button generateButton;
        private TextBox passwordTextBox;


        #endregion

        private void useButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
