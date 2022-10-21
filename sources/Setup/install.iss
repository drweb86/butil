; Review of script before creation of installer
; 1. Verify links for existence
;   a)	DOT_NET_2_URL = 'http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5&DisplayLang=en';
;   b)  MSI_URL = 'http://support.microsoft.com/kb/942288/en-us';

[Setup]
AppName=BUtil
AppVerName=BUtil 5.0Alpha3
AppPublisher=Siarhei Kuchuk
AppPublisherURL=https://github.com/drweb86/butil
AppSupportURL=https://github.com/drweb86/butil
AppUpdatesURL=https://github.com/drweb86/butil
DefaultDirName={pf}\BUtil\5.0Alpha
DefaultGroupName={cm:Backup}\BUtil 5.0Alpha3
AllowNoIcons=yes
OutputDir=..\..\Output\Deployment
OutputBaseFilename=BUtil-5.0Alpha-DotNet2-bin
Compression=lzma2/ultra64
SolidCompression=yes
PrivilegesRequired=none
UsePreviousAppDir=no
UsePreviousGroup=no
RestartIfNeededByRun=no
WizardImageFile=BUTilWizardImageFile164x314.bmp
WizardSmallImageFile=BUtilWizModernSmallImage.bmp
SetupIconFile=..\Media\Images and Icons\Other's guys\Crystal Clear (Everaldo Coelho)\SetupIcon.ico
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x64

[Languages]
Name: "en"; MessagesFile: ".\BUtil-Default.isl"
Name: "ru"; MessagesFile: ".\BUtil-Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]

Source: "..\..\Output\BUtil\*.*"; Excludes: ".svn"; DestDir: "{app}"; Flags: recursesubdirs

[INI]
Filename: "{app}\Configurator.url"; Section: "InternetShortcut"; Key: "URL"; String: "https://github.com/drweb86/butil"
Filename: "{app}\Help.url"; Section: "InternetShortcut"; Key: "URL"; String: "https://github.com/drweb86/butil/blob/master/help/TOC.md"

[Icons]
; Main app data
Name: "{group}\{cm:Configurator}"; Filename: "{app}\bin\Configurator.exe"
Name: "{group}\{cm:Console_Backup}"; Filename: "{app}\bin\Backup.exe"
Name: "{group}\{cm:Backup_Wizard}"; Filename: "{app}\bin\Configurator.exe"; Parameters: "JustBackupMaster"; IconFilename: "{app}\data\BackupUi.ico"
Name: "{group}\{cm:Restoration}"; Filename: "{app}\bin\Configurator.exe"; Parameters: "JustRestorationMaster"; IconFilename: "{app}\data\RestorationMaster.ico"
Name: "{group}\{cm:Journals}"; Filename: "{app}\bin\Configurator.exe"; Parameters: "JustJournals"; IconFilename: "{app}\data\Journals.ico"

; Toolkit
Name: "{group}\{cm:Toolkit}\{cm:Md5_Signer}"; Filename: "{app}\bin\MD5.exe"

; Documentation
Name: "{group}\{cm:Documentation}\{cm:Manual}"; Filename: "{app}\Help.url"
Name: "{group}\{cm:Documentation}\{cm:ProgramOnTheWeb,BUtil}"; Filename: "{app}\Configurator.url"

; Uninstall
Name: "{group}\{cm:Uninstall}\{cm:UninstallProgram,BUtil}"; Filename: "{uninstallexe}"

; Destop
Name: "{userdesktop}\{cm:Backup_Wizard}"; Filename: "{app}\bin\Configurator.exe"; Parameters: "JustBackupMaster"; IconFilename: "{app}\data\BackupUi.ico"; Tasks: desktopicon

[Run]
Filename: "{app}\bin\SpeedUp.bat";
Filename: "{app}\bin\Configurator.exe"; Description: "{cm:LaunchProgram,{cm:Configurator}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: files; Name: "{app}\Configurator.url"
Type: files; Name: "{app}\Help.url"
