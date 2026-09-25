[Languages](README.md)

# Privacy policy

Last update: 25 September 2026

**BUtil** by Siarhei Kuchuk

App name: BUtil
Developer name: Siarhei Kuchuk

BUtil dey back up, sync, and restore files for dis computer. E fit also import media, share folder, or upload files go server wey you set. E no dey create developer account. Di developer no dey run server wey dey receive your files, passwords, or how you dey use am.

## Data wey di developer no dey collect

Di app no get ads, analytics, crash report, or tracking SDK. Di developer no dey collect, sell, or share personal data.

## Data wey dey stay for your computer

### Tasks and settings

Task definitions dey stay only for dis computer. Task fit get folder paths, schedule, storage settings, and passwords or tokens wey you type. Passwords and storage secrets dey encrypt for dis computer before dem save, and na only dis computer fit read dem. Dem no dey upload dose values give di developer.

- Windows tasks: `%AppData%\BUtil Backup Tasks`
- Linux tasks: `~/.config/BUtil Backup Tasks`
- Windows settings (including theme and di language wey you last choose for License or Privacy): `%AppData%\BUtil\Settings\v1`
- Linux settings: `~/.config/BUtil/Settings/v1`
- Windows task state: `%AppData%\BUtil\States`
- Linux task state: `~/.config/BUtil/States`
- Windows import-media state: `%AppData%\BUtil Backup Tasks - States`
- Linux import-media state: `~/.config/BUtil Backup Tasks - States`

### Files wey you choose

Backup, sync, restore, and import dey read and write di folders wey you select. Dose files dey stay for dis computer, or for di storage wey you set. Di app no dey upload dem give di developer.

### Logs

Diagnostic logs dey write only for dis computer:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Dose files no dey go anywhere.

No developer server dey store your data.

## Network use

### Places wey you set

When task dey run, di app dey connect only to di place wey you set. E fit be local folder, or server wey you enter: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-compatible storage, or Azure Blob Storage. File names, file contents, and di credentials wey you enter dey go dat server so di task fit run. Each of dose services get e own privacy policy. Di developer no dey receive dat traffic.

BUtil Server fit listen for dis computer so BUtil client wey you set fit send files. Dat traffic dey stay between di computers wey you set up.

### Update check

Builds wey no be Store fit request di latest GitHub release:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) dey receive normal HTTPS request (IP address, user-agent, time). Di developer no dey receive dat traffic.

Install from Microsoft Store no dey use dis check; na Store dey deliver updates.

### Links wey you open

Di app fit open dis pages for your system browser. Dose sites get dia own privacy policies:

- Project homepage: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Latest release: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- File-pattern help: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Date-format help: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Icon credits: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Di license and dis privacy policy dey show inside di app. Dem no dey open as web pages.

## Schedule

For Windows you fit run task when you sign in, or on weekly schedule. Di app dey register am with Windows Task Scheduler under name wey start with `BUtil`. Dat one na only to launch dis app for your computer.

## Children

Di app na backup and file-sync tool. E no dey for children wey never reach 13 years.

## Other people

GitHub dey process di update-check request and pages wey you open, as e dey above. Microsoft Store dey process Store installs and updates. Storage providers wey you set dey process di files and credentials wey di task send give dem. Di developer no dey receive dat traffic.

## Changes

Updates to dis policy go dey for dis file inside di project repository.

## Contact

App name: BUtil
Developer name: Siarhei Kuchuk

Questions: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
