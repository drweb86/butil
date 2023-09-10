This task allows you to move photos from your camera and phone to place at your PC with expanding of date strings.

# Scenarios

## Move photos and videos to PC from Camera

1. Plug SD card into card reader.
2. Launch job.
3. Job will move all files from SD card (specified folder) to destination folder. During moving of files, it will take last write time and converts it to path of saved file at destination directory.

## Move photos and videos from phone via Wi-Fi via FTPS

### Setup move photos and videos from phone via Wi-Fi via FTPS
1. Install **FTPS Server** application on your phone.
https://play.google.com/store/apps/details?id=net.xnano.android.ftpserver&hl=en_US

2. Setup FTPS.
FTP is not supported, because it has no encryption and main use case is to move folders via WI-FI.

3. Add user with write access to folder DCIM. As a root (/) folder, specify folder DCIM for the user (it's important, since utility will try to move all files).

4. Now launch FTP server at your phone.
It will display IP address and port. You need them to configure FTPS.

5. Open BUTIL console CLI and create "Move photo, video task", specify FTPS.

### Use case move photos and videos from phone via Wi-Fi via FTPS
1. Open phone and start FTP Server application.

2. Launch move photo, video task.

3. Job will move all files from phone FTPS to destination folder. During moving of files, it will take last write time and converts it to path of saved file at destination directory.
