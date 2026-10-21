[Languages](README.md)

# Rule for privacy

Last time we update am: 21 October 2026

**BUtil** by Siarhei Kuchuk

Name of the app: BUtil
Name of the person wey develop am: Siarhei Kuchuk

BUtil dey keep copy of files, make dem match, and bring dem back for dis device. E fit still carry media come inside, share one folder through FTPS, or carry files go server wey you don set. E no dey open developer account. The person wey develop am no dey run any backend wey dey collect your files, your passwords, or how you take dey use am.

## Data wey the person wey develop am no dey collect

Dis app no get advert, no get analytics, no get crash reporter, and no get tracking SDK. The person wey develop am no dey collect personal data, no dey sell am, and no dey share am.

Dis privacy rule and the licence dey show inside the app. Dem no dey open like web page.

## Data wey dey stay for your device

### Tasks and settings

How task take be dey stay only for the private storage of the app for dis device. Task fit carry folder path, schedule, storage settings, and passwords or tokens wey you type. Before dem save password and storage secret, dem go seal am with the Android keystore, and na only dis device fit read am. Dem no dey send those values go the person wey develop am. Dem no dey inside Android backup. If you comot the app, dem go comot too.

The theme wey you choose dey stay for the private settings of the app for dis device.

### Files wey you choose

Backup, sync, restore, import, and upload dey read and write the folders wey you select with the system document picker, or the server wey you don set. Those files go still dey for the folders wey you choose, or for the storage place wey you don set. The app no dey carry dem go the person wey develop am. If you comot the app, e no go delete the files wey dey inside the folders wey you select.

### FTPS certificate

When you share folder through FTPS, the app go create certificate wey e sign by itself for the private storage of the app. If you comot the app, that certificate go comot. The files wey you share go still dey for the folder wey you give access.

No server of the person wey develop am dey store your data.

## How e dey use network

### Places wey you set

When task dey run, the app dey connect only to the place wey you set. E fit be folder for dis device, or server wey you enter: FTP, FTPS, SFTP, WebDAV, SMB, NFS, storage wey dey work with S3, or Azure Blob Storage. File name, wetin dey inside the file, and the credentials wey you enter go go that server so the task fit run. Every one of those services get im own privacy rule. The person wey develop am no dey receive that traffic.

If you share folder through FTPS, the app go dey listen for dis device so the FTPS client wey you set fit read or write the folder wey you give, with the username and password wey you set. That traffic go dey between your devices (and anybody for the network wey get those credentials). The person wey develop am no be part of am. Na you dey responsible for who fit reach the port, which folder you share, and how strong that password be.

### Link wey you open

The version line for the menu fit open the project homepage for the system browser:

[github.com/drweb86/butil](https://github.com/drweb86/butil)

That site get im own privacy rule. The person wey develop am no dey receive that traffic.

The app no dey check for update, and e no dey contact any update service.

## Schedule

You fit run task when the device start, or for the days wey you choose. The app go register that one with the system alarm clock. Na only dis app e go start for your device.

## Permissions

The app dey use only these permissions:

- **Internet** and **how network be** to reach server wey you set and to share folder through FTPS.
- **Alerts** and **service wey dey front** to make task wey dey run continue even when screen off.
- **Correct alarms** and **run when e start** to start task wey you don schedule.
- **Lock wey hold am awake** to reduce sleep when transfer dey happen.

## Pikin

The app na tool wey dey back up and sync file. E no dey target pikin wey never reach 13 years.

## Other people

The storage people wey you set dey handle the files and credentials wey the task send go dem. To open the project homepage na the system browser and that site dey do am. The person wey develop am no dey receive that traffic.

## Changes

Any update for dis rule dey inside the app.

## Contact

Name of the app: BUtil
Name of the person wey develop am: Siarhei Kuchuk

Question about dis Android app fit go through the store listing wey you use take install am, or for [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
