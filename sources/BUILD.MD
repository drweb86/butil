# Prerequisites

- Windows 11x64 Professional;
- Microsoft Visual Studio 2022;

# Packaging

## Windows

### Prerequisites

- InnoSetup 6.2.2 (must be installed in default directory);
- 7-zip x64 (must be installed in default directory).

### Build.cmd

Update version (it should match the version name and tag of release!) in build.cmd. Launch it.

It will create ..\output\butil folders with compiled application.

It will create ..\output\BUtil-x.exe with setup.

It will create ..\output\BUtil-binaries.7z with binaries.

## Ubuntu

### Prepare

Install wsl-2

```
sudo apt-get update
sudo apt install snapcraft
cd /mnt/d/sources/butil
sudo snapcraft
# It will propose to install multipass, agree.
# LXD is required but not installed. Do you wish to install LXD and configure it with the defaults?, agree
```

### Build

Do following

```
cd /mnt/d/sources/butil
sudo snapcraft clean
sudo snapcraft

```
