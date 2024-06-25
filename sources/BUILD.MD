# Prerequisites

- Windows 11x64 Professional;
- Microsoft Visual Studio 2022;
- AMD architecture

# Packaging

## Windows

### Prerequisites

- InnoSetup 6.3.1 (must be installed in default directory);
- Chinese simplified language
File: https://github.com/kira-96/Inno-Setup-Chinese-Simplified-Translation/blob/main/ChineseSimplified.isl
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\ChineseSimplified.isl
- Arabic language
File: https://github.com/parmaja/innosetup/blob/master/languages/Arabic.isl
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Arabic.isl
- Hindi language
File: https://jrsoftware.org/files/istrans/; search for Hindi
To be put into
C:\Program Files (x86)\Inno Setup 6\Languages\Hindi.islu
- 7-zip x64 (must be installed in default directory).
- .Net 8 SDK

### Build.cmd

Update version (it should match the version name and tag of release!) in Version History file. 

Launch build.cmd.

It will create <root>\output folder with all compiled.
Special note about Ubuntu-ARM64: since snapcraft require hardware to build for this platform, it won't be available.
