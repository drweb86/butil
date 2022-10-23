using BUtil.Configurator.Configurator.Controls;

namespace BUtil.Configurator.Configurator.Forms
{
	partial class MainForm : System.Windows.Forms.Form
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
		
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.MainmenuStrip = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restorationToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.journalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miscToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.haveNoNetworkAndInternetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dontNeedSchedulerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dontCareAboutScheulerStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAboutTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dontCareAboutPasswordLengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._questionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._beforeAboutToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this._aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.choosePanelUserControl = new LeftPanelUserControl();
            this.helpToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.optionsHeader = new BUtil.Configurator.Controls.OptionsHeader();
            this.nestingControlsPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.helpStatusStrip = new System.Windows.Forms.StatusStrip();
            this.MainmenuStrip.SuspendLayout();
            this.helpStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainmenuStrip
            // 
            this.MainmenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.miscToolStripMenuItem,
            this._questionToolStripMenuItem});
            this.MainmenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainmenuStrip.Name = "MainmenuStrip";
            this.MainmenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainmenuStrip.Size = new System.Drawing.Size(716, 24);
            this.MainmenuStrip.TabIndex = 3;
            this.MainmenuStrip.Text = "MainmenuStrip";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restorationToolToolStripMenuItem,
            this.journalsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tasks";
            // 
            // restorationToolToolStripMenuItem
            // 
            this.restorationToolToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Refresh48x48;
            this.restorationToolToolStripMenuItem.Name = "restorationToolToolStripMenuItem";
            this.restorationToolToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.restorationToolToolStripMenuItem.Text = "Restore Data...";
            this.restorationToolToolStripMenuItem.Click += new System.EventHandler(this.RestorationToolToolStripMenuItemClick);
            // 
            // journalsToolStripMenuItem
            // 
            this.journalsToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Journals;
            this.journalsToolStripMenuItem.Name = "journalsToolStripMenuItem";
            this.journalsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.journalsToolStripMenuItem.Text = "Journals...";
            this.journalsToolStripMenuItem.Click += new System.EventHandler(this.JournalsToolStripMenuItemClick);
            // 
            // miscToolStripMenuItem
            // 
            this.miscToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.haveNoNetworkAndInternetToolStripMenuItem,
            this.dontNeedSchedulerToolStripMenuItem,
            this.dontCareAboutScheulerStartupToolStripMenuItem,
            this.hideAboutTabToolStripMenuItem,
            this.dontCareAboutPasswordLengthToolStripMenuItem});
            this.miscToolStripMenuItem.Name = "miscToolStripMenuItem";
            this.miscToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.miscToolStripMenuItem.Text = "Misc";
            // 
            // haveNoNetworkAndInternetToolStripMenuItem
            // 
            this.haveNoNetworkAndInternetToolStripMenuItem.CheckOnClick = true;
            this.haveNoNetworkAndInternetToolStripMenuItem.Name = "haveNoNetworkAndInternetToolStripMenuItem";
            this.haveNoNetworkAndInternetToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.haveNoNetworkAndInternetToolStripMenuItem.Text = "have no network and Internet";
            this.haveNoNetworkAndInternetToolStripMenuItem.Click += new System.EventHandler(this.HaveNoNetworkAndInternetToolStripMenuItemClick);
            // 
            // dontNeedSchedulerToolStripMenuItem
            // 
            this.dontNeedSchedulerToolStripMenuItem.CheckOnClick = true;
            this.dontNeedSchedulerToolStripMenuItem.Name = "dontNeedSchedulerToolStripMenuItem";
            this.dontNeedSchedulerToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.dontNeedSchedulerToolStripMenuItem.Text = "don\'t need scheduler";
            this.dontNeedSchedulerToolStripMenuItem.Click += new System.EventHandler(this.DontNeedSchedulerToolStripMenuItemClick);
            // 
            // dontCareAboutScheulerStartupToolStripMenuItem
            // 
            this.dontCareAboutScheulerStartupToolStripMenuItem.CheckOnClick = true;
            this.dontCareAboutScheulerStartupToolStripMenuItem.Name = "dontCareAboutScheulerStartupToolStripMenuItem";
            this.dontCareAboutScheulerStartupToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.dontCareAboutScheulerStartupToolStripMenuItem.Text = "don\'t care about scheduler startup";
            this.dontCareAboutScheulerStartupToolStripMenuItem.Click += new System.EventHandler(this.DontCareAboutScheulerStartupToolStripMenuItemClick);
            // 
            // hideAboutTabToolStripMenuItem
            // 
            this.hideAboutTabToolStripMenuItem.CheckOnClick = true;
            this.hideAboutTabToolStripMenuItem.Name = "hideAboutTabToolStripMenuItem";
            this.hideAboutTabToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.hideAboutTabToolStripMenuItem.Text = "hide \'About\' tab";
            this.hideAboutTabToolStripMenuItem.Click += new System.EventHandler(this.HideAboutTabToolStripMenuItemClick);
            // 
            // dontCareAboutPasswordLengthToolStripMenuItem
            // 
            this.dontCareAboutPasswordLengthToolStripMenuItem.CheckOnClick = true;
            this.dontCareAboutPasswordLengthToolStripMenuItem.Name = "dontCareAboutPasswordLengthToolStripMenuItem";
            this.dontCareAboutPasswordLengthToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.dontCareAboutPasswordLengthToolStripMenuItem.Text = "don\'t care about password length";
            this.dontCareAboutPasswordLengthToolStripMenuItem.Click += new System.EventHandler(this.DontCareAboutPasswordLengthToolStripMenuItemClick);
            // 
            // _questionToolStripMenuItem
            // 
            this._questionToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._questionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this._beforeAboutToolStripSeparator,
            this._aboutToolStripMenuItem});
            this._questionToolStripMenuItem.Name = "_questionToolStripMenuItem";
            this._questionToolStripMenuItem.Size = new System.Drawing.Size(24, 20);
            this._questionToolStripMenuItem.Text = "?";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Lamp48x48;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.helpToolStripMenuItem.Text = "Manual";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.OnHelpToolStripMenuItemClick);
            // 
            // _beforeAboutToolStripSeparator
            // 
            this._beforeAboutToolStripSeparator.Name = "_beforeAboutToolStripSeparator";
            this._beforeAboutToolStripSeparator.Size = new System.Drawing.Size(149, 6);
            // 
            // _aboutToolStripMenuItem
            // 
            this._aboutToolStripMenuItem.Image = global::BUtil.Configurator.Icons.ProgramInfo48x48;
            this._aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            this._aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this._aboutToolStripMenuItem.Text = "About...";
            this._aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutToolStripMenuItemClick);
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // choosePanelUserControl
            // 
            this.choosePanelUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.choosePanelUserControl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.choosePanelUserControl.BackgroundImage = global::BUtil.Configurator.Icons.PanelUser;
            this.choosePanelUserControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.choosePanelUserControl.DrawAtractiveBorders = false;
            this.choosePanelUserControl.HelpLabel = this.helpToolStripStatusLabel;
            this.choosePanelUserControl.Location = new System.Drawing.Point(1, 23);
            this.choosePanelUserControl.MinimumSize = new System.Drawing.Size(140, 215);
            this.choosePanelUserControl.Name = "choosePanelUserControl";
            this.choosePanelUserControl.Size = new System.Drawing.Size(140, 475);
            this.choosePanelUserControl.TabIndex = 0;
            this.choosePanelUserControl.ViewChanged += new LeftPanelUserControl.ChangeViewEventHandler(this.ChoosePanelUserControlViewChanged);
            // 
            // helpToolStripStatusLabel
            // 
            this.helpToolStripStatusLabel.Name = "helpToolStripStatusLabel";
            this.helpToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // optionsHeader
            // 
            this.optionsHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsHeader.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.optionsHeader.ForeColor = System.Drawing.Color.White;
            this.optionsHeader.Location = new System.Drawing.Point(144, 23);
            this.optionsHeader.MinimumSize = new System.Drawing.Size(163, 29);
            this.optionsHeader.Name = "optionsHeader";
            this.optionsHeader.Size = new System.Drawing.Size(570, 29);
            this.optionsHeader.TabIndex = 5;
            this.optionsHeader.Title = "Title";
            // 
            // nestingControlsPanel
            // 
            this.nestingControlsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nestingControlsPanel.Location = new System.Drawing.Point(144, 53);
            this.nestingControlsPanel.Name = "nestingControlsPanel";
            this.nestingControlsPanel.Size = new System.Drawing.Size(570, 407);
            this.nestingControlsPanel.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(629, 466);
            this.cancelButton.MinimumSize = new System.Drawing.Size(75, 23);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.okButton.Location = new System.Drawing.Point(548, 466);
            this.okButton.MinimumSize = new System.Drawing.Size(75, 23);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // helpStatusStrip
            // 
            this.helpStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripStatusLabel});
            this.helpStatusStrip.Location = new System.Drawing.Point(0, 501);
            this.helpStatusStrip.Name = "helpStatusStrip";
            this.helpStatusStrip.Size = new System.Drawing.Size(716, 22);
            this.helpStatusStrip.TabIndex = 6;
            this.helpStatusStrip.Text = "statusStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(716, 523);
            this.Controls.Add(this.helpStatusStrip);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.nestingControlsPanel);
            this.Controls.Add(this.optionsHeader);
            this.Controls.Add(this.choosePanelUserControl);
            this.Controls.Add(this.MainmenuStrip);
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.MainMenuStrip = this.MainmenuStrip;
            this.MinimumSize = new System.Drawing.Size(678, 561);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.MainmenuStrip.ResumeLayout(false);
            this.MainmenuStrip.PerformLayout();
            this.helpStatusStrip.ResumeLayout(false);
            this.helpStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripMenuItem journalsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dontCareAboutPasswordLengthToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hideAboutTabToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dontCareAboutScheulerStartupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dontNeedSchedulerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem haveNoNetworkAndInternetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem miscToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel helpToolStripStatusLabel;
		private System.Windows.Forms.StatusStrip helpStatusStrip;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Panel nestingControlsPanel;
		private BUtil.Configurator.Controls.OptionsHeader optionsHeader;
        private LeftPanelUserControl choosePanelUserControl;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.MenuStrip MainmenuStrip;
        private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ToolStripMenuItem restorationToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _questionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator _beforeAboutToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem _aboutToolStripMenuItem;
		
	}
}
