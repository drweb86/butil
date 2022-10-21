using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace BULocalization
{
	public delegate void ApplyLanguageEventHandler(Translation instance);
	public delegate void ChangedLanguageOptionsEventHandler(string languageName);
	
	public class LanguagesManager
	{
		private const string CouldNotReadSettings = "Could not read own settings";
		private const string NoLanguage = "No such language exists!";
		private const string CouldNotSaveConfig = "Could not save changed language";
		
		private WeakReference _menuItemsReference;
		private readonly string _projectName;
		private readonly ReadOnlyCollection<string> _namespacesToLoad;
		private readonly string _pathToLocals;
		private readonly Languages _availableLanguages;
		private readonly ManagerBehaviorSettings _settings;
		private readonly object _syncObj = new object();
		private readonly string _internalConfigurationFilename;
		private string _currentLanguage = string.Empty;
		
		public event ApplyLanguageEventHandler OnApplyLanguage;
		public event ChangedLanguageOptionsEventHandler OnChangedLanguageOptions;
        
		/// <summary>
		/// english name of current language
		/// </summary>
		/// <exception cref="ArgumentNullException">value is null or empty</exception>
		public string CurrentLanguage
		{
			get { return _currentLanguage; }
			set 
			{ 
				if (!string.IsNullOrEmpty(value))
					_currentLanguage = value;
				else
					throw new ArgumentNullException("value");
				
				changeHelper();
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="namespacesToLoad">required namespaces. If you wanna load all namespaces please pass null or empty ReadOnlyCollection</param>
		/// <param name="pathToLocals">directory where *.language files are stored</param>
		/// <param name="projectName">name of your project. This name should be exeptable as folder name</param>
		/// <param name="settings">behaviour settings of program</param>
		/// <exception cref="ArgumentNullException">some parameters not specified</exception>
		public LanguagesManager(ReadOnlyCollection<string> namespacesToLoad, 
                                string pathToLocals,
                                string projectName,
                                ManagerBehaviorSettings settings)
		{
        	if (string.IsNullOrEmpty(pathToLocals))
        		throw new ArgumentNullException("pathToLocals");
        	
        	if (string.IsNullOrEmpty(projectName))
        		throw new ArgumentNullException("projectName");
        	
        	if (settings == null)
        		throw new ArgumentNullException("settings");
        	
        	_projectName = projectName;
        	_settings = settings;
			_namespacesToLoad = namespacesToLoad;
			_pathToLocals = pathToLocals;
			_internalConfigurationFilename = 
				Path.Combine(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					             projectName),
					"LanguageOptions.xml");
			_availableLanguages = new Languages();
		}
		
		/// <summary>
		/// If you selected UseToolGeneratedConfigFile use this variant
		/// otherwise please use Init(string language)
		/// </summary>
		public void Init()
		{
			_availableLanguages.Load(_pathToLocals);
			
			// loading language
			if (_settings.UseToolGeneratedConfigFile)
			{
				if (File.Exists(_internalConfigurationFilename))
				{
					try
					{
						_currentLanguage = File.ReadAllText(_internalConfigurationFilename);
					}
					catch (UnauthorizedAccessException e)
					{
						throw new LocalizationGenericException(CouldNotReadSettings, e);
					}						
					catch (PathTooLongException e)
					{
						throw new LocalizationGenericException(CouldNotReadSettings, e);
					}	
					catch (NotSupportedException e)
					{
						throw new LocalizationGenericException(CouldNotReadSettings, e);
					}					
					catch (SecurityException e)
					{
						throw new LocalizationGenericException(CouldNotReadSettings, e);
					}
					catch (IOException e)
					{
						throw new LocalizationGenericException(CouldNotReadSettings, e);
					}
				}
				else
					_currentLanguage = string.Empty;
			}
			
			// basic checkings
			if (string.IsNullOrEmpty(_currentLanguage))
			{
				if (_settings.RequestLanguageIfNotSpecified)
					ShowSelectLanguageDialog(false, false);
				else
					_currentLanguage = "default";
			}
			
			applyHelper();
			
		}
		
		/// <summary>
		/// If you use own config file use this variant
		/// otherwise please use Init()
		/// </summary>
		public void Init(string language)
		{
			_currentLanguage = language;
			_availableLanguages.Load(_pathToLocals);
			Init();
		}
		
		/// <summary>
		/// Shows the select language dialog
		/// </summary>
		/// <remarks>Should be called after Init method</remarks>
		/// <param name="canCancel">availability of cancel button</param>
		/// <param name="apply">to call or not to call apply language</param>
		/// <returns>true if something selected</returns>
		public bool ShowSelectLanguageDialog(bool canCancel, bool apply)
		{
			bool result;

			using (ChooseLanguages form = new ChooseLanguages(_availableLanguages.GetItems(), canCancel, _projectName))
            {
                result = (form.ShowDialog() == DialogResult.OK);
                if (result)
                    _currentLanguage = form.SelectedLanguage.EnglishName;
            }
            
			if (result)
			{
            	if (apply)
	            	applyHelper();
            
            	changeHelper();
			}

            return result;
		}
		
		private Language getLanguage(string name)
		{
			Language[] languages = _availableLanguages.GetItems();
			
			Language selected = null;

			foreach(Language language in languages)
			{
				if (language.EnglishName.Equals(name, StringComparison.Ordinal))
					selected = language;
                if (language.NaturalName.Equals(name, StringComparison.Ordinal))
					selected = language;
			}
			
			return selected;
		}
		
		private void applyHelper()
		{
			Language selected = getLanguage(_currentLanguage);
			
			if (selected == null)
				throw new LocalizationGenericException(NoLanguage);
			
			Translation.Load2(selected.LinkedFile, _namespacesToLoad);
            
			if (OnApplyLanguage != null)
				OnApplyLanguage(Translation.Current);
		}
		
		private void changeHelper()
		{
			Language selected = getLanguage(_currentLanguage);
			
			if (OnChangedLanguageOptions != null)
				OnChangedLanguageOptions(_currentLanguage);
			
			if (_settings.UseToolGeneratedConfigFile)
			{
				try
				{
					string folder = Directory.GetParent(_internalConfigurationFilename).FullName;
					if (!Directory.Exists(folder))
						Directory.CreateDirectory(folder);
					
					File.WriteAllText(_internalConfigurationFilename, _currentLanguage);
				}
				catch (IOException e)
				{
					throw new LocalizationGenericException(CouldNotSaveConfig, e);
				}
			}
			
			if (_menuItemsReference != null)
			{
				ToolStripMenuItem item = (ToolStripMenuItem)_menuItemsReference.Target;
				if (item != null)
				{
					foreach (ToolStripMenuItem subitem in item.DropDownItems)
					{
						subitem.Checked = (selected.NaturalName == subitem.Text);
					}
				}
			}
		}
        
		/// <summary>
		/// Adds necessary subitems for toolstripmenuitems. Should be called after Init
		/// </summary>
		public void GenerateMenuWithLanguages(ToolStripDropDownItem targetToolStrip)
		{
			if (targetToolStrip == null)
                throw new InvalidOperationException("targetToolStrip");
			
			_menuItemsReference = new WeakReference(targetToolStrip);
			
			Language[] languages = _availableLanguages.GetItems();
			
			foreach(Language language in languages)
			{
				ToolStripMenuItem newMenuItem = new ToolStripMenuItem(language.NaturalName);
				targetToolStrip.DropDownItems.Add(newMenuItem);
				newMenuItem.CheckOnClick = true;
                newMenuItem.Click += selectedOtherLanguageEventHandler;

                if (_currentLanguage == language.EnglishName) 
                    newMenuItem.Checked = true;
			}
		}
		
		
		private void selectedOtherLanguageEventHandler (object sender, EventArgs e)
		{
            lock (_syncObj)
            {
            	ToolStripMenuItem item = (ToolStripMenuItem)sender;;
            	string testToSearch = item.Text;

            	Language selected = getLanguage(testToSearch);
            	
            	if (selected != null)
				{
					CurrentLanguage = selected.EnglishName;
					applyHelper();
				}
            }
		}
		
		/// <summary>
		/// Applies current translation
		/// </summary>
		public void Apply()
		{
			applyHelper();
		}
	}
}
