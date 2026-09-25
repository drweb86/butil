[Languages](README.md)

# Integritetspolicy

Senast uppdaterad: 25 september 2026

**BUtil** by Siarhei Kuchuk

Programnamn: BUtil
Utvecklarens namn: Siarhei Kuchuk

BUtil säkerhetskopierar, synkroniserar och återställer filer på den här datorn. Det kan också importera media, dela en mapp eller ladda upp filer till en server som du anger. Det skapar inget utvecklarkonto. Utvecklaren driver ingen server som tar emot dina filer, lösenord eller användningsdata.

## Data som utvecklaren inte samlar in

Appen innehåller ingen reklam, analys, kraschrapportering eller spårnings-SDK. Utvecklaren samlar inte in, säljer eller delar personuppgifter.

## Data som lagras på din dator

### Uppgifter och inställningar

Uppgiftsdefinitioner lagras bara på den här datorn. En uppgift kan innehålla mappsökvägar, ett schema, lagringsinställningar och lösenord eller token som du skriver. Lösenord och lagringshemligheter krypteras på den här datorn innan de sparas och kan bara läsas på den här datorn. De värdena laddas inte upp till utvecklaren.

- Windows-uppgifter: `%AppData%\BUtil Backup Tasks`
- Linux-uppgifter: `~/.config/BUtil Backup Tasks`
- Windows-inställningar (inklusive tema och det språk som senast valdes för licens eller integritet): `%AppData%\BUtil\Settings\v1`
- Linux-inställningar: `~/.config/BUtil/Settings/v1`
- Windows-uppgiftsstatus: `%AppData%\BUtil\States`
- Linux-uppgiftsstatus: `~/.config/BUtil/States`
- Windows-status för medieimport: `%AppData%\BUtil Backup Tasks - States`
- Linux-status för medieimport: `~/.config/BUtil Backup Tasks - States`

### Filer som du väljer

Säkerhetskopiering, synkronisering, återställning och import läser och skriver de mappar du väljer. De filerna stannar på den här datorn eller på lagringsmålet som du anger. Appen laddar inte upp dem till utvecklaren.

### Loggar

Diagnostikloggar skrivs bara på den här datorn:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

De filerna skickas inte någonstans.

Ingen server hos utvecklaren används för att lagra dina data.

## Nätverksanvändning

### Mål som du anger

När en uppgift körs ansluter appen bara till den plats du anger. Det kan vara en lokal mapp eller en server du skriver in: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-kompatibel lagring eller Azure Blob Storage. Filnamn, filinnehåll och de uppgifter du angav skickas till den servern så att uppgiften kan köras. Var och en av de tjänsterna har en egen integritetspolicy. Utvecklaren tar inte emot den trafiken.

BUtil Server kan lyssna på den här datorn så att en BUtil-klient som du anger kan skicka filer. Den trafiken stannar mellan de datorer du ställer in.

### Uppdateringskontroll

Versioner utanför Store kan begära den senaste GitHub-utgåvan:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) tar emot en vanlig HTTPS-begäran (IP-adress, user-agent, tid). Utvecklaren tar inte emot den trafiken.

Installationer från Microsoft Store använder inte den här kontrollen; Store levererar uppdateringar.

### Länkar som du öppnar

Appen kan öppna de här sidorna i systemets webbläsare. De webbplatserna har egna integritetspolicyer:

- Projektets startsida: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Senaste utgåvan: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Hjälp om filmönster: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Hjälp om datumformat: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Ikonkällor: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Licensen och den här integritetspolicyn visas i appen. De öppnas inte som webbsidor.

## Schema

I Windows kan du köra en uppgift vid inloggning eller enligt ett veckoschema. Appen registrerar det i Schemaläggaren i Windows under ett namn som börjar med `BUtil`. Det startar bara den här appen på din dator.

## Barn

Appen är ett verktyg för säkerhetskopiering och filsynkronisering. Den riktar sig inte till barn under 13 år.

## Tredje parter

GitHub behandlar begäran om uppdateringskontroll och sidorna du öppnar, enligt ovan. Microsoft Store behandlar Store-installationer och uppdateringar. Lagringsleverantörer som du anger behandlar filerna och uppgifterna som uppgiften skickar till dem. Utvecklaren tar inte emot den trafiken.

## Ändringar

Uppdateringar av den här policyn publiceras i den här filen i projektets arkiv.

## Kontakt

Programnamn: BUtil
Utvecklarens namn: Siarhei Kuchuk

Frågor: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
