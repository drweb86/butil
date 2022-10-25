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

function Is7ZipInstalled(): Boolean;
begin
  Result := True;
  if Is64BitInstallMode then
  begin
    Result := FileExists(ExpandConstant('{commonpf32}\7-zip\7z.exe')) or FileExists(ExpandConstant('{commonpf64}\7-zip\7z.exe'));
  end
  else
  begin
    Result := FileExists(ExpandConstant('{commonpf32}\7-zip\7z.exe'));
  end
end;

function InitializeSetup: Boolean;
begin
  Result := IsDotNetCoreInstalled('Microsoft.WindowsDesktop.App 6');
  if not Result then
    SuppressibleMsgBox(FmtMessage(SetupMessage(msgWinVersionTooLowError), ['.NET Desktop Runtime', '6']), mbCriticalError, MB_OK, IDOK);

  if not Is7ZipInstalled() then
  begin
      SuppressibleMsgBox('Please install 7-zip before continuing the installation'
    + #13#10#13#10
    + 'Application uses 7-zip for compression and decompression.'
    + #13#10#13#10
    + 'Please install 7-zip to default location and restart the installation.', mbCriticalError, MB_OK, IDOK);
    Result := False;
  end
end;

[Files]

Source: "..\..\Output\BUtil\*.*"; DestDir: "{app}"; Flags: recursesubdirs

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
