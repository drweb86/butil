@echo off

dotnet publish /p:Configuration=Release /p:PublishDir=../Output/butil/bin /p:PublishReadyToRun=false /p:RunAnalyzersDuringBuild=False

echo %ERRORLEVEL%
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)

del ..\Output\*.exe
"c:\Program Files (x86)\Inno Setup 6\ISCC.exe" setup.iss
if NOT %ERRORLEVEL% == 0 (
	GOTO error;
)

del ..\Output\butil-binaries.7z
"c:\Program Files\7-Zip\7z.exe" a -y ..\Output\butil-binaries.7z "..\Output\BUtil\*" -mx9 -t7z -m0=lzma2 -ms=on -sccUTF-8 -ssw
if NOT %ERRORLEVEL% == 0 (
	GOTO error;
)

echo NOW SEND FILE TO https://www.microsoft.com/en-us/wdsi/filesubmission to fix smart screen detection
exit 0

:error
echo Failed
exit 1