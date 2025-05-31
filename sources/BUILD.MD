# Prerequisites

- Windows 11x64 Professional;
- Microsoft Visual Studio 2022;
- AMD architecture

# Packaging

## Windows

### Prerequisites

- InnoSetup 6.3.1 (must be installed in default directory);
- Chinese simplified language
File: https://raw.githubusercontent.com/kira-96/Inno-Setup-Chinese-Simplified-Translation/refs/heads/main/ChineseSimplified.isl
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\ChineseSimplified.isl
- Arabic language
File: https://github.com/parmaja/innosetup/raw/refs/heads/master/languages/Arabic.isl
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Arabic.isl
- Hindi language
File: https://raw.githubusercontent.com/jrsoftware/issrc/main/Files/Languages/Unofficial/Hindi.islu
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Hindi.islu
- Bengali language
File: https://raw.githubusercontent.com/jrsoftware/issrc/main/Files/Languages/Unofficial/Bengali.islu
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Bengali.islu
- Urdu language
File: https://raw.githubusercontent.com/jrsoftware/issrc/main/Files/Languages/Unofficial/Urdu.isl
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Urdu.isl
- Indonesian
File: https://raw.githubusercontent.com/jrsoftware/issrc/main/Files/Languages/Unofficial/Indonesian.isl
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Indonesian.isl
- Marathi
File: https://raw.githubusercontent.com/jrsoftware/issrc/main/Files/Languages/Unofficial/Marathi.islu
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Marathi.islu
- Tamil
File: https://raw.githubusercontent.com/jrsoftware/issrc/refs/heads/main/Files/Languages/Tamil.isl
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Tamil.isl
- Vietnamese
File: https://raw.githubusercontent.com/jrsoftware/issrc/refs/heads/main/Files/Languages/Unofficial/Vietnamese.isl
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Vietnamese.isl
- .Net 9 SDK
- 'https://github.com/drweb86/winget-pkgs' checkouted to '../winget-pkgs' folder

### Build.cmd

Update version (it should match the version name and tag of release!) in Version History file. 

Update version in the ubuntu-install.sh script

Launch build.cmd.

It will create <root>\output folder with all compiled.
