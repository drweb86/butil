﻿using Avalonia.Data;
using Avalonia.Data.Converters;
using BUtil.Core.Events;
using butil_ui.ViewModels;
using System;
using System.Globalization;

namespace butil_ui.Controls;

public class ProcessingStatusToSolidColorBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return BindingNotification.UnsetValue;

        if (value is ProcessingStatus processingStatusValue)
        {
            return ColorPalette.GetProcessingStatusBrush(processingStatusValue);
        }

        return BindingNotification.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingNotification.UnsetValue;
    }
}
