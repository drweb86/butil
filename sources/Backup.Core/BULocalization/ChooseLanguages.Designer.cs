// BULocalization package 3.2 from http://www.sourceforge.net/projects/bulocalization

namespace BULocalization
{
	partial class ChooseLanguages
	{
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
			if (disposing) {
				if (components != null) {
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
			this._languagelistBox = new System.Windows.Forms.ListBox();
			this._chooseLanguagelabel = new System.Windows.Forms.Label();
			this._selectbutton = new System.Windows.Forms.Button();
			this._cancelbutton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _languagelistBox
			// 
			this._languagelistBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this._languagelistBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this._languagelistBox.FormattingEnabled = true;
			this._languagelistBox.Location = new System.Drawing.Point(12, 44);
			this._languagelistBox.Name = "_languagelistBox";
			this._languagelistBox.ScrollAlwaysVisible = true;
			this._languagelistBox.Size = new System.Drawing.Size(227, 173);
			this._languagelistBox.TabIndex = 0;
			this._languagelistBox.DoubleClick += new System.EventHandler(this.languagelistBoxDoubleClick);
			// 
			// _chooseLanguagelabel
			// 
			this._chooseLanguagelabel.Location = new System.Drawing.Point(12, 9);
			this._chooseLanguagelabel.Name = "_chooseLanguagelabel";
			this._chooseLanguagelabel.Size = new System.Drawing.Size(228, 32);
			this._chooseLanguagelabel.TabIndex = 1;
			this._chooseLanguagelabel.Text = "Please select preferable language among distributed you want to use in the progra" +
			"m";
			this._chooseLanguagelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _selectbutton
			// 
			this._selectbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._selectbutton.Location = new System.Drawing.Point(83, 223);
			this._selectbutton.Name = "_selectbutton";
			this._selectbutton.Size = new System.Drawing.Size(75, 23);
			this._selectbutton.TabIndex = 2;
			this._selectbutton.Text = "Select";
			this._selectbutton.UseVisualStyleBackColor = true;
			this._selectbutton.Click += new System.EventHandler(this.SelectbuttonClick);
			// 
			// _cancelbutton
			// 
			this._cancelbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelbutton.Location = new System.Drawing.Point(164, 223);
			this._cancelbutton.Name = "_cancelbutton";
			this._cancelbutton.Size = new System.Drawing.Size(76, 23);
			this._cancelbutton.TabIndex = 3;
			this._cancelbutton.Text = "Cancel";
			this._cancelbutton.UseVisualStyleBackColor = true;
			// 
			// ChooseLanguages
			// 
			this.AcceptButton = this._selectbutton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelbutton;
			this.ClientSize = new System.Drawing.Size(245, 253);
			this.ControlBox = false;
			this.Controls.Add(this._cancelbutton);
			this.Controls.Add(this._selectbutton);
			this.Controls.Add(this._chooseLanguagelabel);
			this.Controls.Add(this._languagelistBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChooseLanguages";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Choose language - ";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChooseLanguagesFormClosing);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ListBox _languagelistBox;
		private System.Windows.Forms.Label _chooseLanguagelabel;
		private System.Windows.Forms.Button _selectbutton;
		private System.Windows.Forms.Button _cancelbutton;
	}
}
