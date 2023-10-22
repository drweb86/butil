using System;
using Avalonia.Data.Converters;
using System.Globalization;
using BUtil.Core.Events;
using butil_ui.ViewModels;
using Avalonia.Data;

namespace butil_ui.Controls
{
    public class ProcessingStatusToSolidColorBrushConverter : IValueConverter
    {
        public static ProcessingStatusToSolidColorBrushConverter Instance = new ProcessingStatusToSolidColorBrushConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return BindingNotification.UnsetValue;

            if (value is ProcessingStatus processingStatusValue)
            {
                return ColorPalette.GetProcessingStatusBrush(processingStatusValue);
            }

            return BindingNotification.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BindingNotification.UnsetValue;
        }
    }
}
