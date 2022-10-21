using System;
using BULocalization;
using BUtil.Core.Misc;
using BUtil.Core.FileSystem;
using BUtil.Core;
using System.Text;
using BUtil.Core.Options;
using BUtil.Core.PL;

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
			visitWebSiteLabel.Text = Translation.Current[288];
            suggestAFeatureLabel.Text = Translation.Current[289];
            reportABugLabel.Text = Translation.Current[290];
            supportLabel.Text = Translation.Current[291];
            documentationLabel.Text = Translation.Current[294];
            checkForUpdatesLabel.Text = Translation.Current[530];
            var aboutInfo = new StringBuilder();
            aboutInfo.Append(CopyrightInfo.Copyright);
            aboutInfo.Append(Translation.Current[142]);
            aboutInfo.Append(Translation.Current.Copyright);
            aboutInfo.Append("\n\n");
            aboutInfo.Replace("\r", string.Empty);
            aboutInfo.Replace("\n", "\r\n");
            aboutTextBox.Text = aboutInfo.ToString();
            aboutTextBox.Select(0, 0);
		}
	
		public override void SetOptionsToUi(object settings)
		{
			_options = (ProgramOptions)settings;
			
			visitWebSiteLabel.Enabled = !_options.HaveNoNetworkAndInternet;
			suggestAFeatureLabel.Enabled = !_options.HaveNoNetworkAndInternet;
			reportABugLabel.Enabled = !_options.HaveNoNetworkAndInternet;
			supportLabel.Enabled = !_options.HaveNoNetworkAndInternet;
			checkForUpdatesLabel.Enabled = !_options.HaveNoNetworkAndInternet;
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
					Messages.ShowInformationBox(string.Format(Translation.Current[531], newVersion, changes));
					SupportManager.DoSupport(SupportRequest.Releases);
				}
				else
				{
					Messages.ShowInformationBox(Translation.Current[532]);
				}
			}
			catch(InvalidOperationException exc)
			{
				Messages.ShowErrorBox(exc.Message);
			}
		}
		
		void SevenZipPictureBoxClick(object sender, EventArgs e)
		{
			if (!_options.HaveNoNetworkAndInternet)
			{
				SupportManager.DoSupport(SupportRequest.SevenZip);
			}
		}

		void VirtuawinPictureBoxClick(object sender, EventArgs e)
		{
			if (!_options.HaveNoNetworkAndInternet)
			{
				SupportManager.DoSupport(SupportRequest.VirtuaWin);
			}
		}
		
		void LogoPictureBoxClick(object sender, EventArgs e)
		{
			if (!_options.HaveNoNetworkAndInternet)
			{
				SupportManager.DoSupport(SupportRequest.Homepage);
			}
		}
	}
}
