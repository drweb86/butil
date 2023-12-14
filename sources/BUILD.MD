# Prerequisites

- Windows 11x64 Professional;
- Microsoft Visual Studio 2022;

# Packaging

## Windows

### Prerequisites

- InnoSetup 6.2.2 (must be installed in default directory);
- 7-zip x64 (must be installed in default directory).
- .Net 7 SDK

### Build.cmd

Update version (it should match the version name and tag of release!) in Version History file. 

Launch build.cmd.

It will create <root>\output\butil folders with compiled application.

It will create <root>\output\BUtil-x.exe with setup.

It will create <root>\output\BUtil-binaries.7z with binaries.

## Ubuntu

### Prepare

Install wsl-2

```
sudo apt-get update
sudo apt install snapcraft
cd /mnt/d/sources/butil
sudo snapcraft
# It will propose to install multipass, agree.
# It will propose to install LXD, agree.
sudo visudo
# Add this to editor and save, close wsl
# ad ALL = NOPASSWD : ALL
```

### Build

Do following

a. Launch

```
build.cmd

```

To test
```
sudo snap remove butil --purge
sudo snap install butil_3_all.snap --classic --dangerous
```
