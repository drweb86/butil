using System;

namespace BUtil.Core.Options
{
    public sealed class ProgramOptions: ICloneable
	{
		public string LogsFolder { get; set; }
        

        public object Clone()
        {
			return this.MemberwiseClone();
        }

        public bool CompareTo(ProgramOptions other)
        {
			return other.LogsFolder == LogsFolder;

        }
    }
}
