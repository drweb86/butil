# Native Android BUtil (Kotlin)

Empty stub. The screen shows the application name. Backup, sync, and restore are not implemented here yet.

Application id: `com.siarheikuchuk.butil`

## Requirements

- minSdk 23 (Android 6.0)
- compileSdk / targetSdk 36
- JDK 21 (bytecode target stays 17)

## Local build

From the repository root:

```powershell
./build-android.ps1
./build-android.ps1 -Install
./build-android.ps1 -Release
```

The debug APK is `Output/butil_android_debug.apk`. A release build reads the version from `CHANGELOG.md` and writes `Output/butil_<version>_android.apk`.

Set `ANDROID_KEYSTORE_FILE`, `ANDROID_KEYSTORE_PASSWORD`, `ANDROID_KEY_ALIAS`, and `ANDROID_KEY_PASSWORD` before `-Release` to sign. Without them the release package is unsigned.

Third-party components are listed in [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).
