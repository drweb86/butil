[Languages](README.md)

# Gizlilik ilkesi

Son güncelleme: 25 Eylül 2026

**BUtil** by Siarhei Kuchuk

Uygulama adı: BUtil
Geliştirici adı: Siarhei Kuchuk

BUtil, bu bilgisayardaki dosyaları yedekler, eşitler ve geri yükler. Ortam dosyalarını içe aktarabilir, bir klasörü paylaşabilir veya dosyaları yapılandırdığınız bir sunucuya yükleyebilir. Geliştirici hesabı oluşturmaz. Geliştirici, dosyalarınızı, parolalarınızı veya kullanım verilerinizi alan bir arka uç işletmez.

## Geliştiricinin toplamadığı veriler

Uygulamada reklam, analiz, çökme bildirimi veya izleme SDK’sı yoktur. Geliştirici kişisel verileri toplamaz, satmaz veya paylaşmaz.

## Bilgisayarınızda saklanan veriler

### Görevler ve ayarlar

Görev tanımları yalnızca bu bilgisayarda saklanır. Bir görev; klasör yollarını, bir zamanlamayı, depolama ayarlarını ve yazdığınız parolaları veya belirteçleri içerebilir. Parolalar ve depolama gizleri kaydedilmeden önce bu bilgisayarda şifrelenir ve yalnızca bu bilgisayarda okunabilir. Bu değerler geliştiriciye yüklenmez.

- Windows görevleri: `%AppData%\BUtil Backup Tasks`
- Linux görevleri: `~/.config/BUtil Backup Tasks`
- Windows ayarları (tema ve Lisans veya Gizlilik için son seçilen dil dahil): `%AppData%\BUtil\Settings\v1`
- Linux ayarları: `~/.config/BUtil/Settings/v1`
- Windows görev durumu: `%AppData%\BUtil\States`
- Linux görev durumu: `~/.config/BUtil/States`
- Windows ortam içe aktarma durumu: `%AppData%\BUtil Backup Tasks - States`
- Linux ortam içe aktarma durumu: `~/.config/BUtil Backup Tasks - States`

### Seçtiğiniz dosyalar

Yedekleme, eşitleme, geri yükleme ve içe aktarma, seçtiğiniz klasörleri okur ve yazar. Bu dosyalar bu bilgisayarda veya yapılandırdığınız depolama hedefinde kalır. Uygulama bunları geliştiriciye yüklemez.

### Günlükler

Tanılama günlükleri yalnızca bu bilgisayara yazılır:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Bu dosyalar hiçbir yere gönderilmez.

Verilerinizi saklamak için geliştirici sunucusu kullanılmaz.

## Ağ kullanımı

### Yapılandırdığınız hedefler

Bir görev çalıştığında uygulama yalnızca belirlediğiniz yere bağlanır. Bu, yerel bir klasör veya girdiğiniz bir sunucu olabilir: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3 uyumlu depolama veya Azure Blob Storage. Dosya adları, dosya içerikleri ve girdiğiniz kimlik bilgileri, görevin çalışabilmesi için o sunucuya gönderilir. Bu hizmetlerin her birinin kendi gizlilik ilkesi vardır. Geliştirici bu trafiği almaz.

BUtil Server, yapılandırdığınız bir BUtil istemcisinin dosya gönderebilmesi için bu bilgisayarda dinleyebilir. Bu trafik, kurduğunuz bilgisayarlar arasında kalır.

### Güncelleme denetimi

Store dışı derlemeler en son GitHub sürümünü isteyebilir:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) normal bir HTTPS isteği alır (IP adresi, user-agent, saat). Geliştirici bu trafiği almaz.

Microsoft Store kurulumları bu denetimi kullanmaz; güncellemeleri Store sağlar.

### Açtığınız bağlantılar

Uygulama bu sayfaları sistem tarayıcınızda açabilir. Bu sitelerin kendi gizlilik ilkeleri vardır:

- Proje ana sayfası: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- En son sürüm: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Dosya deseni yardımı: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Tarih biçimi yardımı: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Simge kaynakları: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Lisans ve bu gizlilik ilkesi uygulamanın içinde gösterilir. Web sayfası olarak açılmazlar.

## Zamanlama

Windows’ta bir görevi oturum açıldığında veya haftalık bir zamanlamayla çalıştırabilirsiniz. Uygulama bunu, adı `BUtil` ile başlayan bir adla Windows Görev Zamanlayıcı’ya kaydeder. Bu yalnızca bu uygulamayı bilgisayarınızda başlatır.

## Çocuklar

Uygulama bir yedekleme ve dosya eşitleme aracıdır. 13 yaşından küçük çocuklara yönelik değildir.

## Üçüncü taraflar

GitHub, güncelleme denetimi isteğini ve açtığınız sayfaları yukarıda açıklandığı gibi işler. Microsoft Store, Store kurulumlarını ve güncellemelerini işler. Yapılandırdığınız depolama sağlayıcıları, görevin onlara gönderdiği dosyaları ve kimlik bilgilerini işler. Geliştirici bu trafiği almaz.

## Değişiklikler

Bu ilkedeki güncellemeler proje deposundaki bu dosyada yayımlanır.

## İletişim

Uygulama adı: BUtil
Geliştirici adı: Siarhei Kuchuk

Sorular: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
