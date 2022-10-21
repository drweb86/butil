# Scheduler Tray Application

[Scheduler](https://en.wikipedia.org/wiki/Scheduling_(computing)) can run in 2 modes:
- **Hidden**. In this mode program takes less system resources but does not provide additional functions such as Run or stop backup from context menu of tray icon, information about time to backup, warning message 8 minutes before backup.
- **Normal**

Scheduling application stops / restarts automatically when you changed options in configurator(except cases when you enabled option 'don't care about scheduler startup'). If there's no scheduled days for backup/file with options it closes itself automatically.

Scheduler is a separate component of application: it's installation is configured during setup process. In zipped binaries it's included also. You can remove it. After removal scheduling will be unavailable

Tray Application provides next functions:
- scheduling,
- starting/stopping backup,
- exiting,
- showing time remained to the next scheduled backup,
- showing popup in 8 minutes before backup,
- showing popup that backup was started.

## How To Kill Application When It's In She Hidden Mode?
- Press Ctrl+Alt+Del;
- Go to **Processes** tab;
- Kill 'Butil.Ghost.exe' process.

## Block-diagramm of scheduler processing

Diagram is shown on picture 1.
![Image 1 - Scheduler Block Diagram](./Image%201%20-%20Scheduler%20Block%20Diagram.png)
Image 1 - Scheduler Block Diagram

## Command Line

### Usage
BUtil.Backup.Ghost.exe 'argument'

### Command Line Arguments:

#### Without arguments
The standard run of scheduler with message that scheduler is running for 10 seconds visible.

#### StartWithoutMessage
Starts without first message.

#### CreateStartupScript
Creates startup script in ```%userprofile%\MainMenu\Programs\Startup``` and closes the application. Internal command. Subject to change. Do not use it.

#### RemoveStartupScriptIfAny
Removes startup script from ```%userprofile%\MainMenu\Programs\Startup```. Internal command. Subject to change. Do not use it.
