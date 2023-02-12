# 5.9.0.0 (12 February 2023)

## New Features
- Logs list includes now task name;
- Logs location change was dropped because this feature is useless.
- Setup will attempt to install 7-zip if its missing in a system.
- Source items hashes calculation was improved. Hash cache was implemented. Hash is calculated when size or last write time of file is changed.

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
