[Languages](README.md)

# Datenschutzerklärung

Zuletzt aktualisiert: 25. September 2026

**BUtil** by Siarhei Kuchuk

Anwendungsname: BUtil
Entwicklername: Siarhei Kuchuk

BUtil sichert, synchronisiert und stellt Dateien auf diesem Computer wieder her. Es kann auch Medien importieren, einen Ordner freigeben oder Dateien auf einen Server hochladen, den Sie angeben. Es erstellt kein Entwicklerkonto. Der Entwickler betreibt kein Backend, das Ihre Dateien, Kennwörter oder Nutzungsdaten empfängt.

## Daten, die der Entwickler nicht erhebt

Die App enthält keine Werbung, keine Analyse-, Absturzmelde- oder Tracking-SDKs. Der Entwickler erhebt, verkauft oder teilt keine personenbezogenen Daten.

## Daten, die auf Ihrem Computer gespeichert werden

### Aufgaben und Einstellungen

Aufgabendefinitionen werden nur auf diesem Computer gespeichert. Eine Aufgabe kann Ordnerpfade, einen Zeitplan, Speichereinstellungen sowie Kennwörter oder Token enthalten, die Sie eingeben. Kennwörter und Speichergeheimnisse werden auf diesem Computer verschlüsselt, bevor sie gespeichert werden, und können nur auf diesem Computer gelesen werden. Diese Werte werden nicht an den Entwickler hochgeladen.

- Windows-Aufgaben: `%AppData%\BUtil Backup Tasks`
- Linux-Aufgaben: `~/.config/BUtil Backup Tasks`
- Windows-Einstellungen (einschließlich Design und der zuletzt für Lizenz oder Datenschutz gewählten Sprache): `%AppData%\BUtil\Settings\v1`
- Linux-Einstellungen: `~/.config/BUtil/Settings/v1`
- Windows-Aufgabenstatus: `%AppData%\BUtil\States`
- Linux-Aufgabenstatus: `~/.config/BUtil/States`
- Windows-Status des Medienimports: `%AppData%\BUtil Backup Tasks - States`
- Linux-Status des Medienimports: `~/.config/BUtil Backup Tasks - States`

### Dateien, die Sie auswählen

Sicherung, Synchronisierung, Wiederherstellung und Import lesen und schreiben die Ordner, die Sie auswählen. Diese Dateien bleiben auf diesem Computer oder am Speicherziel, das Sie einrichten. Die App lädt sie nicht zum Entwickler hoch.

### Protokolle

Diagnoseprotokolle werden nur auf diesem Computer geschrieben:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Diese Dateien werden nirgendwohin gesendet.

Es wird kein Entwicklerserver verwendet, um Ihre Daten zu speichern.

## Netzwerknutzung

### Ziele, die Sie einrichten

Wenn eine Aufgabe läuft, verbindet sich die App nur mit dem Ort, den Sie festlegen. Das kann ein lokaler Ordner oder ein Server sein, den Sie eingeben: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-kompatibler Speicher oder Azure Blob Storage. Dateinamen, Dateiinhalte und die von Ihnen eingegebenen Anmeldedaten werden an diesen Server gesendet, damit die Aufgabe ausgeführt werden kann. Jeder dieser Dienste hat seine eigene Datenschutzerklärung. Der Entwickler erhält diesen Datenverkehr nicht.

BUtil Server kann auf diesem Computer auf Verbindungen warten, damit ein von Ihnen eingerichteter BUtil-Client Dateien senden kann. Dieser Datenverkehr bleibt zwischen den Computern, die Sie einrichten.

### Updateprüfung

Builds außerhalb des Store können die neueste GitHub-Version abfragen:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) erhält eine normale HTTPS-Anfrage (IP-Adresse, User-Agent, Uhrzeit). Der Entwickler erhält diesen Datenverkehr nicht.

Installationen aus dem Microsoft Store verwenden diese Prüfung nicht; der Store liefert Updates.

### Links, die Sie öffnen

Die App kann diese Seiten in Ihrem Systembrowser öffnen. Diese Websites haben eigene Datenschutzerklärungen:

- Projektstartseite: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Neueste Version: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Hilfe zu Dateimustern: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Hilfe zum Datumsformat: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Symbolnachweise: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Die Lizenz und diese Datenschutzerklärung werden in der App angezeigt. Sie werden nicht als Webseiten geöffnet.

## Zeitplan

Unter Windows können Sie eine Aufgabe bei der Anmeldung oder nach einem wöchentlichen Zeitplan ausführen. Die App registriert das in der Windows-Aufgabenplanung unter einem Namen, der mit `BUtil` beginnt. Dadurch wird nur diese App auf Ihrem Computer gestartet.

## Kinder

Die App ist ein Werkzeug für Sicherung und Dateisynchronisierung. Sie richtet sich nicht an Kinder unter 13 Jahren.

## Dritte

GitHub verarbeitet die Updateanfrage und die Seiten, die Sie öffnen, wie oben beschrieben. Der Microsoft Store verarbeitet Store-Installationen und Updates. Die von Ihnen eingerichteten Speicheranbieter verarbeiten die Dateien und Anmeldedaten, die die Aufgabe an sie sendet. Der Entwickler erhält diesen Datenverkehr nicht.

## Änderungen

Aktualisierungen dieser Erklärung werden in dieser Datei im Projekt-Repository veröffentlicht.

## Kontakt

Anwendungsname: BUtil
Entwicklername: Siarhei Kuchuk

Fragen: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
