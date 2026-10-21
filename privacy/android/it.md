[Languages](README.md)

# Informativa sulla privacy

Ultimo aggiornamento: 21 ottobre 2026

**BUtil** by Siarhei Kuchuk

Nome dell’applicazione: BUtil
Nome dello sviluppatore: Siarhei Kuchuk

BUtil esegue il backup, sincronizza e ripristina i file su questo dispositivo. Può anche importare contenuti multimediali, condividere una cartella tramite FTPS o caricare file su un server configurato dall’utente. Non crea un account dello sviluppatore. Lo sviluppatore non gestisce un’infrastruttura lato server che riceve i file, le password o i dati di utilizzo.

## Dati che lo sviluppatore non raccoglie

L’applicazione non include pubblicità, analisi, segnalazioni di arresti anomali né SDK di tracciamento. Lo sviluppatore non raccoglie, non vende e non condivide dati personali.

Questa informativa sulla privacy e la licenza sono mostrate all’interno dell’applicazione. Non vengono aperte come pagine web.

## Dati memorizzati sul dispositivo

### Attività e impostazioni

Le definizioni delle attività sono memorizzate solo nell’archivio privato dell’applicazione su questo dispositivo. Un’attività può includere percorsi di cartelle, una pianificazione, impostazioni di archiviazione e le password o i token digitati. Le password e i segreti di archiviazione vengono sigillati con l’archivio chiavi di Android prima di essere salvati e possono essere letti solo su questo dispositivo. Tali valori non vengono inviati allo sviluppatore. Non sono inclusi nel backup di Android. La disinstallazione dell’applicazione li rimuove.

Il tema scelto è memorizzato nelle impostazioni private dell’applicazione su questo dispositivo.

### File scelti

Backup, sincronizzazione, ripristino, importazione e caricamento leggono e scrivono le cartelle selezionate con il selettore di documenti di sistema, oppure il server configurato. Quei file restano nelle cartelle scelte o nella destinazione di archiviazione configurata. L’applicazione non li carica verso lo sviluppatore. La disinstallazione dell’applicazione non elimina i file nelle cartelle selezionate.

### Certificato FTPS

Quando si condivide una cartella tramite FTPS, l’applicazione crea un certificato autofirmato nell’archivio privato dell’applicazione. La disinstallazione dell’applicazione rimuove quel certificato. I file condivisi restano nella cartella autorizzata.

Nessun server dello sviluppatore viene usato per memorizzare i dati.

## Uso della rete

### Destinazioni configurate

Quando un’attività viene eseguita, l’applicazione si collega solo al luogo impostato. Può essere una cartella su questo dispositivo oppure un server indicato: FTP, FTPS, SFTP, WebDAV, SMB, NFS, archiviazione compatibile con S3 o Azure Blob Storage. I nomi dei file, il contenuto dei file e le credenziali inserite vengono inviati a quel server affinché l’attività possa essere eseguita. Ciascuno di questi servizi ha una propria informativa sulla privacy. Lo sviluppatore non riceve quel traffico.

La condivisione di una cartella tramite FTPS resta in ascolto su questo dispositivo, in modo che un client FTPS configurato possa leggere o scrivere la cartella autorizzata, usando il nome utente e la password impostati. Quel traffico resta tra i dispositivi (e chiunque nella rete possieda quelle credenziali). Lo sviluppatore non ne è parte. L’utente è responsabile di chi può raggiungere la porta, di quale cartella viene condivisa e della robustezza di quella password.

### Collegamento aperto

La riga della versione nel menu può aprire la pagina iniziale del progetto nel browser di sistema:

[github.com/drweb86/butil](https://github.com/drweb86/butil)

Quel sito ha una propria informativa sulla privacy. Lo sviluppatore non riceve quel traffico.

L’applicazione non controlla gli aggiornamenti e non contatta un servizio di aggiornamento.

## Pianificazione

È possibile eseguire un’attività all’avvio del dispositivo oppure nei giorni scelti. L’applicazione la registra con la sveglia di sistema. Ciò avvia solo questa applicazione sul dispositivo.

## Autorizzazioni

L’applicazione usa solo le seguenti autorizzazioni:

- **Internet** e **stato della rete**, per raggiungere un server configurato e per condividere una cartella tramite FTPS.
- **Notifiche** e un **servizio in primo piano**, per mantenere in esecuzione un’attività in corso con lo schermo spento.
- **Sveglie esatte** e **esecuzione all’avvio**, per avviare un’attività pianificata.
- **Blocco di attivazione**, per ridurre la sospensione durante un trasferimento.

## Minori

L’applicazione è uno strumento di backup e sincronizzazione dei file. Non è rivolta a minori di 13 anni.

## Terze parti

I fornitori di archiviazione configurati elaborano i file e le credenziali che l’attività invia loro. L’apertura della pagina iniziale del progetto è gestita dal browser di sistema e da quel sito. Lo sviluppatore non riceve quel traffico.

## Modifiche

Gli aggiornamenti di questa informativa sono inclusi nell’applicazione.

## Contatto

Nome dell’applicazione: BUtil
Nome dello sviluppatore: Siarhei Kuchuk

Le domande su questa applicazione per Android possono essere inviate tramite la scheda dello store usata per installarla, oppure a [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
