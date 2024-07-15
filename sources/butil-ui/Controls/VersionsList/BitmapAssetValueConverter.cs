using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using System.Reflection;

namespace butil_ui.Controls;

public class BitmapAssetValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        if (value is string rawUri)
        {
            Uri uri;

            // Allow for assembly overrides
            if (rawUri.StartsWith("avares://"))
            {
                uri = new Uri(rawUri);
            }
            else
            {
                string assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
                uri = new Uri($"avares://{assemblyName}{rawUri}");
            }

            var asset = AssetLoader.Open(uri);

            return new Bitmap(asset);
        }

        throw new NotSupportedException();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
