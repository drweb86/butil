namespace BUtil.Core.Compression
{
    public static class ArchiverUtil
    {
        public static int GetCompressionLevel(string extension)
        {
            return extension switch
            {
                ".ico" or ".jpg" or ".png" => 0,
                ".mp4" or ".mov" or ".avi" or ".flv" or ".vob" or ".mkv" => 0,
                ".mp3" or ".m4a" or ".ogg" => 0,
                ".rar" or ".7z" or ".zip" => 0,
                ".docx" or ".pptx" or ".xlsx" => 0,
                ".chm" or ".pdf" or ".epub" => 0,
                ".raf" => 5,
                _ => 9,
            };
        }
    }
}
