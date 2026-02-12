#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BUtil.Core.Misc;

internal static class ObjectValidationExtensions
{
    [return: NotNull]
    public static TData EnsureNotNull<TData>(this TData? obj, [CallerArgumentExpression("obj")] string? reason = null)
        where TData : class
    {
        if (obj == null)
        {
            throw new NullReferenceException(reason);
        }

        return obj;
    }
}
