# Scheduler (Automation)

BUtil integrates with the operating system scheduler under the **current user account**.

- On **Windows**, when you fill in scheduled days and time in the **When?** section of a task, the application creates or updates a dedicated entry in the **Windows Task Scheduler**.
- On **Ubuntu / Linux**, the same UI updates the current user's **crontab**.

The user must be logged in at the scheduled time for the task to actually run (a scheduled task does not wake the machine and is not a system-level service).

> **Sometimes you want to integrate the backup/synchronization process with your own scheduler or with another tool.** This page describes how to do that and what each scheduled command line looks like.

## What the application schedules

The scheduler always launches the **console** binary (not the UI) and passes a `Task=...` argument identifying which task to run.

- Windows console binary: `butilc.exe`, located in the BUtil installation folder (typically `%ProgramFiles%\BUtil\bin\butilc.exe`).
- Linux console binary: `butilc`, located in the BUtil installation folder (typically `/usr/local/butil/butilc`).

The exact command line written to Windows Task Scheduler is:

```
"C:\Program Files\BUtil\bin\butilc.exe" "Task=My task name" HideConsole
```

The exact command line written to crontab on Linux is:

```
"/usr/local/butil/butilc" "Task=My task name"
```

The arguments mean:

| Argument                | Description                                                                                              |
| ----------------------- | -------------------------------------------------------------------------------------------------------- |
| `Task=<name>`           | Required. Name of the task to run, exactly as it appears in the application. Quote it if it has spaces.  |
| `HideConsole`           | Windows-only. Hides the console window so the task runs unobtrusively in the background.                 |
| `Shutdown` / `LogOff` / `Reboot` | Optional. Performs the corresponding power action after the task finishes successfully.          |

See [Command line](./Command%20line.md) for the full set of supported console arguments.

## Integrating with Windows Task Scheduler manually

If you want to use the Windows Task Scheduler directly (or manage scheduling outside of BUtil), create a task with the following:

1. Open **Task Scheduler** and create a new task.
2. Set it to run **Whether the user is logged on or not** if you want it to run while logged out (this requires entering the user's password and is subject to your organization's policies).
3. Add an **Action**:
   - **Program/script:** the full path to `butilc.exe` (for example `C:\Program Files\BUtil\bin\butilc.exe`).
   - **Add arguments:** `"Task=My task name" HideConsole` (replace `My task name` with the actual task name; `HideConsole` is optional but recommended for background runs on Windows).
   - **Start in:** the BUtil `bin` folder.
4. Configure triggers (time, days, repetition) according to your needs.

Tip: you can append `Shutdown`, `LogOff` or `Reboot` after the task name to power-off, sign-out or reboot the machine after the task finishes successfully:

```
"C:\Program Files\BUtil\bin\butilc.exe" "Task=My task name" Shutdown
```

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
