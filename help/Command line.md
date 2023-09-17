# UI Application (butil.exe)

| Example                                               | Description                                                                                                       |
| ----------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| `butil.exe Restore`                                   | Restores from backup with selection of task or backup source on startup.                                          |
| `butil.exe Restore "Task=My task name"`               | Runs restoration and prefills Storage/Passwords from specified task.                                              |
| `butil.exe LaunchTask`                        	| Launches task with selection of task on startup.                                                                  |
| `butil.exe LaunchTask "Task=My task name"`            | Launches specified task.                                                                                          |
| `butil.exe RemoveLocalSettings`                       | Removes configuration data of current user. It can be used for resetting settings. Also this removes logs folder. |

# Console (butilc.exe)

| Example                                   | Description                                           |
| ----------------------------------------- | ----------------------------------------------------- |
| `butilc.exe Help`                         | Shows brief help about all commands.                  |
| `butilc.exe "Task=My task name"`          | Executes the task.                                    |
| `butilc.exe "Task=My task name" Shutdown` | Executes the task and shutdowns the PC.               |
| `butilc.exe "Task=My task name" LogOff`   | Executes the task and ends the user session.          |
| `butilc.exe "Task=My task name" Reboot`   | Executes the task and reboots the PC.                 |