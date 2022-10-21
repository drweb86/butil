using System;

namespace BULocalization
{
    public sealed class Language
    {
        private string _englishName;
        private string _naturalName;
		private string _file;

        public string LinkedFile
        {
            get { return _file; }
        }

        public string EnglishName
        {
            get { return _englishName; }
        }

        public string NaturalName
        {
            get { return _naturalName; }
        }
        
        internal Language(string englishName, string naturalName, string linkedFile)
        {
			if (string.IsNullOrEmpty(naturalName))
				throw new ArgumentException("naturalName");
			
			if (string.IsNullOrEmpty(englishName))
                throw new ArgumentException("englishName");

			if (string.IsNullOrEmpty(linkedFile))
                throw new ArgumentException("linkedFile");

			_file = linkedFile;
			_naturalName = naturalName;
        	_englishName = englishName;
        }
    }
}
