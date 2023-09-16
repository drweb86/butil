@echo off

dotnet publish /p:Configuration=Release /p:PublishDir=../Output/butil/bin /p:PublishReadyToRun=false /p:RunAnalyzersDuringBuild=False
echo %ERRORLEVEL%
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)

"c:\Program Files (x86)\Inno Setup 6\ISCC.exe" setup.iss
if NOT %ERRORLEVEL% == 0 (
	GOTO error;
)


echo NOW SEND FILE TO https://www.microsoft.com/en-us/wdsi/filesubmission to fix smart screen detection
exit 0

:error
echo Failed
exit 1