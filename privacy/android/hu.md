[Languages](README.md)

# Adatvédelmi szabályzat

Utolsó frissítés: 2026. október 21.

**BUtil** by Siarhei Kuchuk

Alkalmazás neve: BUtil
Fejlesztő neve: Siarhei Kuchuk

A BUtil fájlokat ment, szinkronizál és állít vissza ezen az eszközön. Médiatartalmat is importálhat, mappát oszthat meg FTPS-en, vagy fájlokat tölthet fel egy Ön által beállított kiszolgálóra. Nem hoz létre fejlesztői fiókot. A fejlesztő nem üzemeltet olyan háttérrendszert, amely fogadná a fájljait, jelszavait vagy használati adatait.

## Adatok, amelyeket a fejlesztő nem gyűjt

Az alkalmazás nem tartalmaz hirdetéseket, analitikát, összeomlás-jelentőket vagy követő SDK-kat. A fejlesztő nem gyűjt, nem ad el és nem oszt meg személyes adatokat.

Ez az adatvédelmi szabályzat és a licenc az alkalmazáson belül jelenik meg. Nem nyílnak meg weblapként.

## Az eszközén tárolt adatok

### Feladatok és beállítások

A feladatmeghatározások csak az alkalmazás privát tárhelyén tárolódnak ezen az eszközön. Egy feladat tartalmazhat mappaútvonalakat, ütemezést, tárolási beállításokat, valamint az Ön által beírt jelszavakat vagy tokeneket. A jelszavak és a tárolási titkok mentés előtt az Android kulcstárral lezárásra kerülnek, és csak ezen az eszközön olvashatók. Ezek az értékek nem töltődnek fel a fejlesztőhöz. Nem szerepelnek az Android biztonsági mentésben. Az alkalmazás eltávolítása törli őket.

### Az Ön által választott fájlok

A mentés, a szinkronizálás, a visszaállítás, az importálás és a feltöltés olvassa és írja a rendszer dokumentumválasztójával kijelölt mappákat vagy az Ön által beállított kiszolgálót. Ezek a fájlok a választott mappákban vagy a beállított tárolási célhelyen maradnak. Az alkalmazás nem tölti fel őket a fejlesztőhöz. Az alkalmazás eltávolítása nem törli a kijelölt mappákban lévő fájlokat.

### FTPS-tanúsítvány

Ha mappát oszt meg FTPS-en, az alkalmazás önaláírt tanúsítványt hoz létre az alkalmazás privát tárhelyén. Az alkalmazás eltávolítása eltávolítja azt a tanúsítványt. A megosztott fájlok a megadott mappában maradnak.

A fejlesztőnek nincs kiszolgálója az adatok tárolására.

## Hálózathasználat

### Az Ön által beállított célhelyek

Amikor egy feladat fut, az alkalmazás csak az Ön által megadott helyhez csatlakozik. Ez lehet mappa ezen az eszközön, vagy egy Ön által megadott kiszolgáló: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3-kompatibilis tároló vagy Azure Blob Storage. A fájlnevek, a fájltartalom és a megadott hitelesítő adatok arra a kiszolgálóra kerülnek, hogy a feladat lefusson. E szolgáltatások mindegyikének saját adatvédelmi szabályzata van. A fejlesztő nem kapja meg ezt a forgalmat.

A mappa FTPS-en keresztüli megosztása ezen az eszközön figyel, hogy az Ön által beállított FTPS-ügyfél a megadott felhasználónévvel és jelszóval olvashassa vagy írja a megadott mappát. Ez a forgalom az Ön eszközei között marad (és bárki között a hálózaton, akinél megvannak ezek a hitelesítő adatok). A fejlesztő nem részese ennek. Ön felel azért, hogy ki érheti el a portot, melyik mappát osztja meg, és milyen erős a jelszó.

Az alkalmazás nem keres frissítéseket, és nem lép kapcsolatba frissítési szolgáltatással.

## Ütemezés

Futtathat feladatot az eszköz indulásakor vagy a választott napokon. Az alkalmazás ezt a rendszer ébresztőórájánál regisztrálja. Ez csak ezt az alkalmazást indítja el az eszközén.

## Engedélyek

Az alkalmazás csak a következő engedélyeket használja:

- **Internet** és **hálózati állapot**, hogy elérje az Ön által beállított kiszolgálót, és mappát osszon meg FTPS-en.
- **Értesítések** és **előtérszolgáltatás**, hogy a futó feladat kikapcsolt képernyő mellett is folytatódjon.
- **Pontos ébresztések** és **indításkor futás**, hogy elinduljon egy ütemezett feladat.
- **Ébresztési zár**, hogy csökkenjen az alvás átvitel közben.

## Gyermekek

Az alkalmazás mentési és fájlszinkronizáló eszköz. Nem 13 év alatti gyermekeknek szól.

## Harmadik felek

Az Ön által beállított tárolószolgáltatók dolgozzák fel a fájlokat és a hitelesítő adatokat, amelyeket a feladat küld nekik. A fejlesztő nem kapja meg ezt a forgalmat.

## Változások

A szabályzat frissítései az alkalmazásban szerepelnek.

## Kapcsolat

Alkalmazás neve: BUtil
Fejlesztő neve: Siarhei Kuchuk

Az Android-alkalmazással kapcsolatos kérdések elküldhetők azon az áruházi oldalon keresztül, amelyről telepítette, vagy itt: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
