[Languages](README.md)

# Politique de confidentialité

Dernière mise à jour : 21 octobre 2026

**BUtil** by Siarhei Kuchuk

Nom de l’application : BUtil
Nom du développeur : Siarhei Kuchuk

BUtil sauvegarde, synchronise et restaure des fichiers sur cet appareil. Il peut aussi importer des médias, partager un dossier par FTPS ou envoyer des fichiers vers un serveur que vous configurez. Il ne crée pas de compte développeur. Le développeur n’exploite pas d’infrastructure dorsale qui reçoit vos fichiers, vos mots de passe ou vos données d’utilisation.

## Données que le développeur ne collecte pas

L’application ne contient pas de publicité, d’analyse, de rapports de plantage ni de SDK de suivi. Le développeur ne collecte, ne vend ni ne partage de données personnelles.

Cette politique de confidentialité et la licence sont affichées dans l’application. Elles ne sont pas ouvertes comme des pages web.

## Données stockées sur votre appareil

### Tâches et réglages

Les définitions des tâches sont stockées uniquement dans le stockage privé de l’application sur cet appareil. Une tâche peut comprendre des chemins de dossiers, une planification, des réglages de stockage, ainsi que les mots de passe ou jetons que vous saisissez. Les mots de passe et les secrets de stockage sont scellés avec le magasin de clés Android avant d’être enregistrés et ne peuvent être lus que sur cet appareil. Ces valeurs ne sont pas envoyées au développeur. Elles ne sont pas incluses dans la sauvegarde Android. La désinstallation de l’application les supprime.

Le thème que vous choisissez est stocké dans les réglages privés de l’application sur cet appareil.

### Fichiers que vous choisissez

La sauvegarde, la synchronisation, la restauration, l’importation et l’envoi lisent et écrivent les dossiers que vous sélectionnez avec le sélecteur de documents du système, ou le serveur que vous configurez. Ces fichiers restent dans les dossiers que vous avez choisis ou sur la destination de stockage que vous configurez. L’application ne les envoie pas au développeur. La désinstallation de l’application ne supprime pas les fichiers des dossiers que vous avez sélectionnés.

### Certificat FTPS

Lorsque vous partagez un dossier par FTPS, l’application crée un certificat auto-signé dans le stockage privé de l’application. La désinstallation de l’application supprime ce certificat. Les fichiers partagés restent dans le dossier que vous avez autorisé.

Aucun serveur du développeur n’est utilisé pour stocker vos données.

## Utilisation du réseau

### Destinations que vous configurez

Lorsqu’une tâche s’exécute, l’application se connecte uniquement à l’endroit que vous avez défini. Il peut s’agir d’un dossier sur cet appareil ou d’un serveur que vous indiquez : FTP, FTPS, SFTP, WebDAV, SMB, NFS, un stockage compatible S3 ou Azure Blob Storage. Les noms de fichiers, le contenu des fichiers et les identifiants que vous avez saisis sont envoyés à ce serveur pour que la tâche puisse s’exécuter. Chacun de ces services a sa propre politique de confidentialité. Le développeur ne reçoit pas ce trafic.

Le partage d’un dossier par FTPS écoute sur cet appareil afin qu’un client FTPS que vous configurez puisse lire ou écrire le dossier que vous avez autorisé, avec le nom d’utilisateur et le mot de passe que vous avez définis. Ce trafic reste entre vos appareils (et toute personne du réseau qui possède ces identifiants). Le développeur n’en est pas partie. Vous êtes responsable de qui peut atteindre le port, du dossier que vous partagez et de la robustesse de ce mot de passe.

L’application ne recherche pas de mises à jour et ne contacte pas de service de mise à jour.

## Planification

Vous pouvez exécuter une tâche au démarrage de l’appareil ou les jours que vous choisissez. L’application l’enregistre auprès de l’alarme système. Cela ne fait que démarrer cette application sur votre appareil.

## Autorisations

L’application utilise uniquement les autorisations suivantes :

- **Internet** et **état du réseau**, pour joindre un serveur que vous configurez et pour partager un dossier par FTPS.
- **Notifications** et un **service de premier plan**, pour maintenir une tâche en cours lorsque l’écran est éteint.
- **Alarmes exactes** et **exécution au démarrage**, pour lancer une tâche planifiée.
- **Verrou de réveil**, pour réduire la mise en veille pendant un transfert.

## Enfants

L’application est un outil de sauvegarde et de synchronisation de fichiers. Elle ne s’adresse pas aux enfants de moins de 13 ans.

## Tiers

Les fournisseurs de stockage que vous configurez traitent les fichiers et les identifiants que la tâche leur envoie. Le développeur ne reçoit pas ce trafic.

## Modifications

Les mises à jour de cette politique sont incluses dans l’application.

## Contact

Nom de l’application : BUtil
Nom du développeur : Siarhei Kuchuk

Les questions concernant cette application Android peuvent être envoyées par la fiche de la boutique utilisée pour l’installer, ou à [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
