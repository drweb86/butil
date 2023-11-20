# Logs

When task launch fails, you can see reasons why it failed in logs.
Logs are collected for task execution.
All apps that execute tasks write logs.
Logs are in txt format.

Logs are located in 
- Windows: ```%appdata%\Butil\Logs\v3``` ,
- Linux: ```~/.config/BUtil/Logs/v3``` .

You can change logs location by creating a create symbolic links.

# Information in logs

It was part of an effort to reduce log sizes. That's why while everything is OK, not many information is logged.

Passwords are not saved in logs.

# Log in browser

If any errors or warnings occur during task execution, program will attempt to open log file for you in the browser. It works independently on chosen power pc options.

# Ideas behind implementation

## Amount of information in logs is reduced
It was part of an effort to reduce log sizes. That's why while everything is OK, not many information is logged. During opening log from hard drive browser cannot load its fully.

## TXT format of logs, not HTML anymore
Main reason is poor support by Linux of html files outside of Downloads folder (no access). Amount of information was reduced to support Windows notepad editor. Colors present in log because of Unicode colored chars.
