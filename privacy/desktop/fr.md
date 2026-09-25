[Languages](README.md)

# Politique de confidentialité

Dernière mise à jour : 25 septembre 2026

**BUtil** by Siarhei Kuchuk

Nom de l’application : BUtil
Nom du développeur : Siarhei Kuchuk

BUtil sauvegarde, synchronise et restaure des fichiers sur cet ordinateur. Il peut aussi importer des médias, partager un dossier ou envoyer des fichiers vers un serveur que vous indiquez. Il ne crée pas de compte développeur. Le développeur n’exploite pas de serveur qui reçoit vos fichiers, mots de passe ou données d’utilisation.

## Données que le développeur ne collecte pas

L’application ne contient pas de publicité, d’analyse, de rapport de plantage ni de SDK de suivi. Le développeur ne collecte, ne vend ni ne partage de données personnelles.

## Données stockées sur votre ordinateur

### Tâches et paramètres

Les définitions de tâches sont stockées uniquement sur cet ordinateur. Une tâche peut contenir des chemins de dossiers, une planification, des paramètres de stockage, ainsi que les mots de passe ou jetons que vous saisissez. Les mots de passe et secrets de stockage sont chiffrés sur cet ordinateur avant d’être enregistrés, et ne peuvent être lus que sur cet ordinateur. Ces valeurs ne sont pas envoyées au développeur.

- Tâches Windows : `%AppData%\BUtil Backup Tasks`
- Tâches Linux : `~/.config/BUtil Backup Tasks`
- Paramètres Windows (y compris le thème et la langue choisie en dernier pour la licence ou la confidentialité) : `%AppData%\BUtil\Settings\v1`
- Paramètres Linux : `~/.config/BUtil/Settings/v1`
- État des tâches Windows : `%AppData%\BUtil\States`
- État des tâches Linux : `~/.config/BUtil/States`
- État d’importation des médias Windows : `%AppData%\BUtil Backup Tasks - States`
- État d’importation des médias Linux : `~/.config/BUtil Backup Tasks - States`

### Fichiers que vous choisissez

La sauvegarde, la synchronisation, la restauration et l’importation lisent et écrivent les dossiers que vous sélectionnez. Ces fichiers restent sur cet ordinateur, ou sur la destination de stockage que vous configurez. L’application ne les envoie pas au développeur.

### Journaux

Les journaux de diagnostic sont écrits uniquement sur cet ordinateur :

- Windows : `%LocalAppData%\BUtil\logs\v4`
- Linux : `~/.local/share/BUtil/logs/v4`

Ces fichiers ne sont envoyés nulle part.

Aucun serveur du développeur n’est utilisé pour stocker vos données.

## Utilisation du réseau

### Destinations que vous configurez

Lorsqu’une tâche s’exécute, l’application se connecte uniquement à l’endroit que vous indiquez. Il peut s’agir d’un dossier local ou d’un serveur que vous saisissez : FTP, FTPS, SFTP, WebDAV, SMB, NFS, un stockage compatible S3 ou Azure Blob Storage. Les noms de fichiers, leur contenu et les identifiants que vous avez saisis sont envoyés à ce serveur pour que la tâche puisse s’exécuter. Chacun de ces services a sa propre politique de confidentialité. Le développeur ne reçoit pas ce trafic.

BUtil Server peut écouter sur cet ordinateur afin qu’un client BUtil que vous configurez puisse envoyer des fichiers. Ce trafic reste entre les ordinateurs que vous avez configurés.

### Vérification des mises à jour

Les versions hors Store peuvent demander la dernière version GitHub :

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) reçoit une requête HTTPS ordinaire (adresse IP, user-agent, heure). Le développeur ne reçoit pas ce trafic.

Les installations depuis le Microsoft Store n’utilisent pas cette vérification ; le Store fournit les mises à jour.

### Liens que vous ouvrez

L’application peut ouvrir ces pages dans le navigateur du système. Ces sites ont leurs propres politiques de confidentialité :

- Page du projet : [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Dernière version : [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Aide sur les motifs de fichiers : [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Aide sur le format de date : [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Crédits des icônes : [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

La licence et cette politique de confidentialité sont affichées dans l’application. Elles ne sont pas ouvertes comme des pages web.

## Planification

Sous Windows, vous pouvez exécuter une tâche à la connexion ou selon un calendrier hebdomadaire. L’application l’enregistre dans le Planificateur de tâches Windows sous un nom qui commence par `BUtil`. Cela lance seulement cette application sur votre ordinateur.

## Enfants

L’application est un outil de sauvegarde et de synchronisation de fichiers. Elle ne s’adresse pas aux enfants de moins de 13 ans.

## Tiers

GitHub traite la demande de vérification des mises à jour et les pages que vous ouvrez, comme indiqué ci-dessus. Le Microsoft Store traite les installations et mises à jour du Store. Les fournisseurs de stockage que vous configurez traitent les fichiers et identifiants que la tâche leur envoie. Le développeur ne reçoit pas ce trafic.

## Modifications

Les mises à jour de cette politique seront publiées dans ce fichier du dépôt du projet.

## Contact

Nom de l’application : BUtil
Nom du développeur : Siarhei Kuchuk

Questions : [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
