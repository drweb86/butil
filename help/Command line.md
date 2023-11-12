# UI

| Example                                                                                                                                             | Description                                                              |
| --------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------ |
| Windows:<br />`butil-ui.Desktop.exe LaunchTask "Task=My task name"`<br />Linux:<br />`dotnet ./butil-ui.Desktop.dll LaunchTask "Task=My task name"` | Launches specified task.                                                 |
| Windows:<br />`butil-ui.Desktop.exe Restore`<br />Linux:<br />`dotnet ./butil-ui.Desktop.dll Restore`                                               | Restores from backup with selection of task or backup source on startup. |
| Windows:<br />`butil-ui.Desktop.exe Restore "Task=My task name"`<br />Linux:<br />`dotnet ./butil-ui.Desktop.dll Restore "Task=My task name"`       | Runs restoration and prefills Storage/Passwords from specified task.     |

# Console

| Example                                                                                                                     | Description                                           |
| --------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------- |
| Windows:<br />`butilc.exe "Task=My task name"`<br />Linux:<br />`dotnet ./butilc.dll "Task=My task name"`                   | Executes the task.                                    |
| Windows:<br />`butilc.exe "Task=My task name" Shutdown`<br />Linux:<br />`dotnet ./butilc.dll "Task=My task name" Shutdown` | Executes the task and shutdowns the PC.               |
| Windows:<br />`butilc.exe "Task=My task name" LogOff`<br />Linux:<br />`dotnet ./butilc.dll "Task=My task name" LogOff`     | Executes the task and ends the user session.          |
| Windows:<br />`butilc.exe "Task=My task name" Reboot`<br />Linux:<br />`dotnet ./butilc.dll "Task=My task name" Reboot`     | Executes the task and reboots the PC.                 |