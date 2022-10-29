Application is integrated with Windows Scheduler under current user account. When you provide scheduled days and time, they are used to create/update dedicated tasks in Windows scheduler. It is expected that you are logined into the system at the moment of backup.

**Sometimes you wanna integrate backup process with your own scheduler or without being required to be authorized in your account.**

# Integrating BUtil Console Tool With Windows Scheduler

In some cases you want to use the Windows Scheduler or any other scheduler.

Add new task into the system:
- Open Control Panel\Scheduled tasks\Add Scheduled Task\Next;
- Browse for backup scheduler executable 'Backup.exe' in 'bin' directory of BUtil installation folder;
- On the next tab please check 'When my computer starts';
- On the next tab please enter your login and password(without password task may not run);
- On the next tab please configure the way you want to run the scheduler
- In the properties go to 'Task' tab and change the field Execute by adding parameter **-auto**. For example there was string ```C:\PROGRA~1\BUtil\backup.exe```: i this case you should change it to ```C:\PROGRA~1\BUtil\backup.exe -auto "Task=My backup task 1"```.
