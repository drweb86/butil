using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Globalization;
using BUtil.Core.Options;
using BUtil.Core.Localization;

namespace BUtil.Core.PL
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
			this.Text = Resources.PasswordGenerator;
			cancelButton.Text = Resources.Cancel;
			copyToClipboardButton.Text = Resources.CopyToClipboard;
			generateButton.Text = Resources.Generate;
			passwordTextBox.Text = Resources.HereIsYourPassword;
			passwordLengthLabel.Text = Resources.PasswordLength;
			optionsGroupBox.Text = Resources.Options;
			useButton.Text = Resources.Use;
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
					    ch = (char)(tempentropy[i]);
					
					    if (((ch >= 'a') && (ch <='z')) || ((ch >='0')&&(ch <= '9')) || ((ch >='A')&&(ch <='Z'))) suit = true;
				    }
				    while(!suit);
					
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
		    this.DialogResult = DialogResult.Cancel;
		}
		
		void copyToClipboardButtonClick(object sender, EventArgs e)
		{
			passwordTextBox.SelectAll();
			passwordTextBox.Copy();
			passwordTextBox.Select(0,0);
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
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.passwordLengthLabel = new System.Windows.Forms.Label();
            this.passwordLengthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.copyToClipboardButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.useButton = new System.Windows.Forms.Button();
            this.optionsGroupBox.SuspendLayout();
            this._tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passwordLengthNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsGroupBox.Controls.Add(this._tableLayoutPanel);
            this.optionsGroupBox.Location = new System.Drawing.Point(8, 6);
            this.optionsGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.optionsGroupBox.Size = new System.Drawing.Size(569, 53);
            this.optionsGroupBox.TabIndex = 1;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Options";
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.Controls.Add(this.passwordLengthLabel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this.passwordLengthNumericUpDown, 1, 0);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(4, 19);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 1;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(561, 31);
            this._tableLayoutPanel.TabIndex = 4;
            // 
            // passwordLengthLabel
            // 
            this.passwordLengthLabel.AutoSize = true;
            this.passwordLengthLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordLengthLabel.Location = new System.Drawing.Point(4, 0);
            this.passwordLengthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.passwordLengthLabel.Name = "passwordLengthLabel";
            this.passwordLengthLabel.Size = new System.Drawing.Size(97, 31);
            this.passwordLengthLabel.TabIndex = 0;
            this.passwordLengthLabel.Text = "Password length:";
            this.passwordLengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // passwordLengthNumericUpDown
            // 
            this.passwordLengthNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordLengthNumericUpDown.Location = new System.Drawing.Point(109, 3);
            this.passwordLengthNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.passwordLengthNumericUpDown.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.passwordLengthNumericUpDown.Minimum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.passwordLengthNumericUpDown.Name = "passwordLengthNumericUpDown";
            this.passwordLengthNumericUpDown.Size = new System.Drawing.Size(448, 23);
            this.passwordLengthNumericUpDown.TabIndex = 3;
            this.passwordLengthNumericUpDown.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(9, 69);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.passwordTextBox.Multiline = true;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.ReadOnly = true;
            this.passwordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.passwordTextBox.Size = new System.Drawing.Size(443, 123);
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.Text = "<Here is your password>";
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateButton.AutoSize = true;
            this.generateButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.generateButton.Location = new System.Drawing.Point(460, 69);
            this.generateButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.generateButton.MinimumSize = new System.Drawing.Size(118, 29);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(118, 29);
            this.generateButton.TabIndex = 3;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generatePasswordButtonClick);
            // 
            // copyToClipboardButton
            // 
            this.copyToClipboardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyToClipboardButton.AutoSize = true;
            this.copyToClipboardButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.copyToClipboardButton.Enabled = false;
            this.copyToClipboardButton.Location = new System.Drawing.Point(460, 105);
            this.copyToClipboardButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.copyToClipboardButton.MinimumSize = new System.Drawing.Size(118, 29);
            this.copyToClipboardButton.Name = "copyToClipboardButton";
            this.copyToClipboardButton.Size = new System.Drawing.Size(118, 29);
            this.copyToClipboardButton.TabIndex = 4;
            this.copyToClipboardButton.Text = "copy to clipboard";
            this.copyToClipboardButton.UseVisualStyleBackColor = true;
            this.copyToClipboardButton.Click += new System.EventHandler(this.copyToClipboardButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(490, 200);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.MinimumSize = new System.Drawing.Size(88, 27);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 27);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButtonClick);
            // 
            // useButton
            // 
            this.useButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.useButton.AutoSize = true;
            this.useButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.useButton.Enabled = false;
            this.useButton.Location = new System.Drawing.Point(396, 200);
            this.useButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.useButton.MinimumSize = new System.Drawing.Size(88, 27);
            this.useButton.Name = "useButton";
            this.useButton.Size = new System.Drawing.Size(88, 27);
            this.useButton.TabIndex = 5;
            this.useButton.Text = "Use";
            this.useButton.UseVisualStyleBackColor = true;
            this.useButton.Click += new System.EventHandler(this.useButtonClick);
            // 
            // PasswordGeneratorForm
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(582, 234);
            this.Controls.Add(this.useButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.copyToClipboardButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.optionsGroupBox);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(598, 273);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(412, 273);
            this.Name = "PasswordGeneratorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password generator";
            this.optionsGroupBox.ResumeLayout(false);
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passwordLengthNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Button copyToClipboardButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label passwordLengthLabel;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button useButton;
        private System.Windows.Forms.GroupBox optionsGroupBox;
		

        #endregion

        private void useButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
