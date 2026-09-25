[Languages](README.md)

# Kebijakan privasi

Terakhir diperbarui: 25 September 2026

**BUtil** by Siarhei Kuchuk

Nama aplikasi: BUtil
Nama pengembang: Siarhei Kuchuk

BUtil mencadangkan, menyinkronkan, dan memulihkan berkas di komputer ini. Aplikasi ini juga dapat mengimpor media, berbagi folder, atau mengunggah berkas ke server yang Anda atur. Aplikasi ini tidak membuat akun pengembang. Pengembang tidak mengoperasikan server yang menerima berkas, kata sandi, atau data penggunaan Anda.

## Data yang tidak dikumpulkan pengembang

Aplikasi tidak berisi iklan, analitik, pelaporan kerusakan, atau SDK pelacakan. Pengembang tidak mengumpulkan, menjual, atau membagikan data pribadi.

## Data yang disimpan di komputer Anda

### Tugas dan pengaturan

Definisi tugas hanya disimpan di komputer ini. Sebuah tugas dapat berisi jalur folder, jadwal, pengaturan penyimpanan, serta kata sandi atau token yang Anda ketik. Kata sandi dan rahasia penyimpanan dienkripsi di komputer ini sebelum disimpan, dan hanya dapat dibaca di komputer ini. Nilai tersebut tidak diunggah ke pengembang.

- Tugas Windows: `%AppData%\BUtil Backup Tasks`
- Tugas Linux: `~/.config/BUtil Backup Tasks`
- Pengaturan Windows (termasuk tema dan bahasa yang terakhir dipilih untuk Lisensi atau Privasi): `%AppData%\BUtil\Settings\v1`
- Pengaturan Linux: `~/.config/BUtil/Settings/v1`
- Status tugas Windows: `%AppData%\BUtil\States`
- Status tugas Linux: `~/.config/BUtil/States`
- Status impor media Windows: `%AppData%\BUtil Backup Tasks - States`
- Status impor media Linux: `~/.config/BUtil Backup Tasks - States`

### Berkas yang Anda pilih

Pencadangan, sinkronisasi, pemulihan, dan impor membaca serta menulis folder yang Anda pilih. Berkas tersebut tetap di komputer ini atau di tujuan penyimpanan yang Anda atur. Aplikasi tidak mengunggahnya ke pengembang.

### Log

Log diagnostik hanya ditulis di komputer ini:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Berkas tersebut tidak dikirim ke mana pun.

Tidak ada server pengembang yang digunakan untuk menyimpan data Anda.

## Penggunaan jaringan

### Tujuan yang Anda atur

Saat tugas berjalan, aplikasi hanya terhubung ke tempat yang Anda tetapkan. Itu dapat berupa folder lokal atau server yang Anda masukkan: FTP, FTPS, SFTP, WebDAV, SMB, NFS, penyimpanan yang kompatibel dengan S3, atau Azure Blob Storage. Nama berkas, isi berkas, dan kredensial yang Anda masukkan dikirim ke server itu agar tugas dapat berjalan. Setiap layanan tersebut memiliki kebijakan privasinya sendiri. Pengembang tidak menerima lalu lintas itu.

BUtil Server dapat mendengarkan di komputer ini agar klien BUtil yang Anda atur dapat mengirim berkas. Lalu lintas itu tetap di antara komputer yang Anda siapkan.

### Pemeriksaan pembaruan

Build di luar Store dapat meminta rilis GitHub terbaru:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) menerima permintaan HTTPS biasa (alamat IP, user-agent, waktu). Pengembang tidak menerima lalu lintas itu.

Instalasi dari Microsoft Store tidak menggunakan pemeriksaan ini; Store yang mengirimkan pembaruan.

### Tautan yang Anda buka

Aplikasi dapat membuka halaman ini di peramban sistem. Situs tersebut memiliki kebijakan privasinya sendiri:

- Beranda proyek: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Rilis terbaru: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Bantuan pola berkas: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Bantuan format tanggal: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Sumber ikon: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Lisensi dan kebijakan privasi ini ditampilkan di dalam aplikasi. Keduanya tidak dibuka sebagai halaman web.

## Jadwal

Di Windows Anda dapat menjalankan tugas saat masuk atau menurut jadwal mingguan. Aplikasi mendaftarkannya di Penjadwal Tugas Windows dengan nama yang diawali `BUtil`. Itu hanya meluncurkan aplikasi ini di komputer Anda.

## Anak-anak

Aplikasi ini adalah alat cadangan dan sinkronisasi berkas. Aplikasi ini tidak ditujukan untuk anak di bawah 13 tahun.

## Pihak ketiga

GitHub memproses permintaan pemeriksaan pembaruan dan halaman yang Anda buka, seperti dijelaskan di atas. Microsoft Store memproses pemasangan dan pembaruan Store. Penyedia penyimpanan yang Anda atur memproses berkas dan kredensial yang dikirim tugas kepada mereka. Pengembang tidak menerima lalu lintas itu.

## Perubahan

Pembaruan kebijakan ini akan dipublikasikan di berkas ini dalam repositori proyek.

## Kontak

Nama aplikasi: BUtil
Nama pengembang: Siarhei Kuchuk

Pertanyaan: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
