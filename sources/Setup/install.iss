#define ApplicationVersion GetVersionNumbersString('..\..\Output\BUtil\bin\butil.exe')

[Setup]
AppName=BUtil
AppVerName=BUtil {#ApplicationVersion}
AppPublisher=Siarhei Kuchuk
AppPublisherURL=https://github.com/drweb86/butil
AppSupportURL=https://github.com/drweb86/butil
AppUpdatesURL=https://github.com/drweb86/butil
DisableWelcomePage=yes
DefaultDirName={autopf}\BUtil
DefaultGroupName=BUtil {cm:Backup}
AllowNoIcons=yes
OutputDir=..\..\Output\Deployment
OutputBaseFilename=BUtil_v{#ApplicationVersion}_(.NET_Desktop_Runtime_v7)_Setup
Compression=lzma2/ultra64
SolidCompression=yes
PrivilegesRequired=none
PrivilegesRequiredOverridesAllowed=commandline dialog
UsePreviousGroup=no
RestartIfNeededByRun=no
SetupIconFile=..\Media\Images and Icons\Other's guys\Crystal Clear (Everaldo Coelho)\SetupIcon.ico
ArchitecturesInstallIn64BitMode=x64
DisableFinishedPage=yes
DisableProgramGroupPage=yes
DisableDirPage=yes
DisableReadyPage=yes
UsePreviousAppDir=no

[Languages]
Name: "en"; MessagesFile: ".\BUtil-Default.isl"
Name: "ru"; MessagesFile: ".\BUtil-Russian.isl"

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
  Result := IsDotNetCoreInstalled('Microsoft.WindowsDesktop.App 7');
  if not Result then
    SuppressibleMsgBox(FmtMessage(SetupMessage(msgWinVersionTooLowError), ['.NET Desktop Runtime', '7']), mbCriticalError, MB_OK, IDOK);

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

const
  BN_CLICKED = 0;
  WM_COMMAND = $0111;
  CN_BASE = $BC00;
  CN_COMMAND = CN_BASE + WM_COMMAND;

procedure CurPageChanged(CurPageID: Integer);
var
  Param: Longint;
begin
  { if we are on the ready page, then... }
  if CurPageID = wpReady then
  begin
    { the result of this is 0, just to be precise... }
    Param := 0 or BN_CLICKED shl 16;
    { post the click notification message to the next button }
    PostMessage(WizardForm.NextButton.Handle, CN_COMMAND, Param, 0);
  end;
end;

[Files]
Source: "..\..\Output\BUtil\*.*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
; Main app data
Name: "{group}\BUtil {cm:Configurator}"; Filename: "{app}\bin\butil.exe"
Name: "{group}\BUtil {cm:Backup_Wizard}"; Filename: "{app}\bin\butil.exe"; Parameters: "JustBackupMaster"; IconFilename: "{app}\data\BackupUi.ico"
Name: "{group}\BUtil {cm:Restoration}"; Filename: "{app}\bin\butil.exe"; Parameters: "JustRestorationMaster"; IconFilename: "{app}\data\RestorationMaster.ico"

; Desktop
Name: "{autodesktop}\BUtil {cm:Configurator}"; Filename: "{app}\bin\butil.exe"

[Run]
Filename: "{app}\bin\butil.exe"; Description: "{cm:LaunchProgram,BUtil {cm:Configurator}}"; Flags: nowait postinstall skipifsilent

