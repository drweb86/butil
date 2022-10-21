set archivePath=..\deployment\BUtil-binaries.zip

cd "output\BUtil"
del "%archivePath%" /Y
"c:\Program Files\7-zip\7z.exe" a "%archivePath%" "." -mx9

cd ..\..\sources\setup
"c:\Program Files (x86)\Inno Setup 6\ISCC.exe" install.iss