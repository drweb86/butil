# Prerequisites

- Windows 11x64 Professional;
- Microsoft Visual Studio 2022;
- AMD architecture

# Packaging

## Windows

### Prerequisites

- InnoSetup 6.2.2 (must be installed in default directory);
- 7-zip x64 (must be installed in default directory).
- .Net 8 SDK

### Build.cmd

Update version (it should match the version name and tag of release!) in Version History file. 

Launch build.cmd.

It will create <root>\output folder with all compiled.
Special note about Ubuntu-ARM64: since snapcraft require hardware to build for this platform, it won't be available.
