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
Name: "{group}\{#MyAppName}";       Filename: "{app}\bin\butil-ui.Desktop.exe"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\bin\butil-ui.Desktop.exe"

[Run]
Filename: "{app}\bin\butil-ui.Desktop.exe"; Description: "Launch App"; Flags: nowait postinstall skipifsilent

