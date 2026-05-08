# NFS Storage Setup

NFS (Network File System) lets BUtil back up directly to a network-attached file server. It is common in Linux-heavy and enterprise environments with NAS devices (Synology, QNAP, TrueNAS, etc.).

## Prerequisites

### Linux

- The `nfs-common` package must be installed:
  ```
  sudo apt install nfs-common      # Debian / Ubuntu
  sudo dnf install nfs-utils       # Fedora / RHEL
  ```
- The `mount` command requires **root privileges**. Either:
  - Run BUtil as root, or
  - Add a sudoers rule to allow passwordless `mount`/`umount` for your user, or
  - Pre-mount the share in `/etc/fstab` with the `user` option and enter the pre-mounted directory path as Mount Point.

### Windows

- Enable the **NFS Client** Windows feature:
  - **Settings → Optional features → Add a feature → NFS Client** (Windows 11), or
  - Via PowerShell (as Administrator):
    ```
    Enable-WindowsOptionalFeature -Online -FeatureName "ServicesForNFS-ClientOnly" -All
    ```
- After enabling, `mount.exe` and `umount.exe` will be present in `C:\Windows\System32\`.
- If the NFS Client feature is not installed, the NFS storage type will not appear in BUtil's storage picker.

---

## Fields

| Field (Linux) | Field (Windows) | Description |
|---|---|---|
| **Host** | **Host** | Hostname or IP address of the NFS server. |
| **Share Path** | **Share Path** | The exported path on the server, e.g. `/export/backups`. |
| **Mount Point** | **Drive Letter** | Linux: local directory where the share is mounted (e.g. `/mnt/nfs-backup`). Windows: drive letter to assign (e.g. `Z:`). |
| **Mount Options** | **Mount Options** | Optional. Passed as `-o <options>` to the mount command. |

---

## Example — Linux

| Field | Value |
|---|---|
| Host | `192.168.1.10` |
| Share Path | `/volume1/backups` |
| Mount Point | `/mnt/nas-backup` |
| Mount Options | `vers=4,rw,soft,timeo=30` (optional) |

The mount directory is created automatically if it does not exist.

---

## Example — Windows

| Field | Value |
|---|---|
| Host | `192.168.1.10` |
| Share Path | `/volume1/backups` |
| Drive Letter | `Z:` |
| Mount Options | `anon` (optional — for anonymous access) |

The equivalent command run internally is:
```
mount \\192.168.1.10\volume1\backups Z:
```

---

## Exporting an NFS Share on Common NAS Systems

### Synology DSM

1. **Control Panel → File Services → NFS** → enable NFS.
2. Open **Shared Folder** → select your folder → **Edit → NFS Permissions** → **Create**.
3. Set the client IP/subnet, permission *Read/Write*, Squash *No mapping* (or *Map all users to admin* for simple setups).

### TrueNAS

1. **Sharing → Unix Shares (NFS)** → **Add**.
2. Set the dataset path, restrict to your client IP under **Advanced → Hosts**.

### QNAP

1. **Control Panel → Network & File Services → Win/Mac/NFS/WebDAV** → NFS tab → enable.
2. Open **Shared Folders** → select folder → **Edit Shared Folder NFS Privileges**.

---

## Useful Mount Options

| Option | Meaning |
|---|---|
| `vers=4` | Force NFSv4 (recommended; more reliable over WAN). |
| `vers=3` | Force NFSv3 (needed if the server doesn't support v4). |
| `rw` | Read/write (default). |
| `soft` | Return an error after a timeout instead of hanging forever. |
| `timeo=30` | Timeout in tenths of a second before retrying (e.g. `30` = 3 s). |
| `rsize=1048576,wsize=1048576` | Larger transfer blocks for better throughput on fast networks. |

---

## Troubleshooting

- **"Cannot mount NFS share"** — check that the server exports the path and the client IP is permitted. Test with `showmount -e <host>` on Linux.
- **Permission denied** — the NFS server's squash or permission settings are blocking writes. Adjust the NFS export ACL.
- **Stale file handle** — the mount became stale (server rebooted). Stop the task, the drive/mountpoint will be unmounted, and restart.
- **Windows: mount not found** — the NFS Client Windows feature is not installed. See Prerequisites above.
- **Linux: operation not permitted** — `mount` requires elevated permissions. See Prerequisites above.
