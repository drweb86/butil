using System;
using BUtil.Core.Events;
using BUtil.Core.Localization;

namespace BUtil.Core
{
	public static class LocalsHelper
	{
		public static string ToString(ProcessingStatus state)
		{
			switch (state)
			{
				case ProcessingStatus.NotStarted: return Resources.Waiting;
				case ProcessingStatus.InProgress: return Resources.InProgress;
				case ProcessingStatus.FinishedSuccesfully: return Resources.FinishedSuccesfully;
				case ProcessingStatus.FinishedWithErrors: return Resources.FinishedWithErrors;
				default: 
					throw new NotImplementedException("State " + state + " is not implemented");
			}
		}
	}
}
