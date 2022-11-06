namespace BUtil.RestorationMaster
{
	partial class VersionsViewerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionsViewerForm));
            this.imagesList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imagesList
            // 
            this.imagesList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imagesList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesList.ImageStream")));
            this.imagesList.TransparentColor = System.Drawing.Color.White;
            this.imagesList.Images.SetKeyName(0, "16x16 Binary.png");
            this.imagesList.Images.SetKeyName(1, "16x16 Folder.png");
            // 
            // VersionsViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 377);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(437, 301);
            this.Name = "VersionsViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Restoration";
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.ImageList imagesList;
	}
}
