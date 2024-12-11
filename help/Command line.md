# UI

| Example                                                                                                                                                                | Description                                                              |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------ |
| Windows:<br />`butil-ui.Desktop.exe LaunchTask "Task=My task name"`<br />Ubuntu Store Snap:<br />`systemd-inhibit butil.ui LaunchTask "Task=My task name"  &`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butil-ui.Desktop.dll LaunchTask "Task=My task name"  &` | Launches specified task.                                                 |

# Console

| Example                                                                                                                                     | Description                                           |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------- |
| Windows:<br />`butilc.exe "Task=My task name"`<br />Ubuntu Store Snap:<br />`systemd-inhibit butil.cli "Task=My task name"`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butilc.dll "Task=My task name"`                   | Executes the task.                                    |
| Windows:<br />`butilc.exe "Task=My task name" Shutdown`<br />Ubuntu Store Snap:<br />`systemd-inhibit butil.cli "Task=My task name" Shutdown`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butilc.dll "Task=My task name" Shutdown` | Executes the task and shutdowns the PC.               |
| Windows:<br />`butilc.exe "Task=My task name" LogOff`<br />Ubuntu Store Snap:<br />`systemd-inhibit butil.cli "Task=My task name" LogOff`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butilc.dll "Task=My task name" LogOff`     | Executes the task and ends the user session.          |
| Windows:<br />`butilc.exe "Task=My task name" Reboot`<br />Ubuntu Store Snap:<br />`systemd-inhibit butil.cli "Task=My task name" Reboot`<br />Linux binaries package:<br />`systemd-inhibit dotnet ./butilc.dll "Task=My task name" Reboot`     | Executes the task and reboots the PC.                 |