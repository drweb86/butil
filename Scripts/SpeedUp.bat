@echo off
rem Backup tool BUtil speed up script
rem You should run it with administrative privileges to speed up application speed
set FrmwrkPath=%windir%\Microsoft.NET\Framework\v2.0.50727
%FrmwrkPath%\ngen.exe BUtil.Core.dll
%FrmwrkPath%\ngen.exe Backup.exe
%FrmwrkPath%\ngen.exe BUtil.Backup.Ghost.exe
%FrmwrkPath%\ngen.exe Configurator.exe
%FrmwrkPath%\ngen.exe MD5.exe