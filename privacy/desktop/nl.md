[Languages](README.md)

# Privacyverklaring

Laatst bijgewerkt: 25 september 2026

**BUtil** by Siarhei Kuchuk

Naam van de toepassing: BUtil
Naam van de ontwikkelaar: Siarhei Kuchuk

BUtil maakt back-ups, synchroniseert en herstelt bestanden op deze computer. Het kan ook media importeren, een map delen of bestanden uploaden naar een server die u instelt. Het maakt geen ontwikkelaarsaccount. De ontwikkelaar heeft geen server die uw bestanden, wachtwoorden of gebruiksgegevens ontvangt.

## Gegevens die de ontwikkelaar niet verzamelt

De app bevat geen advertenties, analyse, crashrapportage of tracking-SDK’s. De ontwikkelaar verzamelt, verkoopt of deelt geen persoonsgegevens.

## Gegevens die op uw computer worden opgeslagen

### Taken en instellingen

Taakdefinities worden alleen op deze computer opgeslagen. Een taak kan mappaden, een schema, opslaginstellingen en wachtwoorden of tokens bevatten die u typt. Wachtwoorden en opslaggeheimen worden op deze computer versleuteld voordat ze worden opgeslagen en kunnen alleen op deze computer worden gelezen. Die waarden worden niet naar de ontwikkelaar geüpload.

- Windows-taken: `%AppData%\BUtil Backup Tasks`
- Linux-taken: `~/.config/BUtil Backup Tasks`
- Windows-instellingen (inclusief thema en de taal die het laatst is gekozen voor licentie of privacy): `%AppData%\BUtil\Settings\v1`
- Linux-instellingen: `~/.config/BUtil/Settings/v1`
- Windows-taakstatus: `%AppData%\BUtil\States`
- Linux-taakstatus: `~/.config/BUtil/States`
- Windows-status van media-import: `%AppData%\BUtil Backup Tasks - States`
- Linux-status van media-import: `~/.config/BUtil Backup Tasks - States`

### Bestanden die u kiest

Back-up, synchronisatie, herstel en import lezen en schrijven de mappen die u selecteert. Die bestanden blijven op deze computer of op de opslagbestemming die u instelt. De app uploadt ze niet naar de ontwikkelaar.

### Logboeken

Diagnostische logboeken worden alleen op deze computer geschreven:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Die bestanden worden nergens naartoe gestuurd.

Er wordt geen server van de ontwikkelaar gebruikt om uw gegevens op te slaan.

## Netwerkgebruik

### Bestemmingen die u instelt

Wanneer een taak wordt uitgevoerd, maakt de app alleen verbinding met de plaats die u opgeeft. Dat kan een lokale map zijn of een server die u invoert: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-compatibele opslag of Azure Blob Storage. Bestandsnamen, bestandsinhoud en de referenties die u hebt ingevoerd, worden naar die server gestuurd zodat de taak kan worden uitgevoerd. Elk van die diensten heeft een eigen privacyverklaring. De ontwikkelaar ontvangt dat verkeer niet.

BUtil Server kan op deze computer luisteren zodat een BUtil-client die u instelt bestanden kan verzenden. Dat verkeer blijft tussen de computers die u instelt.

### Updatecontrole

Builds buiten de Store kunnen de nieuwste GitHub-release opvragen:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) ontvangt een gewone HTTPS-aanvraag (IP-adres, user-agent, tijd). De ontwikkelaar ontvangt dat verkeer niet.

Installaties vanuit de Microsoft Store gebruiken deze controle niet; de Store levert updates.

### Koppelingen die u opent

De app kan deze pagina’s openen in de systeembrowser. Die sites hebben hun eigen privacyverklaringen:

- Projectpagina: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Nieuwste release: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Hulp bij bestandspatronen: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Hulp bij datumnotatie: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Bronvermelding van pictogrammen: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

De licentie en deze privacyverklaring worden in de app getoond. Ze worden niet als webpagina’s geopend.

## Planning

In Windows kunt u een taak uitvoeren bij het aanmelden of volgens een wekelijks schema. De app registreert dat in Windows Taakplanner onder een naam die begint met `BUtil`. Dat start alleen deze app op uw computer.

## Kinderen

De app is een hulpmiddel voor back-up en bestandssynchronisatie. Ze is niet gericht op kinderen jonger dan 13 jaar.

## Derden

GitHub verwerkt het verzoek om updates te controleren en de pagina’s die u opent, zoals hierboven beschreven. De Microsoft Store verwerkt Store-installaties en updates. De opslagproviders die u instelt, verwerken de bestanden en referenties die de taak naar hen stuurt. De ontwikkelaar ontvangt dat verkeer niet.

## Wijzigingen

Updates van deze verklaring worden in dit bestand in de projectrepository geplaatst.

## Contact

Naam van de toepassing: BUtil
Naam van de ontwikkelaar: Siarhei Kuchuk

Vragen: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
