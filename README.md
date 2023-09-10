# COMPRESSING, SAVING, STORING, PROVIDING easiest RESTORING

Welcome!

![BUtil Logotype](./help/Readme%20Assets/Logotype.bmp)

BUtil is a powerful set of tools for creating backups of documents and other data on your PC.

![The main window of configurator, backup ui master](./help/Readme%20Assets/Screenshot%201.png)

## Software requirements

- Microsoft Windows 10 or later;
- Microsoft .Net Desktop Runtime 7 is required. Setup will install it.
- Igor Pavlov's 7-zip. Setup will install it.

## Key functions and characteristics of software

- Incremental backup model with deduplication of files;
- Photo/Video sync from external card, phone (Move file sync with SAMBA, FTPS support (use CLI to configure it));
- Simple configuring takes less time and goes easily and with comfort;
- Multiple backup tasks are supported;
- Html format for log files takes less time on reviewing of it;
- Two modes of creating a backup exists: manual from UI and console tool and with scheduling/automation possibilities;
- Uses [7-zip](https://www.7-zip.org/) compression and AES-256 encryption.

## Documentation

- [Version history](./help/Other/Version%20History%20(Changelog).md)
- [Congifurator](./help/Configure/Configurator.md)
- [Console CLI UI](./help/Configure/Console%20CLI%20UI.md)
- [Command line version of backup tool](./help/Backup/Backup%20via%20Console%20Tool.md)
- [Restorate](./help/Restore/Restoration%20Wizard.md)
- [Frequently Asked Questions (FAQ)](./help/Other/Frequently%20Asked%20Questions.md)
and many more. See [all topics](./help/TOC.md) .

## Build

Must be installed:
- Windows 11x64 Professional;
- Microsoft Visual Studio 2022;
- InnoSetup 6.2.1 (must be installed in default directory);
- 7-zip x64 (must be installed in default directory).

Compile all with **publish.cmd** script (artefacts will be located in **Output** folder)

[Help:](https://github.com/drweb86/butil/blob/master/help/TOC.md)
