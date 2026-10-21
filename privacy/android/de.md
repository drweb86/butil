[Languages](README.md)

# Datenschutzerklärung

Zuletzt aktualisiert: 21. Oktober 2026

**BUtil** by Siarhei Kuchuk

Name der Anwendung: BUtil
Name des Entwicklers: Siarhei Kuchuk

BUtil sichert, synchronisiert und stellt Dateien auf diesem Gerät wieder her. Die Anwendung kann außerdem Medien importieren, einen Ordner über FTPS freigeben oder Dateien auf einen von Ihnen eingerichteten Server hochladen. Sie erstellt kein Entwicklerkonto. Der Entwickler betreibt kein Serversystem, das Ihre Dateien, Kennwörter oder Nutzungsdaten empfängt.

## Daten, die der Entwickler nicht erhebt

Die Anwendung enthält keine Werbung, keine Analyse, keine Absturzberichte und keine SDKs zur Nachverfolgung. Der Entwickler erhebt, verkauft oder gibt keine personenbezogenen Daten weiter.

Diese Datenschutzerklärung und die Lizenz werden in der Anwendung angezeigt. Sie werden nicht als Webseiten geöffnet.

## Auf Ihrem Gerät gespeicherte Daten

### Aufgaben und Einstellungen

Aufgabendefinitionen werden nur im privaten Speicher der Anwendung auf diesem Gerät gespeichert. Eine Aufgabe kann Ordnerpfade, einen Zeitplan, Speichereinstellungen sowie Kennwörter oder Token enthalten, die Sie eingeben. Kennwörter und Speichergeheimnisse werden vor dem Speichern mit dem Android-Schlüsselspeicher versiegelt und können nur auf diesem Gerät gelesen werden. Diese Werte werden nicht an den Entwickler hochgeladen. Sie sind nicht in der Android-Sicherung enthalten. Beim Deinstallieren der Anwendung werden sie entfernt.

Das von Ihnen gewählte Design wird in den privaten Einstellungen der Anwendung auf diesem Gerät gespeichert.

### Dateien, die Sie auswählen

Sicherung, Synchronisierung, Wiederherstellung, Import und Hochladen lesen und schreiben die Ordner, die Sie mit der Systemauswahl für Dokumente auswählen, oder den Server, den Sie einrichten. Diese Dateien bleiben in den von Ihnen gewählten Ordnern oder am von Ihnen eingerichteten Speicherziel. Die Anwendung lädt sie nicht zum Entwickler hoch. Beim Deinstallieren der Anwendung werden Dateien in von Ihnen ausgewählten Ordnern nicht gelöscht.

### FTPS-Zertifikat

Wenn Sie einen Ordner über FTPS freigeben, erstellt die Anwendung ein selbstsigniertes Zertifikat im privaten Speicher der Anwendung. Beim Deinstallieren der Anwendung wird dieses Zertifikat entfernt. Freigegebene Dateien bleiben in dem Ordner, den Sie freigegeben haben.

Es wird kein Server des Entwicklers verwendet, um Ihre Daten zu speichern.

## Netzwerknutzung

### Ziele, die Sie einrichten

Wenn eine Aufgabe ausgeführt wird, verbindet sich die Anwendung nur mit dem von Ihnen festgelegten Ort. Das kann ein Ordner auf diesem Gerät oder ein von Ihnen eingegebener Server sein: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-kompatibler Speicher oder Azure Blob Storage. Dateinamen, Dateiinhalte und die von Ihnen eingegebenen Anmeldedaten werden an diesen Server gesendet, damit die Aufgabe ausgeführt werden kann. Jeder dieser Dienste hat eine eigene Datenschutzerklärung. Der Entwickler empfängt diesen Datenverkehr nicht.

Die Freigabe eines Ordners über FTPS lauscht auf diesem Gerät, damit ein von Ihnen eingerichteter FTPS-Client den freigegebenen Ordner mit dem von Ihnen festgelegten Benutzernamen und Kennwort lesen oder schreiben kann. Dieser Datenverkehr bleibt zwischen Ihren Geräten (und jeder Person im Netzwerk, die über diese Anmeldedaten verfügt). Der Entwickler ist daran nicht beteiligt. Sie sind dafür verantwortlich, wer den Port erreichen kann, welchen Ordner Sie freigeben und wie stark dieses Kennwort ist.

Die Anwendung prüft nicht auf Aktualisierungen und kontaktiert keinen Aktualisierungsdienst.

## Zeitplan

Sie können eine Aufgabe beim Start des Geräts oder an den von Ihnen gewählten Tagen ausführen. Die Anwendung registriert das beim Systemwecker. Dadurch wird nur diese Anwendung auf Ihrem Gerät gestartet.

## Berechtigungen

Die Anwendung verwendet nur die folgenden Berechtigungen:

- **Internet** und **Netzwerkstatus**, um einen von Ihnen eingerichteten Server zu erreichen und einen Ordner über FTPS freizugeben.
- **Benachrichtigungen** und einen **Vordergrunddienst**, damit eine laufende Aufgabe bei ausgeschaltetem Bildschirm weiterläuft.
- **Exakte Alarme** und **Ausführung beim Start**, um eine geplante Aufgabe zu starten.
- **Aufwecksperre**, um den Ruhezustand während einer Übertragung zu verringern.

## Kinder

Die Anwendung ist ein Werkzeug für Sicherung und Dateisynchronisierung. Sie richtet sich nicht an Kinder unter 13 Jahren.

## Dritte

Von Ihnen eingerichtete Speicheranbieter verarbeiten die Dateien und Anmeldedaten, die die Aufgabe an sie sendet. Der Entwickler empfängt diesen Datenverkehr nicht.

## Änderungen

Aktualisierungen dieser Erklärung sind in der Anwendung enthalten.

## Kontakt

Name der Anwendung: BUtil
Name des Entwicklers: Siarhei Kuchuk

Fragen zu dieser Android-Anwendung können über den Store-Eintrag gesendet werden, über den Sie sie installiert haben, oder an [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
