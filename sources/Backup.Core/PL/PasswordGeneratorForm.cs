using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Security;
using System.IO;
using System.Globalization;
using BULocalization;
using BUtil;
using BUtil.Core.Options;

namespace BUtil.Core.PL
{
	/// <summary>
	/// Password generator
	/// </summary>
	public partial class PasswordGeneratorForm : Form
	{
        NumericUpDown passwordLengthNumericUpDown;

        /// <summary>
        /// Returns generated password
        /// </summary>
        public string Password
        {
            get { return passwordTextBox.Text; }
        }
        
		public PasswordGeneratorForm()
		{
			InitializeComponent();

			passwordLengthNumericUpDown.Value = Constants.MinimumPasswordLength;
			passwordLengthNumericUpDown.Minimum = Constants.MinimumPasswordLength;
			passwordLengthNumericUpDown.Maximum = Constants.MaximumPasswordLength;

			// localization initialization ("Password generator Program Interface" namespace);
			this.Text = Translation.Current[3];
			cancelButton.Text = Translation.Current[4];
			copyToClipboardButton.Text = Translation.Current[5];
			generateButton.Text = Translation.Current[6];
			passwordTextBox.Text = Translation.Current[7];
			passwordLengthLabel.Text = Translation.Current[8];
			optionsGroupBox.Text = Translation.Current[10];
			useButton.Text = Translation.Current[307];
		}
		
		void generatePasswordButtonClick(object sender, EventArgs e)
		{
			int count = Convert.ToInt32(passwordLengthNumericUpDown.Value, CultureInfo.InvariantCulture);
			string temp = string.Empty;
			bool suit = false;
			char ch;
			
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] resultentropy = new byte[count];
			byte[] tempentropy = new byte[count];
			
			rng.GetBytes(resultentropy);
			rng.GetBytes(tempentropy);
			
			
			for (int i = 0; i < count; i++)
			{
				suit = false;
				do
				{
					rng.GetBytes(tempentropy);
					ch = (char)(tempentropy[i]);
					
					if (((ch >= 'a') && (ch <='z')) || ((ch >='0')&&(ch <= '9')) || ((ch >='A')&&(ch <='Z'))) suit = true;
				}
				while(!suit);
					
				resultentropy[i] = tempentropy[i];
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
        	this.passwordLengthNumericUpDown = new System.Windows.Forms.NumericUpDown();
        	this.passwordLengthLabel = new System.Windows.Forms.Label();
        	this.passwordTextBox = new System.Windows.Forms.TextBox();
        	this.generateButton = new System.Windows.Forms.Button();
        	this.copyToClipboardButton = new System.Windows.Forms.Button();
        	this.cancelButton = new System.Windows.Forms.Button();
        	this.useButton = new System.Windows.Forms.Button();
        	this.optionsGroupBox.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.passwordLengthNumericUpDown)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// optionsGroupBox
        	// 
        	this.optionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.optionsGroupBox.Controls.Add(this.passwordLengthNumericUpDown);
        	this.optionsGroupBox.Controls.Add(this.passwordLengthLabel);
        	this.optionsGroupBox.Location = new System.Drawing.Point(7, 5);
        	this.optionsGroupBox.Name = "optionsGroupBox";
        	this.optionsGroupBox.Size = new System.Drawing.Size(488, 46);
        	this.optionsGroupBox.TabIndex = 1;
        	this.optionsGroupBox.TabStop = false;
        	this.optionsGroupBox.Text = "Options";
        	// 
        	// passwordLengthNumericUpDown
        	// 
        	this.passwordLengthNumericUpDown.Location = new System.Drawing.Point(130, 19);
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
        	this.passwordLengthNumericUpDown.Size = new System.Drawing.Size(120, 20);
        	this.passwordLengthNumericUpDown.TabIndex = 3;
        	this.passwordLengthNumericUpDown.Value = new decimal(new int[] {
        	        	        	12,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	// 
        	// passwordLengthLabel
        	// 
        	this.passwordLengthLabel.AutoSize = true;
        	this.passwordLengthLabel.Location = new System.Drawing.Point(18, 20);
        	this.passwordLengthLabel.Name = "passwordLengthLabel";
        	this.passwordLengthLabel.Size = new System.Drawing.Size(88, 13);
        	this.passwordLengthLabel.TabIndex = 0;
        	this.passwordLengthLabel.Text = "Password length:";
        	// 
        	// passwordTextBox
        	// 
        	this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.passwordTextBox.Location = new System.Drawing.Point(8, 60);
        	this.passwordTextBox.Multiline = true;
        	this.passwordTextBox.Name = "passwordTextBox";
        	this.passwordTextBox.ReadOnly = true;
        	this.passwordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        	this.passwordTextBox.Size = new System.Drawing.Size(380, 107);
        	this.passwordTextBox.TabIndex = 2;
        	this.passwordTextBox.Text = "<Here is your password>";
        	// 
        	// generateButton
        	// 
        	this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.generateButton.AutoSize = true;
        	this.generateButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.generateButton.Location = new System.Drawing.Point(394, 60);
        	this.generateButton.MinimumSize = new System.Drawing.Size(101, 25);
        	this.generateButton.Name = "generateButton";
        	this.generateButton.Size = new System.Drawing.Size(101, 25);
        	this.generateButton.TabIndex = 3;
        	this.generateButton.Text = "Generate";
        	this.generateButton.UseVisualStyleBackColor = true;
        	this.generateButton.Click += new System.EventHandler(this.generatePasswordButtonClick);
        	// 
        	// CopyClipboardbutton
        	// 
        	this.copyToClipboardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.copyToClipboardButton.AutoSize = true;
        	this.copyToClipboardButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.copyToClipboardButton.Enabled = false;
        	this.copyToClipboardButton.Location = new System.Drawing.Point(394, 91);
        	this.copyToClipboardButton.MinimumSize = new System.Drawing.Size(101, 25);
        	this.copyToClipboardButton.Name = "CopyClipboardbutton";
        	this.copyToClipboardButton.Size = new System.Drawing.Size(101, 25);
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
        	this.cancelButton.Location = new System.Drawing.Point(420, 173);
        	this.cancelButton.MinimumSize = new System.Drawing.Size(75, 23);
        	this.cancelButton.Name = "cancelButton";
        	this.cancelButton.Size = new System.Drawing.Size(75, 23);
        	this.cancelButton.TabIndex = 6;
        	this.cancelButton.Text = "Cancel";
        	this.cancelButton.UseVisualStyleBackColor = true;
        	this.cancelButton.Click += new System.EventHandler(this.cancelButtonClick);
        	// 
        	// OKbutton
        	// 
        	this.useButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.useButton.AutoSize = true;
        	this.useButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.useButton.Enabled = false;
        	this.useButton.Location = new System.Drawing.Point(339, 173);
        	this.useButton.MinimumSize = new System.Drawing.Size(75, 23);
        	this.useButton.Name = "OKbutton";
        	this.useButton.Size = new System.Drawing.Size(75, 23);
        	this.useButton.TabIndex = 5;
        	this.useButton.Text = "Use";
        	this.useButton.UseVisualStyleBackColor = true;
        	this.useButton.Click += new System.EventHandler(this.useButtonClick);
        	// 
        	// PasswordGeneratorForm
        	// 
        	this.AcceptButton = this.generateButton;
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.CancelButton = this.cancelButton;
        	this.ClientSize = new System.Drawing.Size(507, 208);
        	this.Controls.Add(this.useButton);
        	this.Controls.Add(this.cancelButton);
        	this.Controls.Add(this.copyToClipboardButton);
        	this.Controls.Add(this.generateButton);
        	this.Controls.Add(this.passwordTextBox);
        	this.Controls.Add(this.optionsGroupBox);
        	this.MaximizeBox = false;
        	this.MaximumSize = new System.Drawing.Size(515, 242);
        	this.MinimizeBox = false;
        	this.MinimumSize = new System.Drawing.Size(355, 242);
        	this.Name = "PasswordGeneratorForm";
        	this.ShowIcon = false;
        	this.ShowInTaskbar = false;
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Password generator";
        	this.optionsGroupBox.ResumeLayout(false);
        	this.optionsGroupBox.PerformLayout();
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
			ProgramOptionsManager.ValidatePassword(true, passwordTextBox.Text);

            this.DialogResult = DialogResult.OK;
        }
    }
}
