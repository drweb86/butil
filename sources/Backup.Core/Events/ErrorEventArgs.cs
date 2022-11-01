using System;

namespace BUtil.Core.Events
{
    public class ErrorEventArgs: EventArgs
	{
		public ErrorEventArgs(string error)
		{
			Error = error;
		}
        public string Error { get; private set; }
	}
}
