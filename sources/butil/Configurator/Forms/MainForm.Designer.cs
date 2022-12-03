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
            this.restorationToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._beforeAboutToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this._aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.choosePanelUserControl = new BUtil.Configurator.Configurator.Controls.MainNavigationUserControl();
            this.helpToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.optionsHeader = new BUtil.Configurator.Controls.OptionsHeader();
            this.nestingControlsPanel = new System.Windows.Forms.Panel();
            this.helpStatusStrip = new System.Windows.Forms.StatusStrip();
            this.MainmenuStrip.SuspendLayout();
            this.helpStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainmenuStrip
            // 
            this.MainmenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restorationToolToolStripMenuItem,
            this._helpToolStripMenuItem});
            this.MainmenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainmenuStrip.Name = "MainmenuStrip";
            this.MainmenuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.MainmenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainmenuStrip.Size = new System.Drawing.Size(835, 24);
            this.MainmenuStrip.TabIndex = 3;
            this.MainmenuStrip.Text = "MainmenuStrip";
            // 
            // restorationToolToolStripMenuItem
            // 
            this.restorationToolToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Refresh48x48;
            this.restorationToolToolStripMenuItem.Name = "restorationToolToolStripMenuItem";
            this.restorationToolToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.restorationToolToolStripMenuItem.Text = "Restore Data...";
            this.restorationToolToolStripMenuItem.Click += new System.EventHandler(this.RestorationToolToolStripMenuItemClick);
            // 
            // _helpToolStripMenuItem
            // 
            this._helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this._beforeAboutToolStripSeparator,
            this._aboutToolStripMenuItem});
            this._helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            this._helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this._helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Lamp48x48;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.helpToolStripMenuItem.Text = "Manual";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.OnHelpToolStripMenuItemClick);
            // 
            // _beforeAboutToolStripSeparator
            // 
            this._beforeAboutToolStripSeparator.Name = "_beforeAboutToolStripSeparator";
            this._beforeAboutToolStripSeparator.Size = new System.Drawing.Size(130, 6);
            // 
            // _aboutToolStripMenuItem
            // 
            this._aboutToolStripMenuItem.Image = global::BUtil.Configurator.Icons.ProgramInfo48x48;
            this._aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            this._aboutToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
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
            this.choosePanelUserControl.Location = new System.Drawing.Point(1, 27);
            this.choosePanelUserControl.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.choosePanelUserControl.MinimumSize = new System.Drawing.Size(163, 248);
            this.choosePanelUserControl.Name = "choosePanelUserControl";
            this.choosePanelUserControl.Size = new System.Drawing.Size(163, 548);
            this.choosePanelUserControl.TabIndex = 0;
            this.choosePanelUserControl.ViewChanged += new BUtil.Configurator.Configurator.Controls.MainNavigationUserControl.ChangeViewEventHandler(this.ChoosePanelUserControlViewChanged);
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
            this.optionsHeader.Location = new System.Drawing.Point(168, 27);
            this.optionsHeader.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.optionsHeader.MinimumSize = new System.Drawing.Size(190, 33);
            this.optionsHeader.Name = "optionsHeader";
            this.optionsHeader.Size = new System.Drawing.Size(665, 33);
            this.optionsHeader.TabIndex = 5;
            this.optionsHeader.Title = "Title";
            // 
            // nestingControlsPanel
            // 
            this.nestingControlsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nestingControlsPanel.Location = new System.Drawing.Point(168, 61);
            this.nestingControlsPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nestingControlsPanel.Name = "nestingControlsPanel";
            this.nestingControlsPanel.Size = new System.Drawing.Size(665, 514);
            this.nestingControlsPanel.TabIndex = 1;
            // 
            // helpStatusStrip
            // 
            this.helpStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripStatusLabel});
            this.helpStatusStrip.Location = new System.Drawing.Point(0, 581);
            this.helpStatusStrip.Name = "helpStatusStrip";
            this.helpStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.helpStatusStrip.Size = new System.Drawing.Size(835, 22);
            this.helpStatusStrip.TabIndex = 6;
            this.helpStatusStrip.Text = "statusStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(835, 603);
            this.Controls.Add(this.helpStatusStrip);
            this.Controls.Add(this.nestingControlsPanel);
            this.Controls.Add(this.optionsHeader);
            this.Controls.Add(this.choosePanelUserControl);
            this.Controls.Add(this.MainmenuStrip);
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.MainMenuStrip = this.MainmenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(788, 641);
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
		private System.Windows.Forms.ToolStripMenuItem _helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel helpToolStripStatusLabel;
		private System.Windows.Forms.StatusStrip helpStatusStrip;
		private System.Windows.Forms.Panel nestingControlsPanel;
		private BUtil.Configurator.Controls.OptionsHeader optionsHeader;
        private MainNavigationUserControl choosePanelUserControl;
		private System.Windows.Forms.MenuStrip MainmenuStrip;
        private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ToolStripMenuItem restorationToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator _beforeAboutToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem _aboutToolStripMenuItem;
		
	}
}
