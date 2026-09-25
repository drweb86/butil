[Languages](README.md)

# Informativa sulla privacy

Ultimo aggiornamento: 25 settembre 2026

**BUtil** by Siarhei Kuchuk

Nome dell’applicazione: BUtil
Nome dello sviluppatore: Siarhei Kuchuk

BUtil esegue il backup, sincronizza e ripristina i file su questo computer. Può anche importare contenuti multimediali, condividere una cartella o caricare file su un server che configuri. Non crea un account sviluppatore. Lo sviluppatore non gestisce un server che riceve i tuoi file, le password o i dati di utilizzo.

## Dati che lo sviluppatore non raccoglie

L’app non include pubblicità, analisi, segnalazioni di arresti anomali né SDK di tracciamento. Lo sviluppatore non raccoglie, non vende e non condivide dati personali.

## Dati memorizzati sul tuo computer

### Attività e impostazioni

Le definizioni delle attività sono memorizzate solo su questo computer. Un’attività può includere percorsi di cartelle, una pianificazione, impostazioni di archiviazione e le password o i token che digiti. Le password e i segreti di archiviazione vengono cifrati su questo computer prima di essere salvati e possono essere letti solo su questo computer. Questi valori non vengono inviati allo sviluppatore.

- Attività Windows: `%AppData%\BUtil Backup Tasks`
- Attività Linux: `~/.config/BUtil Backup Tasks`
- Impostazioni Windows (incluso il tema e la lingua scelta per ultima per la licenza o la privacy): `%AppData%\BUtil\Settings\v1`
- Impostazioni Linux: `~/.config/BUtil/Settings/v1`
- Stato delle attività Windows: `%AppData%\BUtil\States`
- Stato delle attività Linux: `~/.config/BUtil/States`
- Stato di importazione multimediale Windows: `%AppData%\BUtil Backup Tasks - States`
- Stato di importazione multimediale Linux: `~/.config/BUtil Backup Tasks - States`

### File che scegli

Backup, sincronizzazione, ripristino e importazione leggono e scrivono le cartelle che selezioni. Quei file restano su questo computer o nella destinazione di archiviazione che configuri. L’app non li invia allo sviluppatore.

### Registri

I registri diagnostici vengono scritti solo su questo computer:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Quei file non vengono inviati da nessuna parte.

Nessun server dello sviluppatore viene usato per memorizzare i tuoi dati.

## Uso della rete

### Destinazioni che configuri

Quando un’attività viene eseguita, l’app si collega solo al luogo che indichi. Può essere una cartella locale o un server che inserisci: FTP, FTPS, SFTP, WebDAV, SMB, NFS, archiviazione compatibile con S3 o Azure Blob Storage. Nomi dei file, contenuti e le credenziali che hai inserito vengono inviati a quel server affinché l’attività possa essere eseguita. Ciascuno di questi servizi ha la propria informativa sulla privacy. Lo sviluppatore non riceve quel traffico.

BUtil Server può restare in ascolto su questo computer in modo che un client BUtil che configuri possa inviare file. Quel traffico resta tra i computer che configuri.

### Controllo degli aggiornamenti

Le build non dello Store possono richiedere l’ultima versione su GitHub:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) riceve una normale richiesta HTTPS (indirizzo IP, user-agent, ora). Lo sviluppatore non riceve quel traffico.

Le installazioni da Microsoft Store non usano questo controllo; lo Store fornisce gli aggiornamenti.

### Link che apri

L’app può aprire queste pagine nel browser di sistema. Quei siti hanno le proprie informative sulla privacy:

- Pagina del progetto: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Ultima versione: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Guida ai modelli di file: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Guida al formato data: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Crediti delle icone: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

La licenza e questa informativa sulla privacy sono mostrate nell’app. Non vengono aperte come pagine web.

## Pianificazione

In Windows puoi eseguire un’attività all’accesso o con una pianificazione settimanale. L’app la registra in Utilità di pianificazione di Windows con un nome che inizia con `BUtil`. Questo avvia solo questa app sul tuo computer.

## Minori

L’app è uno strumento di backup e sincronizzazione dei file. Non è destinata a minori di 13 anni.

## Terze parti

GitHub elabora la richiesta di controllo degli aggiornamenti e le pagine che apri, come indicato sopra. Microsoft Store elabora installazioni e aggiornamenti dello Store. I provider di archiviazione che configuri elaborano i file e le credenziali che l’attività invia loro. Lo sviluppatore non riceve quel traffico.

## Modifiche

Gli aggiornamenti di questa informativa saranno pubblicati in questo file nel repository del progetto.

## Contatto

Nome dell’applicazione: BUtil
Nome dello sviluppatore: Siarhei Kuchuk

Domande: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
