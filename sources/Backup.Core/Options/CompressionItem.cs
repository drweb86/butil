using System;
using BUtil.Core.ButilImage;

namespace BUtil.Core.Options
{
	public class SourceItem
	{
		public SourceItem() : this(String.Empty, false, CompressionDegree.Normal) { } // deserialization

		public SourceItem(string target, bool isFolder, CompressionDegree compressionDegree)
		{
			Target = target;
			IsFolder = isFolder;
			CompressionDegree = compressionDegree;
		}

		public string Target { get; set; }

		public CompressionDegree CompressionDegree { get; set; }

		public bool IsFolder { get; set; }
		
	}
}
