using System;
using BULocalization;
using BUtil.Core.ButilImage;

namespace BUtil.Core
{
	public static class LocalsHelper
	{
		public static string ToString(CompressionDegree degree)
		{
			switch (degree)
			{
				case CompressionDegree.Store: return Translation.Current[295];
				case CompressionDegree.Fastest: return Translation.Current[296];
				case CompressionDegree.Fast: return Translation.Current[297];
				case CompressionDegree.Normal: return Translation.Current[298];
				case CompressionDegree.Maximum: return Translation.Current[299];
				case CompressionDegree.Ultra: return Translation.Current[300];
				default: 
					throw new NotImplementedException("Degree " + degree + " is not implemented");
			}
		}
		
		public static string ToString(ProcessingState state)
		{
			switch (state)
			{
				case ProcessingState.NotStarted: return Translation.Current[544];
				case ProcessingState.InProgress: return Translation.Current[543];
				case ProcessingState.FinishedSuccesfully: return Translation.Current[545];
				case ProcessingState.FinishedWithErrors: return Translation.Current[546];
				default: 
					throw new NotImplementedException("State " + state + " is not implemented");
			}
		}
	}
}
