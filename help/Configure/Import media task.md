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

### Setup import photos and videos from phone via Wi-Fi via FTPS

Setup will be demonstrated on application **FTPS Server**. It can be done in similar way for other apps.

#### Install **FTPS Server** application on your phone.

https://play.google.com/store/apps/details?id=net.xnano.android.ftpserver&hl=en_US

#### Open application

Its window will look like this

![FTP Server application](./Image%20-%20FTP%20Server%20-%20After%20Install.png)

#### Change encryption mode to **FTPS Implicit** or **FTPS Explicit** as it is shown on a picture below.

In example we will go next with FTPS Implicit.

![FTP Server - Change encryption mode](./Image%20-%20FTP%20Server%20-%20Change%20encryption%20mode.png)

#### Add FTPS user with readonly access to DCIM

a. Tap Users, click Add

![FTP Server - Add User 1](./Image%20-%20FTP%20Server%20-%20Add%20User%201.png)

b. Create user.

In example we will go next with user sync with password 123 .

![FTP Server - Add User 2.png](./Image%20-%20FTP%20Server%20-%20Add%20User%202.png)

a. Specify anything for as **Full Name**.

b. Specify **sync** as **Username** (1).

c. Specify **123** as **Password** (2).

d. Delete all paths in **Path** section.

e. Click **Add new path** (3) and select **DCIM** folder.

You will see in **Path** section you should see single folder **/storage/emulated/0/DCIM** (4) .

f. Disable **Writable** (5).

g. Confirm new user creation.

#### Make sure your phone is connected to WI-FI, the same one as your PC.

#### Launch FTPS Server.

a. Click Home

b. Click Start.

You will see IP and Port addresses.

![FTP Server - IP and Port.png](./Image%20-%20FTP%20Server%20-%20IP%20and%20Port.png)

In example above we will go next with server 172.16.92.15 and 2121 port.

#### Setup BUtil to import multimedia task in What? section from this FTPS server.

a. Choose **FTPS**

b. Specify **Server**: 172.16.92.15

c. Specify **Encryption SSL/TLS**: Implicit

d. Specify **Port**: 2121

e. Specify **User**: sync

f. Specify **Password**: 123

#### Save task.

#### On mobile phone tap **Stop** server.

### Each time you will need sync over wi-fi

a. Open **FTP Server** at phone and tap **Start**;

b. Launch import task;

c. Open **FTP Server** at phone and tap **Stop**.

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