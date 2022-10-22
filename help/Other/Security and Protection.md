Here is the clarified a couples of security questions.

# Protection of Configuration Data
Configuration data is encrypted under windows user account if Windows supports encrypted file system EFS and if system partition is formatted in Ntfs. In all other cases it's not encrypted. 

# Network Storages and Encryption Under Local System Account

This is an additional level of protection.

Requirements to remote server for encryption under local system account:
- Win 2k on remote machine;
- NTFS file system on shared folder with support of EFS.

Notes:
- If you will lose and reset your password or reinstall OS or recreate your account - you could not gain access to data in backup on this remote storage.
- Nobody(including you) acting from other account can access your data on this remote storage.

# Protection of Password Protected Images
Encryption of 7z archiver(built-in encryption) is AES-256

# Generation Of Passwords
Implemented password generator uses .Net RNGCryptoServiceProvider and collects entropy for generating strong passwords from high Latin, low Latin symbols and and numbers.
Passwords are stored securely in encrypted under your OS account file
It will be a good idea if you will have different passwords for different programs, sites and so on. The best(IMHO) open-source program for such thing is [KeePas](http://www.sourceforge.net/projects/keepas) (AES-256 encryption, good password generator, portable, Win).

# Protection From Viruses
All assemblies use strong name
7-zip binaries are checked for md5 checksumm each time on start of the program

# Deletion Of Temporary Files For Password Protected Images
Program deletes files securely when your backup was encrypted when backups and restores data. Program overwrites this files with zeros and then after that deletes them. Archiver also encrypts its own temporary files, but it deletes them by itself.

# How Can I Protect Software Logs?
You can open this folder in Windows Explorer and limit the access to this folder for other users or open properties of logs folder and check 'Encrypted' flag
