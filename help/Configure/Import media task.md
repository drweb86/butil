Imports audios, photos, videos from SD Card of camera, recorder; photos and videos from your phone via WI-FI through FTPS Server application using template file names.

Features includes:
- Deduplication of files;
- Templated file names.

# Scenarios

## Import photos and videos to PC from Camera, Import Audio from Recorder SD Card

![Import photos and videos to PC from Camera, Import Audio from Recorder SD Card](./Image%20-%20Import%20media%20task%20-%20Case%201.png)

1. Plug SD card into card reader.
2. Launch job.
3. Job will copy all files from SD card (specified folder) to destination folder. During copying of files, it will take last write time and converts it to path of saved file at destination directory.

## Import photos and videos from phone via Wi-Fi via FTPS

![Import photos and videos from phone via Wi-Fi via FTPS](./Image%20-%20Import%20media%20task%20-%20Case%202.png)

### Setup copy photos and videos from phone via Wi-Fi via FTPS
1. Install **FTPS Server** application on your phone.
https://play.google.com/store/apps/details?id=net.xnano.android.ftpserver&hl=en_US

2. Setup FTPS.
FTP is not supported, because it has no encryption and main use case is to import media via WI-FI.

3. Add user with read access to folder DCIM. As a root (/) folder, specify folder DCIM for the user. 

4. Now launch FTP server at your phone.
It will display IP address and port. You need them to configure FTPS.

5. Open BUTIL console CLI and create "Import media task", specify FTPS.

### Use case import photos and videos from phone via Wi-Fi via FTPS
1. Open phone and start FTP Server application.

2. Launch import media task.

3. Job will copy all files from phone FTPS to destination folder. During moving of files, it will take last write time and converts it to path of saved file at destination directory.

# Settings

## Skip already imported files
When this setting is on, application will track imported file names. During next import, already imported files (by name) will be skipped.
When this setting is off, application will always try to copy all files from external media.

When file is retrieved, application will save to destination folder only in case it is missing in it (by last write time and SHA512 are compared) or its subfolders disregarding this setting (deduplication does not depend if this setting is on or off).

# Ideas behind implementation

## Why application copies files, not moves it?

It is done to reduce risk of accidental removal of files when user inputted invalid data or application have issue. Copying is safer.