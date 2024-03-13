using Avalonia.Media;
using BUtil.Core.Events;
using BUtil.Core.Settings;
using System;
using System.Collections.Concurrent;

namespace butil_ui.ViewModels;

public static class ColorPalette
{

    private static ConcurrentDictionary<SemanticColor, SolidColorBrush> _brushCache = new();

    private static SemanticColor ProcessingStatusToColor(ProcessingStatus status)
    {
        return status switch
        {
            ProcessingStatus.FinishedSuccesfully => SemanticColor.Success,
            ProcessingStatus.FinishedWithErrors => SemanticColor.Error,
            ProcessingStatus.InProgress => SemanticColor.InProgress,
            ProcessingStatus.NotStarted => SemanticColor.Normal,
            _ => throw new NotImplementedException(status.ToString()),
        };
    }

    public static SolidColorBrush GetProcessingStatusBrush(ProcessingStatus status)
    {
        var color = ProcessingStatusToColor(status);
        if (_brushCache.TryGetValue(color, out var brush))
            return brush;

        brush = new SolidColorBrush(GetColor(color));
        _brushCache.AddOrUpdate(color, brush, (a, b) => b);
        return brush;
    }

    public static SolidColorBrush GetBrush(SemanticColor color)
    {
        if (_brushCache.TryGetValue(color, out var brush))
            return brush;

        brush = new SolidColorBrush(GetColor(color));
        _brushCache.AddOrUpdate(color, brush, (a, b) => b);
        return brush;
    }

    private static Color GetColor(SemanticColor color)
    {
        if (ApplicationSettings.Theme == ThemeSetting.DarkValue)
        {
            switch (color)
            {
                case SemanticColor.Normal: return Colors.White;
                case SemanticColor.Success: return Color.FromRgb(42, 130, 67);
                case SemanticColor.Error: return Color.FromRgb(222, 98, 89);
                case SemanticColor.InProgress: return Colors.Yellow;
                case SemanticColor.HeaderBackground: return Color.FromRgb(40, 39, 44);
                case SemanticColor.WindowBackground: return Color.FromRgb(40, 39, 44);
                case SemanticColor.ForegroundWindowFont: return Color.FromRgb(162, 168, 175);
                case SemanticColor.ForegroundWindowFontAccented: return Color.FromRgb(214, 222, 235);
                case SemanticColor.WindowFrontBackground: return Color.FromRgb(28, 27, 32);
            }

        }
        else if (ApplicationSettings.Theme == ThemeSetting.LightValue)
        {
            switch (color)
            {
                case SemanticColor.Normal: return Colors.Black;
                case SemanticColor.Success: return Color.FromRgb(5, 139, 0);
                case SemanticColor.Error: return Color.FromRgb(218, 59, 1);
                case SemanticColor.InProgress: return Color.FromRgb(128, 0, 224);
                case SemanticColor.HeaderBackground: return Color.FromRgb(243, 243, 243);
                case SemanticColor.WindowBackground: return Color.FromRgb(243, 243, 243);
                case SemanticColor.ForegroundWindowFont: return Colors.Black;
                case SemanticColor.ForegroundWindowFontAccented: return Colors.Black;
                case SemanticColor.WindowFrontBackground: return Color.FromRgb(243, 243, 243);
            }
        }

        return Colors.Red;
    }
}
