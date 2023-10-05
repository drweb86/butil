using Avalonia.Media;
using BUtil.Core.Settings;

namespace butil_ui.ViewModels;

public static class ColorPalette
{
    public static Color GetForeground(string theme, SemanticColor color)
    {
        if (theme == ThemeSetting.DarkValue)
        {
            switch (color)
            {
                case SemanticColor.Normal: return Colors.White;
                case SemanticColor.Success: return Color.FromRgb(147, 199, 93);
                case SemanticColor.Error: return Color.FromRgb(222, 98, 89);
                case SemanticColor.InProgress: return Colors.Yellow;
            }

        } 
        else if (theme == ThemeSetting.LightValue)
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
