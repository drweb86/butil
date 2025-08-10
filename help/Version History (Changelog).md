# 2025.08.07
(unreleased)

## New Features
- SFTP support.
SFTP was added because its easy to setup its on cheap Ubuntu VPS servers that can be rented online. Since credentials are stored unprotected, it is expected that you create single purpose user for each task.

## Changes
- Some libraries were updated.
 
# 2025.07.21

## Changes
- Some libraries were updated.
- Italian language was updated by https://github.com/bovirus .
- Task name is trimmed on UI. It was possible to have tasks with spaces in the end, now UI will trim it.

## New Features
- New tasks were added to copy folder across network without server with AES-256 protection, skipping unmodified files, overwriting modified files, not touching files at server that exists on client machine. Primary purpose is to have one way sync between local PC and VM with Ubuntu.

## Bug Fixes
- Command line for Linux was fixed.

# 2025.05.31

## Bug Fixes
- Hashes cache simultaneous read was crashing application.

## Changes
- Some libraries were updated.
- After task execution Close button returns to main view.

## Removed Features
- 7-zip is dropped due to no possibility to use it in Android.
- 7-zip encrypted files are no longer recognized. You need to use v2024.12.06 to migrate backups or restore if you did not do it already. Version 2024.12.06 will be retained to the end of the year.
- MTP is dropped because of no usage.

# 2024.12.16

## Bug Fixes
- .Net 9 broken ConcurrentBag component was removed. v.2024.11.27 was crashing.

## Changes
- Some libraries were updated.
- Reduced logging for synchronization task.
- Restoration is done without spawning new process (cmd line argument is removed).
- Task execution is done without spawning new process (prepare for Android version).
- Thanks to Microsoft it is possible using Windows PC to Import photos and videos from phone, Synchronize from and to phone using Windows PC, Backup phone data using Windows PC
Related article 'Import media task' was updated with how to do it on example of import photos. No more needed to have dedicated FTPS application on the phone.

## Removed Features
- It is not possible to open via drag-n-drop previous storage format index file in restoration mode.

# 2024.11.27

## New Features
- Added Turkish, Tamil, Yue Chinese, Vietnamese languages.

## Removed Features
- For incremental backup it won't be possible to specify Files (Add Files...) anymore due to low usage, only folders.

## Changes
- Italian language was updated by https://github.com/bovirus .
- AvaloniaUI was updated to 11.2.2.
- Upgrade to .Net 9.

# 2024.10.25

## Changes

- Old task format tasks are automigrated during load to the latest format.
After half a year old format version support will be dropped. Automigrate of old all tasks happens on load of UI apploication and on executing any task.
- Old storage format will be upgraded to the latest. Format upgrade is repacking of 7-zip files in the storage.
During upgrade Storage Quota setting will be disregarded.
Upgrade is done by chunks by 50 files. It might take some time.
During uppgrade you can stop it at any moment but except during state upload (which might damage the backup index).
Upgrade is started on any task execution.
Old storage format will be dropped after half a year. You will need to use previous app version to open it.
If you have huge backup on slow remote NAS, consider recreating backup from scratch.
- AvaloniaUI was updated to 11.1.4

## Removed Features

- Synchroniпzation mode subdirectory parameter is removed.
It is removed because in secure scenarios you need to create a separate account and separate storage for such tasks.
If you were using this parameter - edit and save sync task given that this param will be empty.

## New Features

- Possibility to specify minimum last write time for import of pictures.
Supported case: user moved to new PC and needs to import files he created after some point in the time.

## Bug Fixes

- SHA 512 hashes were not calculated.
! Hashes cache was inproperly calculated on adding new files.
- If hashes cache had incorrect JSON, application failed to start task.

# 2024.09.29

## New Features

- Bengali, Urdu, Indonesian, Japanese, Nigerian Pidgin, Marathi, Telugu languages.
- It is possible to copy text from message.
- On task deletion log files are deleted as well.

## Changes

- 7-zip update to 24.08
- 7-zip is not used anymore to compress/encrypt files because of 7% average space profit (each file is compressed individually) with drawbacks of not supporting Android devices
- Update libraries
- Unit-tests are part of build.ps1 script execution
- Storage format upgrade. Previous versions of applications won't be able to open it. Backward compatibility for restoration etc will be preserved for some time.
- Checking for updates was switched back to github since upload was fixed, personal site at NAS will be shutdown after some time.
- Messages for questions was improved.

## Bug Fixes

- Message buttons locations was invalid
- Message text locations was invalid
! Incremental backup creates version when duplicate files exists
- Task status was missing on rename
- Import multimedia with skip already imported flag was not working correctly during rename of task
 
# 2024.08.08

## Bug Fixes

- Scheduler failed to launch when task name had spaces.
Manual actions are needed. Open task in editor and click save one by one. Scheduler tasks will be recreated.

# 2024.08.02

## Bug Fixes

- 7-zip in Windows installation was missing.

# 2024.07.29

## Changes

- Reduced storage validation errors message size.

## Bug Fixes

- FTPS validation could prevent creation of task.

# 2024.07.16

## Changes

- Skipped tasks are visible.
- Storage verification was improved.
- Windows: New synchronization tasks will have by default midnight execution schedule.
- Code base was upgraded to language c# latest version.

## Bug Fixes

- Ubuntu: Gnome: Wayland: application icon is properly resolved thanks to guys from Gnome and Avalonia Teams.
- FTPS default mode was missing.
- Attempt to fix empty progress log on scrolling.
- Synchronization: Message about quota reach limit was missing.
- Failover errors were treated as task errors.

# 2024.07.06

## Bug Fixes
- Ubuntu: synchronization was building incorrect relative paths.
- Ubuntu 24: handle devices added as files to user profile.
- Ubuntu: more clear help about globbing.

# 2024.06.29.8

## New Features
- Ubuntu: installation script installs and uninstall application to proper Linux locations. Desktop and quick search shortcuts are created.

## Changes
- Dialogs are cross-platform now.

## Bug Fixes
- Ubuntu 24: spawn restoration, backup from UI crashes (they broke system-inhibit).
- Ubuntu 24: backup of userprofile folder crashes app (they put bus devices there).
- Ubuntu 24: 7-zip resolved name is different (its not Ubuntu 23 compatible anymore).
- Ubuntu 24: Default font is changed and it breaks menu (they broke font).
- Ubuntu 24: In new OS version fonts are not propagated to child windows which causes squares instead of localized text (they broke child dialogs in snaps and now its broken in for non-snapped apps).

# 2024.06.29

## New Features
- Synchronization of files between devices (ALPHA).
- Added Ukrainian language by https://github.com/Kopejkin .
- Added Arabic, Chinese, German, Hindi, French, Portuguese, Spanish languages by https://ask.chadgpt.ru/ .

## Changes
- Italian language was updated by https://github.com/bovirus .
- Libraries updates.
- Igor Pavlov's 7-zip version update to 24.07.

## Bug Fixes
- Errors for FTPS were not logged.
- Wrong task name fix.
- FTPS backup with mass amount of files was failing.

# 2024.04.14.1

## Bug Fixes
- Offline mode could crash app.

# 2024.04.14

## Changes
- Change version lookup URL.
- Italian language was updated by https://github.com/bovirus .

# 2024.04.05

## New Features
- During recovery or version deletion, real progress is shown
- During recovery or version deletion, logs are written.
- Recovery speed should be increased, but PC might be a little bit unresponsive during process.
- If user mounted network SMB/CIFS share on Windows, then he may skip for CIFS storage credentials. In this case invoking 'net use' command will be skipped.
Main idea behind change is to avoid already mounted error. And use it recovery.
- Restoration speed was increased in 2 times for Windows.

## Bug Fixes
- FTPS MITM attacks. Be aware that if you had previously setupped FTPS with self-signed certificate, you need to do the following steps:
a. Open task with FTPS in Edit mode,
b. Click Save.

# 2024.03.28

## Bug Fixes
- 7-zip binaries were missing with setup due to lack of internet during build.

# 2024.03.16

## New Features
- Windows support for local accounts (setup was not able to install 7-zip via win-get, because Microsoft does not support installation for users not authorized in Store).
- Windows binaries are ready to launch, no need to install 7-zip separately.
- Update to .Net 8.
- Ability to delete previous versions (can be done from restoration view). With this feature incremental backup becomes self-sufficient when storage quota is exceeded.

## Removed Features
- SNAP support is dropped (.Net does not support confined environment properly, Snap does not support .Net, not possible to get proper access to files, Avalonia does not support snap properly)

# 2024.01.10

## New Features
- Windows ARM64 support.

# 2024.01.01

## Changes
- Deduplication of files was improved, last write time is not taken into account anymore.

## Removed Features
- Windows: x86 support.

# 2023.12.14

## New Features
- Log format was changed to text, because in Linux browser cannot open html files located outside of Download folder. And this folder is not a good choice for logs. Since statuses of backups are calculated based on logs location, it will be undefined until next backup.
- Video, Music, Photo libraries are added to new backup tasks by default.
- Windows, binaries: binaries are self-contained, no more need to install .Net separately.
- Icons reference for task names.
- During restoration already good destination files (by size and checksum) are not overwritten to reduce traffic usage for remote storages.
- Ubuntu snap publish package as delivery

## Changes
- Ubuntu logs were relocated to ```~/.config/BUtil/Logs/v3``` .
- Ubuntu Powershell is replaced with bash, because snap does not allow to launch powershell.

## Bug Fixes
- Ubuntu: Microsoft broke full path support in .Net 8. It affected launch of tasks and restore app.
- Ubuntu .Net 7: invalid folders for default backup task.
- Avalonia UI: In latest update AutoComplete for template selector does not work. It was replaced with Text box.

## Removed Features
- Downgrade to .Net 7. Ubuntu build tool snapcraft does not support .Net 8 unfortunately.

# 2023.11.18

## New Features
- Upgrade to .Net 8.
- Linux Power PC feature support.
- Linux sleep prevention during task/restore support.
- Linux powershell mount/unmount script support.
- Linux Window header.
- Linux SAMBA transport.

## Changes
- Linux console commands.

## Bug Fixes
- Linux Logs location was fixed.
- Linux set process priority fails sometimes.
- Integrity verification scripts task error crashed application.

# 2023.11.12

## New Features
- Ubuntu support preview (only Folder, FTPS transports for now, no poweroff switches, no pwsh support).
Use binaries archive. Instructions are inside it. 

## Known Issues
- On Ubuntu wrong folder for documents folder is returned. This will be fixed with upgrade to .Net 8 (see https://learn.microsoft.com/en-us/dotnet/core/compatibility/8.0 ).

# 2023.11.1

## New Features
- Import multimedia: popular file name template examples were added and available as dropdown autocomplete.

## Changes
- Italian language was updated by https://github.com/bovirus .
- Launch button grabs less attention.
- During backup task edit, its no longer possible to change password. Previously it could lead to impossibility to perform backup.

# 2023.10.22

## Changes
- UI was freezing on fast computers on backup of huge amount of files in launch app.

## Bug Fixes
! FTPS file delete was terminating the Launch app.
- Null reference exception on attempt to store file with invalid state.

# 2023.10.21

## New Features
- Restore app was upgraded to AvaloniaUI. Migration of product is completed from WinForms to Avalonia. Now its theoretically possible to add port layers for Ubuntu and Android.

## Bug Fixes
- Restore app was not launching.

# 2023.10.16

## New Features
- Cards border color matches task status.

## Changes
- Italian language was updated by https://github.com/bovirus .
- Unhandled exceptions reporting added to UIv2.
- Reduce space for each cards on Tasks View.
- Reduce amount of buttons on card.

## Bug Fixes
- Scroll viewer for task launch.
- Removed selection for task cards.

# 2023.10.15

## New Features
- Tasks app was upgraded to AvaloniaUI.

## Changes
- Italian language was updated by https://github.com/bovirus .
- Help became simpler.
- Cross-platform compatibility 2nd phase: all windows-related functional was moved to separate assembly. From now its possible to create platform-specific experiences in theory.
- Update check became less intrusive.
- Restart app on theme change. Theme selector was moved to Tasks app.

## Bug Fixes
- Workaround of github bug with public release API was added. As a result of this wrong latest release info was shown.
- Validation of FTPS storage with empty folder was fixed.

## Removed Features
- Password hiding and password confirmation since of no usage.
- Drag and drop of files into What? section due to poor support from AvaloniaUI.
- Double click to open file or folder in What? section.
- Expand what on incremental backup edit.

## Known Issues
- Launch task has issue with scrollbar.

# 2023.10.06

## New Features
- Launch Task app was moved to AvaloniaUI and dark theme is used. For icons Fluent icons were used.

## Removed Features
- Dropped support of non-encrypted incremental backups for recovery due to no usage.
If you used this feature, install previous version of application, update task (set any password), perform backup.

## Changes
- Italian language was updated by https://github.com/bovirus .

# 2023.09.28

## New Features
- Media transfer protocol support was added for import photo support via connection of phone via usb via MTP protocol for similar to FTP experience.

# 2023.09.27

## Removed Features
- Reduce amount of apps in Setup, since most people anyway search app by name.

## Bug Fixes
- 175% DPI breaks password generator, Where? section.

## Changes
- Italian language was updated by https://github.com/bovirus .
- Less icons on main view, tasks view.

# 2023.09.23

## Warning: FTPS storage encryption setting default was changed from Auto detect to Explicit. Update FTPS storage setting if it should be different.

## New Features
- FTPS encryption is now configurable and FTPS connection works stable.
- CLI: samba configuration is possible
- Import pictures from phone via FTPS through Wi-Fi guide with pictures (https://github.com/drweb86/butil/blob/master/help/Configure/Import%20media%20task.md).

## Changes
- Setup and Italian language were improved by https://github.com/bovirus
- Icons a little bit updated

## Bug Fixes
- 150% DPI breaks progress window;
- 150% DPI breaks launch task control, bottom buttons;
- UI: not possible to specify FTPS port more than 100;
- FTPS storage: autoconnect of fluent ui library did not work in some cases. It was dropped.
- FTPS storage: sometimes testing storage was hanging the UI, CLI apps.
- CLI: folder storage some settings were missing during configuration.

## Removed Features
- SAMBA form for folder storage, since of no usage.

# 2023.09.21

## New Features
- Italian language was added by https://github.com/bovirus
- Setup now shows proper copyright and version (thanks to https://github.com/bovirus)

## Removed Features
- RemoveLocalSettings argument is dropped because of no usage.
 
## Bug Fixes
- 150% DPI breaks tasks form layout.
- Restore menu.

# 2023.09.20

## New Features
- FTPS support for backups in UI.
- UI: configure import multimedia is possible in UI.
- CLI: same menu items as UI, show log statuses.
- To avoid issues with smart screen application is published as 7z archive (however note that 7z and .Net 7 Desktop are required to be installed).

## Changes
~ Upgrade UI keys approach, changing names across application

## Bug Fixes
- On restore of multimedia task application fails.

# 2023.09.16

## Bug fixes
- Edit of V2 tasks fails.

## Removed Features
- Dropped about dialog, since nobody uses it nowadays
- Dropped wizard dialog, since amount of options reduced to 4.

# 2023.09.15 (Preview)

## New Features
- Add support of format storage format V1 for backward compatibility.

# 2023.09.13 (Preview)

## New Features
- CLI: backup dialog title shows percentage
- [New task type: Move photo and videos task from SD card and phone via Wi-Fi](https://github.com/drweb86/butil/blob/master/help/Configure/Move%20photo%2C%20video%20task.md)

## Warning! Format of backup tasks was updated. Please reconfigure your tasks!

# 2023.08.12

## Bug Fixes
- CLI: date in autoreplacement parameter is not system
- CLI: very long move file name
- Fix for media move task

# 2023.08.11

## New Features
- DCIM files moval with template string
- CLI for creating DCIM files moval task, launch them

## Removed Features
- Possibility to disable compression
! Restoration of this backup way
- Drop execute tasks before/after backup because of no usage.

# 2023.06.12

## New Features
- Deduplication of files packed during current iteration

## Changes
- Reduce log size.

## Bug Fixes
- Deduplication support for integrity scripts add.
- Deduplication support for quota add.
- Backup UI: show completion time.

# 2023.06.05

## Changes
- Backup UI: Improved delta show, shows number of tasks in progress/completed
- Backup UI: Main form Title was improved
- Backup: Archiving opened for writing files

## Bug Fixes
- Backup UI: After backup completion results are no longer shown on separate message, because its impossible to focus on it. Main window will flash

# 2023.06.02

## New Features
- Journal for selected... for list of changes and Tree in Restoration

# 6.4 (28 May 2023)

## Bug Fixes
- When backup is completed fast, wrong progress is shown.

## Changes
- UI for viewing changed files in restoration tool was improved
- UI for versions selection was improved

## New Features
- Information about storage size, version sizes is shown

# 6.3 (23 May 2023)

## New Features
- Backup UI: Progress is more real. UI became better

# 6.2 (20 May 2023)

## New Features
- In tasks list view you can see last status of last backup
- Samba/HDD disk storage can be used in restoration view
- Restoration/Version Management of particular task is added.
- Last minute message in log and to user will be information about if backup was partial due to storage quota.

## Changes
- On double click on task in Configurator, its going to be executed.

## Bug Fixes
- After UI backup, when form stays alive, PC sleep is not possible.

## Removed Features
- It's not possible anymore to specify and use multiple storages. In real life scenarios nowadays usually 1 storage only is used because of data volume and ISP provider limits. If you had multiple storage specified, only first one will be used.
- Beep after completion of backup was removed, because most backups takes more than half an hour due to copying to network storages.
- Removal of obsoleted tasks in Backup UI was removed, because it looks as bug.

# 6.1 (15 May 2023)

## New Features
- Restoration UI: versions now show its year

## Changes
- Backup UI: successful items are removed over some iterations count to make ui be oriented on work in progress.
- Restoration UI: open large backups speed increased in 4 times.

## Bug fixes
- Restoration UI: tab stops were fixed on main form

# 6.0 (22 February 2023)

## New Features
- Logs list was removed from application. Status of log is added to log name, so reviewing of logs is done easy with Explorer.

# 5.9.0.0 (12 February 2023)

## New Features
- Logs list includes now task name;
- Logs location change was dropped because this feature is useless.
- Setup will attempt to install 7-zip if its missing in a system.
- Source items hashes calculation was improved. Hash cache was implemented. Hash is calculated when size or last write time of file is changed.
- Renamed, copies, moved files will be optimized in the storage (except for Plain Incremental Backup Model). 
- Cleanup of unfinished backups from storage is performed on start of backup.

## Changes
- Updates are checked on Configurator start. Update check was removed from About screen.
- Update check was reworked to use Github API.
- Setup will install .Net Desktop Runtime 7 when it is not installed.

# 5.8.0.0 (26 December 2022)

## New Features
- Inrecemented backup model is documented.

## Bug Fixes
- Packer output was fixed.

## Changes
- Logs were simplified;
- Icons were updated.

# 5.7.1.0 (23 December 2022)

## New Features
- Allow non-administrative installations. Archive deployment was dropped. Dialogs are removed.

# 5.7.0.0 (13 December 2022)

## Bug Fixes
! GUI tool was freezing during processing large amount of files.

# 5.6.1.0 RC1 (3 December 2022)

## Bug Fixes
! Subsequent encrypted or compressed backups were failing because sate could not be read.
! .Net Core 7 has issues with encodings in console. Transcoding of packer messages was dropped because it causes random crashes. It will mean - no longer packer in non-english messages can contain issues with encodings with non-english file names.
! Recovery scripts were saved to wrong folder.
- Samba share with spaces, user, password support was fixed

# 5.6RC1 (3 December 2022)

## New Features
- Ability to execute mount and unmount commands for Folder Storage (adds support of CLI tools to mount SFTP as drive via third-party tools)
- Samba

# 5.5c (23 November 2022)

## New Features
- Workaround of sporadic disconnects/reconnects of external USB in Windows 11
- Significant performance increase in file copy and SHA512 calculating.
- Skipping non-accessible folders
- Retry in case storage failures, 5 attempts with 30 seconds delay between them.
- Speedup console log and UI backup responsibilities.

# Changes
- FTP & Samba were removed (use proxy file system tools like cryptomator, etc.)

# 5.4b (20 November 2022)

This is beta version.
FTP & Samba are not working.

## New Features
- Added quotas for single backup side to support ISP
- Added possibility to disable storages

## Changes

## Bug Fixes
- UI fixes.

# 5.3b (16 November 2022)

This is beta version.
FTP & Samba are not working.

## New Features
- New section for setting up incremental model options.
- Incremental backup.
- Support for Wildcard exclusions for Folder Source Items based on Microsoft https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats

## Changes
- Update to .Net 7
- Setup is x32/x64
- 7-zip is not bundled with app
- Setup demands 7-zip to be installed in order to continue
- 7-zip is searched in 
Program Files, Program Files (x86), PATH environment variable directories
- MD5 tool DEV command is dropped
- phased out BU Localization because it is outdated
- dropped EFS encryption for configuration files (it is phased out by MS and not cross-platform)
- dropped 'No internet' option
- removed overwriting files with zeros since there's no sence in it: if someone has access to user files, then he won't need temp files with them stored as backup
- removed overwriting setting files with zeros since there's no sence in it: if someone has access to user files, then he won't need temp files with them stored as backup
- removed NTFS EFS for network copies, since Microsoft phases out it.
- removed password presence check for those items that are copied to network shares since it is up to user responsibility to take care of it and since private NAS had appeared and became popular
- removed FTP password check because usually for FTP backup purpose a dedicated user is created.
- 7-zip process count now takes into account logical cores. Default setting is changed to amount of logical cores.
- phased out Ghost application (it is replaced with console tool integrated with Windows Scheduler).
- md5 tool was dropped (certuil -hashfile) since getting MD5 is part of Windows OS and powershell now.
- Task name command line argument now only one is accepted by both UI and console. The idea is to simplify the tool.
- event integration was upated.
- dropped $backupFileName after backup task parameter support because none of new strategies will support it.
- dropped image file backup functionality.
- dropped Suspend, Hibernate modes since they are not supported by Windows 11 by default.

## Bug Fixes
- Passwords are not in debug log anymore
- Passwordless butil images were not created due to missing packer arguments

# 5.1 ( 22 October 2022 )

## New Features
- 7-zip was updated to 22.01 x64 (2022-07-15);
- .Net was updated to **.Net Desktop Runtime v 6**.

## Changes
- update support web-sites;
- update self-update mechanism to use github;
- setup will no longer install .Net, but will requrie **.Net Desktop Runtime v.6**;
- setup is x64 and demands x64 CPU;
- Windows 64-bit support is required;
- documentation was moved to web-site and excluded from project.

# 4.8 ( 22 November 2009 )

## New Features
- Console log: colors markup added

## Changes
- Log Viewer: Ui changed again

## Bug Fixes:
- Improper EFS configuration could lead to fault of program

## 4.7 ( 23 August 2009 )

## New Features
- Performance was speeded up
- Now amount of parralel packing processes can be setted up in Configurator
- Options became portable through versions
- Documentation now is under GOST RF on documentation of software
- Logs location now is possible to define again, however it has good default value

## Changes
- Scheduler now uses .net internal GC for performance
- Scheduler is more stable for not NT windows
- Restoration Tool merged to Configurator
- Logotype added
- KNeo package since now isn't used in application
- Installer now can increase speed of application Ui if user has asministrative privileges
- Logging simplified;
- Scheduler: messages added;
- In support mode settings are added to the file log
- Configuration change: schedule and configurator options were moved to profile options
- Desktop folder(with Documents one) is added to default backup task
- Now events before and after backup can be easily configured, also backup can be passed as parameter, those tasks can be turned off in backup master and cancelled

## Bug Fixes:
- HOT FIX: Blocking issue with reading output of an archiver
- Method not found: 'Void System.Runtime.GCSettings.set_LatencyMode(System.Runtime.GCLatencyMode)'. Fixed;
- Stopping of copying to storages could lead to backupUi fail;
- Configurator -removeLocalSettings could crash if there any data was in folders;
- Several logs in one day could lead to Configurator fail.
- When scheduler was disabled or there was nothing to schedule Configurator could not stop the running at this moment scheduler.
- Critial: Backup could not cancelled. This is a MS .Net Framework 2 bug. Workarounded
- Single instance of scheduler sometimes could lead to halt(mutexes issue)
- Cancellation of running events before backup fixed and does not crash program any more
- When there's not enough place in temp folder program crashed: now it outputs message to log with hint on issue

## 4.6 ( 9 July 2009 )

## Features
- Password length check can be configured via Configurator\Misc\Don't care about password length
- Added possibility to review and delete logs in configurator. Keyboard shortcuts: Del, Enter, F5 works in logs list
- 7-zip 32 installer now is included in this installer
- Configuration change: now option 'delete all files in destination place before backup' changed to 'delete all butil image files in target folder'
- Added possibility for synchronous storage processing(from 1 to 10 threads with default value 5, can be configured in Misc\Other tab)
- Added possibility to turn on and off compression items and storages in BackupUi tool
- Added possibility to reboot at finish of backup in backupUi application

## Usability improvements:
- Password entering simplified(exact error message is shown)
- Reduced time on reviewing logs by grouping them by result and then by date
- When option 'have no network and internet' is turned on, on pressing on + on storages tab - no context menu, just configuration form for hdd storage is shown
- Icons for folders and files fixed
- Size of column in configurator in source item now autochange it's size
- Removed check for storage unique name

## Bugs:
- Setting 'Run scheduler w/o icon in tray' was available when scheduler was turned off
- Fixed issue with location of log information in backup Ui
- Fixed issue with startup of browser at the end of backupUi
- Fixed issue with configuring OS to schedule show log
- Password check for encryption fixed
- Tray and from tray behaviour of backupUi fixed
- When computer account's session is locked and power off is planned at the end of backup: fixed
- Storages after modifying were changing order
- Caption of data in log files was adapted for russian culture only

## Several key decisions:
- For about a year since this summer project development may be freezed(army duty)
