# Synchronization

> Synchronization can result in data loss when the same file is changed on several devices between runs - the older edit will be overwritten by the newer one. The good news is that the centralized storage keeps **versioned** copies of every change, so any earlier version of any file can be restored later. Always keep an independent backup of important files anyway.

BUtil synchronization keeps a single folder consistent across multiple devices through a shared remote storage. This document explains, at a high level, how it works and what you have to configure on the server side to make it function.

## When to use synchronization

Use a **Synchronization** task when you want the same set of files to be available and editable from several computers (for example, a desktop and a laptop), and you want changes done on one device to propagate to the others on the next run.

If you only need to protect data against loss without sharing it between devices, use [Incremental backup](./Backup%20Format.md) instead.

## High-level overview

```
   +------------+        +------------+        +------------+
   |  Device A  |        |  Device B  |        |  Device C  |
   | local copy |        | local copy |        | local copy |
   +-----+------+        +------+-----+        +-----+------+
         |                      |                    |
         |     two-way sync     |     two-way sync   |
         +------+---------------+--------+-----------+
                |                        |
                v                        v
              +---------------------------+
              |   Centralized storage     |
              |  (shared by all devices)  |
              |  Folder / SMB / SFTP /    |
              |          FTPS             |
              |                           |
              | Versioned, encrypted      |
              +---------------------------+
```

Each device keeps a local copy of the synchronized folder and runs the same Synchronization task pointed at the **same centralized storage**. On every run BUtil:

1. Reads the **state of remote files** from the centralized storage.
2. Reads the **state of local files** in the configured local folder.
3. Reads the **last known synchronized state** persisted locally from the previous run.
4. Compares all three to decide, for each file, whether to:
   - download it from the remote storage to the local folder,
   - upload it to the remote storage from the local folder,
   - delete it locally,
   - delete it remotely,
   - or do nothing.
5. Applies the resulting actions and saves the new local synchronized state.

## Versioning and restoring previous versions of files

The centralized storage uses the same versioned [incremental backup format](./Backup%20Format.md) as a regular backup task. Every successful synchronization run creates a new version on the storage that records the differences from the previous version. Files are deduplicated and encrypted with the task password.

This means that even though synchronization can overwrite or delete files (locally or remotely), **no version is lost on the centralized storage**. From any device you can:

1. Open the application.
2. Go to **Restore** for the synchronization task.
3. Pick any previous version from the **Version** drop-down.
4. Browse the file tree, select a file or folder and click **Restore** to copy it back out.

So even if a wrong file wins a conflict, or a deletion propagates incorrectly, you can always recover the previous content from any earlier version on the centralized storage. Old versions can also be deleted from the **Restore** view to reclaim space.

## Synchronization modes

A Synchronization task supports two modes (configured in the **What?** section):

- **Two-way** - changes made locally are uploaded, changes made remotely are downloaded, deletions on either side propagate to the other side. This is the typical mode for sharing a working folder between several of your own devices.
- **Read** - only changes from the centralized storage are downloaded. Local changes are not pushed out and may be overwritten or removed when the remote side reports changes or deletions. Use this mode when one device is the authoritative source and other devices should only consume.

## Conflict resolution

When the same file has been changed on more than one device since the last successful synchronization, BUtil resolves the conflict automatically by **most recent modification time**. The older version is overwritten - but, as noted above, the overwritten content is still available as a previous version on the centralized storage and can be restored from any device.

If you cannot afford automatic conflict resolution, do not edit the same file from multiple devices between sync runs.

## What to configure

### On every device that participates

In the **Synchronization task**:

1. **Name** - any name; it does not need to match across devices.
2. **What?**
   - **Folder** - the local folder that will be synchronized. It can be different per device (for example `C:\Users\alice\Sync` on one machine and `/home/alice/Sync` on another).
   - **Synchronization mode** - typically **Two-way** (see also [Setting up the second device](#setting-up-the-second-device-and-beyond) below).
3. **When?** - the schedule. All devices should run reasonably often so changes have a chance to converge.
4. **Where?** - the centralized storage. **All devices must point to the same physical location**: same FTPS/SFTP server and folder, same SMB share, same shared/cloud-mounted folder, etc. If devices point to different folders they will not see each other's changes.
5. **Encryption** - if a password is set, the **same password must be used on every device**. Files in the centralized storage are encrypted; without the matching password the other devices cannot decrypt them.

### On the server / shared storage side

The centralized storage is just a place every device can reach. BUtil itself does not run a synchronization service - it relies on whatever transport you configure. Pick one of:

- **Folder** - any directory that every device can mount (for example a NAS share mounted as a drive, an OS-level cloud-drive folder such as OneDrive/Google Drive, or a network drive). Make sure every device has read/write access and that the folder really is the *same* folder on the storage and not a per-device copy.
- **SMB / CIFS** - a shared folder on a NAS, Windows file server, or Samba server. Configure a user that has read/write access to the share and use that user/password in the **Where?** section.
- **SFTP** - any SSH/SFTP server (Linux server, NAS with SSH enabled, etc.). Create an account with read/write access to a dedicated synchronization folder. SFTP server's host key fingerprint will be detected on the first test connection and you must verify it out of band before saving.
- **FTPS** - any FTPS server (for example you can host one yourself with the **BUtil Server** task type). Create a user with read/write access to a dedicated synchronization folder. The server certificate will be detected on the first test connection and you must verify it before saving.

What you typically have to do once on the storage side:

1. Provision an account or share with **read and write** access.
2. Create a **dedicated, empty folder** for the synchronization task (do not point several different sync tasks at the same folder).
3. Make sure the storage has enough free space for the data plus version history.
4. For SFTP and FTPS - keep the host key / certificate stable. If it changes, every device will refuse to connect until you re-verify the new fingerprint/certificate.

### What you do *not* need to do

- You do not need to install BUtil on the server. The server only has to expose a folder over a supported transport (Folder/SMB/SFTP/FTPS).
- You do not need to set up replication or merging on the server. All of the synchronization logic runs on the devices.
- You do not need a fixed primary device - in **Two-way** mode all devices are equal.

## Setting up the second device and beyond

The very first run on a brand-new device is the most dangerous one, because the device has no record of what has been synchronized before. Follow this two-step procedure to avoid accidentally overwriting either side:

1. **First run as Read mode.** When you create the synchronization task on a new device, especially when **the local folder is not empty**, configure **Synchronization mode = Read** in the **What?** section. Run the task once. This will:
   - download the current state of the centralized storage into the local folder,
   - never upload anything from the local folder,
   - never delete anything that exists on the centralized storage (because the local side cannot push changes in Read mode).

   At this point the local folder reflects what is on the centralized storage. Inspect it and verify everything looks right.

2. **Switch to Two-way.** Once you are happy with the result of the first run, edit the task and change **Synchronization mode** to **Two-way**. From this run on, local changes will start being uploaded and deletions will propagate normally.

If the local folder is **empty** when you create the task on the new device, you can skip the Read step and create the task directly in **Two-way** mode - there is nothing local that could be overwritten or pushed unexpectedly.

## First run

On the very first run on a device, the local synchronized state does not yet exist. BUtil treats every file present in the centralized storage as something to download and, in **Two-way** mode, every local file that is not yet remote as something to upload. Expect the first run to take longer than later, incremental runs.

If a device has only just been added and you want it to start from a clean state, start with an empty local folder so that nothing local is uploaded by accident, or follow the [second-device procedure](#setting-up-the-second-device-and-beyond) above.

## Behavior summary

- When a previous remote upload was interrupted, the next run on any device will retry the upload.
- When a device has never completed a download, the run will overwrite the local files with the centralized state.
- Once the local state matches the remote state, two-way reconciliation is performed.
- Conflicts are resolved by the most recent modification timestamp; the overwritten content remains restorable from previous versions on the centralized storage.

## Encryption

Files stored in the centralized storage use the same encrypted [incremental backup format](./Backup%20Format.md) as regular backups. Each device must use the same password configured in the **Encryption** section to be able to read what other devices have uploaded.

## Limitations and warnings

- Synchronization can result in data loss for an individual run when the same file is edited on multiple devices simultaneously - the older edit is overwritten by the newer one. The overwritten content is still recoverable from previous versions on the centralized storage via the **Restore** view.
- Deletions propagate. Removing a file on one device and running the task will eventually remove that file from every other device. Deleted files remain available in older versions until those versions are deleted from the centralized storage.
- For mission-critical data, also keep an independent [backup](./Backup%20Format.md) of important files - synchronization is not a substitute for a backup.
