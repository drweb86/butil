using System;

namespace BUtil.Core.Options
{
	public class SourceItem
    {
		public Guid Id { get; set; } = Guid.NewGuid();

		public SourceItem() : this(String.Empty, false) { } // deserialization

		public SourceItem(string target, bool isFolder)
		{
			Target = target;
			IsFolder = isFolder;
		}

		public string Target { get; set; }

		public bool IsFolder { get; set; }

		public bool CompareTo(SourceItem x)
		{
			return 
				x.Target == Target &&
				x.IsFolder == IsFolder;
		}
	}
}
