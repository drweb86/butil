using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.Content;
using Avalonia;
using Avalonia.Android;
using Android.Provider;
using Uri = Android.Net.Uri;
using Permission= Android.Content.PM.Permission;
using BUtil.Core;
namespace butil_ui.Android;

[Activity(
    Label = "butil_ui.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>, AndroidHack.IAndroidHackActivity
{
    public MainActivity()
    {
        AndroidHack.Instance = this;
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }

    public bool RequestManageExternalStoragePermission()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
        {
            if (!Environment.IsExternalStorageManager)
            {
                var intent = new Intent(Settings.ActionManageAllFilesAccessPermission);
                // intent.SetData(Uri.Parse($"package:{PackageName}"));
                StartActivity(intent);
                return false; // User must manually grant
            }
            return true;
        }
        else
        {
            // For older versions, use WRITE_EXTERNAL_STORAGE
            var hasWritePermission = ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Granted;
            if (!hasWritePermission)
            {
                RequestPermissions(new[] { Manifest.Permission.WriteExternalStorage }, 101);
                return false;
            }
            return true;
        }
    }
}
