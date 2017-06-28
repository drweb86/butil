
namespace BUtil.Configurator.Controls
{
	partial class OptionsHeader
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
			this.titleLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// titleLabel
			// 
			this.titleLabel.BackColor = System.Drawing.Color.Transparent;
			this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.titleLabel.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.titleLabel.Location = new System.Drawing.Point(0, 0);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(399, 29);
			this.titleLabel.TabIndex = 0;
			this.titleLabel.Text = "Title";
			this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptionsHeader
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.Controls.Add(this.titleLabel);
			this.ForeColor = System.Drawing.Color.White;
			this.MinimumSize = new System.Drawing.Size(163, 29);
			this.Name = "OptionsHeader";
			this.Size = new System.Drawing.Size(399, 29);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label titleLabel;
	}
}
