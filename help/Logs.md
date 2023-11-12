# Logs

When task launch fails, you can see reasons why it failed in logs.
Logs are collected for task execution.
All apps that execute tasks write logs.
Logs are in HTML format.

Logs are located in 
- Windows: ```%appdata%\Butil\Logs\v2``` ,
- Linux: ```<Download folder>\Butil\Logs\v2``` .

You can change logs location by creating a create symbolic links.

# Information in logs

It was part of an effort to reduce log sizes. That's why while everything is OK, not many information is logged.

Passwords are not saved in logs.

# Log in browser

If any errors or warnings occur during task execution, program will attempt to open log file for you in the browser. It works independently on chosen power pc options.

# Ideas behind implementation

## Amount of information in logs is reduced
It was part of an effort to reduce log sizes. That's why while everything is OK, not many information is logged. During opening log from hard drive browser cannot load its fully.

## HTML format of logs
Since text editor does not work normally with huge logs and for making colors for errors, html format is used.

## Linux logs storage
Logs are located in Downloads folder, because Firefox snap has no access to other folders.