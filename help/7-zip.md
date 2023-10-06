# Where 7-zip Package is Being Searched For

Package should installed in
- Program Files or
- Program Files (x86) or
- be in any path that is in PATH environment variable.

# Ideas behind implementation

## Archiver selection

It has best in the world compression ration and speed.

## 7z is used instead of ZIP format for compression

Why Zip format is not used and app relies on external 7-ZIP? It's because while ZIP is part of .Net and has popular implementations, it does not have encryption of file names. That's why in password protected archives file names in ZIP are exposed. In 7-zip file names are also encrypted.