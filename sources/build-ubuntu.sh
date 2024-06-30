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
echo Creating shortcuts

cp ${tempFolder}sources/butil-ui/Assets/butil.ico ${outputFolder}/butil.ico

uiShortcut=${HOME}/Desktop/BUtil.desktop

cat > ${uiShortcut} << EOL
[Desktop Entry]
Encoding=UTF-8
Version=1.0
Type=Application
Terminal=false
Exec=$(pwd)/${outputFolder}/butil-ui.Desktop
Name=BUtil
Icon=$(pwd)/${outputFolder}/butil.ico
EOL

chmod -R 775 ${uiShortcut}
gio set "${uiShortcut}" metadata::trusted yes

consoleShortcut="${HOME}/Desktop/BUtil Console.desktop"

cat > "${consoleShortcut}" << EOL
[Desktop Entry]
Encoding=UTF-8
Version=1.0
Type=Application
Terminal=true
Exec=$(pwd)/${outputFolder}/butilc
Name=BUtil Console
Icon=$(pwd)/${outputFolder}/butil.ico
EOL

chmod -R 775 "${consoleShortcut}"
gio set "${consoleShortcut}" metadata::trusted yes

rm -rf ${tempFolder}

echo Everything is completed 
echo Folder      : ${outputFolder}
echo UI tool     : butil-ui.Desktop, see desktop shortcut
echo Console tool: butilc, see desktop shortcut
