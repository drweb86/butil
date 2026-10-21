[Languages](README.md)

# Integritetspolicy

Senast uppdaterad: 21 oktober 2026

**BUtil** by Siarhei Kuchuk

Appens namn: BUtil
Utvecklarens namn: Siarhei Kuchuk

BUtil säkerhetskopierar, synkroniserar och återställer filer på den här enheten. Appen kan också importera media, dela en mapp via FTPS eller ladda upp filer till en server som du konfigurerar. Den skapar inget utvecklarkonto. Utvecklaren driver ingen serverdel som tar emot dina filer, lösenord eller användningsdata.

## Data som utvecklaren inte samlar in

Appen innehåller inte annonser, analys, haverirapporter eller spårnings-SDK:er. Utvecklaren samlar inte in, säljer eller delar personuppgifter.

Den här integritetspolicyn och licensen visas i appen. De öppnas inte som webbsidor.

## Data som lagras på din enhet

### Uppgifter och inställningar

Uppgiftsdefinitioner lagras bara i appens privata lagring på den här enheten. En uppgift kan innehålla mappsökvägar, ett schema, lagringsinställningar och lösenord eller token som du skriver. Lösenord och lagringshemligheter förseglas med Android-nyckellagret innan de sparas och kan bara läsas på den här enheten. De värdena laddas inte upp till utvecklaren. De ingår inte i Android-säkerhetskopian. Om du avinstallerar appen tas de bort.

### Filer du väljer

Säkerhetskopiering, synkronisering, återställning, import och uppladdning läser och skriver de mappar du väljer med systemets dokumentväljare, eller servern du konfigurerar. De filerna stannar i mapparna du valde eller på lagringsmålet du konfigurerar. Appen laddar inte upp dem till utvecklaren. Om du avinstallerar appen raderas inte filer i mappar du har valt.

### FTPS-certifikat

När du delar en mapp via FTPS skapar appen ett självsignerat certifikat i appens privata lagring. Om du avinstallerar appen tas certifikatet bort. Delade filer stannar i mappen du har gett åtkomst till.

Ingen server hos utvecklaren används för att lagra dina data.

## Nätverksanvändning

### Mål du konfigurerar

När en uppgift körs ansluter appen bara till platsen du har angett. Det kan vara en mapp på den här enheten eller en server du skriver in: FTP, FTPS, SFTP, WebDAV, SMB, NFS, lagring som är kompatibel med S3 eller Azure Blob Storage. Filnamn, filinnehåll och de inloggningsuppgifter du har angett skickas till den servern så att uppgiften kan köras. Var och en av de tjänsterna har en egen integritetspolicy. Utvecklaren tar inte emot den trafiken.

Att dela en mapp via FTPS lyssnar på den här enheten så att en FTPS-klient du konfigurerar kan läsa eller skriva mappen du har gett åtkomst till, med användarnamnet och lösenordet du har angett. Den trafiken stannar mellan dina enheter (och den på nätverket som har de inloggningsuppgifterna). Utvecklaren är inte part i den. Du ansvarar för vem som kan nå porten, vilken mapp du delar och hur starkt lösenordet är.

Appen söker inte efter uppdateringar och kontaktar ingen uppdateringstjänst.

## Schema

Du kan köra en uppgift när enheten startar eller på de dagar du väljer. Appen registrerar det hos systemets alarmklocka. Det startar bara den här appen på din enhet.

## Behörigheter

Appen använder bara följande behörigheter:

- **Internet** och **nätverksstatus** för att nå en server du konfigurerar och för att dela en mapp via FTPS.
- **Aviseringar** och en **förgrundstjänst** för att hålla en pågående uppgift igång när skärmen är av.
- **Exakta alarm** och **körning vid start** för att starta en schemalagd uppgift.
- **Väckningslås** för att minska viloläge under en överföring.

## Barn

Appen är ett verktyg för säkerhetskopiering och filsynkronisering. Den riktar sig inte till barn under 13 år.

## Tredje parter

Lagringsleverantörer du konfigurerar behandlar de filer och inloggningsuppgifter som uppgiften skickar till dem. Utvecklaren tar inte emot den trafiken.

## Ändringar

Uppdateringar av den här policyn ingår i appen.

## Kontakt

Appens namn: BUtil
Utvecklarens namn: Siarhei Kuchuk

Frågor om den här Android-appen kan skickas via butiksposten som du använde för att installera den, eller till [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
