# UI

| Example                                                                                                                                                                    | Description                |
| -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------- |
| Windows:<br />`butil-ui.Desktop.exe LaunchTask "Task=My task name"`<br />Linux:<br />`systemd-inhibit /usr/local/butil/butil-ui.Desktop LaunchTask "Task=My task name"  &` | Launches specified task.   |

# Console

| Example                                                                                                                                           | Description                                     |
| ------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------- |
| Windows:<br />`butilc.exe "Task=My task name"`<br />Linux:<br />`systemd-inhibit /usr/local/butil/butilc "Task=My task name"`                     | Executes the task.                              |
| Windows:<br />`butilc.exe "Task=My task name" Shutdown`<br />Linux:<br />`systemd-inhibit /usr/local/butil/butilc "Task=My task name" Shutdown`   | Executes the task and shutdowns the PC.         |
| Windows:<br />`butilc.exe "Task=My task name" LogOff`<br />Linux:<br />`systemd-inhibit /usr/local/butil/butilc "Task=My task name" LogOff`       | Executes the task and ends the user session.    |
| Windows:<br />`butilc.exe "Task=My task name" Reboot`<br />Linux:<br />`systemd-inhibit /usr/local/butil/butilc "Task=My task name" Reboot`       | Executes the task and reboots the PC.           |
