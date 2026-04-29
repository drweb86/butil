# Import media

Imports audios, photos and videos from an SD card of a camera or recorder, from a phone connected by Wi-Fi (over an FTPS server app) or by Media Transfer Protocol, and from any folder reachable from the host. While copying files, BUtil renames each file according to a configurable date-based template so that all imported media end up in a clean, predictable folder layout (year / month / day / ...).

Features:

- Deduplication: a file is copied to the destination only if no file with the same content (last-write-time + size + SHA-512) already exists in the destination folder or any of its subfolders.
- Configurable date-based file-name template (year, month, day, ...).
- Optional **Skip already imported files** memory, so re-running the task on the same source does not even re-read files it has already imported.
- Optional **Process files newer than** date filter, so only files modified after a given date are imported.
- Source can be a local **Folder**, an **SMB/CIFS** share, an **SFTP** server or an **FTPS** server, including FTPS servers running on a phone over Wi-Fi.

# Scenarios

## Import photos and videos to PC from a camera, or audio from a recorder via SD card

![Import photos and videos to PC from Camera, Import Audio from Recorder SD Card](./Assets/Image%20-%20Import%20media%20task%20-%20Case%201.png)

1. Plug the SD card into a card reader.
2. Launch the task.
3. The task copies all files from the SD card (the configured source folder) to the destination folder. While copying it reads each file's last-write-time and uses it to build the destination path according to the **File name transformations** template.

## Import photos and videos from a camera, or audio from a recorder via Media Transfer Protocol (MTP)

1. Plug the device into the PC.
2. Launch the task.
3. The task copies all files from the device to the destination folder, naming each output file from the source file's last-write-time and the configured template.

Be aware that on Windows the source **Folder** name should match what File Explorer shows for the device, e.g. `External Storage\DCIM` or similar.

## Import photos and videos from a phone via Phone Link on Windows

### 1. Connect your device to Windows using Phone Link

Details: https://www.microsoft.com/en-us/windows/sync-across-your-devices .

Verify that you can see your phone's files in File Explorer.

### 2. Set up the Import Media task

In the **Where?** section (the source):

a. Choose **Folder**.

b. Specify the **DCIM** folder of your phone as it appears in Explorer.

c. Save the task.

### 3. Each time you want to sync over Wi-Fi

a. Open **Phone Link** on the PC.

b. Open File Explorer and navigate to your phone.

c. Click the disconnected indicator in the address bar of Explorer and reconnect.

d. Launch the import task.

Be aware: this Microsoft feature has historically been unstable on Windows 11; you may need to restart the application the first time because of access exceptions.

## Import photos and videos from a phone via Wi-Fi via FTPS

![Import photos and videos from phone via Wi-Fi via FTPS](./Assets/Image%20-%20Import%20media%20task%20-%20Case%202.png)

This is the recommended cross-platform way (Windows and Linux) to bring photos and videos off a phone without cables.

### Set up the FTPS server on the phone

The example below uses the Android **FTP Server** (free) app. Other FTPS server apps work the same way.

#### Install the **FTP Server** application on the phone

https://play.google.com/store/apps/details?id=net.xnano.android.ftpserver&hl=en_US

#### Open the application

The main screen looks like this:

![FTP Server application](./Assets/Image%20-%20FTP%20Server%20-%20After%20Install.png)

#### Switch encryption to **FTPS Implicit** or **FTPS Explicit**

The example below uses **FTPS Implicit**.

![FTP Server - Change encryption mode](./Assets/Image%20-%20FTP%20Server%20-%20Change%20encryption%20mode.png)

#### Add an FTPS user with read-only access to DCIM

a. Tap **Users**, then **Add**.

![FTP Server - Add User 1](./Assets/Image%20-%20FTP%20Server%20-%20Add%20User%201.png)

b. Create the user. The example below uses user **sync** with password **123**.

![FTP Server - Add User 2](./Assets/Image%20-%20FTP%20Server%20-%20Add%20User%202.png)

a. Specify anything as **Full Name**.

b. Specify **sync** as **Username** (1).

c. Specify **123** as **Password** (2).

d. Delete all paths under **Path**.

e. Click **Add new path** (3) and select the **DCIM** folder.

The **Path** section should now show a single folder, e.g. **/storage/emulated/0/DCIM** (4).

f. Disable **Writable** (5) - the import task only needs read access on the phone.

g. Confirm the new user.

#### Make sure the phone is on the same Wi-Fi network as the PC

#### Start the FTPS server

a. Tap **Home**.

b. Tap **Start**.

The screen will show the IP address and port.

![FTP Server - IP and Port](./Assets/Image%20-%20FTP%20Server%20-%20IP%20and%20Port.png)

The example below uses server `172.16.92.15` on port `2121`.

### Set up the BUtil Import Media task pointed at the FTPS server

In the **Where?** section (the source):

a. Choose **FTPS**.

b. **Server**: `172.16.92.15`

c. **Encryption SSL/TLS**: `Implicit`

d. **Port**: `2121`

e. **User**: `sync`

f. **Password**: `123`

g. The first time you save, BUtil will detect the server's certificate and present its details. Verify the certificate identity out-of-band, then click **Save** again to store the trusted fingerprint.

#### Save the task

#### On the phone, tap **Stop** to stop the FTPS server until next time

### Each time you want to sync over Wi-Fi

a. Open **FTP Server** on the phone and tap **Start**.

b. Launch the import task in BUtil.

c. Open **FTP Server** on the phone and tap **Stop**.

# Settings

Settings are configured in the **Where?** section of the import-media task editor.

## Output folder

The folder on the local machine where imported files end up. Subfolders inside it are created according to the **File name transformations** template. The default value is platform-specific (typically `Pictures\Camera Roll` on Windows and `~/Pictures` on Linux).

## Skip already imported files

When this setting is **on**, the application records imported file names. On the next run, files with names already imported are skipped without being read. This is the fast path - useful when the source is a phone over Wi-Fi or an SD card with thousands of files.

When this setting is **off**, the application always reads every source file and decides whether to copy it based on deduplication.

In both cases, when a file does pass the skip check, BUtil writes it to the destination only if no file with the same content (last-write-time and SHA-512) already exists in the destination folder or any of its subfolders. Deduplication is independent of this setting.

## Process files newer than

Optional date filter. When set, only source files whose last-write-time is **on or after** the configured date are imported. Older files are skipped. Useful for incremental imports - for example, to import only the photos taken since your last vacation.

## File name transformations

A template that converts each source file's last-write-time into a destination path relative to the **Output folder**. Use the **◄** / **►** buttons to cycle through prepared examples (year/month/day, year-month-day, etc.). The current template is shown live with a sample output below the field.

Supported tokens:

- `{DATE:Format}` - inserts the file's modification date in the given .NET date format (for example `{DATE:yyyy}`, `{DATE:yyyy-MM-dd}`, `{DATE:HH-mm-ss}`).

# Ideas behind the implementation

## Why does the task copy files instead of moving them?

To reduce the risk of accidentally removing files when the user enters wrong settings or the application has a bug. Copying is safer; the source is never modified.
