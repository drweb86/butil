# Scheduler (Automation)

BUtil integrates with the operating system scheduler under the **current user account**.

- On **Windows**, when you fill in scheduled days and time in the **When?** section of a task, the application creates or updates a dedicated entry in the **Windows Task Scheduler**.
- On **Linux**, the same UI updates the current user's **crontab**.

The user must be logged in at the scheduled time for the task to actually run (a scheduled task does not wake the machine and is not a system-level service).

> **Sometimes you want to integrate the backup/synchronization process with your own scheduler or with another tool.** This page describes how to do that and what each scheduled command line looks like.

## What the application schedules

The scheduler always launches the **console** binary (not the UI) and passes a `Task=...` argument identifying which task to run.

| Platform | Example | Description |
| --- | --- | --- |
| Windows (Installer) | `"C:\Program Files\BUtil\bin\butilc.exe" "Task=My task name" HideConsole` | Written to Windows Task Scheduler. |
| Windows (Store) | `"%LOCALAPPDATA%\Microsoft\WindowsApps\SiarheiKuchuk.BUtil.Console.exe" "Task=My task name" HideConsole` | Written to Windows Task Scheduler. |
| Linux | `"/usr/local/butil/butilc" "Task=My task name"` | Written to the current user's crontab. |

A Store update installs the app into a new folder. The Store command uses an app execution alias, so that path stays valid. Open BUtil once after installing the version that adds the alias. It rewrites existing Start menu shortcuts and scheduled tasks. Later Store updates leave those entries working.

The arguments mean:

| Argument                | Description                                                                                              |
| ----------------------- | -------------------------------------------------------------------------------------------------------- |
| `Task=<name>`           | Required. Name of the task to run, exactly as it appears in the application. Quote it if it has spaces.  |
| `HideConsole`           | Windows-only. Hides the console window so the task runs unobtrusively in the background.                 |
| `Shutdown` / `LogOff` / `Reboot` | Optional. Performs the corresponding power action after the task finishes successfully.          |

`Shutdown`, `LogOff`, or `Reboot` after the task name powers off, signs out, or reboots the machine after the task finishes successfully. `LogOff` and `Reboot` are written the same way as `Shutdown`.

| Platform | Example | Description |
| --- | --- | --- |
| Windows (Installer) | `"C:\Program Files\BUtil\bin\butilc.exe" "Task=My task name" Shutdown` | Executes the task and shutdowns the PC. |
| Windows (Store) | `"%LOCALAPPDATA%\Microsoft\WindowsApps\SiarheiKuchuk.BUtil.Console.exe" "Task=My task name" Shutdown` | Executes the task and shutdowns the PC. |
| Linux | `"/usr/local/butil/butilc" "Task=My task name" Shutdown` | Executes the task and shutdowns the PC. |

See [Command line](./Command%20line.md) for the full set of supported console arguments.

## Integrating with Windows Task Scheduler manually

If you want to use the Windows Task Scheduler directly (or manage scheduling outside of BUtil), create a task with the following:

1. Open **Task Scheduler** and create a new task.
2. Set it to run **Whether the user is logged on or not** if you want it to run while logged out (this requires entering the user's password and is subject to your organization's policies).
3. Add an **Action**. Replace `My task name` with the actual task name. `HideConsole` is optional and keeps the run in the background on Windows.

| Platform | Program/script | Arguments | Start in |
| --- | --- | --- | --- |
| Windows (Installer) | `C:\Program Files\BUtil\bin\butilc.exe` | `"Task=My task name" HideConsole` | BUtil `bin` folder |
| Windows (Store) | `%LOCALAPPDATA%\Microsoft\WindowsApps\SiarheiKuchuk.BUtil.Console.exe` | `"Task=My task name" HideConsole` | `%LOCALAPPDATA%\Microsoft\WindowsApps` |

4. Configure triggers (time, days, repetition) according to your needs.

## Integrating with cron on Linux

The application writes its own cron entries under the current user's crontab (`crontab -e`), each marked with a `# BUtil Schedule: <task name>` comment line above the actual cron line. If you want to manage scheduling manually, simply add a cron line of your own pointing to the console binary:

```
30 22 * * 1,2,3,4,5 "/usr/local/butil/butilc" "Task=My task name"
```

The five fields before the command are standard cron: minute, hour, day-of-month, month, day-of-week.

## Notes

- The task name in the command line must exactly match the task name in the application (case-sensitive on Linux).
- If you rename a task in the application, BUtil rewrites the corresponding scheduler entry automatically; manually-created entries are not touched.
- The application's UI will only display schedules it created itself (those that match its own format). Manual entries are still executed by the OS but will not appear in the **When?** section.
