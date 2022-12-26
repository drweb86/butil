# Incremented Backup Format

2 models of incremented backup are supported:
- non-encrypted non-compressed;
- compressed (encrypted or not encypted).

During incremented backup we do:
- get state of files (file, its size and hash);
- get state of storage (last saved version);
- calculate difference between files in storage and file states (difference is separately calculated for each storage);
- upload files;
- upload version state;
- upload verification script.

## Non-compressed

Structure of files in storage:
- State file;
- Verification script;
- Folder with version;
- Folder with version contains:
a) source item + unique id with
b) files located in relative to source item location paths.

Files are stored without encryption because encryption is part of 7-zip.

Files are stored with relative paths to promote trust to backup process and make possible manual recovery.

## Compressed

Structure of files in storage:
- State file;
- Verification script;
- Folder with version;
- Folder with version contains files with random guids instead of names in 7z format.

If encryption is used, then 7z encryption of headers and content is used.

If encryption is used, state file is encrypted with specified password. Each stored file is encrypted with unique password stored in state file.

Compression degrees are predefined for some formats. For other formats - ultra.