#!/bin/bash

# Fail on first error.
set -e

version=2024.06.29

sourceCodeInstallationDirectory=/usr/local/src/butil
binariesInstallationDirectory=/usr/local/butil

if [ "$(id -u)" -ne 0 ]; then
        echo 'This script must be run by root' >&2
        exit 1
fi

echo
echo Installing dependencies
echo
apt-get update
apt-get install -y git dotnet-sdk-8.0 7zip

echo
echo Cleaning installation directories
echo
rm -rf ${sourceCodeInstallationDirectory}
rm -rf ${binariesInstallationDirectory}

echo
echo Get source code
echo
git clone https://github.com/drweb86/butil.git ${sourceCodeInstallationDirectory}
cd ${sourceCodeInstallationDirectory}

echo
echo Update to tag
echo
git checkout tags/${version}

echo
echo Building
echo
cd ./sources
dotnet publish /p:Version=${version} /p:AssemblyVersion=${version} -c Release --property:PublishDir=${binariesInstallationDirectory}

echo
echo Prepare icon
echo
cp ${sourceCodeInstallationDirectory}/sources/butil-ui/Assets/butil.ico ${binariesInstallationDirectory}/butil.ico

echo
echo Create shortcuts
echo
declare -a shortcutLocations=("/usr/share/applications" "~/Desktop")

for shortcutLocation in "${shortcutLocations[@]}"
do
    echo
    echo Create shortcut in ${shortcutLocation}
    echo

    shortcutFile=${shortcutLocation}/BUtil.desktop
cat > ${shortcutFile} << EOL
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

    chmod -R 775 ${shortcutFile}
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