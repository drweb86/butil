[Languages](README.md)

# Patakaran sa pribasiya

Huling na-update: 21 Oktubre 2026

**BUtil** by Siarhei Kuchuk

Pangalan ng aplikasyon: BUtil
Pangalan ng developer: Siarhei Kuchuk

Nagsasagawa ang BUtil ng backup, nagsi-synchronize, at nagre-restore ng mga file sa device na ito. Maaari rin itong mag-import ng media, magbahagi ng folder sa pamamagitan ng FTPS, o mag-upload ng mga file sa server na ise-set mo. Hindi ito gumagawa ng developer account. Hindi nagpapatakbo ang developer ng backend na tumatanggap ng mga file mo, mga password, o datos ng paggamit.

## Datos na hindi kinokolekta ng developer

Walang mga ad, analytics, crash reporter, o tracking SDK ang app. Hindi nangongolekta, nagbebenta, o nagbabahagi ng personal na datos ang developer.

Ipinapakita sa loob ng app ang patakarang ito sa pribasiya at ang lisensya. Hindi binubuksan ang mga ito bilang mga web page.

## Datos na nakaimbak sa device mo

### Mga gawain at mga setting

Ang mga kahulugan ng gawain ay nakaimbak lamang sa pribadong imbakan ng app sa device na ito. Maaaring kasama sa isang gawain ang mga path ng folder, iskedyul, mga setting ng imbakan, at mga password o token na tina-type mo. Sinasarhan ang mga password at mga lihim ng imbakan gamit ang Android keystore bago i-save, at mababasa lamang ang mga ito sa device na ito. Hindi ina-upload ang mga halagang iyon sa developer. Hindi kasama ang mga ito sa Android backup. Tinatanggal ng pag-uninstall ng app ang mga ito.

Ang tema na pipiliin mo ay nakaimbak sa mga pribadong setting ng app sa device na ito.

### Mga file na pipiliin mo

Binabasa at sinusulat ng backup, synchronization, restore, import, at upload ang mga folder na pipiliin mo gamit ang document picker ng sistema, o ang server na ise-set mo. Nanatili ang mga file na iyon sa mga folder na pinili mo, o sa patutunguhan ng imbakan na ise-set mo. Hindi ina-upload ng app ang mga ito sa developer. Hindi binubura ng pag-uninstall ng app ang mga file sa mga folder na pinili mo.

### Sertipiko ng FTPS

Kapag nagbabahagi ka ng folder sa pamamagitan ng FTPS, gumagawa ang app ng self-signed na sertipiko sa pribadong imbakan ng app. Tinatanggal ng pag-uninstall ng app ang sertipikong iyon. Nanatili ang mga ibinahaging file sa folder na ibinigay mo.

Walang server ng developer na ginagamit para itago ang datos mo.

## Paggamit ng network

### Mga patutunguhan na ise-set mo

Kapag tumatakbo ang isang gawain, kumokonekta ang app lamang sa lugar na itinakda mo. Maaaring iyon ay isang folder sa device na ito, o server na ilalagay mo: FTP, FTPS, SFTP, WebDAV, SMB, NFS, imbakan na tugma sa S3, o Azure Blob Storage. Ipinapadala sa server na iyon ang mga pangalan ng file, mga nilalaman ng file, at ang mga kredensyal na inilagay mo upang tumakbo ang gawain. May sariling patakaran sa pribasiya ang bawat isa sa mga serbisyong iyon. Hindi tinatanggap ng developer ang trapikong iyon.

Nakikinig sa device na ito ang pagbabahagi ng folder sa pamamagitan ng FTPS upang mabasa o maisulat ng FTPS client na ise-set mo ang folder na ibinigay mo, gamit ang username at password na itinakda mo. Nanatili ang trapikong iyon sa pagitan ng mga device mo (at ng sinumang nasa network na may mga kredensyal na iyon). Hindi partido ang developer dito. Ikaw ang responsable kung sino ang makakaabot sa port, aling folder ang ibabahagi mo, at gaano kalakas ang password na iyon.

### Link na bubuksan mo

Maaaring buksan ng hanay ng bersyon sa menu ang homepage ng proyekto sa browser ng sistema:

[github.com/drweb86/butil](https://github.com/drweb86/butil)

May sariling patakaran sa pribasiya ang site na iyon. Hindi tinatanggap ng developer ang trapikong iyon.

Hindi tumitingin ang app ng mga update at hindi kumokontak sa serbisyo ng update.

## Iskedyul

Maaari kang magpatakbo ng gawain kapag nagsimula ang device, o sa mga araw na pipiliin mo. Irerehistro iyon ng app sa alarm clock ng sistema. Sinisimulan lamang nito ang app na ito sa device mo.

## Mga pahintulot

Ginagamit lamang ng app ang mga sumusunod na pahintulot:

- **Internet** at **katayuan ng network** upang maabot ang server na ise-set mo at upang magbahagi ng folder sa pamamagitan ng FTPS.
- **Mga abiso** at **serbisyo sa harapan** upang magpatuloy ang tumatakbong gawain kahit nakapatay ang screen.
- **Eksaktong mga alarm** at **pagtakbo sa pagsisimula** upang simulan ang naka-iskedyul na gawain.
- **Kandado ng paggising** upang bawasan ang pagtulog habang may paglilipat.

## Mga bata

Ang app ay kasangkapan sa backup at pag-sync ng file. Hindi ito nakatuon sa mga batang wala pang 13 taong gulang.

## Mga ikatlong partido

Pinoproseso ng mga tagapagbigay ng imbakan na ise-set mo ang mga file at kredensyal na ipinapadala sa kanila ng gawain. Ang pagbukas ng homepage ng proyekto ay hinahawakan ng browser ng sistema at ng site na iyon. Hindi tinatanggap ng developer ang trapikong iyon.

## Mga pagbabago

Kasama sa app ang mga update sa patakarang ito.

## Pakikipag-ugnayan

Pangalan ng aplikasyon: BUtil
Pangalan ng developer: Siarhei Kuchuk

Maaaring ipadala ang mga tanong tungkol sa Android app na ito sa pamamagitan ng listahan ng tindahan na ginamit mo para i-install ito, o sa [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
