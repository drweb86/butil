# Binaries Readme

## Windows 10 or 11

### Prerequisites

You need to have installed **Igor Pavlov's 7-zip** in default installation folder (or be available in any folder inside environment variable PATH).

### Executables

UI executable is **bin\butil-ui.Desktop.exe**.

Console executable is **bin\butilc.exe**.

## Ubuntu 23.10+

### Prerequisites

OS: Application is tested on Ubuntu 23.10+ . Probably it might work on other linux distributions as well.

You need to have installed **Igor Pavlov's 7-zip** (or be available in any folder inside environment variable PATH):

a. Open **Terminal**.

b. Execute `sudo apt-get install -y 7zip`.

### Executables

UI executable is **bin\butil-ui.Desktop.dll**. To launch do the following steps:

a. Open **Terminal**.

b. Execute `dotnet ./bin/butil-ui.Desktop.dll &`.

Console executable is **bin\butilc.dll**. To launch do the following steps:

a. Open **Terminal**.

b. Execute `dotnet ./bin/butilc.dll`.