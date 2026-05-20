using System;
using System.IO;
using System.Text;

namespace BUtil.Core.Misc;
internal class StringHelper
{
    public static MemoryStream StringToMemoryStream(string str)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(str);
        MemoryStream stream = new MemoryStream(byteArray);
        stream.Position = 0;
        return stream;
    }

    public static string StringFromMemoryStream(MemoryStream stream)
    {
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public static MemoryStream MemoryStreamFromBase64(string text)
    {
        var stream = new MemoryStream(Convert.FromBase64String(text));
        stream.Position = 0;
        return stream;
    }

    public static string MemoryStreamToBase64(MemoryStream stream)
    {
        return Convert.ToBase64String(stream.ToArray());
    }
}
