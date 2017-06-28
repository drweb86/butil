using System;
using System.Runtime.Serialization;

namespace BUtil.Core.Options
{
	/// <summary>
	/// Description of OptionsException.
	/// </summary>
	[Serializable]
	public sealed class OptionsException : Exception
	{
		public OptionsException(string message, Exception innerException)
			: base(message, innerException)
		{
			;
		}

		public OptionsException()
		{
        	;
		}

		private OptionsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ 
		
		}
		public OptionsException(string message)
			: base(message)
		{ 
            
		}
	}
}
