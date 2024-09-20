# Where 7-zip Package is Being Searched For

Package should installed in
- Program Files (Windows) or
- bin\7-zip folder (Windows)
- be in any path that is in PATH environment variable (Linux).

# Ideas behind implementation

## Archiver selection

It has best in the world compression ration and speed.

## 7z is used instead of ZIP format for compression

Why Zip format is not used and app relies on external 7-ZIP? It's because while ZIP is part of .Net and has popular implementations, it does not have encryption of file names. That's why in password protected archives file names in ZIP are exposed. In 7-zip file names are also encrypted.

## 7z is included in the package

Reason for that is broken by MS support for Win-Get for unauthorized in Store users to download any software.

## Why archiver was removed ?

Reason for that, that each file is compressed individually and gain of space was just 7%. Also nowadays stored files are already compressed in most cases. Also 7-zip was not cross-platform.