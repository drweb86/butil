# Links in about tab are not opened
This is your OS issue. To fix it, please do the next with administrative privileges Start\Control Panel\Programs\Default programs tab and configure it. Then reboot.

# Where can I configure the temporary folder?
In some cases free size of temp folder is too low. To change TEMP folder location please do:

- Right-click on "My Computer" icon on desktop, open Properties
 - Go to "Advanced" tab;
 - Open "Environment variables";
 - Set user variable "TEMP" to folder you want;
 - Set options;
 - Reboot PC.


# Where does the program keep settings and logs

| What: | Where: |
| --- | --- | 
| Configuration data | ``` %userprofile%\Application data\BUtil-x.y\Settings\ProfileOptions.xml``` - this file contains options in Xml-format and encrypted(when program run in full mode) |
| Logs | ``` %userprofile%\Application data\BUtil-x.y\Logs ``` |

# What programming language and platform program was written for?
Components of program were written on the following languages

| Component: | Language: |
| --- | --- | 
| Program and libraries | C# |
| Setup | InnoSetup |

# What is the structure of software?
Software has the next structure

| Component: | Description: | Functions: |
| --- | --- | --- | 
| ```7-zip``` | Installed 7-zip 32-bit software. | Compresses the data for backup and restoration in this software package. |
| ```local``` | Folder with preinstalled translations of tool | Allows to show program ui and logs in localized way |
| ```data\logtemplate.dat``` | Template file for html logs that produced during backup | This file is used to generate log |
| ```data\RestorationMaster.ico``` | The icon for restoration master | Used for User ```Menu\Backup Master``` |
| ```data\BackupUi.ico``` | The icon for backup master | Used for User ```Menu\Restoration``` |
| ```bin\Backup.Core.dll``` | Library with the common functions | Provides functions for executables |
| ```bin\Backup.exe``` | Console backup tool | Performs the console based backup |
| ```bin\BUtil.Backup.Ghost.exe``` | Scheduler tool | Provides ability to schedule the backup |
| ```bin\Configurator.exe``` | The program configurator with built-in Restoration Master and Backup Ui Master | configures program; restores from backup; backups with Ui. |
| ```bin\MD5.exe``` | Tool for making md5 checksumms | Counts md5 checksumms |

# How was the program tested?
Program is tested in next ways
| Component: | How it tested: |
| --- | --- | 
|  Documentation |  Hand testing according to ISO 9127-94. |
|  Source codes | Code inspection with hands and Microsoft FxCop(rare) |
|  Binaries | Hand testing |
|  Program | Regression testing(rare); Functional testing; Automated NUnit testing(rare). Such test cases are in the sources and in functional design documents to big tasks |
|  Ui | Tabs testing |

# How can I post a feedback about documentation?
You can do it [here](https://github.com/drweb86/butil/issues).

# What are the standards were used during development of software?
They are the GOST RF ISO 9127-94, internal code style guide and internal testing guide.

# What should I do if the configurator fails to load settings?
You should reset them either with running configurator with special switch or by removing configuration files.

# What should I do to make program to run in full mode?
You should use NTFS at system drive and install Windows OS that supports encrypted file system.

# What should I do if backup fails?
You should investigate reasons why backup failled. To do this please open Configurator and open the backup log. If there will be not enough information you can try to set log mode to Setup and repeat backup. There will be more information. If nothing helped you can ask for support.

# What should I do if I cannot run the program?
Make sure that all requirements are satisfied. If nothing helped you can ask for support.

# How to inegrate the Console Backup tool with Windows Scheduler
Please see the next [guide](../Schedule/Integration%20with%20Windows%20Scheduler.md).

# BUtil Scheduler does not work when i didn't logged to the system!
Yes, it works in standard way only when you logged to the system, but you can either inegrate the Console Backup tool or this software with Windows Scheduler to avoid this. For the guide please see the next [document](../Schedule/Integration%20with%20Windows%20Scheduler.md).

# What are the main sphears of using the software?
Sotware was written to be used at home as personal backup program on Windows OS.

## Updating 7-zip 32 package

Package should installed to '7-zip' folder
Md5-checksumms of some components should be updated in code
- open and compile solution in Visual Studio
- go to 'Output\BUtil\bin' folder and execute command
```md5 dev```
- all md5 checksumms must be seted to appropriate constants in file
\sources\Backup.Core\FileSystem\Files.cs:
Packer7ZipExeMD5 = "93C7B7A3E3051BBB9630E41425CFDB3C";
Packer7ZipGExeMD5 ="3F317B59A522F0BC19AC1620BBEA0718";
Packer7ZipDllMD5 = "CA41D56630191E61565A343C59695CA1";

# What Should I Do If Program Faills To Load Settings?
You can either run program with special argument or delete options files in %appdata%\butil-xx folder.

# How Can I Make The Software Work?
You can just install the software package. If you donloaded the binaries archive you can extract it any folder you like.
First program configuration can be done in Configurator. After that you can use other tools and can perform backup.

# What's About Support of Microsoft Outlook Mail Backups?
This feature is not supported, because program will have to terminate Outlook in order to correctly backup the PST file, find the file location and that start the Outlook again. Backups must be taken in MsExchange on server side, also free mail providers takes care about backups of data of their data.
But if you want to do this using this tool, you may do the following workaround:
- In run before backup add a command to kill Outlook process: c:\windows\system32\taskkill.exe with arguments: /im outlook.exe
- Add PST file to to 'What';
- In run after backup add a command to start Outlook;
- Configure computer to not sleep at 2 AM;
- Schedule the backup at this time;

Also you may look for Microsoft Sync Toy and Data Mills

# What's About Support of Internet Explorer Settings Backup?
Please download the special tools designed to do this and add running of them as a special task before backup in some folder, add this folder to backup items.
