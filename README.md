# ![BUtil Logotype](./help/Assets/Logotype.gif) BUtil

> Backup/synchronize your data Windows, Linux.

> Import photos from phone via WI-FI and more.

[![Image Gallery](./help/Assets/Image%20Gallery.gif)](./help/Screenshots.md)

[Screenshots](./help/Screenshots.md)

<details>
<summary>🌐 Supported languages (70+)</summary>

Afrikaans, Albanian, Arabic, Armenian, Asturian, Basque, Belarusian,
Bengali, Bosnian, Breton, Bulgarian, Catalan, Chinese, Croatian, Czech,
Danish, Dutch, Efik, Esperanto, Estonian, Farsi, Finnish, French,
Galician, Georgian, German, Greek, Hebrew, Hindi, Hungarian, Icelandic,
Indonesian, Irish, Italian, Japanese, Korean, Kurdish, Latvian,
Lithuanian, Luxembourgish, Macedonian, Malay, Marathi, Mongolian,
Nigerian Pidgin, Norwegian, Pashto, Polish, Portuguese, Romanian,
Russian, Scottish Gaelic, Serbian, Slovak, Slovenian, Spanish, Swedish,
Tamil, Tatar, Telugu, Thai, Traditional Chinese, Turkish, Ukrainian,
Urdu, Uzbek, Vietnamese, Welsh, Yue Chinese, English

</details>

## Features

- **Incremental backup** with deduplication of files and **SFTP**, **FTPS**, **SMB/CIFS**, **[WebDAV](./help/WebDAV%20storage%20setup.md)** (Nextcloud, ownCloud, Synology, Yandex Disk, …), **[S3-compatible](./help/S3%20storage%20setup.md)** (AWS S3, Backblaze B2, Wasabi, Cloudflare R2, DigitalOcean Spaces, Linode/Akamai, Scaleway, Google Cloud Storage, Yandex Object Storage, VK Cloud, Cloud.ru, Selectel, Alibaba OSS, Tencent COS, Huawei OBS, MinIO, …), **[Azure Blob Storage](./help/Azure%20Blob%20Storage%20setup.md)**, **[NFS](./help/NFS%20storage%20setup.md)** transports support;
- **[Synchronization](./help/Synchronization.md)** of files between devices using centralized storage and versioning support;
- **Import media** - [Import audios, photos, videos from SD Card of camera, recorder; photos and videos from your phone via WI-FI through FTPS Server application using template file names or via MTP](./help/Import%20media%20task.md);
- **P2P file transfer** via FTPS Server;
- **Console** for automation [scheduling](./help/Command%20line.md);
- **AES-256** encryption.

## Requirements

**Windows 11 x64, ARM64** or [**Ubuntu 24+**](./help/Ubuntu.md).

## 📦 Installation Options

<details>

<summary>📦 Installation for Windows</summary>

A. [Microsoft Store](https://apps.microsoft.com/detail/9nwsw92p3x1c)

Best option. Store will keep application up to date.

B. WinGet

```
winget install --id SiarheiKuchuk.BUtil
```

C. Setup [look for asset **windows_setup.exe**](https://github.com/drweb86/butil/releases/latest)

Setup is good when you can't use Store (no Microsoft Account, etc). Application will check for self-updates however you should manually update the application.

D. Binaries [look for windows_archive.7z](https://github.com/drweb86/butil/releases/latest)

Binaries are good if setups and zip archives are blocked by corporate policies. Application will check for self-updates however you should manually update the application.

</details>

<details>

<summary>📦 Installation for Linux</summary>

A. Installation via APT Repository

Best option. System will keep application updated.

One-Time Setup - add repository

```
curl -fsSL https://drweb86.github.io/butil/gpg-key.pub | sudo gpg --dearmor -o /usr/share/keyrings/butil.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/butil.gpg] https://drweb86.github.io/butil stable main" | sudo tee /etc/apt/sources.list.d/butil.list > /dev/null
```

Install

```
sudo apt update && sudo apt install butil
```

Update

```
sudo apt update && sudo apt upgrade butil
```

Uninstall

```
sudo apt remove butil
sudo rm /etc/apt/sources.list.d/butil.list /usr/share/keyrings/butil.gpg
```

B. DEB [look for asset linux_arm64.deb and linux_amd64.deb](https://github.com/drweb86/butil/releases/latest)

For amd64:

```
sudo dpkg -i butil_*_linux_amd64.deb
sudo apt-get install -f
```

For ARM64:

```
sudo dpkg -i butil_*_linux_arm64.deb
sudo apt-get install -f
```

Uninstall

```
sudo apt remove butil
```

C. Bash script

Installation:

`wget -O - https://raw.githubusercontent.com/drweb86/butil/master/sources/ubuntu-install.sh | bash`

Installation of preview:

`wget -O - https://raw.githubusercontent.com/drweb86/butil/master/sources/ubuntu-install.sh | bash -s -- --latest`

Uninstallation (source install only):

`wget -O - https://raw.githubusercontent.com/drweb86/butil/master/sources/ubuntu-uninstall.sh | bash`

After installation (APT or .deb), the following commands are available:

- **`butil-ui`** — graphical user interface
- **`butilc`** — console tool for automation and scheduling

</details>

## [Documentation](https://github.com/drweb86/butil/tree/master/help)

- [Changelog](./CHANGELOG.md)
