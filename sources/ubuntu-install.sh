#!/bin/bash

# Fail on first error.
set -e

version=2024.07.06

sourceCodeInstallationDirectory=/usr/local/src/butil
binariesInstallationDirectory=/usr/local/butil

echo
echo Installing dependencies
echo
sudo apt-get update
sudo apt-get install -y git dotnet-sdk-8.0 7zip

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
echo Prepare icon
echo
sudo cp ${sourceCodeInstallationDirectory}/sources/butil-ui/Assets/butil.ico ${binariesInstallationDirectory}/butil.ico

echo
echo Prepare shortcut
echo

temporaryShortcut=/tmp/BUtil.desktop
cat > ${temporaryShortcut} << EOL
[Desktop Entry]
Encoding=UTF-8
Version=${version}
Name=BUtil
GenericName=Incremental backup, Synchronization, Import media
Categories=Incremental backup;Synchronization;Import media
Comment=BUtil creates incremental backups and imports multimedia on your PC with deduplication and FTPS, SMB/CIFS, MTP transports support for Windows and Linux.
Type=Application
Terminal=false
Exec=${binariesInstallationDirectory}/butil-ui.Desktop
Icon=${binariesInstallationDirectory}/butil.ico
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
echo Binaries: ${sourceCodeInstallationDirectory}
echo Sources: ${binariesInstallationDirectory}
echo
echo Shortcut on desktop and for quick search are provisioned for UI tool.
echo Console tool: ${binariesInstallationDirectory}/butilc
echo
echo
sleep 2m