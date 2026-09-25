[Languages](README.md)

# Zásady ochrany osobních údajů

Poslední aktualizace: 25. září 2026

**BUtil** by Siarhei Kuchuk

Název aplikace: BUtil
Jméno vývojáře: Siarhei Kuchuk

BUtil zálohuje, synchronizuje a obnovuje soubory na tomto počítači. Umí také importovat média, sdílet složku nebo nahrát soubory na server, který nastavíte. Nevytváří vývojářský účet. Vývojář neprovozuje server, který by přijímal vaše soubory, hesla nebo údaje o používání.

## Údaje, které vývojář neshromažďuje

Aplikace neobsahuje reklamy, analytiku, hlášení pádů ani sledovací sady SDK. Vývojář neshromažďuje, neprodává ani nesdílí osobní údaje.

## Údaje uložené ve vašem počítači

### Úlohy a nastavení

Definice úloh jsou uloženy jen na tomto počítači. Úloha může obsahovat cesty ke složkám, plán, nastavení úložiště a hesla nebo tokeny, které zadáte. Hesla a tajné údaje úložiště se před uložením na tomto počítači zašifrují a lze je přečíst jen na tomto počítači. Tyto hodnoty se vývojáři neodesílají.

- Úlohy ve Windows: `%AppData%\BUtil Backup Tasks`
- Úlohy v Linuxu: `~/.config/BUtil Backup Tasks`
- Nastavení ve Windows (včetně motivu a jazyka naposledy zvoleného pro licenci nebo zásady ochrany osobních údajů): `%AppData%\BUtil\Settings\v1`
- Nastavení v Linuxu: `~/.config/BUtil/Settings/v1`
- Stav úloh ve Windows: `%AppData%\BUtil\States`
- Stav úloh v Linuxu: `~/.config/BUtil/States`
- Stav importu médií ve Windows: `%AppData%\BUtil Backup Tasks - States`
- Stav importu médií v Linuxu: `~/.config/BUtil Backup Tasks - States`

### Soubory, které vyberete

Záloha, synchronizace, obnovení a import čtou a zapisují složky, které vyberete. Tyto soubory zůstávají na tomto počítači nebo v úložišti, které nastavíte. Aplikace je vývojáři nenahrává.

### Protokoly

Diagnostické protokoly se zapisují jen na tomto počítači:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Tyto soubory se nikam neodesílají.

K ukládání vašich údajů se nepoužívá žádný server vývojáře.

## Použití sítě

### Cíle, které nastavíte

Když úloha běží, aplikace se připojí jen k místu, které určíte. Může to být místní složka nebo server, který zadáte: FTP, FTPS, SFTP, WebDAV, SMB, NFS, úložiště kompatibilní s S3 nebo Azure Blob Storage. Názvy souborů, jejich obsah a zadané přihlašovací údaje se odešlou na tento server, aby úloha mohla proběhnout. Každá z těchto služeb má vlastní zásady ochrany osobních údajů. Vývojář tento provoz nedostává.

BUtil Server může na tomto počítači naslouchat, aby klient BUtil, kterého nastavíte, mohl odesílat soubory. Tento provoz zůstává mezi počítači, které nastavíte.

### Kontrola aktualizací

Sestavení mimo Store mohou požádat o nejnovější verzi na GitHubu:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) obdrží běžný požadavek HTTPS (IP adresa, user-agent, čas). Vývojář tento provoz nedostává.

Instalace z Microsoft Store tuto kontrolu nepoužívají; aktualizace dodává Store.

### Odkazy, které otevřete

Aplikace může v systémovém prohlížeči otevřít tyto stránky. Tyto weby mají vlastní zásady ochrany osobních údajů:

- Stránka projektu: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Nejnovější verze: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Nápověda ke vzorům souborů: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Nápověda k formátu data: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Autoři ikon: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Licence a tyto zásady ochrany osobních údajů se zobrazují v aplikaci. Neotevírají se jako webové stránky.

## Plán

Ve Windows můžete úlohu spouštět při přihlášení nebo podle týdenního plánu. Aplikace ji zaregistruje v Plánovači úloh Windows pod názvem, který začíná na `BUtil`. Tím se na vašem počítači spustí jen tato aplikace.

## Děti

Aplikace je nástroj pro zálohování a synchronizaci souborů. Není určena dětem mladším 13 let.

## Třetí strany

GitHub zpracovává požadavek na kontrolu aktualizací a stránky, které otevřete, jak je popsáno výše. Microsoft Store zpracovává instalace a aktualizace ze Storu. Poskytovatelé úložiště, které nastavíte, zpracovávají soubory a přihlašovací údaje, které jim úloha odešle. Vývojář tento provoz nedostává.

## Změny

Aktualizace těchto zásad budou zveřejněny v tomto souboru v repozitáři projektu.

## Kontakt

Název aplikace: BUtil
Jméno vývojáře: Siarhei Kuchuk

Dotazy: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
