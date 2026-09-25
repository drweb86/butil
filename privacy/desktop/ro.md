[Languages](README.md)

# Politica de confidențialitate

Ultima actualizare: 25 septembrie 2026

**BUtil** by Siarhei Kuchuk

Numele aplicației: BUtil
Numele dezvoltatorului: Siarhei Kuchuk

BUtil face copii de rezervă, sincronizează și restaurează fișiere pe acest computer. Poate importa și fișiere media, poate partaja un folder sau poate încărca fișiere pe un server pe care îl configurați. Nu creează un cont de dezvoltator. Dezvoltatorul nu operează un server care primește fișierele, parolele sau datele dvs. de utilizare.

## Date pe care dezvoltatorul nu le colectează

Aplicația nu include reclame, analize, rapoarte de erori sau SDK-uri de urmărire. Dezvoltatorul nu colectează, nu vinde și nu partajează date personale.

## Date stocate pe computerul dvs.

### Activități și setări

Definițiile activităților sunt stocate doar pe acest computer. O activitate poate include căi de foldere, un program, setări de stocare și parolele sau tokenurile pe care le introduceți. Parolele și secretele de stocare sunt criptate pe acest computer înainte de a fi salvate și pot fi citite doar pe acest computer. Aceste valori nu sunt trimise dezvoltatorului.

- Activități Windows: `%AppData%\BUtil Backup Tasks`
- Activități Linux: `~/.config/BUtil Backup Tasks`
- Setări Windows (inclusiv tema și limba aleasă ultima dată pentru licență sau confidențialitate): `%AppData%\BUtil\Settings\v1`
- Setări Linux: `~/.config/BUtil/Settings/v1`
- Starea activităților Windows: `%AppData%\BUtil\States`
- Starea activităților Linux: `~/.config/BUtil/States`
- Starea importului media Windows: `%AppData%\BUtil Backup Tasks - States`
- Starea importului media Linux: `~/.config/BUtil Backup Tasks - States`

### Fișierele pe care le alegeți

Copierea de rezervă, sincronizarea, restaurarea și importul citesc și scriu folderele pe care le selectați. Acele fișiere rămân pe acest computer sau la destinația de stocare pe care o configurați. Aplicația nu le încarcă către dezvoltator.

### Jurnale

Jurnalele de diagnostic sunt scrise doar pe acest computer:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Acele fișiere nu sunt trimise nicăieri.

Nu se folosește niciun server al dezvoltatorului pentru a stoca datele dvs.

## Utilizarea rețelei

### Destinații pe care le configurați

Când rulează o activitate, aplicația se conectează doar la locul pe care îl stabiliți. Poate fi un folder local sau un server pe care îl introduceți: FTP, FTPS, SFTP, WebDAV, SMB, NFS, stocare compatibilă S3 sau Azure Blob Storage. Numele fișierelor, conținutul și datele de autentificare pe care le-ați introdus sunt trimise către acel server ca activitatea să poată rula. Fiecare dintre aceste servicii are propria politică de confidențialitate. Dezvoltatorul nu primește acel trafic.

BUtil Server poate asculta pe acest computer, astfel încât un client BUtil pe care îl configurați să poată trimite fișiere. Traficul rămâne între computerele pe care le configurați.

### Verificarea actualizărilor

Compilările din afara Store pot solicita cea mai recentă versiune GitHub:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) primește o cerere HTTPS obișnuită (adresă IP, user-agent, oră). Dezvoltatorul nu primește acel trafic.

Instalările din Microsoft Store nu folosesc această verificare; Store livrează actualizările.

### Linkuri pe care le deschideți

Aplicația poate deschide aceste pagini în browserul sistemului. Acele site-uri au propriile politici de confidențialitate:

- Pagina proiectului: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Cea mai recentă versiune: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Ajutor pentru tipare de fișiere: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Ajutor pentru formatul datei: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Surse pentru pictograme: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Licența și această politică de confidențialitate sunt afișate în aplicație. Nu sunt deschise ca pagini web.

## Programare

În Windows puteți rula o activitate la conectare sau după un program săptămânal. Aplicația o înregistrează în Planificatorul de activități Windows cu un nume care începe cu `BUtil`. Aceasta doar pornește aplicația pe computerul dvs.

## Copii

Aplicația este un instrument de copiere de rezervă și sincronizare a fișierelor. Nu este destinată copiilor sub 13 ani.

## Terți

GitHub prelucrează cererea de verificare a actualizărilor și paginile pe care le deschideți, așa cum este descris mai sus. Microsoft Store prelucrează instalările și actualizările din Store. Furnizorii de stocare pe care îi configurați prelucrează fișierele și datele de autentificare pe care activitatea le trimite. Dezvoltatorul nu primește acel trafic.

## Modificări

Actualizările acestei politici vor fi publicate în acest fișier din depozitul proiectului.

## Contact

Numele aplicației: BUtil
Numele dezvoltatorului: Siarhei Kuchuk

Întrebări: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
