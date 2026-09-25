[Languages](README.md)

# Adatvédelmi nyilatkozat

Utolsó frissítés: 2026. szeptember 25.

**BUtil** by Siarhei Kuchuk

Alkalmazás neve: BUtil
Fejlesztő neve: Siarhei Kuchuk

A BUtil biztonsági mentést készít, szinkronizál és visszaállít fájlokat ezen a számítógépen. Médiatartalmat is importálhat, mappát oszthat meg, vagy fájlokat tölthet fel egy Ön által beállított kiszolgálóra. Nem hoz létre fejlesztői fiókot. A fejlesztő nem üzemeltet olyan kiszolgálót, amely a fájljait, jelszavait vagy használati adatait fogadná.

## Adatok, amelyeket a fejlesztő nem gyűjt

Az alkalmazás nem tartalmaz hirdetést, elemzést, összeomlásjelentést vagy követő SDK-t. A fejlesztő nem gyűjt, nem ad el és nem oszt meg személyes adatokat.

## Az Ön számítógépén tárolt adatok

### Feladatok és beállítások

A feladatdefiníciók csak ezen a számítógépen tárolódnak. Egy feladat tartalmazhat mappaútvonalakat, ütemezést, tárolási beállításokat, valamint az Ön által beírt jelszavakat vagy tokeneket. A jelszavak és a tárolási titkok mentés előtt titkosítva kerülnek erre a számítógépre, és csak ezen a számítógépen olvashatók. Ezek az értékek nem kerülnek fel a fejlesztőhöz.

- Windows-feladatok: `%AppData%\BUtil Backup Tasks`
- Linux-feladatok: `~/.config/BUtil Backup Tasks`
- Windows-beállítások (beleértve a témát és a licenchez vagy az adatvédelemhez utoljára választott nyelvet): `%AppData%\BUtil\Settings\v1`
- Linux-beállítások: `~/.config/BUtil/Settings/v1`
- Windows-feladatok állapota: `%AppData%\BUtil\States`
- Linux-feladatok állapota: `~/.config/BUtil/States`
- Windows médiaimport állapota: `%AppData%\BUtil Backup Tasks - States`
- Linux médiaimport állapota: `~/.config/BUtil Backup Tasks - States`

### Az Ön által választott fájlok

A biztonsági mentés, a szinkronizálás, a visszaállítás és az importálás az Ön által kiválasztott mappákat olvassa és írja. Ezek a fájlok ezen a számítógépen vagy az Ön által beállított tárolási helyen maradnak. Az alkalmazás nem tölti fel őket a fejlesztőhöz.

### Naplók

A diagnosztikai naplók csak erre a számítógépre íródnak:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Ezek a fájlok sehova sem kerülnek elküldésre.

A fejlesztő egyetlen kiszolgálóját sem használjuk az adatai tárolására.

## Hálózathasználat

### Az Ön által beállított célhelyek

Amikor egy feladat fut, az alkalmazás csak az Ön által megadott helyhez csatlakozik. Ez lehet helyi mappa vagy egy Ön által megadott kiszolgáló: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-kompatibilis tároló vagy Azure Blob Storage. A fájlnevek, a fájlok tartalma és a megadott hitelesítő adatok erre a kiszolgálóra kerülnek, hogy a feladat lefusson. Ezeknek a szolgáltatásoknak saját adatvédelmi nyilatkozatuk van. A fejlesztő nem kapja meg ezt a forgalmat.

A BUtil Server figyelhet ezen a számítógépen, hogy egy Ön által beállított BUtil-ügyfél fájlokat küldhessen. Ez a forgalom az Ön által beállított számítógépek között marad.

### Frissítésellenőrzés

A Store-on kívüli buildek lekérhetik a legújabb GitHub-kiadást:

`https://api.github.com/repos/drweb86/butil/releases/latest`

A GitHub (Microsoft) egy szokásos HTTPS-kérést kap (IP-cím, user-agent, idő). A fejlesztő nem kapja meg ezt a forgalmat.

A Microsoft Store-ból telepített példányok nem használják ezt az ellenőrzést; a frissítéseket a Store kézbesíti.

### Megnyitott hivatkozások

Az alkalmazás ezeket az oldalakat megnyithatja a rendszer böngészőjében. Ezeknek a webhelyeknek saját adatvédelmi nyilatkozatuk van:

- A projekt kezdőlapja: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Legújabb kiadás: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Fájlminta-súgó: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Dátumformátum-súgó: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Ikonforrások: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

A licenc és ez az adatvédelmi nyilatkozat az alkalmazáson belül jelenik meg. Nem nyílnak meg weblapként.

## Ütemezés

Windows rendszeren egy feladatot bejelentkezéskor vagy heti ütemezéssel futtathat. Az alkalmazás ezt a Windows Feladatütemezőben `BUtil` kezdetű néven regisztrálja. Ez csak ezt az alkalmazást indítja el a számítógépén.

## Gyermekek

Az alkalmazás biztonsági mentési és fájlszinkronizálási eszköz. Nem 13 év alatti gyermekeknek szól.

## Harmadik felek

A GitHub a fentiek szerint dolgozza fel a frissítésellenőrzési kérést és a megnyitott oldalakat. A Microsoft Store a Store-telepítéseket és -frissítéseket dolgozza fel. Az Ön által beállított tárolószolgáltatók dolgozzák fel a feladat által nekik küldött fájlokat és hitelesítő adatokat. A fejlesztő nem kapja meg ezt a forgalmat.

## Változások

A nyilatkozat frissítései a projekt tárolójában, ebben a fájlban jelennek meg.

## Kapcsolat

Alkalmazás neve: BUtil
Fejlesztő neve: Siarhei Kuchuk

Kérdések: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
