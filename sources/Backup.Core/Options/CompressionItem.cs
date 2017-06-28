using System;
using BUtil.Core.ButilImage;

namespace BUtil.Core.Options
{
    [Serializable]
	public class CompressionItem
	{
		private string _target = string.Empty;
		private bool _isFolder; // auto: false;
		private CompressionDegree _compressionDegree = CompressionDegree.Normal;

		public CompressionItem()
		{
		
		}
		
		public CompressionItem(string target, bool isFolder, CompressionDegree compressionDegree)
		{
			Target = target;
			IsFolder = isFolder;
			CompressionDegree = compressionDegree;
		}

		/// <summary>
		/// Filename or folder
		/// </summary>
		public string Target
		{
			get { return _target; }
			set 
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("Target");

				_target = value; 
			}
		}

		public CompressionDegree CompressionDegree
		{
			get { return _compressionDegree; }
			set { _compressionDegree = value; }
		}

		/// <summary>
		/// True-folder
		/// </summary>
		public bool IsFolder
		{
			get { return _isFolder; }
			set { _isFolder = value; }
		}
		
	}
}
