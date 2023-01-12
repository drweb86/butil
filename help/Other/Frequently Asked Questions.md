# Links in about tab are not opened
This is your OS issue. To fix it, please do the next with administrative privileges Start\Control Panel\Programs\Default programs tab and configure it.

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
| ```data\RestorationMaster.ico``` | The icon for restoration master | Used for User ```Menu\Backup Master``` |
| ```data\BackupUi.ico``` | The icon for backup master | Used for User ```Menu\Restoration``` |
| ```bin\BUtil.Core.dll``` | Library with the common functions | Provides functions for executables |
| ```bin\butilc.exe``` | Console backup tool | Performs the console based backup |
| ```bin\butil.exe``` | The program configurator with built-in Restoration Master and Backup Ui Master | configures program; restores from backup; backups with Ui. |


# How was the program tested?
Program is tested in next ways
| Component: | How it tested: |
| --- | --- | 
|  Binaries | Hand testing |
|  Program | Regression testing(rare); Functional testing; Automated NUnit testing(rare). Such test cases are in the sources and in functional design documents to big tasks |
|  Ui | Tabs testing |

# How can I post a feedback about documentation?
You can do it [here](https://github.com/drweb86/butil/issues).

# What should I do if backup fails?
You should investigate reasons why backup failed. To do this please open Configurator and open the backup log. If there will be not enough information you can try to set log mode to Setup and repeat backup. There will be more information. If nothing helped you can ask for support.

# What should I do if I cannot run the program?
Make sure that all requirements are satisfied. If nothing helped you can ask for support.

# How to inegrate the Console Backup tool with Windows Scheduler
Please see the next [guide](../Schedule/Integration%20with%20Windows%20Scheduler.md).

# What are the main sphears of using the software?
Sotware was written to be used at home as personal backup program on Windows OS.

# Where 7-zip Package is Being Searched For
Package should installed in
- Program Files or
- Program Files (x86) or
- be in any path that is in PATH environment variable.

# What's About Support of Microsoft Outlook Mail Backups?
This feature is not supported, because program will have to terminate Outlook in order to correctly backup the PST file, find the file location and that start the Outlook again. Backups must be taken in MsExchange on server side, also free mail providers takes care about backups of data of their data.
But if you want to do this using this tool, you may do the following workaround:
- In run before backup add a command to kill Outlook process: c:\windows\system32\taskkill.exe with arguments: /im outlook.exe
- Add PST file to to 'What';
- In run after backup add a command to start Outlook;
- Configure computer to not sleep at 2 AM;
- Schedule the backup at this time;

Also you may look for Microsoft Sync Toy and Data Mills
