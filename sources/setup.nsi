; Compression
SetCompressor /FINAL /SOLID lzma
SetCompressorDictSize 64

Unicode true

; Defines
!define PRODUCT_NAME "BUtil"
!ifndef PRODUCT_VERSION
  !define PRODUCT_VERSION "0.0.0"
!endif
!define PRODUCT_PUBLISHER "Siarhei Kuchuk"
!define PRODUCT_WEB_SITE "https://github.com/drweb86/butil"
!define START_YEAR "2011"
!define CURRENT_YEAR "$%DATE:~-4%"

; MUI Settings
!include "MUI2.nsh"
!include "x64.nsh"
!include "WinVer.nsh"
!include "FileFunc.nsh"
!include "nsDialogs.nsh"
!include "LogicLib.nsh"

!insertmacro GetParameters
!insertmacro GetOptions

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "BUtil.UI\Assets\butil.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Language selection dialog settings
!define MUI_LANGDLL_ALLLANGUAGES
!define MUI_LANGDLL_REGISTRY_ROOT "HKCU"
!define MUI_LANGDLL_REGISTRY_KEY "Software\${PRODUCT_NAME}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

; Welcome page
!define MUI_WELCOMEPAGE_TITLE_3LINES
!insertmacro MUI_PAGE_WELCOME

; Custom installation mode page
Page custom InstallModePageCreate InstallModePageLeave

; Directory page
!define MUI_PAGE_CUSTOMFUNCTION_PRE SkipDirectoryPage
!insertmacro MUI_PAGE_DIRECTORY

; Start menu page
Var StartMenuFolder
!define MUI_PAGE_CUSTOMFUNCTION_PRE SkipStartMenuPage
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "${PRODUCT_NAME}"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\${PRODUCT_NAME}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder

; Instfiles page
!insertmacro MUI_PAGE_INSTFILES

; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\bin\butil-ui.Desktop.exe"
!define MUI_FINISHPAGE_RUN_TEXT "$(Installer_LaunchApp)"
!define MUI_PAGE_CUSTOMFUNCTION_SHOW FinishPageShow
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files (auto-generated from .resx resources by ResxSorter)
!include "setup-languages.nsh"

; Installer attributes
Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "..\Output\BUtil_v${PRODUCT_VERSION}.exe"
InstallDir "$PROGRAMFILES\${PRODUCT_NAME}"
ShowInstDetails show
ShowUnInstDetails show
RequestExecutionLevel user

; Version Information
VIProductVersion "${PRODUCT_VERSION}.0"
VIAddVersionKey "ProductName" "${PRODUCT_NAME}"
VIAddVersionKey "ProductVersion" "${PRODUCT_VERSION}"
VIAddVersionKey "CompanyName" "${PRODUCT_PUBLISHER}"
VIAddVersionKey "LegalCopyright" "© ${START_YEAR}-${CURRENT_YEAR} ${PRODUCT_PUBLISHER}"
VIAddVersionKey "FileDescription" "${PRODUCT_NAME} installer"
VIAddVersionKey "FileVersion" "${PRODUCT_VERSION}"

; Architecture handling - Runtime detection
Var InstallMode
Var MultiUser
Var InstallModeDialog
Var InstallModeRadio1
Var InstallModeRadio2
Var InstallModeLabel
Var CmdLineMode

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY

  ; Initialize default to current user
  StrCpy $InstallMode "CurrentUser"
  StrCpy $MultiUser "0"
  StrCpy $CmdLineMode "0"

  ; Parse command line arguments
  ; /ALLUSERS or /AllUsers = install for all users (requires admin)
  ; /CURRENTUSER or /CurrentUser = install for current user only (default)
  ${GetParameters} $0
  ${GetOptions} $0 "/ALLUSERS" $1
  ${IfNot} ${Errors}
    StrCpy $InstallMode "AllUsers"
    StrCpy $MultiUser "1"
    StrCpy $CmdLineMode "1"
  ${EndIf}
  
  ${GetOptions} $0 "/AllUsers" $1
  ${IfNot} ${Errors}
    StrCpy $InstallMode "AllUsers"
    StrCpy $MultiUser "1"
    StrCpy $CmdLineMode "1"
  ${EndIf}
  
  ${GetOptions} $0 "/CURRENTUSER" $1
  ${IfNot} ${Errors}
    StrCpy $InstallMode "CurrentUser"
    StrCpy $MultiUser "0"
    StrCpy $CmdLineMode "1"
  ${EndIf}
  
  ${GetOptions} $0 "/CurrentUser" $1
  ${IfNot} ${Errors}
    StrCpy $InstallMode "CurrentUser"
    StrCpy $MultiUser "0"
    StrCpy $CmdLineMode "1"
  ${EndIf}
  
  ; Check if admin rights are available when installing for all users
  ${If} $MultiUser == "1"
    UserInfo::GetAccountType
    Pop $0
    ${If} $0 != "admin"
      MessageBox MB_OK|MB_ICONSTOP "$(Installer_AdminRequired)"
      Abort
    ${EndIf}
  ${EndIf}

  ; Check architecture
  ${If} ${RunningX64}
    SetRegView 64
    ${If} $MultiUser == "1"
      StrCpy $INSTDIR "$PROGRAMFILES64\${PRODUCT_NAME}"
    ${Else}
      StrCpy $INSTDIR "$LOCALAPPDATA\Programs\${PRODUCT_NAME}"
    ${EndIf}
  ${Else}
    MessageBox MB_OK|MB_ICONSTOP "$(Installer_ArchRequired)"
    Abort
  ${EndIf}

FunctionEnd


; Custom page to select installation mode
Function InstallModePageCreate
  ; Skip this page if mode was specified on command line
  ${If} $CmdLineMode == "1"
    Abort
  ${EndIf}
  
  !insertmacro MUI_HEADER_TEXT "$(Installer_InstallModeTitle)" "$(Installer_InstallModeSubtitle)"
  
  nsDialogs::Create 1018
  Pop $InstallModeDialog
  
  ${If} $InstallModeDialog == error
    Abort
  ${EndIf}
  
  ; Add label with description
  ${NSD_CreateLabel} 0 0 100% 24u "$(Installer_InstallModeDescription)"
  Pop $InstallModeLabel
  
  ; Add radio button for current user (default)
  ${NSD_CreateRadioButton} 10u 30u 100% 12u "$(Installer_CurrentUserOption)"
  Pop $InstallModeRadio1
  ${NSD_OnClick} $InstallModeRadio1 InstallModeRadio1Click
  
  ; Add description for current user option
  ${NSD_CreateLabel} 20u 45u 95% 16u "$(Installer_CurrentUserDescription)"
  Pop $0
  
  ; Add radio button for all users
  ${NSD_CreateRadioButton} 10u 65u 100% 12u "$(Installer_AllUsersOption)"
  Pop $InstallModeRadio2
  ${NSD_OnClick} $InstallModeRadio2 InstallModeRadio2Click
  
  ; Add description for all users option
  ${NSD_CreateLabel} 20u 80u 95% 16u "$(Installer_AllUsersDescription)"
  Pop $0
  
  ; Set default selection based on current mode
  ${If} $MultiUser == "1"
    ${NSD_Check} $InstallModeRadio2
  ${Else}
    ${NSD_Check} $InstallModeRadio1
  ${EndIf}
  
  nsDialogs::Show
FunctionEnd

Function InstallModeRadio1Click
  Pop $0
  StrCpy $InstallMode "CurrentUser"
  StrCpy $MultiUser "0"
FunctionEnd

Function InstallModeRadio2Click
  Pop $0
  StrCpy $InstallMode "AllUsers"
  StrCpy $MultiUser "1"
FunctionEnd

Function InstallModePageLeave
  ; Check if admin rights are available when installing for all users
  ${If} $MultiUser == "1"
    UserInfo::GetAccountType
    Pop $0
    ${If} $0 != "admin"
      MessageBox MB_OK|MB_ICONSTOP "$(Installer_AdminRequiredDetailed)"
      Abort
    ${EndIf}
  ${EndIf}
  
  ; Set installation directory based on mode
  ${If} $MultiUser == "1"
    StrCpy $INSTDIR "$PROGRAMFILES64\${PRODUCT_NAME}"
  ${Else}
    StrCpy $INSTDIR "$LOCALAPPDATA\Programs\${PRODUCT_NAME}"
  ${EndIf}
  
  DetailPrint "$(Installer_InstallingFor) $InstallMode"
FunctionEnd

; Custom function to skip directory page
Function SkipDirectoryPage
  Abort
FunctionEnd

; Custom function to skip start menu page
Function SkipStartMenuPage
  Abort
FunctionEnd

Function FinishPageShow
FunctionEnd

Section "MainSection" SEC01
  SetOutPath "$INSTDIR\bin"
  SetOverwrite on
  
  ${If} $MultiUser == "1"
    SetShellVarContext all
  ${Else}
    SetShellVarContext current
  ${EndIf}

  ${If} ${IsNativeARM64}
  File /r "..\output\publish\arm64\*.*"
  ${Else}
  File /r "..\output\publish\x64\*.*"
  ${EndIf}

  ; Create uninstaller
  WriteUninstaller "$INSTDIR\uninst.exe"
  
  ; Add uninstall information to Add/Remove Programs
  ${If} $MultiUser == "1"
    WriteRegStr HKLM "Software\${PRODUCT_NAME}" "" $INSTDIR
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayName" "${PRODUCT_NAME}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "UninstallString" "$INSTDIR\uninst.exe"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayIcon" "$INSTDIR\bin\butil-ui.Desktop.exe"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayVersion" "${PRODUCT_VERSION}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "Publisher" "${PRODUCT_PUBLISHER}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "HelpLink" "${PRODUCT_WEB_SITE}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "URLUpdateInfo" "${PRODUCT_WEB_SITE}"
  ${Else}
    WriteRegStr HKCU "Software\${PRODUCT_NAME}" "" $INSTDIR
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayName" "${PRODUCT_NAME}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "UninstallString" "$INSTDIR\uninst.exe"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayIcon" "$INSTDIR\bin\butil-ui.Desktop.exe"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayVersion" "${PRODUCT_VERSION}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "Publisher" "${PRODUCT_PUBLISHER}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "HelpLink" "${PRODUCT_WEB_SITE}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "URLUpdateInfo" "${PRODUCT_WEB_SITE}"
  ${EndIf}

  ; Create shortcuts
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\${PRODUCT_NAME}.lnk" "$INSTDIR\bin\butil-ui.Desktop.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
  
  ; Create desktop shortcut
  CreateShortcut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\bin\butil-ui.Desktop.exe"
SectionEnd

Function un.onInit
  !insertmacro MUI_UNGETLANGUAGE
FunctionEnd

Section Uninstall
  ; Remove shortcuts
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
  
  Delete "$SMPROGRAMS\$StartMenuFolder\${PRODUCT_NAME}.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
  
  ; Remove files and directories
  RMDir /r "$INSTDIR\bin"
  Delete "$INSTDIR\uninst.exe"
  RMDir "$INSTDIR"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  DeleteRegKey HKLM "Software\${PRODUCT_NAME}"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  DeleteRegKey HKCU "Software\${PRODUCT_NAME}"
  
  SetAutoClose true
SectionEnd
