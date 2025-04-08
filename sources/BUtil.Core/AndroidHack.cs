namespace BUtil.Core;

public class AndroidHack
{
    public interface IAndroidHackActivity
    {
        bool RequestManageExternalStoragePermission();
    }

    public static IAndroidHackActivity Instance { get; set; } = null!;
}
