# Log Modes
Logs can be produced in 2 modes: in html file log and console application.

# Logs Location
Logs are stored in folder '%appdata%\Butil\Logs\v2'.
To override logs location make a simbolic link to this folder.
Backup Wizard program stores logs in logs folder too.

# Information Included To Log
In normal logging mode program parses 7-zip messages and extracts errors and warnings from them to log. In support mode program writes additional debug information. Passwords are not saved in logs.

# Logs Legend
- **Errors** uses red color in html logs;
- **Packer messages** have the black color;
- **Debug messages** have the blue color;

# Logs Start Up After Completion Of Backup Master Work
If any errors or warnings occur during backup, program configures the system to open log independently on chosen power pc options.

# See also:
[Configuring of Logging](../Configure/Configurator.md)
