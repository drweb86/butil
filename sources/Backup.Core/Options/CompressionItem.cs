using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BUtil.Core.ButilImage;

namespace BUtil.Core.Options
{
	public class SourceItem: IEqualityComparer<SourceItem>
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

		public bool Equals(SourceItem x, SourceItem y)
		{
			return 
				x.Target == y.Target &&
				x.IsFolder == IsFolder;
		}

		public int GetHashCode([DisallowNull] SourceItem obj)
		{
			return obj.Target.GetHashCode();
		}
	}
}
