using System;
using System.Text;
using System.Collections;
using System.IO;

namespace BUtil.Core.ButilImage
{
	/// <summary>
	/// Description of MetaRecord.
	/// </summary>
	public sealed class MetaRecord
	{
        private string _LINKEDFILE = "LinkedFile";

        private string _SETTONULLOREMPTY = "Set to null or empty";

		string _initialTarget;
		Int64 _sizeOfData = -1;
		string _linkedFile = string.Empty;
		
        #region public properties

        public bool IsFolder { get; set; }
		
		/// <summary>
		/// Source name with path of compression item
		/// </summary>
		public string InitialTarget
		{
            get { return _initialTarget; }
			set 
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("InitialTarget");

                _initialTarget = value; 
			}
		}
		
		public string LinkedFile
		{
            get { return _linkedFile; }
			set 
			{
				if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(_LINKEDFILE, _SETTONULLOREMPTY);
                _linkedFile = value; 
			}
		}
		
		public Int64 SizeOfData
		{
			get 
            {
                if (_sizeOfData != -1)
                    return _sizeOfData;
                else
                {
                    FileInfo info = new FileInfo(_linkedFile);
                    _sizeOfData = info.Length;
                    return _sizeOfData;
                }
			}
		}

        #endregion

        #region Constructors
		
		public MetaRecord(bool isFolder, string initialTarget)
		{
			IsFolder = isFolder;
			InitialTarget = initialTarget;
		}

        public MetaRecord()
        { 
        
        }

        #endregion

		/// <summary>
		/// Creates an instance of MetaRecord
		/// </summary>
		/// <param name="ByteArray">metadata in an array</param>
		public static MetaRecord FromByteArray(byte[] array)
		{
			MetaRecord rm = new MetaRecord();
			
			int length = array.Length - 9;
			
			rm.IsFolder = Convert.ToBoolean(array[length]);
			
			char[] strarray = new char[1024];
			int charUsed, bytesUsed;
			bool isCompleted;
			Encoding.Default.GetDecoder().Convert(array, 0, length, strarray,
			                                      0, strarray.Length, true, out bytesUsed, out charUsed, out isCompleted);
			
			rm.InitialTarget = new string(strarray, 0, charUsed);
			
			rm._sizeOfData = BitConverter.ToInt64(array, length + 1);
			
			return rm;
		}
		
		
		/// <summary>
		/// Creates full metadata context
		/// </summary>
		/// <returns>Bytes array</returns>
		public byte[] ToByteArray()
		{
			byte[] srcFileNameBytes = Encoding.Default.GetBytes(InitialTarget);
			byte[] sizeBytes = BitConverter.GetBytes(SizeOfData);
			byte folderFlag = Convert.ToByte(IsFolder);
			int length = srcFileNameBytes.Length;
			
			Array.Resize(ref srcFileNameBytes, length + 9);
			
			srcFileNameBytes[length] = folderFlag;
			for (int i = 0; i < 8; i++)
				srcFileNameBytes[length + 1 + i] = sizeBytes[i];

			return srcFileNameBytes;
		}
	}
}
