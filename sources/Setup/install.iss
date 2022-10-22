; Review of script before creation of installer
; 1. Verify links for existence
;   a)	DOT_NET_2_URL = 'http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5&DisplayLang=en';
;   b)  MSI_URL = 'http://support.microsoft.com/kb/942288/en-us';  1

#define ApplicationVersion GetVersionNumbersString('..\..\Output\BUtil\bin\Configurator.exe')

[Setup]
AppName=BUtil
AppVerName=BUtil {#ApplicationVersion}
AppPublisher=Siarhei Kuchuk
AppPublisherURL=https://github.com/drweb86/butil
AppSupportURL=https://github.com/drweb86/butil
AppUpdatesURL=https://github.com/drweb86/butil
DefaultDirName={pf}\BUtil\{#ApplicationVersion}
DefaultGroupName={cm:Backup}\BUtil {#ApplicationVersion}
AllowNoIcons=yes
OutputDir=..\..\Output\Deployment
OutputBaseFilename=BUtil v.{#ApplicationVersion} (.NET Desktop Runtime v6) Setup
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

[Code]
function IsDotNetCoreInstalled(DotNetName: string): Boolean;
var
  Cmd, Args: string;
  FileName: string;
  Output: AnsiString;
  Command: string;
  ResultCode: Integer;
begin
  FileName := ExpandConstant('{tmp}\dotnet.txt');
  Cmd := ExpandConstant('{cmd}');
  Command := 'dotnet --list-runtimes';
  Args := '/C ' + Command + ' > "' + FileName + '" 2>&1';
  if Exec(Cmd, Args, '', SW_HIDE, ewWaitUntilTerminated, ResultCode) and
     (ResultCode = 0) then
  begin
    if LoadStringFromFile(FileName, Output) then
    begin
      if Pos(DotNetName, Output) > 0 then
      begin
        Log('"' + DotNetName + '" found in output of "' + Command + '"');
        Result := True;
      end
        else
      begin
        Log('"' + DotNetName + '" not found in output of "' + Command + '"');
        Result := False;
      end;
    end
      else
    begin
      Log('Failed to read output of "' + Command + '"');
    end;
  end
    else
  begin
    Log('Failed to execute "' + Command + '"');
    Result := False;
  end;
  DeleteFile(FileName);
end;

function InitializeSetup: Boolean;
begin
  Result := IsDotNetCoreInstalled('Microsoft.WindowsDesktop.App 6');
  if not Result then
    SuppressibleMsgBox(FmtMessage(SetupMessage(msgWinVersionTooLowError), ['.NET Desktop Runtime', '6']), mbCriticalError, MB_OK, IDOK);
end;

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
Filename: "{app}\bin\Configurator.exe"; Description: "{cm:LaunchProgram,{cm:Configurator}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: files; Name: "{app}\Configurator.url"
Type: files; Name: "{app}\Help.url"
