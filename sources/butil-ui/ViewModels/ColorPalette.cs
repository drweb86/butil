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
            ProcessingStatus.FinishedSuccesfully => new SolidColorBrush(GetColor(SemanticColor.Success)),
            ProcessingStatus.FinishedWithErrors => new SolidColorBrush(GetColor(SemanticColor.Error)),
            ProcessingStatus.InProgress => new SolidColorBrush(GetColor(SemanticColor.InProgress)),
            ProcessingStatus.NotStarted => new SolidColorBrush(GetColor(SemanticColor.Normal)),
            _ => throw new NotImplementedException(state.ToString()),
        };
    }

    public static Color GetColor(SemanticColor color)
    {
        if (ApplicationSettings.Theme == ThemeSetting.DarkValue)
        {
            switch (color)
            {
                case SemanticColor.Normal: return Colors.White;
                case SemanticColor.Success: return Color.FromRgb(42, 130, 67);
                case SemanticColor.Error: return Color.FromRgb(222, 98, 89);
                case SemanticColor.InProgress: return Colors.Yellow;
                case SemanticColor.HeaderBackground: return Color.FromRgb(17,34,51);
                case SemanticColor.WindowBackground: return Color.FromRgb(40,40,34);
                case SemanticColor.ForegroundWindowFont: return Color.FromRgb(162,168,175);
                case SemanticColor.ForegroundWindowFontAccented: return Color.FromRgb(214, 222, 235);
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
                case SemanticColor.HeaderBackground: return Color.FromRgb(243, 243, 243);
                case SemanticColor.WindowBackground: return Color.FromRgb(243, 243, 243);
                case SemanticColor.ForegroundWindowFont: return Colors.Black;
                case SemanticColor.ForegroundWindowFontAccented: return Colors.Black;
            }
        }

        return Colors.Red;
    }
}
