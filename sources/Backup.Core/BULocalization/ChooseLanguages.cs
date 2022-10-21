using System;
using System.Windows.Forms;

namespace BULocalization
{
	public partial class ChooseLanguages : Form
	{
		private Language _selectedLanguage;
		private bool _allowCancel;
		private Language[] _languages;

        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
        }
        
        public ChooseLanguages(Language[] languages, bool allowCancel, string projectName)
		{
            if (languages == null)
                throw new ArgumentNullException("languages");

            if (languages.Length == 0)
                throw new ArgumentNullException("languages");

			InitializeComponent();
		
			Text += projectName;
			_allowCancel = allowCancel;
			_languages = languages;
			
            foreach (Language item in languages)
            {
                _languagelistBox.Items.Add(item.NaturalName);
            }
            
            _cancelbutton.Visible = _allowCancel;

            if (!_allowCancel)
            {
                _selectbutton.Location = _cancelbutton.Location;
            }
		}
		
		void ChooseLanguagesFormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_allowCancel) 
                e.Cancel = true;
		}
		
		void SelectbuttonClick(object sender, EventArgs e)
		{
			if (_languagelistBox.SelectedIndex >= 0)
			{
                _selectedLanguage = _languages[_languagelistBox.SelectedIndex];
				_allowCancel = true;
				DialogResult = DialogResult.OK;
			}
		}
		
				
		void languagelistBoxDoubleClick(object sender, System.EventArgs e)
		{
			SelectbuttonClick(sender, e);
		}
	}
}
