[Languages](README.md)

# Polityka prywatności

Ostatnia aktualizacja: 25 września 2026 r.

**BUtil** by Siarhei Kuchuk

Nazwa aplikacji: BUtil
Nazwa dewelopera: Siarhei Kuchuk

BUtil tworzy kopie zapasowe, synchronizuje i przywraca pliki na tym komputerze. Może też importować multimedia, udostępniać folder lub wysyłać pliki na serwer, który skonfigurujesz. Nie tworzy konta dewelopera. Deweloper nie prowadzi serwera, który odbiera Twoje pliki, hasła lub dane o użyciu.

## Dane, których deweloper nie zbiera

Aplikacja nie zawiera reklam, analityki, raportów o awariach ani zestawów SDK do śledzenia. Deweloper nie zbiera, nie sprzedaje i nie udostępnia danych osobowych.

## Dane przechowywane na Twoim komputerze

### Zadania i ustawienia

Definicje zadań są przechowywane tylko na tym komputerze. Zadanie może zawierać ścieżki folderów, harmonogram, ustawienia magazynu oraz hasła lub tokeny, które wpisujesz. Hasła i sekrety magazynu są szyfrowane na tym komputerze przed zapisaniem i można je odczytać tylko na tym komputerze. Te wartości nie są wysyłane do dewelopera.

- Zadania Windows: `%AppData%\BUtil Backup Tasks`
- Zadania Linux: `~/.config/BUtil Backup Tasks`
- Ustawienia Windows (w tym motyw i język ostatnio wybrany dla licencji lub prywatności): `%AppData%\BUtil\Settings\v1`
- Ustawienia Linux: `~/.config/BUtil/Settings/v1`
- Stan zadań Windows: `%AppData%\BUtil\States`
- Stan zadań Linux: `~/.config/BUtil/States`
- Stan importu multimediów Windows: `%AppData%\BUtil Backup Tasks - States`
- Stan importu multimediów Linux: `~/.config/BUtil Backup Tasks - States`

### Pliki, które wybierasz

Kopia zapasowa, synchronizacja, przywracanie i import odczytują i zapisują wybrane foldery. Te pliki pozostają na tym komputerze albo w miejscu przechowywania, które skonfigurujesz. Aplikacja nie wysyła ich do dewelopera.

### Dzienniki

Dzienniki diagnostyczne są zapisywane tylko na tym komputerze:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Te pliki nie są nigdzie wysyłane.

Żaden serwer dewelopera nie służy do przechowywania Twoich danych.

## Korzystanie z sieci

### Miejsca, które konfigurujesz

Gdy zadanie działa, aplikacja łączy się tylko z miejscem, które wskażesz. Może to być folder lokalny albo serwer, który podasz: FTP, FTPS, SFTP, WebDAV, SMB, NFS, magazyn zgodny z S3 lub Azure Blob Storage. Nazwy plików, ich zawartość i podane przez Ciebie poświadczenia są wysyłane na ten serwer, aby zadanie mogło się wykonać. Każda z tych usług ma własną politykę prywatności. Deweloper nie otrzymuje tego ruchu.

BUtil Server może nasłuchiwać na tym komputerze, aby skonfigurowany przez Ciebie klient BUtil mógł wysyłać pliki. Ten ruch pozostaje między komputerami, które skonfigurujesz.

### Sprawdzanie aktualizacji

Kompilacje spoza Store mogą pobierać najnowsze wydanie z GitHub:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) otrzymuje zwykłe żądanie HTTPS (adres IP, user-agent, czas). Deweloper nie otrzymuje tego ruchu.

Instalacje z Microsoft Store nie używają tego sprawdzenia; aktualizacje dostarcza Store.

### Otwierane linki

Aplikacja może otwierać te strony w przeglądarce systemu. Te witryny mają własne polityki prywatności:

- Strona projektu: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Najnowsze wydanie: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Pomoc dotycząca wzorców plików: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Pomoc dotycząca formatu daty: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Autorzy ikon: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Licencja i ta polityka prywatności są pokazywane w aplikacji. Nie są otwierane jako strony internetowe.

## Harmonogram

W systemie Windows możesz uruchamiać zadanie przy logowaniu albo według harmonogramu tygodniowego. Aplikacja rejestruje to w Harmonogramie zadań Windows pod nazwą zaczynającą się od `BUtil`. To tylko uruchamia tę aplikację na Twoim komputerze.

## Dzieci

Aplikacja służy do kopii zapasowych i synchronizacji plików. Nie jest skierowana do dzieci poniżej 13 lat.

## Podmioty trzecie

GitHub przetwarza żądanie sprawdzenia aktualizacji i strony, które otwierasz, jak opisano powyżej. Microsoft Store przetwarza instalacje i aktualizacje ze Store. Dostawcy magazynu, których konfigurujesz, przetwarzają pliki i poświadczenia wysyłane przez zadanie. Deweloper nie otrzymuje tego ruchu.

## Zmiany

Aktualizacje tej polityki będą publikowane w tym pliku w repozytorium projektu.

## Kontakt

Nazwa aplikacji: BUtil
Nazwa dewelopera: Siarhei Kuchuk

Pytania: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
