Here is the clarified a couples of security questions.

# Protection of Configuration Data
Configuration data is tasks and they are not encrypted. Because if attacker has access to system, he already has access to what is being protected. And also he can act on behalf of any application. That's why encryption of files was removed.

# Generation of Passwords
Implemented password generator uses .Net RNGCryptoServiceProvider and collects entropy for generating strong passwords from high Latin, low Latin symbols and and numbers.
It will be a good idea if you will have different passwords for different programs, sites and so on. The best(IMHO) open-source program for such thing is [KeePas](http://www.sourceforge.net/projects/keepas) (AES-256 encryption, good password generator, portable, Win).
