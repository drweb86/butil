
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
            _specifyFolderDirectoryStorageButton = new System.Windows.Forms.Button();
            destinationFolderTextBox = new System.Windows.Forms.TextBox();
            whereToStoreBackupLabel = new System.Windows.Forms.Label();
            _uploadLimitGbNumericUpDownV2 = new System.Windows.Forms.NumericUpDown();
            _unmountScriptLabel = new System.Windows.Forms.Label();
            _mountScriptLabel = new System.Windows.Forms.Label();
            _unmountTextBox = new System.Windows.Forms.TextBox();
            _mountTextBox = new System.Windows.Forms.TextBox();
            _mountButton = new System.Windows.Forms.Button();
            _unmountButton = new System.Windows.Forms.Button();
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
            _ftpsTabPage = new System.Windows.Forms.TabPage();
            _ftpsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _ftpsServerLabel = new System.Windows.Forms.Label();
            _ftpsServerTextBox = new System.Windows.Forms.TextBox();
            _ftpsQuotaLabel = new System.Windows.Forms.LinkLabel();
            _ftpsFolderLabel = new System.Windows.Forms.Label();
            _ftpsPasswordLabel = new System.Windows.Forms.Label();
            _ftpsUserLabel = new System.Windows.Forms.Label();
            _ftpsPortLabel = new System.Windows.Forms.Label();
            _ftpsEncryptionLabel = new System.Windows.Forms.Label();
            _ftpsQuotaNumericUpDown = new System.Windows.Forms.NumericUpDown();
            _ftpsFolderTextBox = new System.Windows.Forms.TextBox();
            _ftpsPasswordTextBox = new System.Windows.Forms.TextBox();
            _ftpsUserTextBox = new System.Windows.Forms.TextBox();
            _ftpsPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
            _ftpsEncryptionComboBox = new System.Windows.Forms.ComboBox();
            _mtpTabPage = new System.Windows.Forms.TabPage();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            _mtpFolderLabel = new System.Windows.Forms.Label();
            _mtpDeviceLabel = new System.Windows.Forms.Label();
            _mtpFolderTextBox = new System.Windows.Forms.TextBox();
            _mtpDeviceComboBox = new System.Windows.Forms.ComboBox();
            fbd = new System.Windows.Forms.FolderBrowserDialog();
            _storageTypesTabControl.SuspendLayout();
            _hddStorageTabPage.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_uploadLimitGbNumericUpDownV2).BeginInit();
            _sambaTabPage.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_uploadLimitGbNumericUpDown).BeginInit();
            _ftpsTabPage.SuspendLayout();
            _ftpsTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_ftpsQuotaNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_ftpsPortNumericUpDown).BeginInit();
            _mtpTabPage.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // storageTypesImageList
            // 
            storageTypesImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            storageTypesImageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("storageTypesImageList.ImageStream");
            storageTypesImageList.TransparentColor = System.Drawing.Color.White;
            storageTypesImageList.Images.SetKeyName(0, "RedFolder48x48.png");
            storageTypesImageList.Images.SetKeyName(1, "Share48x48.png");
            storageTypesImageList.Images.SetKeyName(2, "FTPS.png");
            storageTypesImageList.Images.SetKeyName(3, "mp3player2.png");
            // 
            // _storageTypesTabControl
            // 
            _storageTypesTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            _storageTypesTabControl.Controls.Add(_hddStorageTabPage);
            _storageTypesTabControl.Controls.Add(_sambaTabPage);
            _storageTypesTabControl.Controls.Add(_ftpsTabPage);
            _storageTypesTabControl.Controls.Add(_mtpTabPage);
            _storageTypesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _storageTypesTabControl.ImageList = storageTypesImageList;
            _storageTypesTabControl.Location = new System.Drawing.Point(0, 0);
            _storageTypesTabControl.Multiline = true;
            _storageTypesTabControl.Name = "_storageTypesTabControl";
            _storageTypesTabControl.SelectedIndex = 0;
            _storageTypesTabControl.Size = new System.Drawing.Size(469, 248);
            _storageTypesTabControl.TabIndex = 4;
            _storageTypesTabControl.SelectedIndexChanged += OnTabIndexChanged;
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
            _hddStorageTabPage.Text = "Folder";
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
            tableLayoutPanel2.Controls.Add(_specifyFolderDirectoryStorageButton, 2, 0);
            tableLayoutPanel2.Controls.Add(destinationFolderTextBox, 1, 0);
            tableLayoutPanel2.Controls.Add(whereToStoreBackupLabel, 0, 0);
            tableLayoutPanel2.Controls.Add(_uploadLimitGbNumericUpDownV2, 1, 1);
            tableLayoutPanel2.Controls.Add(_unmountScriptLabel, 0, 4);
            tableLayoutPanel2.Controls.Add(_mountScriptLabel, 0, 3);
            tableLayoutPanel2.Controls.Add(_unmountTextBox, 1, 4);
            tableLayoutPanel2.Controls.Add(_mountTextBox, 1, 3);
            tableLayoutPanel2.Controls.Add(_mountButton, 2, 3);
            tableLayoutPanel2.Controls.Add(_unmountButton, 2, 4);
            tableLayoutPanel2.Controls.Add(_scriptsLabel, 0, 2);
            tableLayoutPanel2.Controls.Add(_limitUploadLabelV2, 0, 1);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(10);
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
            // _specifyFolderDirectoryStorageButton
            // 
            _specifyFolderDirectoryStorageButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            _specifyFolderDirectoryStorageButton.AutoSize = true;
            _specifyFolderDirectoryStorageButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _specifyFolderDirectoryStorageButton.Location = new System.Drawing.Point(389, 12);
            _specifyFolderDirectoryStorageButton.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            _specifyFolderDirectoryStorageButton.Name = "_specifyFolderDirectoryStorageButton";
            _specifyFolderDirectoryStorageButton.Size = new System.Drawing.Size(55, 25);
            _specifyFolderDirectoryStorageButton.TabIndex = 3;
            _specifyFolderDirectoryStorageButton.Text = "Browse";
            _specifyFolderDirectoryStorageButton.UseVisualStyleBackColor = true;
            _specifyFolderDirectoryStorageButton.Click += searchButtonClick;
            // 
            // destinationFolderTextBox
            // 
            destinationFolderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            destinationFolderTextBox.Location = new System.Drawing.Point(211, 13);
            destinationFolderTextBox.Name = "destinationFolderTextBox";
            destinationFolderTextBox.Size = new System.Drawing.Size(167, 23);
            destinationFolderTextBox.TabIndex = 2;
            // 
            // whereToStoreBackupLabel
            // 
            whereToStoreBackupLabel.AutoSize = true;
            whereToStoreBackupLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            whereToStoreBackupLabel.Location = new System.Drawing.Point(14, 10);
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
            _uploadLimitGbNumericUpDownV2.Location = new System.Drawing.Point(211, 42);
            _uploadLimitGbNumericUpDownV2.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            _uploadLimitGbNumericUpDownV2.Name = "_uploadLimitGbNumericUpDownV2";
            _uploadLimitGbNumericUpDownV2.Size = new System.Drawing.Size(167, 23);
            _uploadLimitGbNumericUpDownV2.TabIndex = 7;
            // 
            // _unmountScriptLabel
            // 
            _unmountScriptLabel.AutoSize = true;
            _unmountScriptLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _unmountScriptLabel.Location = new System.Drawing.Point(14, 148);
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
            _mountScriptLabel.Location = new System.Drawing.Point(14, 98);
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
            _unmountTextBox.Location = new System.Drawing.Point(211, 151);
            _unmountTextBox.Multiline = true;
            _unmountTextBox.Name = "_unmountTextBox";
            _unmountTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _unmountTextBox.Size = new System.Drawing.Size(167, 44);
            _unmountTextBox.TabIndex = 12;
            // 
            // _mountTextBox
            // 
            _mountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _mountTextBox.Location = new System.Drawing.Point(211, 101);
            _mountTextBox.Multiline = true;
            _mountTextBox.Name = "_mountTextBox";
            _mountTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _mountTextBox.Size = new System.Drawing.Size(167, 44);
            _mountTextBox.TabIndex = 11;
            // 
            // _mountButton
            // 
            _mountButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            _mountButton.AutoSize = true;
            _mountButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _mountButton.Location = new System.Drawing.Point(389, 110);
            _mountButton.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            _mountButton.Name = "_mountButton";
            _mountButton.Size = new System.Drawing.Size(56, 25);
            _mountButton.TabIndex = 13;
            _mountButton.Text = "Launch";
            _mountButton.UseVisualStyleBackColor = true;
            _mountButton.Click += OnMountScript;
            // 
            // _unmountButton
            // 
            _unmountButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            _unmountButton.AutoSize = true;
            _unmountButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _unmountButton.Location = new System.Drawing.Point(389, 160);
            _unmountButton.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            _unmountButton.Name = "_unmountButton";
            _unmountButton.Size = new System.Drawing.Size(56, 25);
            _unmountButton.TabIndex = 14;
            _unmountButton.Text = "Launch";
            _unmountButton.UseVisualStyleBackColor = true;
            _unmountButton.Click += OnUnmount;
            // 
            // _scriptsLabel
            // 
            _scriptsLabel.AutoSize = true;
            tableLayoutPanel2.SetColumnSpan(_scriptsLabel, 3);
            _scriptsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _scriptsLabel.Location = new System.Drawing.Point(14, 68);
            _scriptsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _scriptsLabel.MaximumSize = new System.Drawing.Size(600, 0);
            _scriptsLabel.Name = "_scriptsLabel";
            _scriptsLabel.Size = new System.Drawing.Size(427, 30);
            _scriptsLabel.TabIndex = 16;
            _scriptsLabel.Text = "If folder becomes accessible after mounting, specify PowerShell scripts for  mounting and unmounting";
            _scriptsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _limitUploadLabelV2
            // 
            _limitUploadLabelV2.AutoSize = true;
            _limitUploadLabelV2.Dock = System.Windows.Forms.DockStyle.Fill;
            _limitUploadLabelV2.Location = new System.Drawing.Point(13, 39);
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
            _sambaTabPage.Text = "SMB/CIFS";
            _sambaTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
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
            tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(455, 180);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // _shareTextBox
            // 
            _shareTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _shareTextBox.Location = new System.Drawing.Point(115, 13);
            _shareTextBox.Name = "_shareTextBox";
            _shareTextBox.PlaceholderText = "\\\\192.168.11.1\\share\\folder";
            _shareTextBox.Size = new System.Drawing.Size(327, 23);
            _shareTextBox.TabIndex = 2;
            _shareTextBox.TabStop = false;
            // 
            // _shareLabel
            // 
            _shareLabel.AutoSize = true;
            _shareLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _shareLabel.Location = new System.Drawing.Point(14, 10);
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
            _uploadLimitGbNumericUpDown.Location = new System.Drawing.Point(115, 42);
            _uploadLimitGbNumericUpDown.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            _uploadLimitGbNumericUpDown.Name = "_uploadLimitGbNumericUpDown";
            _uploadLimitGbNumericUpDown.Size = new System.Drawing.Size(327, 23);
            _uploadLimitGbNumericUpDown.TabIndex = 7;
            // 
            // _passwordLabel
            // 
            _passwordLabel.AutoSize = true;
            _passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _passwordLabel.Location = new System.Drawing.Point(14, 97);
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
            _userLabel.Location = new System.Drawing.Point(14, 68);
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
            _passwordTextBox.Location = new System.Drawing.Point(115, 100);
            _passwordTextBox.Name = "_passwordTextBox";
            _passwordTextBox.Size = new System.Drawing.Size(327, 23);
            _passwordTextBox.TabIndex = 12;
            // 
            // _userTextBox
            // 
            _userTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _userTextBox.Location = new System.Drawing.Point(115, 71);
            _userTextBox.Name = "_userTextBox";
            _userTextBox.Size = new System.Drawing.Size(327, 23);
            _userTextBox.TabIndex = 11;
            // 
            // _limitUploadLabel
            // 
            _limitUploadLabel.AutoSize = true;
            _limitUploadLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _limitUploadLabel.Location = new System.Drawing.Point(13, 39);
            _limitUploadLabel.Name = "_limitUploadLabel";
            _limitUploadLabel.Size = new System.Drawing.Size(96, 29);
            _limitUploadLabel.TabIndex = 17;
            _limitUploadLabel.TabStop = true;
            _limitUploadLabel.Text = "Upload limit, GB:";
            _limitUploadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            _limitUploadLabel.LinkClicked += OnUploadLimitClick;
            // 
            // _ftpsTabPage
            // 
            _ftpsTabPage.Controls.Add(_ftpsTableLayoutPanel);
            _ftpsTabPage.ImageIndex = 2;
            _ftpsTabPage.Location = new System.Drawing.Point(4, 58);
            _ftpsTabPage.Name = "_ftpsTabPage";
            _ftpsTabPage.Size = new System.Drawing.Size(461, 186);
            _ftpsTabPage.TabIndex = 2;
            _ftpsTabPage.Text = "FTPS";
            _ftpsTabPage.UseVisualStyleBackColor = true;
            // 
            // _ftpsTableLayoutPanel
            // 
            _ftpsTableLayoutPanel.ColumnCount = 2;
            _ftpsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _ftpsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _ftpsTableLayoutPanel.Controls.Add(_ftpsServerLabel, 0, 0);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsServerTextBox, 1, 0);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsQuotaLabel, 0, 6);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsFolderLabel, 0, 5);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsPasswordLabel, 0, 4);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsUserLabel, 0, 3);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsPortLabel, 0, 2);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsEncryptionLabel, 0, 1);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsQuotaNumericUpDown, 1, 6);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsFolderTextBox, 1, 5);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsPasswordTextBox, 1, 4);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsUserTextBox, 1, 3);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsPortNumericUpDown, 1, 2);
            _ftpsTableLayoutPanel.Controls.Add(_ftpsEncryptionComboBox, 1, 1);
            _ftpsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _ftpsTableLayoutPanel.Name = "_ftpsTableLayoutPanel";
            _ftpsTableLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            _ftpsTableLayoutPanel.RowCount = 8;
            _ftpsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _ftpsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _ftpsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _ftpsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _ftpsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _ftpsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _ftpsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _ftpsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _ftpsTableLayoutPanel.Size = new System.Drawing.Size(461, 186);
            _ftpsTableLayoutPanel.TabIndex = 0;
            // 
            // _ftpsServerLabel
            // 
            _ftpsServerLabel.AutoSize = true;
            _ftpsServerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsServerLabel.Location = new System.Drawing.Point(13, 10);
            _ftpsServerLabel.Name = "_ftpsServerLabel";
            _ftpsServerLabel.Size = new System.Drawing.Size(96, 29);
            _ftpsServerLabel.TabIndex = 0;
            _ftpsServerLabel.Text = "Server:";
            _ftpsServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _ftpsServerTextBox
            // 
            _ftpsServerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsServerTextBox.Location = new System.Drawing.Point(115, 13);
            _ftpsServerTextBox.Name = "_ftpsServerTextBox";
            _ftpsServerTextBox.Size = new System.Drawing.Size(333, 23);
            _ftpsServerTextBox.TabIndex = 5;
            // 
            // _ftpsQuotaLabel
            // 
            _ftpsQuotaLabel.AutoSize = true;
            _ftpsQuotaLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsQuotaLabel.Location = new System.Drawing.Point(13, 182);
            _ftpsQuotaLabel.Name = "_ftpsQuotaLabel";
            _ftpsQuotaLabel.Size = new System.Drawing.Size(96, 29);
            _ftpsQuotaLabel.TabIndex = 18;
            _ftpsQuotaLabel.TabStop = true;
            _ftpsQuotaLabel.Text = "Upload limit, GB:";
            _ftpsQuotaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            _ftpsQuotaLabel.LinkClicked += OnUploadLimitClick;
            // 
            // _ftpsFolderLabel
            // 
            _ftpsFolderLabel.AutoSize = true;
            _ftpsFolderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsFolderLabel.Location = new System.Drawing.Point(13, 153);
            _ftpsFolderLabel.Name = "_ftpsFolderLabel";
            _ftpsFolderLabel.Size = new System.Drawing.Size(96, 29);
            _ftpsFolderLabel.TabIndex = 1;
            _ftpsFolderLabel.Text = "Folder:";
            _ftpsFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _ftpsPasswordLabel
            // 
            _ftpsPasswordLabel.AutoSize = true;
            _ftpsPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsPasswordLabel.Location = new System.Drawing.Point(13, 124);
            _ftpsPasswordLabel.Name = "_ftpsPasswordLabel";
            _ftpsPasswordLabel.Size = new System.Drawing.Size(96, 29);
            _ftpsPasswordLabel.TabIndex = 3;
            _ftpsPasswordLabel.Text = "Password:";
            _ftpsPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _ftpsUserLabel
            // 
            _ftpsUserLabel.AutoSize = true;
            _ftpsUserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsUserLabel.Location = new System.Drawing.Point(13, 95);
            _ftpsUserLabel.Name = "_ftpsUserLabel";
            _ftpsUserLabel.Size = new System.Drawing.Size(96, 29);
            _ftpsUserLabel.TabIndex = 4;
            _ftpsUserLabel.Text = "User:";
            _ftpsUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _ftpsPortLabel
            // 
            _ftpsPortLabel.AutoSize = true;
            _ftpsPortLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsPortLabel.Location = new System.Drawing.Point(13, 66);
            _ftpsPortLabel.Name = "_ftpsPortLabel";
            _ftpsPortLabel.Size = new System.Drawing.Size(96, 29);
            _ftpsPortLabel.TabIndex = 2;
            _ftpsPortLabel.Text = "Port:";
            _ftpsPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _ftpsEncryptionLabel
            // 
            _ftpsEncryptionLabel.AutoSize = true;
            _ftpsEncryptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsEncryptionLabel.Location = new System.Drawing.Point(13, 39);
            _ftpsEncryptionLabel.Name = "_ftpsEncryptionLabel";
            _ftpsEncryptionLabel.Size = new System.Drawing.Size(96, 27);
            _ftpsEncryptionLabel.TabIndex = 20;
            _ftpsEncryptionLabel.Text = "Encryption:";
            _ftpsEncryptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _ftpsQuotaNumericUpDown
            // 
            _ftpsQuotaNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsQuotaNumericUpDown.Location = new System.Drawing.Point(115, 185);
            _ftpsQuotaNumericUpDown.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            _ftpsQuotaNumericUpDown.Name = "_ftpsQuotaNumericUpDown";
            _ftpsQuotaNumericUpDown.Size = new System.Drawing.Size(333, 23);
            _ftpsQuotaNumericUpDown.TabIndex = 19;
            // 
            // _ftpsFolderTextBox
            // 
            _ftpsFolderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsFolderTextBox.Location = new System.Drawing.Point(115, 156);
            _ftpsFolderTextBox.Name = "_ftpsFolderTextBox";
            _ftpsFolderTextBox.Size = new System.Drawing.Size(333, 23);
            _ftpsFolderTextBox.TabIndex = 9;
            // 
            // _ftpsPasswordTextBox
            // 
            _ftpsPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsPasswordTextBox.Location = new System.Drawing.Point(115, 127);
            _ftpsPasswordTextBox.Name = "_ftpsPasswordTextBox";
            _ftpsPasswordTextBox.Size = new System.Drawing.Size(333, 23);
            _ftpsPasswordTextBox.TabIndex = 8;
            // 
            // _ftpsUserTextBox
            // 
            _ftpsUserTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsUserTextBox.Location = new System.Drawing.Point(115, 98);
            _ftpsUserTextBox.Name = "_ftpsUserTextBox";
            _ftpsUserTextBox.Size = new System.Drawing.Size(333, 23);
            _ftpsUserTextBox.TabIndex = 7;
            // 
            // _ftpsPortNumericUpDown
            // 
            _ftpsPortNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsPortNumericUpDown.Location = new System.Drawing.Point(115, 69);
            _ftpsPortNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            _ftpsPortNumericUpDown.Name = "_ftpsPortNumericUpDown";
            _ftpsPortNumericUpDown.Size = new System.Drawing.Size(333, 23);
            _ftpsPortNumericUpDown.TabIndex = 6;
            // 
            // _ftpsEncryptionComboBox
            // 
            _ftpsEncryptionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _ftpsEncryptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _ftpsEncryptionComboBox.FormattingEnabled = true;
            _ftpsEncryptionComboBox.Location = new System.Drawing.Point(115, 41);
            _ftpsEncryptionComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            _ftpsEncryptionComboBox.Name = "_ftpsEncryptionComboBox";
            _ftpsEncryptionComboBox.Size = new System.Drawing.Size(333, 23);
            _ftpsEncryptionComboBox.TabIndex = 21;
            // 
            // _mtpTabPage
            // 
            _mtpTabPage.Controls.Add(tableLayoutPanel3);
            _mtpTabPage.ImageIndex = 3;
            _mtpTabPage.Location = new System.Drawing.Point(4, 58);
            _mtpTabPage.Name = "_mtpTabPage";
            _mtpTabPage.Size = new System.Drawing.Size(461, 186);
            _mtpTabPage.TabIndex = 3;
            _mtpTabPage.Text = "MTP";
            _mtpTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(_mtpFolderLabel, 0, 1);
            tableLayoutPanel3.Controls.Add(_mtpDeviceLabel, 0, 0);
            tableLayoutPanel3.Controls.Add(_mtpFolderTextBox, 1, 1);
            tableLayoutPanel3.Controls.Add(_mtpDeviceComboBox, 1, 0);
            tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(10);
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new System.Drawing.Size(461, 186);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // _mtpFolderLabel
            // 
            _mtpFolderLabel.AutoSize = true;
            _mtpFolderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _mtpFolderLabel.Location = new System.Drawing.Point(13, 37);
            _mtpFolderLabel.Name = "_mtpFolderLabel";
            _mtpFolderLabel.Size = new System.Drawing.Size(45, 29);
            _mtpFolderLabel.TabIndex = 1;
            _mtpFolderLabel.Text = "Folder:";
            _mtpFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _mtpDeviceLabel
            // 
            _mtpDeviceLabel.AutoSize = true;
            _mtpDeviceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _mtpDeviceLabel.Location = new System.Drawing.Point(13, 10);
            _mtpDeviceLabel.Name = "_mtpDeviceLabel";
            _mtpDeviceLabel.Size = new System.Drawing.Size(45, 27);
            _mtpDeviceLabel.TabIndex = 20;
            _mtpDeviceLabel.Text = "Device:";
            _mtpDeviceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _mtpFolderTextBox
            // 
            _mtpFolderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _mtpFolderTextBox.Location = new System.Drawing.Point(64, 40);
            _mtpFolderTextBox.Name = "_mtpFolderTextBox";
            _mtpFolderTextBox.Size = new System.Drawing.Size(384, 23);
            _mtpFolderTextBox.TabIndex = 9;
            // 
            // _mtpDeviceComboBox
            // 
            _mtpDeviceComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _mtpDeviceComboBox.FormattingEnabled = true;
            _mtpDeviceComboBox.Location = new System.Drawing.Point(64, 12);
            _mtpDeviceComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            _mtpDeviceComboBox.Name = "_mtpDeviceComboBox";
            _mtpDeviceComboBox.Size = new System.Drawing.Size(384, 23);
            _mtpDeviceComboBox.TabIndex = 21;
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
            _ftpsTabPage.ResumeLayout(false);
            _ftpsTableLayoutPanel.ResumeLayout(false);
            _ftpsTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_ftpsQuotaNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)_ftpsPortNumericUpDown).EndInit();
            _mtpTabPage.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
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
        private System.Windows.Forms.Button _specifyFolderDirectoryStorageButton;
        private System.Windows.Forms.TextBox destinationFolderTextBox;
        private System.Windows.Forms.Label whereToStoreBackupLabel;
        private System.Windows.Forms.NumericUpDown _uploadLimitGbNumericUpDownV2;
        private System.Windows.Forms.Label _unmountScriptLabel;
        private System.Windows.Forms.Label _mountScriptLabel;
        private System.Windows.Forms.TextBox _unmountTextBox;
        private System.Windows.Forms.TextBox _mountTextBox;
        private System.Windows.Forms.Button _mountButton;
        private System.Windows.Forms.Button _unmountButton;
        private System.Windows.Forms.Label _scriptsLabel;
        private System.Windows.Forms.LinkLabel _limitUploadLabelV2;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.TabPage _ftpsTabPage;
        private System.Windows.Forms.TableLayoutPanel _ftpsTableLayoutPanel;
        private System.Windows.Forms.Label _ftpsUserLabel;
        private System.Windows.Forms.Label _ftpsPortLabel;
        private System.Windows.Forms.Label _ftpsServerLabel;
        private System.Windows.Forms.Label _ftpsPasswordLabel;
        private System.Windows.Forms.Label _ftpsFolderLabel;
        private System.Windows.Forms.TextBox _ftpsServerTextBox;
        private System.Windows.Forms.NumericUpDown _ftpsPortNumericUpDown;
        private System.Windows.Forms.TextBox _ftpsUserTextBox;
        private System.Windows.Forms.TextBox _ftpsPasswordTextBox;
        private System.Windows.Forms.TextBox _ftpsFolderTextBox;
        private System.Windows.Forms.NumericUpDown _ftpsQuotaNumericUpDown;
        private System.Windows.Forms.LinkLabel _ftpsQuotaLabel;
        private System.Windows.Forms.Label _ftpsEncryptionLabel;
        private System.Windows.Forms.ComboBox _ftpsEncryptionComboBox;
        private System.Windows.Forms.TabPage _mtpTabPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label _mtpFolderLabel;
        private System.Windows.Forms.Label _mtpDeviceLabel;
        private System.Windows.Forms.TextBox _mtpFolderTextBox;
        private System.Windows.Forms.ComboBox _mtpDeviceComboBox;
    }
}
