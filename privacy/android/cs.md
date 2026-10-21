[Languages](README.md)

# Zásady ochrany osobních údajů

Naposledy aktualizováno: 21. října 2026

**BUtil** by Siarhei Kuchuk

Název aplikace: BUtil
Jméno vývojáře: Siarhei Kuchuk

BUtil zálohuje, synchronizuje a obnovuje soubory v tomto zařízení. Může také importovat média, sdílet složku přes FTPS nebo nahrávat soubory na server, který nastavíte. Nevytváří účet vývojáře. Vývojář neprovozuje serverovou část, která by přijímala vaše soubory, hesla nebo údaje o používání.

## Údaje, které vývojář neshromažďuje

Aplikace neobsahuje reklamy, analytiku, hlášení pádů ani sledovací sady SDK. Vývojář neshromažďuje, neprodává ani nesdílí osobní údaje.

Tyto zásady ochrany osobních údajů a licence se zobrazují v aplikaci. Neotevírají se jako webové stránky.

## Údaje uložené ve vašem zařízení

### Úlohy a nastavení

Definice úloh se ukládají pouze do soukromého úložiště aplikace v tomto zařízení. Úloha může obsahovat cesty ke složkám, plán, nastavení úložiště a hesla nebo tokeny, které zadáte. Hesla a tajné údaje úložiště se před uložením zapečetí úložištěm klíčů Android a lze je číst pouze v tomto zařízení. Tyto hodnoty se nenahrávají vývojáři. Nejsou součástí zálohy Android. Odinstalování aplikace je odstraní.

Zvolený motiv se ukládá do soukromého nastavení aplikace v tomto zařízení.

### Soubory, které vyberete

Zálohování, synchronizace, obnova, import a nahrávání čtou a zapisují složky vybrané systémovým výběrem dokumentů nebo server, který nastavíte. Tyto soubory zůstávají ve složkách, které jste vybrali, nebo v cíli úložiště, který nastavíte. Aplikace je nenahrává vývojáři. Odinstalování aplikace nesmaže soubory ve složkách, které jste vybrali.

### Certifikát FTPS

Když sdílíte složku přes FTPS, aplikace vytvoří v soukromém úložišti aplikace certifikát podepsaný sám sebou. Odinstalování aplikace tento certifikát odstraní. Sdílené soubory zůstávají ve složce, ke které jste udělili přístup.

K ukládání vašich údajů se nepoužívá žádný server vývojáře.

## Používání sítě

### Cíle, které nastavíte

Když se úloha spustí, aplikace se připojí pouze k místu, které jste nastavili. Může to být složka v tomto zařízení nebo server, který zadáte: FTP, FTPS, SFTP, WebDAV, SMB, NFS, úložiště kompatibilní s S3 nebo Azure Blob Storage. Názvy souborů, obsah souborů a zadané přihlašovací údaje se odešlou na tento server, aby se úloha mohla provést. Každá z těchto služeb má vlastní zásady ochrany osobních údajů. Vývojář tento provoz nepřijímá.

Sdílení složky přes FTPS naslouchá v tomto zařízení, aby klient FTPS, který nastavíte, mohl číst nebo zapisovat složku, ke které jste udělili přístup, pomocí uživatelského jména a hesla, která jste nastavili. Tento provoz zůstává mezi vašimi zařízeními (a kýmkoli v síti, kdo má tyto přihlašovací údaje). Vývojář není stranou této komunikace. Odpovídáte za to, kdo se může dostat k portu, kterou složku sdílíte a jak silné je toto heslo.

### Odkaz, který otevřete

Řádek verze v nabídce může otevřít domovskou stránku projektu v systémovém prohlížeči:

[github.com/drweb86/butil](https://github.com/drweb86/butil)

Tento web má vlastní zásady ochrany osobních údajů. Vývojář tento provoz nepřijímá.

Aplikace nekontroluje aktualizace a nekontaktuje službu aktualizací.

## Plán

Úlohu můžete spustit při startu zařízení nebo ve dnech, které zvolíte. Aplikace to zaregistruje u systémového budíku. Tím se v zařízení spustí pouze tato aplikace.

## Oprávnění

Aplikace používá pouze tato oprávnění:

- **Internet** a **stav sítě** pro přístup k serveru, který nastavíte, a pro sdílení složky přes FTPS.
- **Oznámení** a **službu na popředí**, aby běžící úloha pokračovala při vypnuté obrazovce.
- **Přesné budíky** a **spuštění při startu** pro spuštění naplánované úlohy.
- **Zámek probuzení**, aby se během přenosu omezil spánek.

## Děti

Aplikace je nástroj pro zálohování a synchronizaci souborů. Není určena dětem mladším 13 let.

## Třetí strany

Poskytovatelé úložiště, které nastavíte, zpracovávají soubory a přihlašovací údaje, které jim úloha odešle. Otevření domovské stránky projektu zajišťuje systémový prohlížeč a tento web. Vývojář tento provoz nepřijímá.

## Změny

Aktualizace těchto zásad jsou součástí aplikace.

## Kontakt

Název aplikace: BUtil
Jméno vývojáře: Siarhei Kuchuk

Dotazy k této aplikaci pro Android lze poslat prostřednictvím záznamu obchodu, ze kterého jste ji nainstalovali, nebo na [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
