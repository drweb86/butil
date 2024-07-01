#!/bin/bash

echo
echo Removing sources installation folder
echo
sudo rm -rf /usr/local/src/butil

echo
echo Removing binaries installation folder
echo
sudo rm -rf /usr/local/butil

echo
echo Removing configuration files
echo
rm -rf ~/.config/BUtil

desktopDir=$(xdg-user-dir DESKTOP)

echo
echo Removing shortcuts
echo
sudo rm /usr/share/applications/BUtil.desktop
sudo rm "${desktopDir}/BUtil.desktop"

echo
echo Application was uninstalled
echo
echo
echo
sleep 2m