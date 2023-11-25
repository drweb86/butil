#define  MyAppName           "BUtil"
#define  ApplicationVersion  GetVersionNumbersString('..\Output\BUtil\bin\butil-ui.Desktop.exe')
#define  CurrentYear         GetDateTimeString('yyyy','','')
#define  StartYearCopyright  "2011"
#define  MyAppSupportURL     "https://github.com/drweb86/butil"
#define  MyAppAuthor         "Siarhei Kuchuk"
#include ".\setup-strings.iss"

[Setup]
AppName={#MyAppName}
AppVersion={#ApplicationVersion}
AppVerName={#MyAppName} {#ApplicationVersion}

AppCopyright={#StartYearCopyright}-{#CurrentYear}  {#MyAppAuthor}
AppPublisher={#MyAppAuthor}
AppPublisherURL={#MyAppSupportURL}
AppSupportURL={#MyAppSupportURL}
AppUpdatesURL={#MyAppSupportURL}

VersionInfoDescription={#MyAppName} installer
VersionInfoProductName={#MyAppName}
VersionInfoVersion={#ApplicationVersion}

UninstallDisplayName={#MyAppName}
UninstallDisplayIcon={app}\bin\butil-ui.Desktop.exe

SetupIconFile=.\butil-ui\Assets\butil.ico

WizardStyle=modern

DisableWelcomePage=yes
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=..\Output
OutputBaseFilename=BUtil_v{#ApplicationVersion}
Compression=lzma2/ultra64
SolidCompression=yes
PrivilegesRequired=none
PrivilegesRequiredOverridesAllowed=commandline dialog
UsePreviousGroup=no
RestartIfNeededByRun=no
ArchitecturesInstallIn64BitMode=x64
DisableFinishedPage=yes
DisableProgramGroupPage=yes
DisableDirPage=yes
DisableReadyPage=yes
UsePreviousAppDir=no


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
      SuppressibleMsgBox('BUtil depends on "Microsoft .Net Desktop Runtime 7". Please install it manually.', mbInformation, MB_OK, IDOK);
      Result := true;
    end;
  end;

  if not Is7ZipInstalled() then
  begin
    Result := Exec('winget', 'install -e --id 7zip.7zip --disable-interactivity --accept-source-agreements --accept-package-agreements', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
    if (not Result) or (ResultCode <> 0) then
    begin
      SuppressibleMsgBox('BUtil depends on 7-zip. Please install it manually.', mbInformation, MB_OK, IDOK);
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
Name: "{group}\{#MyAppName}";       Filename: "{app}\bin\butil-ui.Desktop.exe"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\bin\butil-ui.Desktop.exe"

[Run]
Filename: "{app}\bin\butil-ui.Desktop.exe"; Description: "Launch App"; Flags: nowait postinstall skipifsilent

