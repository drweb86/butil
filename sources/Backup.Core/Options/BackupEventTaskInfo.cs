
namespace BUtil.Core.Options
{
	public sealed class ExecuteProgramTaskInfo
	{
		public string Name { get; set; }
		public string Program { get; set; }
		public string Arguments { get; set; }

		public ExecuteProgramTaskInfo() { }
        public ExecuteProgramTaskInfo(string name, string program, string arguments)
		{
			Name = name;
			Program = program;
			Arguments = arguments;
		}
	}
}
