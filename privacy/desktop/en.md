[Languages](README.md)

# Privacy policy

Last updated: 25 September 2026

**BUtil** by Siarhei Kuchuk

Application name: BUtil
Developer name: Siarhei Kuchuk

BUtil backs up, synchronizes, and restores files on this computer. It can also import media, share a folder, or upload files to a server you configure. It does not create a developer account. The developer does not operate a backend that receives your files, passwords, or usage data.

## Data the developer does not collect

The app does not include ads, analytics, crash reporters, or tracking SDKs. The developer does not collect, sell, or share personal data.

## Data stored on your computer

### Tasks and settings

Task definitions are stored only on this computer. A task can include folder paths, a schedule, storage settings, and passwords or tokens you type. Passwords and storage secrets are encrypted on this computer before they are saved, and can be read only on this computer. Those values are not uploaded to the developer.

- Windows tasks: `%AppData%\BUtil Backup Tasks`
- Linux tasks: `~/.config/BUtil Backup Tasks`
- Windows settings (including theme and the language last chosen for License or Privacy): `%AppData%\BUtil\Settings\v1`
- Linux settings: `~/.config/BUtil/Settings/v1`
- Windows task state: `%AppData%\BUtil\States`
- Linux task state: `~/.config/BUtil/States`
- Windows import-media state: `%AppData%\BUtil Backup Tasks - States`
- Linux import-media state: `~/.config/BUtil Backup Tasks - States`

### Files you choose

Backup, synchronization, restore, and import read and write the folders you select. Those files stay on this computer, or on the storage destination you configure. The app does not upload them to the developer.

### Logs

Diagnostic logs are written only on this computer:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Those files are not sent anywhere.

No developer server is used to store your data.

## Network use

### Destinations you configure

When a task runs, the app connects only to the place you set. That can be a local folder, or a server you enter: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-compatible storage, or Azure Blob Storage. File names, file contents, and the credentials you entered are sent to that server so the task can run. Each of those services has its own privacy policy. The developer does not receive that traffic.

BUtil Server can listen on this computer so a BUtil client you configure can send files. That traffic stays between the computers you set up.

### Update check

Non-Store builds may request the latest GitHub release:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) receives a normal HTTPS request (IP address, user-agent, time). The developer does not receive that traffic.

Installs from the Microsoft Store do not use this check; the Store delivers updates.

### Links you open

The app can open these pages in your system browser. Those sites have their own privacy policies:

- Project homepage: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Latest release: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- File-pattern help: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Date-format help: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Icon credits: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

The license and this privacy policy are shown inside the app. They are not opened as web pages.

## Schedule

On Windows you can run a task at sign-in or on a weekly schedule. The app registers that with Windows Task Scheduler under a name that starts with `BUtil`. That only launches this app on your computer.

## Children

The app is a backup and file-sync tool. It is not directed at children under 13.

## Third parties

GitHub processes the update-check request and pages you open, as above. The Microsoft Store processes Store installs and updates. Storage providers you configure process the files and credentials the task sends to them. The developer does not receive that traffic.

## Changes

Updates to this policy will be posted in this file in the project repository.

## Contact

Application name: BUtil
Developer name: Siarhei Kuchuk

Questions: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
