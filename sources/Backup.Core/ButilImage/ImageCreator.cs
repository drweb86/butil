using System;
using System.Globalization;
using System.IO;
using System.Collections.ObjectModel;

namespace BUtil.Core.ButilImage
{
	/// <summary>
	/// Description of ImageCreator.
	/// </summary>
	public sealed class ImageCreator
	{
		private const int _IMAGE_FORMAT_VERSION = 1;
        private const int _BUFFER_SIZE = 10 * 1024 * 1024;

        private const string _FILE_EXISTS = "File '{0}' exists!!!";

		ImageHeader _metadata = new ImageHeader();
		private readonly string _file;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="FileName">File name of an image</param>
		public ImageCreator(string file, ImageHeader metadata)
		{
			if (string.IsNullOrEmpty(file))
			{
				throw new ArgumentNullException("file");
			}
			
			if (metadata == null)
			{
				throw new ArgumentException("metadata");
			}

			_metadata = metadata;
			_file = file;
		}

        /// <summary>
        /// Packs several items to an image
        /// </summary>
        /// <param name="overwrite">specifies wheather to overwrite image or not</param>
        /// <exception cref="InvalidDataException">Image file exists and overwrition didn't allowed</exception>
        /// <exception cref="FileNotFoundException">Any of linked files to pack to an image not found</exception>
        /// <exception cref="other"></exception>
		public void Pack(bool overwrite)
		{
			if (File.Exists(_file))
                if (!overwrite)
					throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, _FILE_EXISTS, _file));

            Collection<MetaRecord> records = _metadata.Records;

            foreach (MetaRecord record in records)
            {
                if (!File.Exists(record.LinkedFile))
					throw new FileNotFoundException(record.LinkedFile);
            }

            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(_file)))
            {
				writer.Write(_IMAGE_FORMAT_VERSION);
                byte[] metadatabytes = _metadata.ToArray();
                writer.Write(BitConverter.GetBytes(metadatabytes.Length));
                writer.Write(metadatabytes);

                byte[] buffer;

                foreach (MetaRecord record in records)
                {
                    using (BinaryReader reader = new BinaryReader(File.OpenRead(record.LinkedFile)))
                    {
                        do
                        {
                            buffer = reader.ReadBytes(_BUFFER_SIZE);
                            writer.Write(buffer);
                        }
                        while (buffer.Length == _BUFFER_SIZE);

                        reader.Close();
                    }
                }
                writer.Flush();
                writer.Close();
            }
		}
	}
}
