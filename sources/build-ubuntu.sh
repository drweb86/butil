#!/bin/bash

version=2024.06.29

tempFolder=./butil-${version}-temporary
outputFolder=./butil-${version}

echo Installing dependencies

sudo apt-get update
sudo apt-get install -y git dotnet-sdk-8.0 7zip
sudo apt update

echo Getting sources

rm -rf ${tempFolder}
rm -rf ${outputFolder}
git clone https://github.com/drweb86/butil.git ${tempFolder}
cd ${tempFolder}
git checkout tags/${version}

echo Building

cd ./sources
dotnet publish /p:Version=${version} /p:AssemblyVersion=${version} -c Release --self-contained true -o ../.${outputFolder}

cd ../..
rm -rf ${tempFolder}

echo Everything is completed 
echo Folder      : ${outputFolder}
echo UI tool     : butil-ui.Desktop
echo Console tool: butilc

# "~/Desktop/BUtil ${version}.desktop" << EOF 
# [Desktop Entry]
# Encoding=UTF-8
# Version=1.0
# Type=Application
# Terminal=false
# Exec=$1
# Name=$2
# Icon=$3
# EOF
