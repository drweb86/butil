namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    partial class ImportMediaTaskWhereUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _outputFolderLabel = new System.Windows.Forms.Label();
            _outputFolderTextBox = new System.Windows.Forms.TextBox();
            _tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _folderBrowseButton = new System.Windows.Forms.Button();
            _transformFileNameLabel = new System.Windows.Forms.Label();
            _backupModelLabel = new System.Windows.Forms.Label();
            _transformFileTemplateTextBox = new System.Windows.Forms.TextBox();
            _transformFIleNameExampleLabel = new System.Windows.Forms.Label();
            _helpTokensLabel = new System.Windows.Forms.Label();
            _skipPreviouslyImportedFilesCheckbox = new System.Windows.Forms.CheckBox();
            _folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            _tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _outputFolderLabel
            // 
            _outputFolderLabel.AutoSize = true;
            _outputFolderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _outputFolderLabel.Location = new System.Drawing.Point(29, 27);
            _outputFolderLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            _outputFolderLabel.Name = "_outputFolderLabel";
            _outputFolderLabel.Size = new System.Drawing.Size(168, 48);
            _outputFolderLabel.TabIndex = 0;
            _outputFolderLabel.Text = "Output folder:";
            _outputFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _outputFolderTextBox
            // 
            _outputFolderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _outputFolderTextBox.Location = new System.Drawing.Point(207, 39);
            _outputFolderTextBox.Margin = new System.Windows.Forms.Padding(4, 12, 4, 5);
            _outputFolderTextBox.Name = "_outputFolderTextBox";
            _outputFolderTextBox.Size = new System.Drawing.Size(574, 31);
            _outputFolderTextBox.TabIndex = 1;
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _tableLayoutPanel.ColumnCount = 3;
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.Controls.Add(_outputFolderLabel, 0, 0);
            _tableLayoutPanel.Controls.Add(_outputFolderTextBox, 1, 0);
            _tableLayoutPanel.Controls.Add(_folderBrowseButton, 2, 0);
            _tableLayoutPanel.Controls.Add(_transformFileNameLabel, 0, 1);
            _tableLayoutPanel.Controls.Add(_backupModelLabel, 1, 5);
            _tableLayoutPanel.Controls.Add(_transformFileTemplateTextBox, 1, 1);
            _tableLayoutPanel.Controls.Add(_transformFIleNameExampleLabel, 1, 2);
            _tableLayoutPanel.Controls.Add(_helpTokensLabel, 1, 3);
            _tableLayoutPanel.Controls.Add(_skipPreviouslyImportedFilesCheckbox, 1, 4);
            _tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _tableLayoutPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.Padding = new System.Windows.Forms.Padding(23, 27, 23, 0);
            _tableLayoutPanel.RowCount = 6;
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tableLayoutPanel.Size = new System.Drawing.Size(854, 638);
            _tableLayoutPanel.TabIndex = 2;
            // 
            // _folderBrowseButton
            // 
            _folderBrowseButton.AutoSize = true;
            _folderBrowseButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _folderBrowseButton.Image = Icons.FolderOrangeOpen_32x32;
            _folderBrowseButton.Location = new System.Drawing.Point(789, 32);
            _folderBrowseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _folderBrowseButton.Name = "_folderBrowseButton";
            _folderBrowseButton.Size = new System.Drawing.Size(38, 38);
            _folderBrowseButton.TabIndex = 5;
            _folderBrowseButton.UseVisualStyleBackColor = true;
            _folderBrowseButton.Click += OnFolderBrowseButtonClick;
            // 
            // _transformFileNameLabel
            // 
            _transformFileNameLabel.AutoSize = true;
            _transformFileNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _transformFileNameLabel.Location = new System.Drawing.Point(27, 75);
            _transformFileNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _transformFileNameLabel.Name = "_transformFileNameLabel";
            _transformFileNameLabel.Size = new System.Drawing.Size(172, 41);
            _transformFileNameLabel.TabIndex = 6;
            _transformFileNameLabel.Text = "Transform file name:";
            _transformFileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _backupModelLabel
            // 
            _backupModelLabel.AutoSize = true;
            _backupModelLabel.Location = new System.Drawing.Point(207, 255);
            _backupModelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _backupModelLabel.MaximumSize = new System.Drawing.Size(757, 0);
            _backupModelLabel.Name = "_backupModelLabel";
            _backupModelLabel.Size = new System.Drawing.Size(0, 25);
            _backupModelLabel.TabIndex = 4;
            // 
            // _transformFileTemplateTextBox
            // 
            _transformFileTemplateTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _transformFileTemplateTextBox.Location = new System.Drawing.Point(207, 80);
            _transformFileTemplateTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _transformFileTemplateTextBox.Name = "_transformFileTemplateTextBox";
            _transformFileTemplateTextBox.Size = new System.Drawing.Size(574, 31);
            _transformFileTemplateTextBox.TabIndex = 7;
            _transformFileTemplateTextBox.TextChanged += OnChangeTransormFileName;
            // 
            // _transformFIleNameExampleLabel
            // 
            _transformFIleNameExampleLabel.AutoSize = true;
            _transformFIleNameExampleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _transformFIleNameExampleLabel.Location = new System.Drawing.Point(207, 116);
            _transformFIleNameExampleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _transformFIleNameExampleLabel.Name = "_transformFIleNameExampleLabel";
            _transformFIleNameExampleLabel.Size = new System.Drawing.Size(574, 25);
            _transformFIleNameExampleLabel.TabIndex = 8;
            _transformFIleNameExampleLabel.Text = "<Example>";
            // 
            // _helpTokensLabel
            // 
            _helpTokensLabel.AutoSize = true;
            _helpTokensLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _helpTokensLabel.Location = new System.Drawing.Point(207, 141);
            _helpTokensLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _helpTokensLabel.Name = "_helpTokensLabel";
            _helpTokensLabel.Size = new System.Drawing.Size(574, 75);
            _helpTokensLabel.TabIndex = 9;
            _helpTokensLabel.Text = "Help1\r\nHelp2\r\nHelp3";
            // 
            // _skipPreviouslyImportedFilesCheckbox
            // 
            _skipPreviouslyImportedFilesCheckbox.AutoSize = true;
            _skipPreviouslyImportedFilesCheckbox.Dock = System.Windows.Forms.DockStyle.Fill;
            _skipPreviouslyImportedFilesCheckbox.Location = new System.Drawing.Point(207, 221);
            _skipPreviouslyImportedFilesCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _skipPreviouslyImportedFilesCheckbox.Name = "_skipPreviouslyImportedFilesCheckbox";
            _skipPreviouslyImportedFilesCheckbox.Size = new System.Drawing.Size(574, 29);
            _skipPreviouslyImportedFilesCheckbox.TabIndex = 10;
            _skipPreviouslyImportedFilesCheckbox.Text = "Skip previously imported files";
            _skipPreviouslyImportedFilesCheckbox.UseVisualStyleBackColor = true;
            // 
            // ImportMediaTaskWhereUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            Name = "ImportMediaTaskWhereUserControl";
            Size = new System.Drawing.Size(854, 638);
            _tableLayoutPanel.ResumeLayout(false);
            _tableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _outputFolderLabel;
        private System.Windows.Forms.TextBox _outputFolderTextBox;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label _backupModelLabel;
        private System.Windows.Forms.Button _folderBrowseButton;
        private System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog;
        private System.Windows.Forms.Label _transformFileNameLabel;
        private System.Windows.Forms.TextBox _transformFileTemplateTextBox;
        private System.Windows.Forms.Label _transformFIleNameExampleLabel;
        private System.Windows.Forms.Label _helpTokensLabel;
        private System.Windows.Forms.CheckBox _skipPreviouslyImportedFilesCheckbox;
    }
}
