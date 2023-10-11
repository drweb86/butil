using Avalonia.Media;
using BUtil.Core.Events;
using BUtil.Core.Settings;
using System;

namespace butil_ui.ViewModels;

public static class ColorPalette
{
    public static SolidColorBrush GetResultColor(ProcessingStatus state)
    {
        return state switch
        {
            ProcessingStatus.FinishedSuccesfully => new SolidColorBrush(GetForeground(SemanticColor.Success)),
            ProcessingStatus.FinishedWithErrors => new SolidColorBrush(GetForeground(SemanticColor.Error)),
            ProcessingStatus.InProgress => new SolidColorBrush(GetForeground(SemanticColor.InProgress)),
            ProcessingStatus.NotStarted => new SolidColorBrush(GetForeground(SemanticColor.Normal)),
            _ => throw new NotImplementedException(state.ToString()),
        };
    }

    public static Color GetForeground(SemanticColor color)
    {
        if (ApplicationSettings.Theme == ThemeSetting.DarkValue)
        {
            switch (color)
            {
                case SemanticColor.Normal: return Colors.White;
                case SemanticColor.Success: return Color.FromRgb(147, 199, 93);
                case SemanticColor.Error: return Color.FromRgb(222, 98, 89);
                case SemanticColor.InProgress: return Colors.Yellow;
            }

        } 
        else if (ApplicationSettings.Theme == ThemeSetting.LightValue)
        {
            switch (color)
            {
                case SemanticColor.Normal: return Colors.Black;
                case SemanticColor.Success: return Color.FromRgb(5, 139, 0);
                case SemanticColor.Error: return Color.FromRgb(218,59,1);
                case SemanticColor.InProgress: return Color.FromRgb(128,0,224);
            }
        }

        return Colors.Red;
    }
}
