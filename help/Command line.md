# UI

| Example                                                                                                                                                                | Description                                                              |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------ |
| Windows:<br />`butil-ui.Desktop.exe LaunchTask "Task=My task name"`<br />Ubuntu Store Snap:<br />`systemd-inhibit butil.ui LaunchTask "Task=My task name"  &`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butil-ui.Desktop.dll LaunchTask "Task=My task name"  &` | Launches specified task.                                                 |

# Console

| Example                                                                                                                                     | Description                                           |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------- |
| Windows:<br />`butilc.exe "Task=My task name"`<br />Ubuntu Store Snap:<br />`systemd-inhibit butil.cli "Task=My task name"`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butilc.dll "Task=My task name"`                   | Executes the task.                                    |
| Windows:<br />`butilc.exe "Task=My task name" Shutdown`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butilc.dll "Task=My task name" Shutdown` | Executes the task and shutdowns the PC.               |
| Windows:<br />`butilc.exe "Task=My task name" LogOff`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butilc.dll "Task=My task name" LogOff`     | Executes the task and ends the user session.          |
| Windows:<br />`butilc.exe "Task=My task name" Reboot`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butilc.dll "Task=My task name" Reboot`     | Executes the task and reboots the PC.                 |

## Sending and Receiving folder across intranet

AES-256 encryption is used.
Existing files are not copied if they are the same as on target machine.
Files are overwrited.

First start server on machine that should receive the folder.
```
butilc receive "my incoming folder" 999 "my password"
```
Where 
- "my incoming folder" is your folder for receiving files.
- 999 - port
- "my password" is your password.

Program will print all IPs at local machine its listening on.


Now start client on machine from which you want to upload files.
```
butilc send "my upload folder" 127.0.0.1 999 "my password"
```
Where 
- "my upload folder" is your folder for uploading to server.
- 127.0.0.1 - is your server IP (that server part is printed after start)
- 999 - server port
- "my password" is your password.
