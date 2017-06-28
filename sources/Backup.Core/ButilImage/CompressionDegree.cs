using System;
using BULocalization;

namespace BUtil.Core.ButilImage
{
    [Serializable]
	public enum CompressionDegree
	{
		Store = 0,// required
		Fastest = 1,
		Fast = 2,
		Normal = 3,
		Maximum = 4,
		Ultra = 5
	}
}
