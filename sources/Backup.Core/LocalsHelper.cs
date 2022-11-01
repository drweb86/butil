using System;

using BUtil.Core.ButilImage;
using BUtil.Core.Events;
using BUtil.Core.Localization;

namespace BUtil.Core
{
	public static class LocalsHelper
	{
		public static string ToString(CompressionDegree degree)
		{
			switch (degree)
			{
				case CompressionDegree.Store: return Resources.WithoutCompression;
				case CompressionDegree.Fastest: return Resources.FastestCompression;
				case CompressionDegree.Fast: return Resources.FastCompression;
				case CompressionDegree.Normal: return Resources.NormalCompression;
				case CompressionDegree.Maximum: return Resources.MaximumCompression;
				case CompressionDegree.Ultra: return Resources.UltraCompression;
				default: 
					throw new NotImplementedException("Degree " + degree + " is not implemented");
			}
		}
		
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
