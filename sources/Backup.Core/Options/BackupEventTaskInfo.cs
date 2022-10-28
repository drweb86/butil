
using System;

namespace BUtil.Core.Options
{
	public sealed class ExecuteProgramTaskInfo
	{
		
		public string Program { get; set; }
		
		public string Arguments { get; set; }

        public ExecuteProgramTaskInfo() { } // deserialization
        
		public ExecuteProgramTaskInfo(string program, string arguments)
		{
			Program = program;
			Arguments = arguments;
		}
		
		public override string ToString()
		{
			return $"{Program} {Arguments}";
		}
	}
}
