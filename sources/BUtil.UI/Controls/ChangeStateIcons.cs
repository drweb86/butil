using System;

namespace BUtil.UI.Controls;

public static class ChangeStateIcons
{
    public const string Created = "✨";
    public const string Updated = "✏️";
    public const string Deleted = "🗑️";

    public static string Get(ChangeState state) => state switch
    {
        ChangeState.Created => Created,
        ChangeState.Updated => Updated,
        ChangeState.Deleted => Deleted,
        _ => throw new ArgumentOutOfRangeException(nameof(state)),
    };
}
