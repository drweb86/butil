# Log Modes</H2>
Logs can be produced in 2 modes: in html file log and console application.

# Logs Location</H2>
Logs are stored in folder '%userprofile%\Application Data\BUtil-x.x\Logs'.
You can override logs location in Configurator in logging tab.
Backup Wizard program stores logs in logs folder too.

# Information Included To Log</H2>
In normal logging mode program parses 7-zip messages and extracts errors and warnings from them to log. In support mode program writes additional debug information. Passwords are not saved in logs.

# Logs Legend
- **Errors** uses red color in html logs;
- **Packer messages** have the black color;
- **Debug messages** have the blue color;
- **Warnings** have the blue color.

# Logs Start Up After Completion Of Backup Master Work</H2>
If any errors or warnings occur during backup, program configures the system to open log independently on chosen power pc options.

# See also:</H4>
[Configuring of Logging](../Configure/Configurator.md)
