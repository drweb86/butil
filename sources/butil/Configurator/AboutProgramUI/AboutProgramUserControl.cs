using System;
using BUtil.Core.Misc;
using BUtil.Core;
using System.Text;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Controls
{
    internal sealed partial class AboutProgramUserControl : BUtil.Core.PL.BackUserControl
    {
        public AboutProgramUserControl()
		{
			InitializeComponent();
		}

        #region Overrides

        public override void ApplyLocalization() 
		{
			visitWebSiteLabel.Text = BUtil.Core.Localization.Resources.VisitProjectHomepage;
            documentationLabel.Text = Resources.Documentation;
            var aboutInfo = new StringBuilder();
            aboutInfo.Append(CopyrightInfo.Copyright);
            aboutInfo.Append(Resources.UtilityForCreatingBackupsNNwebSitesHttpsGithubComDrweb86ButilNNlocalizationCreatedByN);
            aboutInfo.Append(Resources.TranslationAuthor);
            aboutInfo.Append("\n\n");
            aboutInfo.Replace("\r", string.Empty);
            aboutInfo.Replace("\n", "\r\n");
            aboutTextBox.Text = aboutInfo.ToString();
            aboutTextBox.Select(0, 0);
		}
	
		public override void SetOptionsToUi(object settings) { }
		
		public override void GetOptionsFromUi() { }
		
		#endregion
		
		void VisitWebSiteLabelClick(object sender, EventArgs e)
		{
			SupportManager.DoSupport(SupportRequest.Homepage);
		}
		
		void DocumentationLabelClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.Documentation);
        }
		
		void SevenZipPictureBoxClick(object sender, EventArgs e)
		{
			SupportManager.DoSupport(SupportRequest.SevenZip);
		}

		void VirtuawinPictureBoxClick(object sender, EventArgs e)
		{
			SupportManager.DoSupport(SupportRequest.VirtuaWin);
		}
		
		void LogoPictureBoxClick(object sender, EventArgs e)
		{
			SupportManager.DoSupport(SupportRequest.Homepage);
		}
	}
}
