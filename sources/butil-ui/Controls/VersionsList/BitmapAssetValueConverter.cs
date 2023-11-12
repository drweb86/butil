using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using Avalonia.Data.Converters;
using System.Globalization;
using System.Reflection;

namespace butil_ui.Controls
{
    public class BitmapAssetValueConverter : IValueConverter
    {
        public static BitmapAssetValueConverter Instance = new BitmapAssetValueConverter();

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            if (value == null)
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.

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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    uri = new Uri($"avares://{assemblyName}{rawUri}");
                }

                var asset = AssetLoader.Open(uri);

                return new Bitmap(asset);
            }

            throw new NotSupportedException();
        }

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            throw new NotSupportedException();
        }
    }
}
