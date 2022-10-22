**Sometimes you wanna integrate backup process with your own scheduler**

# Integrating BUtil Scheduler With Windows Scheduler

Standard startup way of Scheduler tool has disadvantage: you should be logged into the system, when you want your scheduled backup to be started. To avoid this disadvantage you may integrate Ghost startup with Windows Scheduler. The following guide shows you how to do it.

1. Change program configuration
- Run Configurator;
- Check 'Misc\don't care about scheduler startup';
- Close Configurator;
2. Add new task into the system
- Open Control Panel\Scheduled tasks\Add Scheduled Task\Next;
- Browse for ghost scheduler executable 'BUtil.Backup.Ghost.exe' in 'bin' directory of BUtil installation folder;
- On the next tab please check 'When my computer starts';
- On the next tab please enter your login and password;
- On the next tab please check **Open advanced properties for this task when I click Finish**
- In the properties go to 'Settings' tab and uncheck **Stop the task if it runs for:** and check **Wake the computer to run this task**

# Integrating BUtil Console Tool With Windows Scheduler

In some cases you want to use the Windows Scheduler or any other scheduler.

Add new task into the system:
- Open Control Panel\Scheduled tasks\Add Scheduled Task\Next;
- Browse for backup scheduler executable 'Backup.exe' in 'bin' directory of BUtil installation folder;
- On the next tab please check 'When my computer starts';
- On the next tab please enter your login and password(without password task may not run);
- On the next tab please configure the way you want to run the scheduler
- In the properties go to 'Task' tab and change the field Execute by adding parameter **-auto**. For example there was string ```C:\PROGRA~1\BUtil\backup.exe```: i this case you should change it to ```C:\PROGRA~1\BUtil\backup.exe -auto "Task=My backup task 1"```.
