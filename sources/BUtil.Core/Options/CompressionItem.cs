using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BUtil.Core.ButilImage;

namespace BUtil.Core.Options
{
	public class SourceItem: IEqualityComparer<SourceItem>
    {
		public SourceItem() : this(String.Empty, false) { } // deserialization

		public SourceItem(string target, bool isFolder)
		{
			Target = target;
			IsFolder = isFolder;
		}

		public string Target { get; set; }

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
