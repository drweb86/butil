@echo off

cd sources
dotnet publish /p:Configuration=Release /p:PublishDir=../Output/butil/bin /p:PublishReadyToRun=false /p:RunAnalyzersDuringBuild=False
echo %ERRORLEVEL%
if %ERRORLEVEL% NEQ 0 (
	GOTO error;
)

cd ..

cd "sources\setup"
"c:\Program Files (x86)\Inno Setup 6\ISCC.exe" install.iss
if NOT %ERRORLEVEL% == 0 (
	GOTO error;
)

exit 0

:error
echo Failed
exit 1