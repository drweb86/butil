
namespace BUtil.Configurator.Configurator.Controls
{
    partial class WhereUserControl
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the control.
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhereUserControl));
            storageTypesImageList = new System.Windows.Forms.ImageList(components);
            _storageTypesTabControl = new System.Windows.Forms.TabControl();
            _hddStorageTabPage = new System.Windows.Forms.TabPage();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            searchButton = new System.Windows.Forms.Button();
            destinationFolderTextBox = new System.Windows.Forms.TextBox();
            whereToStoreBackupLabel = new System.Windows.Forms.Label();
            _uploadLimitGbNumericUpDownV2 = new System.Windows.Forms.NumericUpDown();
            _unmountScriptLabel = new System.Windows.Forms.Label();
            _mountScriptLabel = new System.Windows.Forms.Label();
            _unmountTextBox = new System.Windows.Forms.TextBox();
            _mountTextBox = new System.Windows.Forms.TextBox();
            _mountButton = new System.Windows.Forms.Button();
            _unmountButton = new System.Windows.Forms.Button();
            _sambaButton = new System.Windows.Forms.Button();
            _scriptsLabel = new System.Windows.Forms.Label();
            _limitUploadLabelV2 = new System.Windows.Forms.LinkLabel();
            _sambaTabPage = new System.Windows.Forms.TabPage();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            _shareTextBox = new System.Windows.Forms.TextBox();
            _shareLabel = new System.Windows.Forms.Label();
            _uploadLimitGbNumericUpDown = new System.Windows.Forms.NumericUpDown();
            _passwordLabel = new System.Windows.Forms.Label();
            _userLabel = new System.Windows.Forms.Label();
            _passwordTextBox = new System.Windows.Forms.TextBox();
            _userTextBox = new System.Windows.Forms.TextBox();
            _limitUploadLabel = new System.Windows.Forms.LinkLabel();
            fbd = new System.Windows.Forms.FolderBrowserDialog();
            _storageTypesTabControl.SuspendLayout();
            _hddStorageTabPage.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_uploadLimitGbNumericUpDownV2).BeginInit();
            _sambaTabPage.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_uploadLimitGbNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // storageTypesImageList
            // 
            storageTypesImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            storageTypesImageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("storageTypesImageList.ImageStream");
            storageTypesImageList.TransparentColor = System.Drawing.Color.White;
            storageTypesImageList.Images.SetKeyName(0, "Hdd48x48.png");
            storageTypesImageList.Images.SetKeyName(1, "Share48x48.png");
            storageTypesImageList.Images.SetKeyName(2, "Ftp16x16.png");
            // 
            // _storageTypesTabControl
            // 
            _storageTypesTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            _storageTypesTabControl.Controls.Add(_hddStorageTabPage);
            _storageTypesTabControl.Controls.Add(_sambaTabPage);
            _storageTypesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _storageTypesTabControl.ImageList = storageTypesImageList;
            _storageTypesTabControl.Location = new System.Drawing.Point(0, 0);
            _storageTypesTabControl.Multiline = true;
            _storageTypesTabControl.Name = "_storageTypesTabControl";
            _storageTypesTabControl.SelectedIndex = 0;
            _storageTypesTabControl.Size = new System.Drawing.Size(469, 248);
            _storageTypesTabControl.TabIndex = 4;
            // 
            // _hddStorageTabPage
            // 
            _hddStorageTabPage.Controls.Add(tableLayoutPanel2);
            _hddStorageTabPage.ImageIndex = 0;
            _hddStorageTabPage.Location = new System.Drawing.Point(4, 58);
            _hddStorageTabPage.Name = "_hddStorageTabPage";
            _hddStorageTabPage.Padding = new System.Windows.Forms.Padding(3);
            _hddStorageTabPage.Size = new System.Drawing.Size(461, 186);
            _hddStorageTabPage.TabIndex = 0;
            _hddStorageTabPage.Text = "HDD";
            _hddStorageTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel2.Controls.Add(searchButton, 2, 0);
            tableLayoutPanel2.Controls.Add(destinationFolderTextBox, 1, 0);
            tableLayoutPanel2.Controls.Add(whereToStoreBackupLabel, 0, 0);
            tableLayoutPanel2.Controls.Add(_uploadLimitGbNumericUpDownV2, 1, 1);
            tableLayoutPanel2.Controls.Add(_unmountScriptLabel, 0, 4);
            tableLayoutPanel2.Controls.Add(_mountScriptLabel, 0, 3);
            tableLayoutPanel2.Controls.Add(_unmountTextBox, 1, 4);
            tableLayoutPanel2.Controls.Add(_mountTextBox, 1, 3);
            tableLayoutPanel2.Controls.Add(_mountButton, 2, 3);
            tableLayoutPanel2.Controls.Add(_unmountButton, 2, 4);
            tableLayoutPanel2.Controls.Add(_sambaButton, 2, 2);
            tableLayoutPanel2.Controls.Add(_scriptsLabel, 0, 2);
            tableLayoutPanel2.Controls.Add(_limitUploadLabelV2, 0, 1);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(20);
            tableLayoutPanel2.RowCount = 6;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.Size = new System.Drawing.Size(455, 180);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // searchButton
            // 
            searchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            searchButton.Location = new System.Drawing.Point(343, 23);
            searchButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            searchButton.Name = "searchButton";
            searchButton.Size = new System.Drawing.Size(88, 23);
            searchButton.TabIndex = 3;
            searchButton.Text = "...";
            searchButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += searchButtonClick;
            // 
            // destinationFolderTextBox
            // 
            destinationFolderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            destinationFolderTextBox.Location = new System.Drawing.Point(222, 23);
            destinationFolderTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            destinationFolderTextBox.Name = "destinationFolderTextBox";
            destinationFolderTextBox.Size = new System.Drawing.Size(113, 23);
            destinationFolderTextBox.TabIndex = 2;
            destinationFolderTextBox.TabStop = false;
            // 
            // whereToStoreBackupLabel
            // 
            whereToStoreBackupLabel.AutoSize = true;
            whereToStoreBackupLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            whereToStoreBackupLabel.Location = new System.Drawing.Point(24, 20);
            whereToStoreBackupLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            whereToStoreBackupLabel.Name = "whereToStoreBackupLabel";
            whereToStoreBackupLabel.Size = new System.Drawing.Size(190, 29);
            whereToStoreBackupLabel.TabIndex = 4;
            whereToStoreBackupLabel.Text = "Folder where to store your backup:";
            whereToStoreBackupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _uploadLimitGbNumericUpDownV2
            // 
            _uploadLimitGbNumericUpDownV2.Dock = System.Windows.Forms.DockStyle.Fill;
            _uploadLimitGbNumericUpDownV2.Location = new System.Drawing.Point(221, 52);
            _uploadLimitGbNumericUpDownV2.Name = "_uploadLimitGbNumericUpDownV2";
            _uploadLimitGbNumericUpDownV2.Size = new System.Drawing.Size(115, 23);
            _uploadLimitGbNumericUpDownV2.TabIndex = 7;
            // 
            // _unmountScriptLabel
            // 
            _unmountScriptLabel.AutoSize = true;
            _unmountScriptLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _unmountScriptLabel.Location = new System.Drawing.Point(24, 161);
            _unmountScriptLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _unmountScriptLabel.Name = "_unmountScriptLabel";
            _unmountScriptLabel.Size = new System.Drawing.Size(190, 50);
            _unmountScriptLabel.TabIndex = 10;
            _unmountScriptLabel.Text = "Unmount:";
            _unmountScriptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _mountScriptLabel
            // 
            _mountScriptLabel.AutoSize = true;
            _mountScriptLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _mountScriptLabel.Location = new System.Drawing.Point(24, 111);
            _mountScriptLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _mountScriptLabel.Name = "_mountScriptLabel";
            _mountScriptLabel.Size = new System.Drawing.Size(190, 50);
            _mountScriptLabel.TabIndex = 9;
            _mountScriptLabel.Text = "Mount:";
            _mountScriptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _unmountTextBox
            // 
            _unmountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _unmountTextBox.Location = new System.Drawing.Point(221, 164);
            _unmountTextBox.Multiline = true;
            _unmountTextBox.Name = "_unmountTextBox";
            _unmountTextBox.Size = new System.Drawing.Size(115, 44);
            _unmountTextBox.TabIndex = 12;
            // 
            // _mountTextBox
            // 
            _mountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _mountTextBox.Location = new System.Drawing.Point(221, 114);
            _mountTextBox.Multiline = true;
            _mountTextBox.Name = "_mountTextBox";
            _mountTextBox.Size = new System.Drawing.Size(115, 44);
            _mountTextBox.TabIndex = 11;
            // 
            // _mountButton
            // 
            _mountButton.Dock = System.Windows.Forms.DockStyle.Fill;
            _mountButton.Location = new System.Drawing.Point(343, 114);
            _mountButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _mountButton.MaximumSize = new System.Drawing.Size(88, 27);
            _mountButton.MinimumSize = new System.Drawing.Size(88, 27);
            _mountButton.Name = "_mountButton";
            _mountButton.Size = new System.Drawing.Size(88, 27);
            _mountButton.TabIndex = 13;
            _mountButton.Text = "Run";
            _mountButton.UseVisualStyleBackColor = true;
            _mountButton.Click += OnMountScript;
            // 
            // _unmountButton
            // 
            _unmountButton.Dock = System.Windows.Forms.DockStyle.Fill;
            _unmountButton.Location = new System.Drawing.Point(343, 164);
            _unmountButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _unmountButton.MaximumSize = new System.Drawing.Size(88, 27);
            _unmountButton.MinimumSize = new System.Drawing.Size(88, 27);
            _unmountButton.Name = "_unmountButton";
            _unmountButton.Size = new System.Drawing.Size(88, 27);
            _unmountButton.TabIndex = 14;
            _unmountButton.Text = "Run";
            _unmountButton.UseVisualStyleBackColor = true;
            _unmountButton.Click += OnUnmount;
            // 
            // _sambaButton
            // 
            _sambaButton.Location = new System.Drawing.Point(343, 81);
            _sambaButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _sambaButton.MaximumSize = new System.Drawing.Size(88, 27);
            _sambaButton.MinimumSize = new System.Drawing.Size(88, 27);
            _sambaButton.Name = "_sambaButton";
            _sambaButton.Size = new System.Drawing.Size(88, 27);
            _sambaButton.TabIndex = 15;
            _sambaButton.Text = "Samba...";
            _sambaButton.UseVisualStyleBackColor = true;
            _sambaButton.Click += OnSambaButtonClick;
            // 
            // _scriptsLabel
            // 
            _scriptsLabel.AutoSize = true;
            tableLayoutPanel2.SetColumnSpan(_scriptsLabel, 2);
            _scriptsLabel.Dock = System.Windows.Forms.DockStyle.Right;
            _scriptsLabel.Location = new System.Drawing.Point(51, 78);
            _scriptsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _scriptsLabel.MaximumSize = new System.Drawing.Size(600, 0);
            _scriptsLabel.Name = "_scriptsLabel";
            _scriptsLabel.Size = new System.Drawing.Size(284, 33);
            _scriptsLabel.TabIndex = 16;
            _scriptsLabel.Text = "If folder becomes accessible after mounting, specify PowerShell scripts for  mounting and unmounting";
            _scriptsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _limitUploadLabelV2
            // 
            _limitUploadLabelV2.AutoSize = true;
            _limitUploadLabelV2.Dock = System.Windows.Forms.DockStyle.Fill;
            _limitUploadLabelV2.Location = new System.Drawing.Point(23, 49);
            _limitUploadLabelV2.Name = "_limitUploadLabelV2";
            _limitUploadLabelV2.Size = new System.Drawing.Size(192, 29);
            _limitUploadLabelV2.TabIndex = 17;
            _limitUploadLabelV2.TabStop = true;
            _limitUploadLabelV2.Text = "Upload limit, GB:";
            _limitUploadLabelV2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            _limitUploadLabelV2.LinkClicked += OnUploadLimitClick;
            // 
            // _sambaTabPage
            // 
            _sambaTabPage.Controls.Add(tableLayoutPanel1);
            _sambaTabPage.ImageIndex = 1;
            _sambaTabPage.Location = new System.Drawing.Point(4, 58);
            _sambaTabPage.Name = "_sambaTabPage";
            _sambaTabPage.Padding = new System.Windows.Forms.Padding(3);
            _sambaTabPage.Size = new System.Drawing.Size(461, 186);
            _sambaTabPage.TabIndex = 1;
            _sambaTabPage.Text = "Samba";
            _sambaTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(_shareTextBox, 1, 0);
            tableLayoutPanel1.Controls.Add(_shareLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(_uploadLimitGbNumericUpDown, 1, 1);
            tableLayoutPanel1.Controls.Add(_passwordLabel, 0, 3);
            tableLayoutPanel1.Controls.Add(_userLabel, 0, 2);
            tableLayoutPanel1.Controls.Add(_passwordTextBox, 1, 3);
            tableLayoutPanel1.Controls.Add(_userTextBox, 1, 2);
            tableLayoutPanel1.Controls.Add(_limitUploadLabel, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20);
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(455, 180);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // _shareTextBox
            // 
            _shareTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _shareTextBox.Location = new System.Drawing.Point(126, 23);
            _shareTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _shareTextBox.Name = "_shareTextBox";
            _shareTextBox.PlaceholderText = "\\\\192.168.11.1\\share\\folder";
            _shareTextBox.Size = new System.Drawing.Size(450, 23);
            _shareTextBox.TabIndex = 2;
            _shareTextBox.TabStop = false;
            // 
            // _shareLabel
            // 
            _shareLabel.AutoSize = true;
            _shareLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _shareLabel.Location = new System.Drawing.Point(24, 20);
            _shareLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _shareLabel.Name = "_shareLabel";
            _shareLabel.Size = new System.Drawing.Size(94, 29);
            _shareLabel.TabIndex = 4;
            _shareLabel.Text = "Share:";
            _shareLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _uploadLimitGbNumericUpDown
            // 
            _uploadLimitGbNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            _uploadLimitGbNumericUpDown.Location = new System.Drawing.Point(125, 52);
            _uploadLimitGbNumericUpDown.Name = "_uploadLimitGbNumericUpDown";
            _uploadLimitGbNumericUpDown.Size = new System.Drawing.Size(452, 23);
            _uploadLimitGbNumericUpDown.TabIndex = 7;
            // 
            // _passwordLabel
            // 
            _passwordLabel.AutoSize = true;
            _passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _passwordLabel.Location = new System.Drawing.Point(24, 107);
            _passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _passwordLabel.Name = "_passwordLabel";
            _passwordLabel.Size = new System.Drawing.Size(94, 29);
            _passwordLabel.TabIndex = 10;
            _passwordLabel.Text = "Password:";
            _passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _userLabel
            // 
            _userLabel.AutoSize = true;
            _userLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _userLabel.Location = new System.Drawing.Point(24, 78);
            _userLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _userLabel.Name = "_userLabel";
            _userLabel.Size = new System.Drawing.Size(94, 29);
            _userLabel.TabIndex = 9;
            _userLabel.Text = "User:";
            _userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _passwordTextBox
            // 
            _passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _passwordTextBox.Location = new System.Drawing.Point(125, 110);
            _passwordTextBox.Name = "_passwordTextBox";
            _passwordTextBox.Size = new System.Drawing.Size(452, 23);
            _passwordTextBox.TabIndex = 12;
            // 
            // _userTextBox
            // 
            _userTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _userTextBox.Location = new System.Drawing.Point(125, 81);
            _userTextBox.Name = "_userTextBox";
            _userTextBox.Size = new System.Drawing.Size(452, 23);
            _userTextBox.TabIndex = 11;
            // 
            // _limitUploadLabel
            // 
            _limitUploadLabel.AutoSize = true;
            _limitUploadLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _limitUploadLabel.Location = new System.Drawing.Point(23, 49);
            _limitUploadLabel.Name = "_limitUploadLabel";
            _limitUploadLabel.Size = new System.Drawing.Size(96, 29);
            _limitUploadLabel.TabIndex = 17;
            _limitUploadLabel.TabStop = true;
            _limitUploadLabel.Text = "Upload limit, GB:";
            _limitUploadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            _limitUploadLabel.LinkClicked += OnUploadLimitClick;
            // 
            // WhereUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_storageTypesTabControl);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            MinimumSize = new System.Drawing.Size(332, 231);
            Name = "WhereUserControl";
            Size = new System.Drawing.Size(469, 248);
            _storageTypesTabControl.ResumeLayout(false);
            _hddStorageTabPage.ResumeLayout(false);
            _hddStorageTabPage.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_uploadLimitGbNumericUpDownV2).EndInit();
            _sambaTabPage.ResumeLayout(false);
            _sambaTabPage.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_uploadLimitGbNumericUpDown).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.ImageList storageTypesImageList;
        private System.Windows.Forms.TabControl _storageTypesTabControl;
        private System.Windows.Forms.TabPage _hddStorageTabPage;
        private System.Windows.Forms.TabPage _sambaTabPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox _shareTextBox;
        private System.Windows.Forms.Label _shareLabel;
        private System.Windows.Forms.NumericUpDown _uploadLimitGbNumericUpDown;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.Label _userLabel;
        private System.Windows.Forms.TextBox _passwordTextBox;
        private System.Windows.Forms.TextBox _userTextBox;
        private System.Windows.Forms.LinkLabel _limitUploadLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox destinationFolderTextBox;
        private System.Windows.Forms.Label whereToStoreBackupLabel;
        private System.Windows.Forms.NumericUpDown _uploadLimitGbNumericUpDownV2;
        private System.Windows.Forms.Label _unmountScriptLabel;
        private System.Windows.Forms.Label _mountScriptLabel;
        private System.Windows.Forms.TextBox _unmountTextBox;
        private System.Windows.Forms.TextBox _mountTextBox;
        private System.Windows.Forms.Button _mountButton;
        private System.Windows.Forms.Button _unmountButton;
        private System.Windows.Forms.Button _sambaButton;
        private System.Windows.Forms.Label _scriptsLabel;
        private System.Windows.Forms.LinkLabel _limitUploadLabelV2;
        private System.Windows.Forms.FolderBrowserDialog fbd;
    }
}
