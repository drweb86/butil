using System;
using System.Collections.Generic;
using System.IO;

namespace BULocalization
{
	public class Languages
	{
        private readonly SortedList<string, Language> _items = new SortedList<string, Language>();

        /// <summary>
        /// List of loaded languages
        /// </summary>
        /// <returns>array of loaded languages</returns>
        public Language[] GetItems()
		{
        	return new List<Language>(_items.Values).ToArray();
		}

		/// <summary>
		/// Loads all languages and their metadata from directory
		/// </summary>
		/// <param name="directoryWithLocals">Directory with *.Language files</param>
		/// <exception cref="LocalizationGenericException"></exception>
		/// <exception cref="ArgumentNullException">Directory not specified</exception>
		public void Load(string directoryWithLocals)
		{
			if (string.IsNullOrEmpty(directoryWithLocals))
                throw new ArgumentNullException("directoryWithLocals");
			
			_items.Clear();
			
			try
			{
				string[] files = Directory.GetFiles(directoryWithLocals, Consts.LanguagePattern);
				
				for (int i = 0; i < files.Length; i++)
				{
					Language item = Translation.GetInfo(files[i]);
					_items.Add(item.NaturalName, item);
				}
			}
			catch (LocalizationGenericException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new LocalizationGenericException(e.Message, e);
			}
		}
	}
}
