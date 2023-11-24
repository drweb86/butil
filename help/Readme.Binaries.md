# Binaries Readme

## Windows

### Prerequisites

OS: Microsoft Windows 10 or 11.

You need to have installed **Microsoft .Net Desktop Runtime 8**.

You need to have installed **Igor Pavlov's 7-zip** in default installation folder (or be available in any folder inside environment variable PATH).

### Executables

UI executable is **bin\butil-ui.Desktop.exe**.

Console executable is **bin\butilc.exe**.

## Ubuntu 23.10+

### Prerequisites

OS: Application is tested on Ubuntu 23.10+ . Probably it might work on other linux distributions as well.

You need to install **Microsoft .Net Desktop Runtime 8**:

a. Open **Terminal**.

b. Execute `lsb_release -a` , see your Ubuntu version.

c. Open https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu , follow instructions for your Ubuntu version, you need to install **runtime**.

d. Reboot your PC.

You need to have installed **Igor Pavlov's 7-zip** (or be available in any folder inside environment variable PATH):

a. Open **Terminal**.

b. Execute `sudo apt-get install 7zip`.

You need to have installed **Microsoft Powershell** (or be available in any folder inside environment variable PATH):

a. Open https://www.linuxcapable.com/how-to-install-powershell-on-ubuntu-linux/ , follow instructions for your Ubuntu version.

b. Reboot.

### Executables

UI executable is **bin\butil-ui.Desktop.dll**. To launch do the following steps:

a. Open **Terminal**.

b. Execute `dotnet ./bin/butil-ui.Desktop.dll &`.

Console executable is **bin\butilc.dll**. To launch do the following steps:

a. Open **Terminal**.

b. Execute `dotnet ./bin/butilc.dll`.