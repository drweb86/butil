Here is the clarified a couples of security questions.

# ZIP format

Why Zip format is not used and app relies on external 7-ZIP? It's because while ZIP is part of .Net and has popular implementations, it does not have encryption of file names. That's why in password protected archives file names in ZIP are exposed. In 7-zip file names are also encrypted.

# Protection of Configuration Data
Configuration data is encrypted under windows user account if Windows supports encrypted file system EFS and if system partition is formatted in Ntfs. In all other cases it's not encrypted. 

# Generation Of Passwords
Implemented password generator uses .Net RNGCryptoServiceProvider and collects entropy for generating strong passwords from high Latin, low Latin symbols and and numbers.
It will be a good idea if you will have different passwords for different programs, sites and so on. The best(IMHO) open-source program for such thing is [KeePas](http://www.sourceforge.net/projects/keepas) (AES-256 encryption, good password generator, portable, Win).

# Protection From Viruses
All assemblies use strong name.
7-zip is expected to be protected by OS means.
