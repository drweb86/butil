# Native Android BUtil (Kotlin)

The home screen is the tasks view. Upload folder to server is available from New in the menu. It keeps the desktop options: source folder, skip existing files, upload limit, optional weekday schedule, and the same storage protocols (Directory, SFTP, FTPS, WebDAV, S3, Azure Blob, SMB/CIFS, and NFSv3) with the same connection fields. Task files use the desktop `.v3.json` shape. Secrets are sealed with the Android keystore.

Compress Brotli, Decompress Brotli, Encrypt AES-256, and Decrypt AES-256 are implemented and use the same file formats as the desktop app. AES-256 is the desktop version-1 header (PBKDF2-HMAC-SHA512, AES-CBC, PKCS7). Brotli streams can be read by the desktop app, and streams written by the desktop app can be decompressed here.

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

Third-party components are listed in [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md). The same file is shown from the Third-party notices menu item.
