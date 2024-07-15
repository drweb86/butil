﻿using System;

namespace butil_ui.Controls;

static class SizeHelper
{
    public static string BytesToString(long byteCount)
    {
        string[] suf = [ "B", "KB", "MB", "GB", "TB", "PB", "EB" ]; //Longs run out around EB
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }
}
