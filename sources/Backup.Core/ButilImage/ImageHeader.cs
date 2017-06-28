using System;
using System.IO;
using System.Collections.ObjectModel;

namespace BUtil.Core.ButilImage
{
	/// <summary>
	/// Description of ImageHeader.
	/// </summary>
	public class ImageHeader
	{
		private readonly Collection<MetaRecord> _records;

        #region public properties

		public Collection<MetaRecord> Records
		{
            get { return _records; }
        }

        #endregion

		#region constructors

		public ImageHeader()
		{
			_records = new Collection<MetaRecord>();
		}

		public ImageHeader(Collection<MetaRecord> records)
		{
			if (records != null)
				_records = records;
			else
				_records = new Collection<MetaRecord>();
		}

		#endregion
		/// <summary>
		/// Loads metadata from an array
		/// </summary>
		/// <param name="bytes">Array of bytes</param>
		public void FromByteArray(byte[] array)
		{
			int recordsAmount = BitConverter.ToInt32(array, 0);
			int LastByteIndex = 4;
			int metarecordsize = -1;
			byte[] splitMe;

			for (int i = 0; i < recordsAmount; i++)
			{
				metarecordsize = BitConverter.ToInt32(array, LastByteIndex);
				LastByteIndex += 4;
				
				splitMe = new byte[metarecordsize];
				Array.Copy(array, LastByteIndex, splitMe, 0, metarecordsize);
				_records.Add( MetaRecord.FromByteArray(splitMe) );
				
				LastByteIndex += metarecordsize;
			}
			
		}
		
		/// <summary>
		/// Converts all metadata to an array of bytes
		/// </summary>
		/// <returns>array of bytes</returns>
		public byte[] ToArray()
		{
            byte[] somearray = BitConverter.GetBytes(_records.Count);
			byte[] splitMe;
			int LastByteIndex = 4;

			foreach (MetaRecord record in _records)
			{
				splitMe = record.ToByteArray();
				Array.Resize(ref somearray, somearray.Length + splitMe.Length + 4);
				Array.Copy(BitConverter.GetBytes(splitMe.Length), 0, somearray, LastByteIndex, 4);
				Array.Copy(splitMe, 0, somearray, LastByteIndex + 4, splitMe.Length);
				LastByteIndex += splitMe.Length + 4;
			}
			
			return somearray;
		}
		
		
		/// <summary>
		/// Searches for the anteposition of file
		/// </summary>
		/// <param name="record">record to search</param>
		/// <returns>anteposition</returns>
		/// <exception cref="InvalidOperationException">record not found</exception>
		public Int64 GetDisplacementOfPackedFile(MetaRecord record)
		{
			Int64 displacement = 0;
			foreach (MetaRecord recordIn in _records)
			{
				if (recordIn == record) 
					return displacement;

				displacement += recordIn.SizeOfData;
			}
			throw new InvalidOperationException("Not found");
		}
	}
}
