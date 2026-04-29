#!/bin/bash

# Fail on first error.
set -e

# Default values
LATEST_SOURCES=false
for arg in "$@"; do
    if [[ "$arg" == "--latest" ]]; then
        echo "Using latest sources! Not sources related to version."
        LATEST_SOURCES=true
    fi
done

sourceCodeInstallationDirectory=/usr/local/src/butil
binariesInstallationDirectory=/usr/local/butil

if [ "$EUID" -eq 0 ]
  then echo "Please do not run this script with sudo, root permissions"
  exit
fi

echo
echo Install .Net 10
echo
wget https://dot.net/v1/dotnet-install.sh -O /tmp/dotnet-install.sh
chmod +x /tmp/dotnet-install.sh
/tmp/dotnet-install.sh --channel 10.0
sudo apt install dbus-x11 cron unzip
echo

echo
echo Cleaning installation directories
echo
sudo rm -rf ${sourceCodeInstallationDirectory}
sudo rm -rf ${binariesInstallationDirectory}

echo
echo Get source code
echo
version=$(wget -qO- https://api.github.com/repos/drweb86/butil/releases/latest | grep '"tag_name"' | cut -d'"' -f4)
echo "Latest release: $version"

if [ "$LATEST_SOURCES" = true ]; then
	wget -O /tmp/butil-src.zip https://github.com/drweb86/butil/archive/refs/heads/master.zip
else
	wget -O /tmp/butil-src.zip https://github.com/drweb86/butil/archive/refs/tags/${version}.zip
fi
sudo unzip -q /tmp/butil-src.zip -d /tmp/butil-src-extracted
sudo mv /tmp/butil-src-extracted/butil-* ${sourceCodeInstallationDirectory}
rm -f /tmp/butil-src.zip
sudo rm -rf /tmp/butil-src-extracted
cd ${sourceCodeInstallationDirectory}

echo
echo Building
echo
cd ./sources
sudo /root/.dotnet/dotnet publish /p:Version=${version} /p:AssemblyVersion=${version} -c Release --property:PublishDir=${binariesInstallationDirectory} --use-current-runtime --self-contained

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