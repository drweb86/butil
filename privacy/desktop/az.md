[Languages](README.md)

# Məxfilik siyasəti

Son yenilənmə: 25 sentyabr 2026

**BUtil** by Siarhei Kuchuk

Tətbiqin adı: BUtil
Tərtibatçının adı: Siarhei Kuchuk

BUtil bu kompüterdə faylların ehtiyat nüsxəsini çıxarır, sinxronlaşdırır və bərpa edir. O, həmçinin media idxal edə, qovluğu paylaşa və ya faylları sizin qurduğunuz serverə yükləyə bilər. Tərtibatçı hesabı yaratmır. Tərtibatçı fayllarınızı, parollarınızı və ya istifadə məlumatlarınızı qəbul edən server işlətmir.

## Tərtibatçının toplamadığı məlumatlar

Tətbiqdə reklam, analitika, qəza hesabatı və izləmə SDK-sı yoxdur. Tərtibatçı şəxsi məlumatları toplamır, satmır və paylaşmır.

## Kompüterinizdə saxlanan məlumatlar

### Tapşırıqlar və parametrlər

Tapşırıq tərifləri yalnız bu kompüterdə saxlanılır. Tapşırıq qovluq yollarını, cədvəli, saxlama parametrlərini və yazdığınız parolları və ya tokenləri əhatə edə bilər. Parollar və saxlama sirləri saxlanılmazdan əvvəl bu kompüterdə şifrələnir və yalnız bu kompüterdə oxuna bilər. Bu dəyərlər tərtibatçıya yüklənmir.

- Windows tapşırıqları: `%AppData%\BUtil Backup Tasks`
- Linux tapşırıqları: `~/.config/BUtil Backup Tasks`
- Windows parametrləri (mövzu və Lisenziya və ya Məxfilik üçün son seçilmiş dil daxil olmaqla): `%AppData%\BUtil\Settings\v1`
- Linux parametrləri: `~/.config/BUtil/Settings/v1`
- Windows tapşırıq vəziyyəti: `%AppData%\BUtil\States`
- Linux tapşırıq vəziyyəti: `~/.config/BUtil/States`
- Windows media idxalı vəziyyəti: `%AppData%\BUtil Backup Tasks - States`
- Linux media idxalı vəziyyəti: `~/.config/BUtil Backup Tasks - States`

### Seçdiyiniz fayllar

Ehtiyat nüsxə, sinxronlaşdırma, bərpa və idxal seçdiyiniz qovluqları oxuyur və yazır. Bu fayllar bu kompüterdə və ya qurduğunuz saxlama yerində qalır. Tətbiq onları tərtibatçıya yükləmir.

### Jurnallar

Diaqnostika jurnalları yalnız bu kompüterə yazılır:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Bu fayllar heç yerə göndərilmir.

Məlumatlarınızı saxlamaq üçün tərtibatçının serveri istifadə olunmur.

## Şəbəkə istifadəsi

### Qurduğunuz təyinatlar

Tapşırıq işləyəndə tətbiq yalnız təyin etdiyiniz yerə qoşulur. Bu, yerli qovluq və ya daxil etdiyiniz server ola bilər: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3 ilə uyğun saxlama və ya Azure Blob Storage. Fayl adları, fayl məzmunu və daxil etdiyiniz etimadnamələr tapşırığın işləməsi üçün həmin serverə göndərilir. Bu xidmətlərin hər birinin öz məxfilik siyasəti var. Tərtibatçı bu trafiki almır.

BUtil Server bu kompüterdə dinləyə bilər ki, qurduğunuz BUtil müştərisi fayl göndərə bilsin. Bu trafik qurduğunuz kompüterlər arasında qalır.

### Yeniləmə yoxlaması

Store xaricindəki qurmalar ən son GitHub buraxılışını istəyə bilər:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) adi HTTPS sorğusu alır (IP ünvanı, user-agent, vaxt). Tərtibatçı bu trafiki almır.

Microsoft Store quraşdırmaları bu yoxlamanı istifadə etmir; yeniləmələri Store çatdırır.

### Açdığınız keçidlər

Tətbiq bu səhifələri sistem brauzerində aça bilər. Bu saytların öz məxfilik siyasətləri var:

- Layihənin ana səhifəsi: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Ən son buraxılış: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Fayl nümunəsi köməyi: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Tarix formatı köməyi: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Nişan mənbələri: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Lisenziya və bu məxfilik siyasəti tətbiqin içində göstərilir. Onlar veb səhifə kimi açılmır.

## Cədvəl

Windows-da tapşırığı giriş zamanı və ya həftəlik cədvəllə işə sala bilərsiniz. Tətbiq bunu Windows Tapşırıq Planlayıcısında `BUtil` ilə başlayan adla qeyd edir. Bu, yalnız bu tətbiqi kompüterinizdə işə salır.

## Uşaqlar

Tətbiq ehtiyat nüsxə və fayl sinxronlaşdırma alətidir. 13 yaşından kiçik uşaqlar üçün nəzərdə tutulmayıb.

## Üçüncü tərəflər

GitHub yeniləmə yoxlaması sorğusunu və açdığınız səhifələri yuxarıda göstərildiyi kimi emal edir. Microsoft Store, Store quraşdırmalarını və yeniləmələrini emal edir. Qurduğunuz saxlama təminatçıları tapşırığın onlara göndərdiyi faylları və etimadnamələri emal edir. Tərtibatçı bu trafiki almır.

## Dəyişikliklər

Bu siyasətin yeniləmələri layihə anbarındakı bu faylda dərc olunacaq.

## Əlaqə

Tətbiqin adı: BUtil
Tərtibatçının adı: Siarhei Kuchuk

Suallar: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
