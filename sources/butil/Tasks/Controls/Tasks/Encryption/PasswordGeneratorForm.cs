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
            optionsGroupBox.Text = string.Empty;
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
            optionsGroupBox = new GroupBox();
            _tableLayoutPanel = new TableLayoutPanel();
            passwordLengthLabel = new Label();
            passwordLengthNumericUpDown = new NumericUpDown();
            passwordTextBox = new TextBox();
            generateButton = new Button();
            copyToClipboardButton = new Button();
            cancelButton = new Button();
            useButton = new Button();
            optionsGroupBox.SuspendLayout();
            _tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)passwordLengthNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // optionsGroupBox
            // 
            optionsGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left
            | AnchorStyles.Right;
            optionsGroupBox.Controls.Add(_tableLayoutPanel);
            optionsGroupBox.Location = new System.Drawing.Point(8, 6);
            optionsGroupBox.Margin = new Padding(4, 3, 4, 3);
            optionsGroupBox.Name = "optionsGroupBox";
            optionsGroupBox.Padding = new Padding(4, 3, 4, 3);
            optionsGroupBox.Size = new System.Drawing.Size(569, 53);
            optionsGroupBox.TabIndex = 1;
            optionsGroupBox.TabStop = false;
            optionsGroupBox.Text = "Options";
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.ColumnCount = 2;
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _tableLayoutPanel.Controls.Add(passwordLengthLabel, 0, 0);
            _tableLayoutPanel.Controls.Add(passwordLengthNumericUpDown, 1, 0);
            _tableLayoutPanel.Dock = DockStyle.Fill;
            _tableLayoutPanel.Location = new System.Drawing.Point(4, 19);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.RowCount = 1;
            _tableLayoutPanel.RowStyles.Add(new RowStyle());
            _tableLayoutPanel.Size = new System.Drawing.Size(561, 31);
            _tableLayoutPanel.TabIndex = 4;
            // 
            // passwordLengthLabel
            // 
            passwordLengthLabel.AutoSize = true;
            passwordLengthLabel.Dock = DockStyle.Fill;
            passwordLengthLabel.Location = new System.Drawing.Point(4, 0);
            passwordLengthLabel.Margin = new Padding(4, 0, 4, 0);
            passwordLengthLabel.Name = "passwordLengthLabel";
            passwordLengthLabel.Size = new System.Drawing.Size(97, 31);
            passwordLengthLabel.TabIndex = 0;
            passwordLengthLabel.Text = "Password length:";
            passwordLengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // passwordLengthNumericUpDown
            // 
            passwordLengthNumericUpDown.Dock = DockStyle.Fill;
            passwordLengthNumericUpDown.Location = new System.Drawing.Point(109, 3);
            passwordLengthNumericUpDown.Margin = new Padding(4, 3, 4, 3);
            passwordLengthNumericUpDown.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            passwordLengthNumericUpDown.Minimum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            passwordLengthNumericUpDown.Name = "passwordLengthNumericUpDown";
            passwordLengthNumericUpDown.Size = new System.Drawing.Size(448, 23);
            passwordLengthNumericUpDown.TabIndex = 3;
            passwordLengthNumericUpDown.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // passwordTextBox
            // 
            passwordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right;
            passwordTextBox.Location = new System.Drawing.Point(9, 69);
            passwordTextBox.Margin = new Padding(4, 3, 4, 3);
            passwordTextBox.Multiline = true;
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.ReadOnly = true;
            passwordTextBox.ScrollBars = ScrollBars.Vertical;
            passwordTextBox.Size = new System.Drawing.Size(443, 123);
            passwordTextBox.TabIndex = 2;
            passwordTextBox.Text = "<Here is your password>";
            // 
            // generateButton
            // 
            generateButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            generateButton.AutoSize = true;
            generateButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            generateButton.Location = new System.Drawing.Point(460, 69);
            generateButton.Margin = new Padding(4, 3, 4, 3);
            generateButton.MinimumSize = new System.Drawing.Size(118, 29);
            generateButton.Name = "generateButton";
            generateButton.Size = new System.Drawing.Size(118, 29);
            generateButton.TabIndex = 3;
            generateButton.Text = "Generate";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += new EventHandler(generatePasswordButtonClick);
            // 
            // copyToClipboardButton
            // 
            copyToClipboardButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyToClipboardButton.AutoSize = true;
            copyToClipboardButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            copyToClipboardButton.Enabled = false;
            copyToClipboardButton.Location = new System.Drawing.Point(460, 105);
            copyToClipboardButton.Margin = new Padding(4, 3, 4, 3);
            copyToClipboardButton.MinimumSize = new System.Drawing.Size(118, 29);
            copyToClipboardButton.Name = "copyToClipboardButton";
            copyToClipboardButton.Size = new System.Drawing.Size(118, 29);
            copyToClipboardButton.TabIndex = 4;
            copyToClipboardButton.Text = "copy to clipboard";
            copyToClipboardButton.UseVisualStyleBackColor = true;
            copyToClipboardButton.Click += new EventHandler(copyToClipboardButtonClick);
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.AutoSize = true;
            cancelButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(490, 200);
            cancelButton.Margin = new Padding(4, 3, 4, 3);
            cancelButton.MinimumSize = new System.Drawing.Size(88, 27);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(88, 27);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += new EventHandler(cancelButtonClick);
            // 
            // useButton
            // 
            useButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            useButton.AutoSize = true;
            useButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            useButton.Enabled = false;
            useButton.Location = new System.Drawing.Point(396, 200);
            useButton.Margin = new Padding(4, 3, 4, 3);
            useButton.MinimumSize = new System.Drawing.Size(88, 27);
            useButton.Name = "useButton";
            useButton.Size = new System.Drawing.Size(88, 27);
            useButton.TabIndex = 5;
            useButton.Text = "Use";
            useButton.UseVisualStyleBackColor = true;
            useButton.Click += new EventHandler(useButtonClick);
            // 
            // PasswordGeneratorForm
            // 
            AcceptButton = generateButton;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(582, 234);
            Controls.Add(useButton);
            Controls.Add(cancelButton);
            Controls.Add(copyToClipboardButton);
            Controls.Add(generateButton);
            Controls.Add(passwordTextBox);
            Controls.Add(optionsGroupBox);
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(598, 273);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(412, 273);
            Name = "PasswordGeneratorForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Password generator";
            optionsGroupBox.ResumeLayout(false);
            _tableLayoutPanel.ResumeLayout(false);
            _tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)passwordLengthNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }
        private Button copyToClipboardButton;
        private Button cancelButton;
        private Label passwordLengthLabel;
        private Button generateButton;
        private TextBox passwordTextBox;
        private Button useButton;
        private GroupBox optionsGroupBox;


        #endregion

        private void useButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
