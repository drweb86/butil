using System;

namespace BUtil.Core.Misc;
internal static class ObjectValidationExtensions
{
    public static void EnsureNotNull(this object obj, string reason)
    {
        if (obj == null)
        {
            throw new NullReferenceException(reason);
        }
    }
}
