# COMPRESSING, SAVING, STORING, PROVIDING easiest RESTORING

Welcome to the 5.1 (22 October 2022) version BUtil!

![BUtil Logotype](./help/Readme%20Assets/Logotype.bmp)

BUtil is a powerfull set of tools for creating backups of documents and other data on your PC.

![The main window of configurator](./help/Readme%20Assets/Screenshot%201.jpg)
![The backup ui master](./help/Readme%20Assets/Screenshot%202.jpg)

## Key Functions And Characteristics Of Software

- It can store backups on multiple HDD, flash, remote FTP servers and shared folders over the network;
- Simple configuring takes less time and goes easily and with comfort;
- AES-256 7-zip encyption makes your data securely protected;
- It includes console md5 signer tool;
- Multiple backup tasks are supported;
- Html format is uded for file logs. It takes less time on reviewing of it;
- Scheduler is present;
- Two modes of creating a backup exists: manual from UI and console tool and with scheduling possibilities;
- .Net based application;
- [Unstructured Data repository model](./help/Other/Glossary.md)
- [Copying files selection and extraction of file data](./help/Other/Glossary.md)
- Built in [7-zip](https://www.7-zip.org/) [compression](https://en.wikipedia.org/wiki/Data_compression)
- Built in [7-zip](https://www.7-zip.org/) [AES-256](https://en.wikipedia.org/wiki/Advanced_Encryption_Standard) [encryption](https://en.wikipedia.org/wiki/Encryption).

## Keywords

Backup, Restoration, Desktop applications, Scheduling, AES-256, 7-zip, md5, .Net

## The documentation for BUtil includes:

- [Congifurator](./help/configurator/CommandLineArguments.htm)
- [Command line version of backup tool](./help/Backup%20Console%20Tool.md)
- [Restorate](./help/Restore/Restoration%20Wizard.md)
- [Checksum Console Tool](./help/Checksum%20Console%20Tool.md)
- [Scheduler](./help/Schedule/Scheduler%20Tray%20Application/Scheduler%20Tray%20Application.md)

and many more. See [all topics](../../wiki) .

## See Also

- [Version history](./help/Other/Version%20History.md)
- [Frequently Asked Questions (FAQ)](./help/Other/Frequently%20Asked%20Questions.md)

## Requirements

### Software

- Microsoft Windows OS x64 7SP1+;
- Microsoft .Net Desktop Runtime 6 is required. To download it you can open the following [url](https://dotnet.microsoft.com/en-us/download/dotnet/6.0), see section **.NET Desktop Runtime 6.0.10**

To function in full mode also required:
- NTFS file system on system partition
- Windows OS should support EFS

### Hardware:

- Pentium 3 computer or better, IA-64 but should work and on IA-64 architecture
- 1 GB RAM
- about 5 mb on HDD

## Feedback

You can ask for support on next languages:
- English,
- Russian,
- Belorussian.

### Advices

Bug reports is advised to contain:
- Name of program, Version, Component name, OS;
- Problem definition(why this is a bug):
What behaviour do you see(you can attach sreenshots and videorolics)
What behaviour do you expect(you can attach sreenshots and videorolics and describe why your variant is the better)
How to reproduce the issue(you can attach sreenshots and videorolics

When you're providing logs, don't forget to set logging level to SUPPORT

## Compilation

How to compile solution without problems?

Must be installed:
- Microsoft .Net 2.0.5;
- Windows 11x64 Professional;
- [BULocalization 3.2](sourceforge.net/projects/bulocalization);
- Microsoft Visual Studio 2022;
- InnoSetup 6.2.1 (must be installed in default directory);
- 7-zip x64 (must be installed in default directory).

1. Compile all
2. Run in ```output\butil\bin``` command ```md5 dev```
3. Update in code ```Files.cs``` MD5 checksums of 7-zip.
4. Done.


Copyright (c) 2008-2010 Siarhei Kuchuk
[Sources repository](https://github.com/drweb86/butil)
[Bug Report:](https://github.com/drweb86/butil/issues)
[Help:](https://github.com/drweb86/butil/blob/master/help/TOC.md)
[Discussion:](https://github.com/drweb86/butil/discussions)

Support period is usually one time per week
