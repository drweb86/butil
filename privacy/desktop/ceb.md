[Languages](README.md)

# Patakaran sa pribasiya

Kataposang gi-update: 25 sa Septyembre 2026

**BUtil** by Siarhei Kuchuk

Ngalan sa aplikasyon: BUtil
Ngalan sa developer: Siarhei Kuchuk

Ang BUtil nag-backup, nag-synchronize, ug nag-restore sa mga file niining kompyuter. Makahimo usab kini pag-import og media, pagpaambit og folder, o pag-upload og mga file ngadto sa server nga imong gi-configure. Wala kini maghimo og developer account. Ang developer walay server nga modawat sa imong mga file, password, o datos sa paggamit.

## Datos nga dili kolektahon sa developer

Walay ads, analytics, crash report, o tracking SDK ang app. Ang developer dili mokuha, mobaligya, o mopaambit og personal nga datos.

## Datos nga gitipigan sa imong kompyuter

### Mga buluhaton ug setting

Ang mga kahulugan sa buluhaton gitipigan lang niining kompyuter. Ang usa ka buluhaton mahimong adunay mga agianan sa folder, iskedyul, setting sa storage, ug mga password o token nga imong i-type. Ang mga password ug sekreto sa storage gi-encrypt niining kompyuter sa dili pa i-save, ug mabasa lang niining kompyuter. Kini nga mga bili dili i-upload sa developer.

- Mga buluhaton sa Windows: `%AppData%\BUtil Backup Tasks`
- Mga buluhaton sa Linux: `~/.config/BUtil Backup Tasks`
- Mga setting sa Windows (lakip ang tema ug ang kataposang pinili nga pinulongan para sa Lisensya o Pribasiya): `%AppData%\BUtil\Settings\v1`
- Mga setting sa Linux: `~/.config/BUtil/Settings/v1`
- Kahimtang sa buluhaton sa Windows: `%AppData%\BUtil\States`
- Kahimtang sa buluhaton sa Linux: `~/.config/BUtil/States`
- Kahimtang sa pag-import og media sa Windows: `%AppData%\BUtil Backup Tasks - States`
- Kahimtang sa pag-import og media sa Linux: `~/.config/BUtil Backup Tasks - States`

### Mga file nga imong pilion

Ang backup, synchronization, restore, ug import mobasa ug mosulat sa mga folder nga imong pilion. Kadtong mga file magpabilin niining kompyuter o sa storage nga imong gi-configure. Ang app dili mag-upload niini ngadto sa developer.

### Mga log

Ang mga diagnostic log gisulat lang niining kompyuter:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Kadtong mga file dili ipadala bisan asa.

Walay server sa developer nga gigamit aron tipigan ang imong datos.

## Paggamit sa network

### Mga destinasyon nga imong i-configure

Kon modagan ang usa ka buluhaton, ang app mokonekta lang sa lugar nga imong gitakda. Mahimo kining lokal nga folder o server nga imong isulod: FTP, FTPS, SFTP, WebDAV, SMB, NFS, storage nga compatible sa S3, o Azure Blob Storage. Ang mga ngalan sa file, sulod, ug kredensyal nga imong gisulod ipadala nianang server aron modagan ang buluhaton. Ang matag serbisyo adunay kaugalingong patakaran sa pribasiya. Ang developer dili makadawat nianang trapiko.

Ang BUtil Server makapaminaw niining kompyuter aron ang BUtil client nga imong gi-configure makapadala og mga file. Kadtong trapiko magpabilin tali sa mga kompyuter nga imong gi-setup.

### Pagsusi sa update

Ang mga build nga dili gikan sa Store mahimong mangayo sa pinakabag-ong release sa GitHub:

`https://api.github.com/repos/drweb86/butil/releases/latest`

Ang GitHub (Microsoft) makadawat og ordinaryong HTTPS request (IP address, user-agent, oras). Ang developer dili makadawat nianang trapiko.

Ang mga install gikan sa Microsoft Store dili mogamit niini nga pagsusi; ang Store ang maghatag og mga update.

### Mga link nga imong ablihan

Ang app makabukas niini nga mga panid sa browser sa sistema. Kadtong mga site adunay kaugalingong patakaran sa pribasiya:

- Homepage sa proyekto: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Pinakabag-ong release: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Tabang sa pattern sa file: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Tabang sa format sa petsa: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Gigikanan sa mga icon: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Ang lisensya ug kini nga patakaran sa pribasiya gipakita sulod sa app. Dili kini ablihan isip mga web page.

## Iskedyul

Sa Windows mahimo nimong padaganon ang buluhaton sa pag-sign in o sumala sa sinemanang iskedyul. Irehistro kini sa app sa Windows Task Scheduler gamit ang ngalan nga nagsugod sa `BUtil`. Kini lang ang maglunsad niini nga app sa imong kompyuter.

## Mga bata

Ang app usa ka himan sa backup ug pag-synchronize sa file. Dili kini para sa mga bata nga ubos sa 13 anyos.

## Mga ikatulong partido

Ang GitHub moproseso sa hangyo sa pagsusi sa update ug sa mga panid nga imong ablihan, sumala sa gihulagway sa ibabaw. Ang Microsoft Store moproseso sa mga install ug update sa Store. Ang mga provider sa storage nga imong i-configure moproseso sa mga file ug kredensyal nga ipadala sa buluhaton ngadto kanila. Ang developer dili makadawat nianang trapiko.

## Mga kausaban

Ang mga update niini nga patakaran ipatik niini nga file sa repository sa proyekto.

## Kontak

Ngalan sa aplikasyon: BUtil
Ngalan sa developer: Siarhei Kuchuk

Mga pangutana: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
