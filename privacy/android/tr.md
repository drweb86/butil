[Languages](README.md)

# Gizlilik politikası

Son güncelleme: 21 Ekim 2026

**BUtil** by Siarhei Kuchuk

Uygulama adı: BUtil
Geliştirici adı: Siarhei Kuchuk

BUtil, bu cihazda dosyaları yedekler, eşitler ve geri yükler. Ortam dosyalarını içe aktarabilir, bir klasörü FTPS üzerinden paylaşabilir veya yapılandırdığınız bir sunucuya dosya yükleyebilir. Bir geliştirici hesabı oluşturmaz. Geliştirici, dosyalarınızı, parolalarınızı veya kullanım verilerinizi alan bir arka uç işletmez.

## Geliştiricinin toplamadığı veriler

Uygulama reklam, analitik, çökme raporları veya izleme SDK’ları içermez. Geliştirici kişisel verileri toplamaz, satmaz veya paylaşmaz.

Bu gizlilik politikası ve lisans uygulama içinde gösterilir. Web sayfası olarak açılmazlar.

## Cihazınızda saklanan veriler

### Görevler ve ayarlar

Görev tanımları yalnızca bu cihazdaki uygulama özel deposunda saklanır. Bir görev, klasör yollarını, bir zamanlamayı, depolama ayarlarını ve yazdığınız parolaları veya belirteçleri içerebilir. Parolalar ve depolama gizleri kaydedilmeden önce Android anahtar deposu ile mühürlenir ve yalnızca bu cihazda okunabilir. Bu değerler geliştiriciye yüklenmez. Android yedeklemesine dahil edilmezler. Uygulamayı kaldırmak bunları siler.

### Seçtiğiniz dosyalar

Yedekleme, eşitleme, geri yükleme, içe aktarma ve yükleme, sistem belge seçicisiyle seçtiğiniz klasörleri veya yapılandırdığınız sunucuyu okur ve yazar. Bu dosyalar seçtiğiniz klasörlerde veya yapılandırdığınız depolama hedefinde kalır. Uygulama bunları geliştiriciye yüklemez. Uygulamayı kaldırmak, seçtiğiniz klasörlerdeki dosyaları silmez.

### FTPS sertifikası

Bir klasörü FTPS üzerinden paylaştığınızda uygulama, uygulama özel deposunda kendinden imzalı bir sertifika oluşturur. Uygulamayı kaldırmak bu sertifikayı siler. Paylaşılan dosyalar izin verdiğiniz klasörde kalır.

Verilerinizi saklamak için bir geliştirici sunucusu kullanılmaz.

## Ağ kullanımı

### Yapılandırdığınız hedefler

Bir görev çalıştığında uygulama yalnızca belirlediğiniz yere bağlanır. Bu, bu cihazdaki bir klasör veya girdiğiniz bir sunucu olabilir: FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3 ile uyumlu depolama veya Azure Blob Storage. Dosya adları, dosya içerikleri ve girdiğiniz kimlik bilgileri, görevin çalışabilmesi için o sunucuya gönderilir. Bu hizmetlerin her birinin kendi gizlilik politikası vardır. Geliştirici bu trafiği almaz.

Bir klasörü FTPS üzerinden paylaşmak, yapılandırdığınız bir FTPS istemcisinin belirlediğiniz kullanıcı adı ve parola ile izin verdiğiniz klasörü okuyabilmesi veya yazabilmesi için bu cihazda dinler. Bu trafik cihazlarınız arasında (ve bu kimlik bilgilerine sahip ağdaki herkes arasında) kalır. Geliştirici bu iletişimin tarafı değildir. Porta kimlerin ulaşabileceğinden, hangi klasörü paylaştığınızdan ve bu parolanın gücünden siz sorumlusunuz.

Uygulama güncelleme denetlemez ve bir güncelleme hizmetine bağlanmaz.

## Zamanlama

Cihaz başladığında veya seçtiğiniz günlerde bir görev çalıştırabilirsiniz. Uygulama bunu sistem alarm saatiyle kaydeder. Bu yalnızca bu uygulamayı cihazınızda başlatır.

## İzinler

Uygulama yalnızca şu izinleri kullanır:

- Yapılandırdığınız bir sunucuya ulaşmak ve bir klasörü FTPS üzerinden paylaşmak için **İnternet** ve **ağ durumu**.
- Ekran kapalıyken çalışan bir görevin sürmesi için **Bildirimler** ve bir **ön plan hizmeti**.
- Zamanlanmış bir görevi başlatmak için **Tam alarmlar** ve **başlangıçta çalıştırma**.
- Aktarım sırasında uykuyu azaltmak için **Uyanık kalma kilidi**.

## Çocuklar

Uygulama bir yedekleme ve dosya eşitleme aracıdır. 13 yaşından küçük çocuklara yönelik değildir.

## Üçüncü taraflar

Yapılandırdığınız depolama sağlayıcıları, görevin kendilerine gönderdiği dosyaları ve kimlik bilgilerini işler. Geliştirici bu trafiği almaz.

## Değişiklikler

Bu politikanın güncellemeleri uygulamaya dahildir.

## İletişim

Uygulama adı: BUtil
Geliştirici adı: Siarhei Kuchuk

Bu Android uygulamasıyla ilgili sorular, uygulamayı yüklemek için kullandığınız mağaza kaydı üzerinden veya [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues) adresinden gönderilebilir.
