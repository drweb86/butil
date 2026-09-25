[Languages](README.md)

# Iwu nzuzo

Emelitere ikpeazụ: 25 Septemba 2026

**BUtil** by Siarhei Kuchuk

Aha ngwa: BUtil
Aha onye mepụtara: Siarhei Kuchuk

BUtil na-echekwa faịlụ ndabere, na-emekọrịta ha, ma na-eweghachi ha na kọmputa a. Ọ nwekwara ike ibubata mgbasa ozi, kesaa folda, ma ọ bụ bulite faịlụ gaa na sava ị haziri. Ọ anaghị emepụta akaụntụ onye mepụtara. Onye mepụtara anaghị arụ sava na-anata faịlụ gị, okwuntughe, ma ọ bụ data ojiji.

## Data onye mepụtara anaghị anakọta

Ngwa enweghị mgbasa ozi, nyocha, akụkọ nkụchi, ma ọ bụ SDK nsochi. Onye mepụtara anaghị anakọta, ere, ma ọ bụ kesaa data nkeonwe.

## Data echekwara na kọmputa gị

### Ọrụ na ntọala

A na-echekwa nkọwa ọrụ naanị na kọmputa a. Ọrụ nwere ike ịgụnye ụzọ folda, usoro oge, ntọala nchekwa, na okwuntughe ma ọ bụ token ị pịnyere. A na-ezochi okwuntughe na ihe nzuzo nchekwa na kọmputa a tupu echekwa ha, a pụkwara ịgụ ha naanị na kọmputa a. A naghị ezipụ ụkpụrụ ndị a nye onye mepụtara.

- Ọrụ Windows: `%AppData%\BUtil Backup Tasks`
- Ọrụ Linux: `~/.config/BUtil Backup Tasks`
- Ntọala Windows (gụnyere isiokwu na asụsụ ikpeazụ ahọpụtara maka ikike ma ọ bụ nzuzo): `%AppData%\BUtil\Settings\v1`
- Ntọala Linux: `~/.config/BUtil/Settings/v1`
- Ọnọdụ ọrụ Windows: `%AppData%\BUtil\States`
- Ọnọdụ ọrụ Linux: `~/.config/BUtil/States`
- Ọnọdụ mbubata mgbasa ozi Windows: `%AppData%\BUtil Backup Tasks - States`
- Ọnọdụ mbubata mgbasa ozi Linux: `~/.config/BUtil Backup Tasks - States`

### Faịlụ ị họọrọ

Nchekwa ndabere, mmekọrịta, mweghachi, na mbubata na-agụ ma dee folda ị họọrọ. Faịlụ ndị ahụ na-anọ na kọmputa a ma ọ bụ na nchekwa ị haziri. Ngwa anaghị ezipụ ha nye onye mepụtara.

### Ndekọ

A na-ede ndekọ nyocha naanị na kọmputa a:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

A naghị ezipụ faịlụ ndị ahụ ebe ọ bụla.

A naghị eji sava onye mepụtara chekwaa data gị.

## Ojiji netwọk

### Ebe ị haziri

Mgbe ọrụ na-agba, ngwa na-ejikọ naanị na ebe ị kpebiri. Nke ahụ nwere ike ịbụ folda mpaghara ma ọ bụ sava ị tinyere: FTP, FTPS, SFTP, WebDAV, SMB, NFS, nchekwa dakọtara na S3, ma ọ bụ Azure Blob Storage. A na-eziga aha faịlụ, ọdịnaya, na nzere ị tinyere na sava ahụ ka ọrụ nwee ike ịgba. Ọrụ ọ bụla nwere iwu nzuzo nke ya. Onye mepụtara anaghị enweta okporo ahụ.

BUtil Server nwere ike ige ntị na kọmputa a ka onye ahịa BUtil ị haziri nwee ike izipu faịlụ. Okporo ahụ na-anọ n'etiti kọmputa ndị ị haziri.

### Nyocha mmelite

Mwube ndị na-abụghị nke Store nwere ike ịrịọ mwepụta GitHub kacha ọhụrụ:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) na-anata arịrịọ HTTPS nkịtị (adreesị IP, user-agent, oge). Onye mepụtara anaghị enweta okporo ahụ.

Ntinye sitere na Microsoft Store anaghị eji nyocha a; Store na-eweta mmelite.

### Njikọ ị meghere

Ngwa nwere ike imepe ibe ndị a na ihe nchọgharị sistemụ. Saịtị ndị ahụ nwere iwu nzuzo nke ha:

- Ibe mbụ nke oru: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Mwepụta kacha ọhụrụ: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Enyemaka ụkpụrụ faịlụ: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Enyemaka usoro ụbọchị: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Isi mmalite akara ngosi: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

A na-egosi ikike na iwu nzuzo a n'ime ngwa. A naghị emepe ha dị ka ibe weebụ.

## Usoro oge

Na Windows ị nwere ike ịgba ọrụ mgbe ị banyere ma ọ bụ site na usoro oge izu. Ngwa na-edeba ya na Windows Task Scheduler n'aha na-amalite na `BUtil`. Nke a na-amalite naanị ngwa a na kọmputa gị.

## Ụmụaka

Ngwa bụ ngwa nchekwa ndabere na mmekọrịta faịlụ. Ọ abụghị maka ụmụaka n'okpuru afọ 13.

## Ndị nke atọ

GitHub na-ahazi arịrịọ nyocha mmelite na ibe ị meghere, dị ka akọwara n'elu. Microsoft Store na-ahazi ntinye na mmelite Store. Ndị na-enye nchekwa ị haziri na-ahazi faịlụ na nzere ọrụ zitere ha. Onye mepụtara anaghị enweta okporo ahụ.

## Mgbanwe

A ga-ebipụta mmelite nke iwu a na faịlụ a na ebe nchekwa oru.

## Kọntaktị

Aha ngwa: BUtil
Aha onye mepụtara: Siarhei Kuchuk

Ajụjụ: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
