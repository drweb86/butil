using System;
using System.Globalization;
using System.IO;

namespace BUtil.Core.ButilImage
{
	/// <summary>
	/// Description of ImageReader.
	/// </summary>
	public class ImageReader
	{
        private const string _FILEDOESNOTEXIST = "File '{0}' does not exist!";
        private const string _INVALIDVERSION = "This image has invalid version {0}. Supported version is {1} version";
        private const string _INVALIDIMAGE = "Incomplete image!";
		private const int _IMAGE_VERSION = 1;
        private const int _BUFFERLENGTH = 10 * 1024 * 1024;

		
		private string _imageFileName = string.Empty;
		private ImageHeader _metadata = new ImageHeader();
		private int _dataStartPos = -1;

        #region public properties

        public string FileName
		{
			get { return _imageFileName; }
			set
			{
				if (string.IsNullOrEmpty(value)) 
					throw new ArgumentException("ImageFileName");

				_imageFileName = value;
			}
		}
		
		public ImageHeader Metadata
		{
			get { return _metadata; }
			set 
			{
				if (value == null) 
					throw new ArgumentException("Metadata");

				_metadata = value;
			}
        }

        #endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotSupportedException">Image version is more fresh than supported</exception>
		/// <exception cref="InvalidDataException">Image file not found, not accessable or corrupted</exception>
        public ImageHeader Open()
		{
			if (!File.Exists(FileName))
				throw new InvalidDataException(
					string.Format(CultureInfo.CurrentCulture,
								_FILEDOESNOTEXIST, 
								FileName));

			try
			{
				using (BinaryReader reader = new BinaryReader(File.OpenRead(FileName)))
				{
					int currentVersion = reader.ReadInt32();

					if (currentVersion > _IMAGE_VERSION)
						throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, _INVALIDVERSION, currentVersion, _IMAGE_VERSION));

					int metadataLength = reader.ReadInt32();

					_metadata.FromByteArray(reader.ReadBytes(metadataLength));
					_dataStartPos = 8 + metadataLength;
				}
			}
			catch (UnauthorizedAccessException e)
			{ throw new InvalidDataException(e.Message); }
			catch (DirectoryNotFoundException e)
			{ throw new InvalidDataException(e.Message); }
			catch (FileNotFoundException e)
			{ throw new InvalidDataException(e.Message); }
			catch (ArgumentException e)
			{ throw new InvalidDataException(e.Message); }
			catch (EndOfStreamException e)
			{ throw new InvalidDataException(e.Message); }
			catch (IOException e)
			{ throw new InvalidDataException(e.Message); }

			return _metadata;
		}
		

		public void Extract(MetaRecord record, string destinationFileName)
		{
            if (File.Exists(destinationFileName))
                File.Delete(destinationFileName);

            Int64 displacement = _dataStartPos + _metadata.GetDisplacementOfPackedFile(record);

            using (FileStream fs = File.OpenRead(FileName))
            {
                fs.Seek(displacement, SeekOrigin.Begin);

                using (BinaryReader reader = new BinaryReader(fs))
                {
                    using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(destinationFileName)))
                    {
                        byte[] buffer = new byte[1];
                        
                        Int64 sizeToRead = record.SizeOfData;

                        while (sizeToRead > 0)
                        {
                            if (buffer.Length == 0) 
                                throw new InvalidDataException(_INVALIDIMAGE);

                            if (sizeToRead >= _BUFFERLENGTH) 
                                buffer = reader.ReadBytes(_BUFFERLENGTH);
                            else 
                                buffer = reader.ReadBytes((int)sizeToRead);

                            writer.Write(buffer);
                            sizeToRead -= buffer.Length;
                        }
                    }
                }
            }
		}
	}
}
