[Languages](README.md)

# Patakaran sa privacy

Huling na-update: 25 Setyembre 2026

**BUtil** by Siarhei Kuchuk

Pangalan ng application: BUtil
Pangalan ng developer: Siarhei Kuchuk

Ang BUtil ay nagba-backup, nagsi-synchronize, at nagre-restore ng mga file sa computer na ito. Maaari rin itong mag-import ng media, magbahagi ng folder, o mag-upload ng mga file sa server na iko-configure mo. Hindi ito gumagawa ng developer account. Hindi nagpapatakbo ang developer ng server na tumatanggap ng iyong mga file, password, o data ng paggamit.

## Data na hindi kinokolekta ng developer

Walang ad, analytics, crash report, o tracking SDK ang app. Hindi kinokolekta, ibinebenta, o ibinabahagi ng developer ang personal na data.

## Data na naka-store sa iyong computer

### Mga gawain at setting

Ang mga kahulugan ng gawain ay naka-store lamang sa computer na ito. Maaaring may mga path ng folder, iskedyul, setting ng storage, at mga password o token na ita-type mo ang isang gawain. Naka-encrypt sa computer na ito ang mga password at lihim ng storage bago i-save, at mababasa lamang sa computer na ito. Hindi ina-upload sa developer ang mga halagang iyon.

- Mga gawain sa Windows: `%AppData%\BUtil Backup Tasks`
- Mga gawain sa Linux: `~/.config/BUtil Backup Tasks`
- Mga setting sa Windows (kasama ang tema at ang huling piniling wika para sa Lisensya o Privacy): `%AppData%\BUtil\Settings\v1`
- Mga setting sa Linux: `~/.config/BUtil/Settings/v1`
- Estado ng gawain sa Windows: `%AppData%\BUtil\States`
- Estado ng gawain sa Linux: `~/.config/BUtil/States`
- Estado ng pag-import ng media sa Windows: `%AppData%\BUtil Backup Tasks - States`
- Estado ng pag-import ng media sa Linux: `~/.config/BUtil Backup Tasks - States`

### Mga file na pipiliin mo

Binabasa at isinusulat ng backup, synchronization, restore, at import ang mga folder na pipiliin mo. Nanatili ang mga file na iyon sa computer na ito o sa destinasyon ng storage na iko-configure mo. Hindi sila ina-upload ng app sa developer.

### Mga log

Ang mga diagnostic log ay isinusulat lamang sa computer na ito:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Hindi ipinapadala ang mga file na iyon kahit saan.

Walang server ng developer na ginagamit para i-store ang iyong data.

## Paggamit ng network

### Mga destinasyon na iko-configure mo

Kapag tumatakbo ang isang gawain, kumokonekta lamang ang app sa lugar na itinakda mo. Maaari itong lokal na folder o server na ilalagay mo: FTP, FTPS, SFTP, WebDAV, SMB, NFS, storage na tugma sa S3, o Azure Blob Storage. Ipinapadala sa server na iyon ang mga pangalan ng file, nilalaman, at mga kredensyal na inilagay mo para tumakbo ang gawain. May sariling patakaran sa privacy ang bawat serbisyong iyon. Hindi natatanggap ng developer ang trapikong iyon.

Maaaring makinig ang BUtil Server sa computer na ito para makapagpadala ng mga file ang BUtil client na iko-configure mo. Nanatili ang trapikong iyon sa pagitan ng mga computer na ise-set up mo.

### Pagsusuri ng update

Maaaring humiling ang mga build na hindi mula sa Store ng pinakabagong release sa GitHub:

`https://api.github.com/repos/drweb86/butil/releases/latest`

Tumatanggap ang GitHub (Microsoft) ng karaniwang kahilingang HTTPS (IP address, user-agent, oras). Hindi natatanggap ng developer ang trapikong iyon.

Hindi ginagamit ng mga install mula sa Microsoft Store ang pagsusuring ito; ang Store ang naghahatid ng mga update.

### Mga link na binubuksan mo

Maaaring buksan ng app ang mga pahinang ito sa browser ng system. May sariling patakaran sa privacy ang mga site na iyon:

- Homepage ng proyekto: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Pinakabagong release: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Tulong sa pattern ng file: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Tulong sa format ng petsa: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Pinagmulan ng mga icon: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Ipinapakita sa loob ng app ang lisensya at ang patakarang ito sa privacy. Hindi sila binubuksan bilang mga web page.

## Iskedyul

Sa Windows maaari kang magpatakbo ng gawain sa pag-sign in o ayon sa lingguhang iskedyul. Inirerehistro ito ng app sa Windows Task Scheduler gamit ang pangalan na nagsisimula sa `BUtil`. Inilulunsad lamang nito ang app na ito sa iyong computer.

## Mga bata

Ang app ay kasangkapan sa backup at pag-synchronize ng file. Hindi ito para sa mga batang wala pang 13 taong gulang.

## Mga third party

Pinoproseso ng GitHub ang kahilingan sa pagsusuri ng update at ang mga pahinang binubuksan mo, gaya ng inilarawan sa itaas. Pinoproseso ng Microsoft Store ang mga install at update ng Store. Pinoproseso ng mga provider ng storage na iko-configure mo ang mga file at kredensyal na ipinapadala ng gawain sa kanila. Hindi natatanggap ng developer ang trapikong iyon.

## Mga pagbabago

Ilalathala ang mga update ng patakarang ito sa file na ito sa repository ng proyekto.

## Contact

Pangalan ng application: BUtil
Pangalan ng developer: Siarhei Kuchuk

Mga tanong: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
