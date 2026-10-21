[Languages](README.md)

# Privacy policy

Last updated: 21 October 2026

**BUtil** by Siarhei Kuchuk

Application name: BUtil
Developer name: Siarhei Kuchuk

BUtil backs up, synchronizes, and restores files on this device. It can also import media, share a folder over FTPS, or upload files to a server you configure. It does not create a developer account. The developer does not operate a backend that receives your files, passwords, or usage data.

## Data the developer does not collect

The app does not include ads, analytics, crash reporters, or tracking SDKs. The developer does not collect, sell, or share personal data.

This privacy policy and the license are shown inside the app. They are not opened as web pages.

## Data stored on your device

### Tasks and settings

Task definitions are stored only in app-private storage on this device. A task can include folder paths, a schedule, storage settings, and passwords or tokens you type. Passwords and storage secrets are sealed with the Android keystore before they are saved, and can be read only on this device. Those values are not uploaded to the developer. They are not included in Android backup. Uninstalling the app removes them.

The theme you choose is stored in app-private settings on this device.

### Files you choose

Backup, synchronization, restore, import, and upload read and write the folders you select with the system document picker, or the server you configure. Those files stay in the folders you chose, or on the storage destination you configure. The app does not upload them to the developer. Uninstalling the app does not delete files in folders you selected.

### FTPS certificate

When you share a folder over FTPS, the app creates a self-signed certificate in app-private storage. Uninstalling the app removes that certificate. Shared files stay in the folder you granted.

No developer server is used to store your data.

## Network use

### Destinations you configure

When a task runs, the app connects only to the place you set. That can be a folder on this device, or a server you enter: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-compatible storage, or Azure Blob Storage. File names, file contents, and the credentials you entered are sent to that server so the task can run. Each of those services has its own privacy policy. The developer does not receive that traffic.

Sharing a folder over FTPS listens on this device so an FTPS client you configure can read or write the folder you granted, using the username and password you set. That traffic stays between your devices (and anyone on the network who has those credentials). The developer is not a party to it. You are responsible for who can reach the port, which folder you share, and how strong that password is.

### Link you open

The version row in the menu can open the project homepage in the system browser:

[github.com/drweb86/butil](https://github.com/drweb86/butil)

That site has its own privacy policy. The developer does not receive that traffic.

The app does not check for updates and does not contact an update service.

## Schedule

You can run a task when the device starts, or on the days you choose. The app registers that with the system alarm clock. That only starts this app on your device.

## Permissions

The app uses the following permissions only:

- **Internet** and **network state** to reach a server you configure and to share a folder over FTPS.
- **Notifications** and a **foreground service** to keep a running task going with the screen off.
- **Exact alarms** and **run at startup** to start a scheduled task.
- **Wake lock** to reduce sleep during a transfer.

## Children

The app is a backup and file-sync tool. It is not directed at children under 13.

## Third parties

Storage providers you configure process the files and credentials the task sends to them. Opening the project homepage is handled by the system browser and by that site. The developer does not receive that traffic.

## Changes

Updates to this policy are included in the app.

## Contact

Application name: BUtil
Developer name: Siarhei Kuchuk

Questions about this Android app can be sent through the store listing you used to install it, or at [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
