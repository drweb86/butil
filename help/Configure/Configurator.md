To configure application you should start the Configurator. Configurator is shown on picture 1. Description to program elements is ib table 1.

![Image 1 - View of configurator](./Image%201%20-%20View%20of%20configurator.png)

Table 1 - Description of elements(to the image 1)
| Program element | Description |
| --- | --- | 
| 1 | Main program menu. Here you can configure Configurator, change language and run some other tools |
| 2 | Left panel with buttons provide access to all configuration items semantically grouped |
| 3 | This is an area where each configuration of each group is shown |
| 4 | Those buttons stands for saving the whole set of options or cancelling all changes made |
| 5 | This is the hint area. To see the hint just hover mouse over the configuration group or any input field or button |

# Items To Backup

## Adding Items To Backup
The place where you can add the items which will be backupped. See picture 2.

![Image 2 - Managing items to backup](./Image%202%20-%20Managing%20items%20to%20backup.png)

You can add files and folders you want to backup in the following ways:
- Drag from explorer (9) and drop to list (1);
- Add via context menu (7, 8) or buttons from the left (2,3).

## Changing The Degree Of Compression
Default degree of compression is **Normal**. To change degree of comression you should select those items (hold Ctrl and click on those items) and open context menu (5)

## Removing Items From The List
To remove items from the list you should select those items (hold Ctrl and click on those items) and open context menu (6) or press at button (4)

# Storages
Storages are the places where your backup will be copied to. They can be configured on 'Where' settings group of Configurator(see picture 3)

![Image 3 - Managing storages](./Image%203%20-%20Managing%20storages.png)

## Adding Storages To The List
Next storage types are supported: hdd, flash and network storages. You can add the storages to the list(1) in the following ways:
- Press on button (2) and context menu with list of storages will be opened (3, 4, 5);
- Open context menu (10) on empty space of list (1) and storages (11, 12, 13) will be available.

**NOTE:** Storages Ftp(4, 12) and Network(5, 13) are available only if option 'Misc\Have No Network And Internet' is turned off.

## Modifying The Storage
To modify the storage you should select it in the list (1) and click on button (6) or open context menu (8) or double click on storage icon

## Removing The Storage From The List
To remove the storage from the list (1) you should select it and press on button (7) or open context menu (9).

# Setting Up The Scheduler
To set up the scheduler you can open the 'When' settings group (see image 4)

![Image 4 - Scheduler Configuration](./Image%204%20-%20Scheduler%20Configuration.png)

## How To Set Up The Schduler?
Scheduler configuration is available only when option 'Misc\Don't Need Scheduler' is turned off. Choose the days of week when backup will be started by clicking on list (1). Then choose the time (2, 3) when backup should be started on those days.

## Hiding the Scheduler Icon From Task Bar
To hide the icon you can run scheduler in non-interactive mode by checking the setting (4).
**NOTE:** This mode is less functional (you cannot stop backup, don't receive notifications, and cannot run non-interactive backup).

## I don't need scheduler at all
In this case you can disable it in Configurator by turning on the option 'Misc\Don't Need Scheduler'

## I Want the Scheduler to Start When The Windows Starts
By default scheduler works only when you login to the system, but sometimes you need to make the backup to go without your loging to the system. To see it please see the next <a href="../scheduler/IntergrationWithWindowsScheduler.htm">guide</a>

## How Can I Integrate Console Backup With Windows Scheduler
Please see the next <a href="../scheduler/IntergrationWithWindowsScheduler.htm">guide</a>

# Protection
AES-256 7-zip protection is used to protect data in an image (see image 5).

![Image 5 - Encryption](./Image%205%20-%20Encryption.png)

## How Can I Protect My Image With The Password?
You can open settings group 'Encryption' and input here a password. 

Requirements to the password:
- Password should not contain blanks;
- Password should have appropriate length (you can disable it in Configurator in setting 'Misc\Don't care about password length').

When password does not meet requirements an error messages will be shown under the text box with the password. Also you can generate random password by clicking on button (2). When both password and confirmation are equal both text boxes become read.

# Performance Speed Up
To speed up the performance you can open in the Controller and open 'Other options' settings group (see image 6).

![Image 6 - Other Options](./Image%206%20-%20Other%20Options.png)

## How To Speed Up The Copying To Storages?
You can speed up copying to storages in (1) by increasing setting value

## How To Speed Up The Compression Of Source Items?
To make 7-zip processes run parallel you should increase value of setting (2). This value for the best performance should be setted to amount of CPUs on your PC or total amount of cores of CPUs

## I Want Backup Didn't Affect Much On Performance Of Currently Running Applications?
To do this please decrease priority of backup in setting (3). Also you can set up CPU loading (4) to 5-10% to postpone backup

# Chains of Executing Programs
This setting can be configured in Configurator in 'Other Options' setting group (see image 7).

![Image 7 - Other Options](./Image%206%20-%20Other%20Options.png)

## How To Make Programs to Run Before Backup?
To do this you can add programs to list (5). Note: all programs in this list are performed sequentally one by one. Directory of each program is set to parent of executable. Links are not allowed

## How To Make Programs to Run After Backup?
To do this you can add programs to list (6). Note: all programs in this list are performed sequentally one by one. Directory of each program is set to parent of executable. Links are not allowed. If you want to pass backup image file as parameter you should use autoreplace case sensitive abbreviation $BackupImageFile. This string will be replaced on image location in temp folder (for example : c:\temp folder\tmp_22.BUtil). To be sure that name passed correctly you can pass as argument "$BackupImageFile". With those quotes application could handle with pathes and file names that contains blanks. When the specified chain of programs will run? Answer is right after the finishing the copying of storage process and right before the deletion of image file

# Logging
You can configure logging in Configurator in 'Logging' settings group (see image 8).

![Image 8 - Logging](./Image%208%20-%20Logging.png)

## How Can I Change Logs Location?
You should use link label (13) to do this. To restore default location press at link label (12). If your specified location will be unavailable, program will fail to backup.

## How Can I Gain Support With Logs?
To get support with the logs you should set logging level (1) to 'Support', then run the backup and after that open support link either in program or from the documentation

# Feedback, Help And Support In The Program
You can open web-links of home page, feature request, bug report and support request from program and from this documentation. To do this from program please open in Configurator 'About' settings group (see image 9).

![Image 9 - About tab](./Image%209%20-%20About%20tab.png)

## How Do I Check If I Have The Latest Version?
To do this open link(6).

## How Do I Open This Help From Program?
To do this open link(5).

## To Whom Can I Send Information About Unproper Translation?
You can send it to the e-mail of guy shown in right panel (7). You can copy data from here.

# Miscellanous

## Before backup event

For integration with other programs you can run script in before backup event which is configured in Configurator. You can create .cmd / .bat script or executables without parameters for scheduling running of your programs before backup.
Program will wait until program termination. If you will interrupt backup, running of this scripts will not be interrupted.
If you want your programs to run synchronously please review the next example of .cmd script as a template in table 2.

Table 2 - Examples of integrations with third-party UI programs
| Example element | Description |
| --- | --- | 
| ```start "" /wait "c:\program files\application.exe"``` | Synchronous |
| ```start "" "c:\program files\application.exe"``` | Asynchronous |

## Critical errors of tools
Critical errors of tools stored in "Butil Bug report.txt" file on Desktop.