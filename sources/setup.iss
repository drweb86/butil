#define  MyAppName           "BUtil"
#define  ApplicationVersion  "###VERSION###"
#define  CurrentYear         GetDateTimeString('yyyy','','')
#define  StartYearCopyright  "2011"
#define  MyAppSupportURL     "https://github.com/drweb86/butil"
#define  MyAppAuthor         "Siarhei Kuchuk"
#include "..\..\sources\setup-strings.iss"

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

SetupIconFile=..\..\sources\butil-ui\Assets\butil.ico

WizardStyle=modern

DisableWelcomePage=yes
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=..
OutputBaseFilename=BUtil_v{#ApplicationVersion}_###CORERUNTIME###
Compression=lzma2/ultra64
SolidCompression=yes
PrivilegesRequired=none
PrivilegesRequiredOverridesAllowed=commandline dialog
UsePreviousGroup=no
RestartIfNeededByRun=no
ArchitecturesInstallIn64BitMode=###ARCHITECTURE###
DisableFinishedPage=yes
DisableProgramGroupPage=yes
DisableDirPage=yes
DisableReadyPage=yes
UsePreviousAppDir=no


[Code]
function Is7ZipInstalled(): Boolean;
begin
  Result := FileExists(ExpandConstant('{commonpf64}\7-zip\7z.exe'));
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
      SuppressibleMsgBox('BUtil depends on 7-zip. Please install it manually.', mbInformation, MB_OK, IDOK);
    end;
  end;
  Result := true;
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
Source: "bin\*.*"; DestDir: "{app}\bin"; Flags: recursesubdirs

[Icons]
Name: "{group}\{#MyAppName}";       Filename: "{app}\bin\butil-ui.Desktop.exe"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\bin\butil-ui.Desktop.exe"

[Run]
Filename: "{app}\bin\butil-ui.Desktop.exe"; Description: "Launch App"; Flags: nowait postinstall skipifsilent

