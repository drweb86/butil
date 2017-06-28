using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Xml;
using System.IO;

namespace BULocalization
{
	public class Translation
	{
		private const string UnsupportedVersion = "Unsupported locals format";
		private const string CouldNotParseXml = "Could not parse xml";
		private const string CouldNotWorkWithFile = "Could not work with file";
	
		private static Translation _current;
		
		private readonly Dictionary<int, string> _items = new Dictionary<int, string>();
		
		private string _translationCopyright;
		private string _translationAuthor;
		private string _authorEmail;
		
		public Translation()
		{
			_current = this;
		}
		
		public static Translation Current
		{
			get { return _current; }
		}
		
		/// <summary>
		/// Gets translation with requested index id
		/// </summary>
		/// <exception cref="KeyNotFoundException">if id absent</exception>
		public string this[int index]
		{
			get 
			{ 
				if (_items.ContainsKey(index))
				{
					return _items[index];
				}
				else
				{
					throw new KeyNotFoundException(string.Format("Localization item with key '{0}' not found", index));
				}
			}
		}
		
		/// <summary>
		/// Copyright of the translation's author
		/// </summary>
		public string Copyright
		{
			get { return _translationCopyright; }
		}
		
		/// <summary>
		/// Author of the translation
		/// </summary>
		public string Author
		{
			get { return _translationAuthor; }
		}
		
		/// <summary>
		/// Email of the author of the translation
		/// </summary>
		public string Email
		{
			get { return _authorEmail; }
		}

		internal static Language GetInfo(string filename)
		{
			if (string.IsNullOrEmpty(filename))
				throw new ArgumentNullException("filename");
		
			string englishName;
			string naturalName = string.Empty;

			englishName = Path.GetFileNameWithoutExtension(filename);
			
			try
			{
				using (XmlTextReader reader = new XmlTextReader(filename))
				{
					reader.ReadStartElement();
					
					reader.ReadStartElement("Version");
					int version = reader.ReadContentAsInt();
					if (Consts.SupportedVersion != version)
						throw new LocalizationGenericException(UnsupportedVersion);
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"Language"
					
					// skipping reading eMail
					reader.ReadStartElement();
					reader.ReadContentAsString();
					reader.ReadEndElement();
					// skipping reading WWW
					reader.ReadStartElement();
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"Debug_Mode"
					reader.ReadContentAsInt();
					reader.ReadEndElement();
					//Debug.WriteLine("Debug mode: " + isDebugMode.ToString());
					
					reader.ReadStartElement();//"Author"
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"AuthorEmail"
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"AuthorFullName"
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					// skipping reading IsSupported
					reader.ReadStartElement();
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					// skipping reading NaturalLanguageName
					reader.ReadStartElement();
					naturalName = reader.ReadContentAsString();
					reader.ReadEndElement();
				}
			}
			catch (LocalizationGenericException)
			{
				throw;
			}
			catch (System.Xml.XmlException e)
			{
				throw new LocalizationGenericException(CouldNotParseXml, e.Message);
			}
			catch (System.IO.IOException e)
			{
				throw new LocalizationGenericException(CouldNotWorkWithFile, e.Message);
			}
			
			return new Language(englishName, naturalName, filename);
			
		}
		
		internal static void Load2(string filename, ReadOnlyCollection<string> namespacesToLoad)
		{
			Translation translation = new Translation();
			translation.Load(filename, namespacesToLoad);
			_current = translation;
		}
		
		/// <summary>
		/// Loads the translation from file
		/// </summary>
		/// <param name="file">.Language file</param>
		/// <param name="namespacesToLoad">list of namespaces to load. If all namespaces to load pass null or empty ReadOnlyCollection</param>
		public void Load(string file, ReadOnlyCollection<string> namespacesToLoad)
		{
			if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");
			
			bool loadAllData = false;
			bool isDebugMode = false;
			
			if (namespacesToLoad == null)
				loadAllData = true;
			else
			{
				if (namespacesToLoad.Count == 0)
					loadAllData = true;
			}
			
			_items.Clear();
			try
			{
				using (XmlTextReader reader = new XmlTextReader(file))
				{
					List<int> requestedNamespaces = new List<int>();
					
					reader.ReadStartElement();
					
					reader.ReadStartElement("Version");
					int version = reader.ReadContentAsInt();
					if (Consts.SupportedVersion != version)
						throw new LocalizationGenericException(UnsupportedVersion);
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"Language"
					
					// skipping reading eMail
					reader.ReadStartElement();
					reader.ReadContentAsString();
					reader.ReadEndElement();
					// skipping reading WWW
					reader.ReadStartElement();
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"Debug_Mode"
					isDebugMode = (reader.ReadContentAsInt() == 1);
					reader.ReadEndElement();
					//Debug.WriteLine("Debug mode: " + isDebugMode.ToString());
					
					reader.ReadStartElement();//"Author"
					_translationCopyright = reader.ReadContentAsString();
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"AuthorEmail"
					_authorEmail = reader.ReadContentAsString();
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"AuthorFullName"
					_translationAuthor = reader.ReadContentAsString();
					reader.ReadEndElement();
					
					// skipping reading IsSupported
					reader.ReadStartElement();
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					// skipping reading NaturalLanguageName
					reader.ReadStartElement();
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					reader.ReadStartElement();//"Namespaces"
					
					reader.ReadStartElement();//"Namespaces_Count"
					int namespacesCount = reader.ReadContentAsInt();
					reader.ReadEndElement();
					
					for (int i = 0; i < namespacesCount; i++)
					{
						string name;
						int id;
						
						reader.ReadStartElement();//"Namespace"
					
						reader.ReadStartElement();//"Name"
						name = reader.ReadContentAsString();
						reader.ReadEndElement();
					
						reader.ReadStartElement();//"N_ID"
						id = reader.ReadContentAsInt();
						reader.ReadEndElement();
					
						reader.ReadEndElement(); // Namespace
						
						if (loadAllData)
							requestedNamespaces.Add(id);
						else
						{
							if (namespacesToLoad.Contains(name))
								requestedNamespaces.Add(id);
						}
					}
					
					reader.ReadEndElement();// Namespaces
					
					reader.ReadStartElement();//"Items"
					
					reader.ReadStartElement();//"Records_Count"
					int itemsAmount = reader.ReadContentAsInt();
					reader.ReadEndElement();				
					
					// skipping reading ID_END
					reader.ReadStartElement();
					reader.ReadContentAsString();
					reader.ReadEndElement();
					
					for (int i = 0; i < itemsAmount; i++)
					{
						int id;
						string source = string.Empty;
						string translation = string.Empty;
						int nid;
						int state;
						
						reader.ReadStartElement();//"Item"
						
						reader.ReadStartElement();//"ID"
						id = reader.ReadContentAsInt();
						reader.ReadEndElement();
						
						reader.ReadStartElement();//"Source"
						source = reader.ReadContentAsString();
						reader.ReadEndElement();
						
						reader.ReadStartElement();//"Translation"
						if (reader.NodeType != XmlNodeType.Whitespace)
						{
							translation = reader.ReadContentAsString();
							reader.ReadEndElement();
						}
						
						reader.ReadStartElement();//"N_ID"
						nid = reader.ReadContentAsInt();
						reader.ReadEndElement();
						
						reader.ReadStartElement();//"State"
						state = reader.ReadContentAsInt();
						reader.ReadEndElement();
						
						reader.ReadEndElement();// item
						
						if (requestedNamespaces.Contains(nid))
						{
							string result;
							if (state == (int)TranslationItemState.Ok)
							{
								if (CheckIfItsCorrect(source, translation))
									result = translation;
								else
									result = source;
							}
							else
								result = source;
							
							if (isDebugMode)
								result = id.ToString(CultureInfo.CurrentUICulture) + ": " + result;
							
							_items.Add(id, ExtractFromStringWithFormatting(result));
						}
					}
					
					reader.ReadEndElement();// Items
					reader.ReadEndElement();// language
					reader.ReadEndElement();// xml
				}
			}
			catch (LocalizationGenericException)
			{
				throw;
			}
			catch (System.Xml.XmlException e)
			{
				throw new LocalizationGenericException(CouldNotParseXml, e.Message);
			}
			catch (System.IO.IOException e)
			{
				throw new LocalizationGenericException(CouldNotWorkWithFile, e.Message);
			}				
		}
		
		protected static bool CheckIfItsCorrect(string source, string translation)
		{
            if (source == translation)
                return true;

            if (string.IsNullOrEmpty(translation))
                return false;

			return true;
		}
		
		protected static string ExtractFromStringWithFormatting(string source)
		{
			string[] Editable = ConvertToSetOfLines(source);
			StringBuilder text = new StringBuilder();
			
			for (int i = 0; i < Editable.Length - 1; i++)
			{
				text.Append(Editable[i]);
				text.Append(Environment.NewLine);
			}
			
			text.Append(Editable[Editable.Length - 1]);
			
			return text.ToString();
		}
		
		/// <summary>
		/// Parses translation item to the set of lines
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public string[] ToSetOfLines(int id)
		{
			return ConvertToSetOfLines(CreateFormattedText(_items[id]));
		}
		
		protected static string[] ConvertToSetOfLines(string formatted)
		{
			if (!string.IsNullOrEmpty(formatted))
			{
				string[] result = formatted.Split(new string[] {"\\n"}, StringSplitOptions.None);
				return result;
			}
			else
				return new string[0];
		}
		
		protected static string CreateFormattedText(string extracted)
		{
			if (string.IsNullOrEmpty(extracted))
				return string.Empty;
			else
			{
				string[] source = extracted.Split(new string[]{Environment.NewLine}, StringSplitOptions.None);
				StringBuilder result = new StringBuilder();
				
				for (int i = 0; i < source.Length - 1; i++) 
				{
					result.Append(source[i]);
					result.Append("\\n");
				}
				result.Append(source[source.Length - 1]);
				return result.ToString();
			}

		}
	}
}
