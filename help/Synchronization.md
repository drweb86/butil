# Synchronization

| Under development

| Beta version, may result in data loss.

File synchronization allows you to synchronize files between your various devices as part incremental backup.

## Behavior

When remote upload was not finished from within any of devices an attempt to upload data will be performed.

When synchronization download is not performed, it will be executed overwriting local files.

When remote upload exists and local synchronization is fully performed, two-way sync is performed.

Conflict resolution is automatic by latest modification date.

Be aware that during synchronization files overwrite, deletion happens. Software may contain errors that might result in data corruption or loss.