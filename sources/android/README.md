# Native Android BUtil (Kotlin)

The home screen is the tasks view. New can create the desktop tasks: incremental backup, import media, synchronization, share folder over FTPS, and upload folder to server. Restore reads an incremental backup version from storage. Each task keeps the desktop options and the desktop `.v3.json` shape. Incremental backup and synchronization use the desktop state file (`State.brotli.aes256v1`): file contents are AES-256 version 1, and the state is Brotli then AES-256. Secrets are sealed with the Android keystore.

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

The debug APK is `Output/butil_android_debug.apk`. A release build reads the version from `CHANGELOG.md` and writes `Output/butil_<version>_android.apk`. The release workflow also writes `Output/butil_<version>_android.aab` and a separate China bundle, `Output/butil_<version>_android_china.aab`.

Set `ANDROID_KEYSTORE_FILE`, `ANDROID_KEYSTORE_PASSWORD`, `ANDROID_KEY_ALIAS`, and `ANDROID_KEY_PASSWORD` before `-Release` to sign. Without them the release package is unsigned.

Privacy policies for this app are in [privacy/android](../../privacy/android/README.md). License texts are in [licenses](../../licenses/README.md). Both are copied into the APK. The China edition also appends [licenses/china-appendix](../../licenses/china-appendix/README.md). That edition asks for the privacy policy and then the license before the app can be used. Disagreeing, or withdrawing agreement later, deletes tasks, settings, and passwords stored by the app and closes it. Files in folders you selected are not deleted.

Third-party components are listed in [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md). That file is English only and is not translated. The same file is shown from the Third-party notices menu item.

The drawer title `BUtil - Siarhei Kuchuk` is not translated.
