#!/bin/bash

# Fail on first error.
set -e

version=2025.10.13

sourceCodeInstallationDirectory=/usr/local/src/butil
binariesInstallationDirectory=/usr/local/butil

if [ "$EUID" -eq 0 ]
  then echo "Please do not run this script with sudo, root permissions"
  exit
fi

echo
echo Installing dependencies
echo
sudo add-apt-repository ppa:dotnet/backports
sudo apt-get update
echo
echo Ubuntu 24.04 specific
echo
sudo apt-get install -y git dotnet-sdk-9.0
sudo apt-get update
echo
echo End of Ubuntu 24.04 specific
echo
echo
echo Cleaning installation directories
echo
sudo rm -rf ${sourceCodeInstallationDirectory}
sudo rm -rf ${binariesInstallationDirectory}

echo
echo Get source code
echo
sudo git clone https://github.com/drweb86/butil.git ${sourceCodeInstallationDirectory}
cd ${sourceCodeInstallationDirectory}

echo
echo Update to tag
echo
sudo git checkout tags/${version}

echo
echo Building
echo
cd ./sources
sudo dotnet publish /p:Version=${version} /p:AssemblyVersion=${version} -c Release --property:PublishDir=${binariesInstallationDirectory} --use-current-runtime --self-contained

echo
echo Prepare PNG icon for Gnome, ico files are not handled
echo
sudo cp "${sourceCodeInstallationDirectory}/help/Assets/Icon 120x120.png" "${binariesInstallationDirectory}/Icon 120x120.png"

echo
echo Prepare shortcut
echo

temporaryShortcut=/tmp/BUtil.desktop
sudo rm -f ${temporaryShortcut}
cat > ${temporaryShortcut} << EOL
[Desktop Entry]
Encoding=UTF-8
Version=${version}
Name=BUtil
GenericName=Incremental backup, Synchronization, Import media
Categories=Incremental backup;Synchronization;Import media
Comment=BUtil creates incremental backups, incremental synchronization and imports multimedia on your PC with deduplication and FTPS, SMB/CIFS, MTP transports support for Windows and Linux.
Type=Application
Terminal=false
Exec=${binariesInstallationDirectory}/butil-ui.Desktop
Icon=${binariesInstallationDirectory}/Icon 120x120.png
StartupWMClass=butil-ui.Desktop
EOL
sudo chmod -R 775 ${temporaryShortcut}

desktopDir=$(xdg-user-dir DESKTOP)
declare -a shortcutLocations=("/usr/share/applications" "${desktopDir}")

for shortcutLocation in "${shortcutLocations[@]}"
do

shortcutFile=${shortcutLocation}/BUtil.desktop

echo
echo Create shortcut in ${shortcutFile}
echo

sudo cp ${temporaryShortcut} "${shortcutFile}"
sudo chmod -R 775 "${shortcutFile}"
gio set "${shortcutFile}" metadata::trusted true

done

echo
echo
echo Everything is completed 
echo
echo
echo Application was installed too:
echo
echo Binaries: ${binariesInstallationDirectory}
echo Sources: ${sourceCodeInstallationDirectory}
echo
echo Shortcut on desktop and for quick search are provisioned for UI tool.
echo Console tool: ${binariesInstallationDirectory}/butilc
echo
echo
sleep 2m
