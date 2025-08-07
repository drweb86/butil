# Incremented Backup Format

Incremented backup is compressed encrypted.

During incremented backup we do:
- get state of files (file, its size and hash);
- get state of storage (last saved version);
- calculate difference between files in storage and file states (difference is separately calculated for each storage);
- upload files;
- upload version state;
- upload verification script.

## Compressed

Structure of files in storage:
- State file;
- Verification script;
- Folder with version;
- Folder with version contains files with random guids instead of names in .aes256 format.

State file is compressed with Brotli and encrypted with specified password. Each stored file is encrypted with unique password stored in state file.
