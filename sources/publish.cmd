@echo off

cd tools\ResxSorter
dotnet run
if NOT %ERRORLEVEL% == 0 (
	GOTO error;
)
cd ..
cd ..

dotnet publish /p:Configuration=Release /p:PublishDir=../Output/butil/bin /p:PublishReadyToRun=false /p:RunAnalyzersDuringBuild=False
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)

rmdir "../Output/butil/bin/runtimes/linux-arm" /S /Q
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)
rmdir "../Output/butil/bin/runtimes/linux-arm64" /S /Q
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)
rmdir "../Output/butil/bin/runtimes/linux-musl-x64" /S /Q
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)
rmdir "../Output/butil/bin/runtimes/linux-x64" /S /Q
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)
rmdir "../Output/butil/bin/runtimes/osx" /S /Q
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)
rmdir "../Output/butil/bin/runtimes/unix" /S /Q
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