using System;

using BUtil.Core.Misc;
using BUtil.Core.FileSystem;
using BUtil.Core;
using System.Text;
using BUtil.Core.Options;
using BUtil.Core.PL;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// Control with update and links to developer page
	/// </summary>
	internal sealed partial class AboutProgramUserControl : BUtil.Core.PL.BackUserControl
    {
        #region Fields

        ProgramOptions _options;

        #endregion

        #region Constructors

        public AboutProgramUserControl()
		{
			InitializeComponent();
			
			documentationLabel.Enabled = !Program.PackageIsBroken;
		}

        #endregion

        #region Overrides

        public override void ApplyLocalization() 
		{
			visitWebSiteLabel.Text = BUtil.Core.Localization.Resources.VisitProjectHomepage;
            suggestAFeatureLabel.Text = BUtil.Core.Localization.Resources.SuggestAFeature;
            reportABugLabel.Text = BUtil.Core.Localization.Resources.ReportABug;
            supportLabel.Text = BUtil.Core.Localization.Resources.AskForSupport;
            documentationLabel.Text = Resources.Documentation;
            checkForUpdatesLabel.Text = Resources.CheckForUpdates;
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
	
		public override void SetOptionsToUi(object settings)
		{
			_options = (ProgramOptions)settings;
		}
		
		public override void GetOptionsFromUi()
		{
			;
		}
		
		#endregion
		
		void VisitWebSiteLabelClick(object sender, EventArgs e)
		{
			SupportManager.DoSupport(SupportRequest.Homepage);
		}
		
		void SuggestAFeatureLabelClick(object sender, EventArgs e)
		{
			SupportManager.DoSupport(SupportRequest.Issue);
		}
		
		void ReportABugLabelClick(object sender, EventArgs e)
		{
			SupportManager.DoSupport(SupportRequest.Issue);
		}
		
		void SupportLabelClick(object sender, EventArgs e)
		{
			SupportManager.DoSupport(SupportRequest.Issue);
		}
		
		void DocumentationLabelClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.Documentation);
        }

		void CheckForUpdatesLabelClick(object sender, EventArgs e)
		{
			string newVersion;
			string changes;
			try
			{
				if (UpdateChecker.CheckForUpdate(out newVersion, out changes))
				{
					Messages.ShowInformationBox(string.Format(Resources.New0VersionIsAvailableNNchangesAreN1NNprogramWillNowOpenBrowserWithTheDownloadPage, newVersion, changes));
					SupportManager.DoSupport(SupportRequest.Releases);
				}
				else
				{
					Messages.ShowInformationBox(Resources.YouHaveTheLatestVersion);
				}
			}
			catch(InvalidOperationException exc)
			{
				Messages.ShowErrorBox(exc.Message);
			}
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
