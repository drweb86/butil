#define ApplicationVersion GetVersionNumbersString('..\Output\BUtil\bin\butil.exe')

[Setup]
AppName=BUtil
AppVerName=BUtil {#ApplicationVersion}
AppPublisher=Siarhei Kuchuk
AppPublisherURL=https://github.com/drweb86/butil
AppSupportURL=https://github.com/drweb86/butil
AppUpdatesURL=https://github.com/drweb86/butil
DisableWelcomePage=yes
DefaultDirName={autopf}\BUtil
DefaultGroupName=BUtil
AllowNoIcons=yes
OutputDir=..\Output
OutputBaseFilename=BUtil_v{#ApplicationVersion}
Compression=lzma2/ultra64
SolidCompression=yes
PrivilegesRequired=none
PrivilegesRequiredOverridesAllowed=commandline dialog
UsePreviousGroup=no
RestartIfNeededByRun=no
SetupIconFile=.\Media\Images and Icons\Other's guys\Crystal Clear (Everaldo Coelho)\SetupIcon.ico
ArchitecturesInstallIn64BitMode=x64
DisableFinishedPage=yes
DisableProgramGroupPage=yes
DisableDirPage=yes
DisableReadyPage=yes
UsePreviousAppDir=no

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: ru; MessagesFile: "compiler:Languages\Russian.isl"

[Messages]
en.BeveledLabel=English
ru.BeveledLabel=Русский

[CustomMessages]
en.Tasks=Tasks
ru.Tasks=Задания

en.LaunchTask=Launch task
ru.LaunchTask=Запуск задачи

en.Restoration=Restoration
ru.Restoration=Восстановление

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
var
  ResultCode: integer;
begin
  
  Result := IsDotNetCoreInstalled('Microsoft.WindowsDesktop.App 7');
  if not Result then
  begin
    Result := Exec('winget', 'install -e --id Microsoft.DotNet.DesktopRuntime.7 --disable-interactivity --accept-source-agreements --accept-package-agreements', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
    if (not Result) or (ResultCode <> 0) then
    begin
      SuppressibleMsgBox('Failed to install Microsoft .Net Desktop Runtime 7'
    + #13#10#13#10
    + 'Application uses Microsoft .Net Desktop Runtime 7 for running.'
    + #13#10#13#10
    + 'Setup will continue, but application will not be able to start.'
    + #13#10#13#10
    + 'Please install Microsoft .Net Desktop Runtime 7 before running the application.', mbCriticalError, MB_OK, IDOK);
      Result := true;
    end;
  end;

  if not Is7ZipInstalled() then
  begin
    Result := Exec('winget', 'install -e --id 7zip.7zip --disable-interactivity --accept-source-agreements --accept-package-agreements', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
    if (not Result) or (ResultCode <> 0) then
    begin
      SuppressibleMsgBox('Failed to install 7-zip'
    + #13#10#13#10
    + 'Application uses 7-zip for compression and decompression.'
    + #13#10#13#10
    + 'Setup will continue, but application will not be able to compress or decompress files.'
    + #13#10#13#10
    + 'Please install 7-zip to default location before running the application.', mbCriticalError, MB_OK, IDOK);
      Result := true;
    end;
  end;
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
Source: "..\Output\BUtil\*.*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\BUtil {cm:Tasks}";		Filename: "{app}\bin\butil.exe"
Name: "{group}\BUtil CLI";			Filename: "{app}\bin\butilc.exe"
Name: "{group}\BUtil {cm:LaunchTask}";		Filename: "{app}\bin\butil.exe";	Parameters: "LaunchTask";	IconFilename: "{app}\data\BackupUi.ico"
Name: "{group}\BUtil {cm:Restoration}";		Filename: "{app}\bin\butil.exe";	Parameters: "Restore";		IconFilename: "{app}\data\RestorationMaster.ico"
Name: "{autodesktop}\BUtil {cm:Tasks}";		Filename: "{app}\bin\butil.exe"

[Run]
Filename: "{app}\bin\butil.exe"; Description: "Launch App"; Flags: nowait postinstall skipifsilent

