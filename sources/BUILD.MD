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

### Prerequisites

A. Ubuntu 23.10.

Info: Since 23.10 .Net 8 is part of Ubuntu feed, installing .Net from Microsoft feed will make Ubuntu non-upgradable.

B. Do following

```
sudo apt-get update
sudo apt-get install -y git

sudo snap install snapcraft --classic

sudo apt-get install -y dotnet-sdk-8.0

cd ~/Desktop
git clone https://github.com/drweb86/butil ./butil
cd ./butil

```

### Build snap

Do following

```
cd ~/Desktop/butil
git pull

dotnet publish /p:Version=2023.11.24 /p:AssemblyVersion=%version% /p:Configuration=Release /p:PublishDir=../Output/butil/bin /p:PublishReadyToRun=false /p:RunAnalyzersDuringBuild=False

sudo snapcraft
```

1. Do once

a. Make 

```
sudo lxd init --auto
sudo adduser <user> lxd

sudo apt-get install libc6 \
libgcc1 \
libgcc-s1 \
libgssapi-krb5-2 \
libicu72 \
liblttng-ust1 \
libssl3 \
libstdc++6 \
libunwind8 \
zlib1g

# Get Ubuntu version
declare repo_version=$(if command -v lsb_release &> /dev/null; then lsb_release -r -s; else grep -oP '(?<=^VERSION_ID=).+' /etc/os-release | tr -d '"'; fi)

# Download Microsoft signing key and repository
wget https://packages.microsoft.com/config/ubuntu/$repo_version/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Install Microsoft signing key and repository
sudo dpkg -i packages-microsoft-prod.deb

# Clean up
rm packages-microsoft-prod.deb

# Update packages
sudo apt update

sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-8.0

sudo snap install multipass
```

2. To build

a. Update version in snap/snapcraft.yaml

b.

```
cd /mnt/...folder of project/
sudo snapcraft clean
sudo snapcraft
```