; Compression
SetCompressor /FINAL /SOLID lzma

; Defines
!define PRODUCT_NAME "BUtil"
!define PRODUCT_VERSION "###VERSION###"
!define PRODUCT_PUBLISHER "Siarhei Kuchuk"
!define PRODUCT_WEB_SITE "https://github.com/drweb86/butil"
!define START_YEAR "2011"
!define CURRENT_YEAR "$%DATE:~-4%" ; Will be replaced at compile time

; MUI Settings
!include "MUI2.nsh"
!include "x64.nsh"
!include "WinVer.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "..\..\sources\butil-ui\Assets\butil.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!define MUI_WELCOMEPAGE_TITLE_3LINES
!insertmacro MUI_PAGE_WELCOME

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
!define MUI_FINISHPAGE_RUN_TEXT "Launch App"
!define MUI_PAGE_CUSTOMFUNCTION_SHOW FinishPageShow
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; Installer attributes
Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "..\BUtil_v${PRODUCT_VERSION}_###CORERUNTIME###.exe"
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

Function .onInit

  !if "###CORERUNTIME###" == "win-arm64"
    ${IfNot} ${IsNativeARM64}
      MessageBox MB_OK "This installer is for Windows ARM64,$\nbut your Windows is X64.$\nPlease downlad and run X64 installer version."
      Quit
    ${EndIf}
  !endif

  !if "###CORERUNTIME###" == "win-x64"
    ${If} ${IsNativeARM64}
      MessageBox MB_OK "This installer is for Windows X64,$\nbut your Windows is ARM64.$\nPlease downlad and run ARM64 installer version."
      Quit
    ${EndIf}
  !endif

  ; Additional check for Windows 11 specifically (build 26100+)
  ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion" "CurrentBuildNumber"
  ${If} $0 < 26100
    MessageBox MB_OK|MB_ICONSTOP "This application requires Windows 11 (build 26100) or later.$\nYour Windows version: build $0"
    Abort
  ${EndIf}

  ; Check architecture if needed
  ${If} ${RunningX64}
    SetRegView 64
    StrCpy $INSTDIR "$PROGRAMFILES64\${PRODUCT_NAME}"
  ${Else}
    MessageBox MB_OK|MB_ICONSTOP "This installer requires a 64-bit version of Windows."
    Abort
  ${EndIf}

FunctionEnd

; Custom function to skip directory page (emulating DisableDirPage=yes)
Function SkipDirectoryPage
  Abort
FunctionEnd

; Custom function to skip start menu page (emulating DisableProgramGroupPage=yes)
Function SkipStartMenuPage
  Abort
FunctionEnd

; Custom function to show finish page immediately (emulating DisableReadyPage and auto-proceed)
Function FinishPageShow
  ; The finish page will show normally, but installation starts immediately
FunctionEnd

Section "MainSection" SEC01
  SetOutPath "$INSTDIR\bin"
  SetOverwrite on
  File /r "bin\*.*"
  
  ; Store installation folder
  WriteRegStr HKCU "Software\${PRODUCT_NAME}" "" $INSTDIR
  
  ; Create uninstaller
  WriteUninstaller "$INSTDIR\uninst.exe"
  
  ; Add uninstall information to Add/Remove Programs
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayName" "${PRODUCT_NAME}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayIcon" "$INSTDIR\bin\butil-ui.Desktop.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "HelpLink" "${PRODUCT_WEB_SITE}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "URLUpdateInfo" "${PRODUCT_WEB_SITE}"
  
  ; Create shortcuts
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\${PRODUCT_NAME}.lnk" "$INSTDIR\bin\butil-ui.Desktop.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
  
  ; Create desktop shortcut
  CreateShortcut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\bin\butil-ui.Desktop.exe"
SectionEnd

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
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  DeleteRegKey HKCU "Software\${PRODUCT_NAME}"
  
  SetAutoClose true
SectionEnd
