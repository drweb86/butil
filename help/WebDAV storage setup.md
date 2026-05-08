# WebDAV Storage Setup

WebDAV is a widely supported protocol for remote file access. It is the transport used by **Nextcloud**, **ownCloud**, **Synology DSM**, **Seafile**, many enterprise web servers, and most cloud providers that expose a "files" interface over HTTPS.

## Fields

| Field | Description |
|---|---|
| **Host** | Hostname or IP address of the WebDAV server (no scheme, no port). |
| **HTTPS** | Use HTTPS (recommended). Set to *No* only for local testing. |
| **Port** | Leave at `0` for the default port (443 for HTTPS, 80 for HTTP). Set explicitly only when the server runs on a non-standard port. |
| **Base Path** | The URL path to the folder used for backups. See per-server examples below. |
| **User** | Username / account name. |
| **Password** | Password or app-specific password. |

---

## Nextcloud / ownCloud

1. Open Nextcloud and create a dedicated folder for backups, e.g. `Backups`.
2. In BUtil, set:
   - **Host**: `your-nextcloud.example.com`
   - **Base Path**: `/remote.php/dav/files/<username>/Backups`
     Replace `<username>` with your Nextcloud login name.
   - **User / Password**: your Nextcloud credentials (or an [App Password](https://docs.nextcloud.com/server/latest/user_manual/en/session_management.html#managing-devices) — recommended).

> **Tip:** App passwords avoid exposing your main password and can be revoked independently. Generate one at **Settings → Security → Devices & sessions**.

---

## Synology DSM

1. Enable WebDAV in **Control Panel → File Services → WebDAV**.
2. Create a shared folder (or subfolder) for backups.
3. In BUtil, set:
   - **Host**: your NAS hostname or IP
   - **Port**: `5006` (HTTPS) or `5005` (HTTP) — Synology defaults
   - **Base Path**: `/`  (or `/subfolder` if you want a specific directory)
   - **User / Password**: your DSM account credentials

---

## Seafile

Seafile exposes library contents over WebDAV.

- **Base Path**: `/seafdav`
- Authenticate with your Seafile username and password (or an API token as password).

---

## Generic / Other Servers

Use whatever path the server exposes. A quick test: open `https://<host>/<base-path>/` in a browser — if you get a 401 (auth required) or a directory listing, the path is correct.

---

## Troubleshooting

- **401 Unauthorized** — wrong username or password. Try an app password if the server supports 2FA.
- **404 Not Found** — the Base Path is wrong. Check that the folder exists and the path matches exactly (case-sensitive on Linux servers).
- **SSL / certificate errors** — if the server uses a self-signed certificate, the connection will fail. Use a trusted certificate or a reverse proxy with a valid certificate (e.g. Let's Encrypt).
- **Slow uploads** — WebDAV upload speed is bounded by the server's bandwidth. No parallel uploads are used for correctness.
