[Languages](README.md)

# Kebijakan privasi

Terakhir diperbarui: 21 Oktober 2026

**BUtil** by Siarhei Kuchuk

Nama aplikasi: BUtil
Nama pengembang: Siarhei Kuchuk

BUtil mencadangkan, menyelaraskan, dan memulihkan berkas di perangkat ini. Aplikasi ini juga dapat mengimpor media, berbagi folder melalui FTPS, atau mengunggah berkas ke server yang Anda atur. Aplikasi ini tidak membuat akun pengembang. Pengembang tidak menjalankan sistem belakang yang menerima berkas, kata sandi, atau data penggunaan Anda.

## Data yang tidak dikumpulkan pengembang

Aplikasi tidak memuat iklan, analitik, pelapor kerusakan, atau SDK pelacakan. Pengembang tidak mengumpulkan, menjual, atau membagikan data pribadi.

Kebijakan privasi ini dan lisensi ditampilkan di dalam aplikasi. Keduanya tidak dibuka sebagai halaman web.

## Data yang disimpan di perangkat Anda

### Tugas dan pengaturan

Definisi tugas hanya disimpan di penyimpanan privat aplikasi pada perangkat ini. Sebuah tugas dapat memuat jalur folder, jadwal, pengaturan penyimpanan, serta kata sandi atau token yang Anda ketik. Kata sandi dan rahasia penyimpanan disegel dengan penyimpanan kunci Android sebelum disimpan, dan hanya dapat dibaca di perangkat ini. Nilai itu tidak diunggah ke pengembang. Nilai itu tidak termasuk dalam cadangan Android. Menghapus pemasangan aplikasi akan menghapus nilai itu.

### Berkas yang Anda pilih

Pencadangan, penyelarasan, pemulihan, impor, dan unggahan membaca serta menulis folder yang Anda pilih dengan pemilih dokumen sistem, atau server yang Anda atur. Berkas itu tetap di folder yang Anda pilih, atau di tujuan penyimpanan yang Anda atur. Aplikasi tidak mengunggah berkas itu ke pengembang. Menghapus pemasangan aplikasi tidak menghapus berkas di folder yang Anda pilih.

### Sertifikat FTPS

Saat Anda berbagi folder melalui FTPS, aplikasi membuat sertifikat yang ditandatangani sendiri di penyimpanan privat aplikasi. Menghapus pemasangan aplikasi akan menghapus sertifikat itu. Berkas yang dibagikan tetap di folder yang Anda izinkan.

Tidak ada server pengembang yang dipakai untuk menyimpan data Anda.

## Penggunaan jaringan

### Tujuan yang Anda atur

Saat sebuah tugas berjalan, aplikasi hanya tersambung ke tempat yang Anda tetapkan. Tempat itu dapat berupa folder di perangkat ini, atau server yang Anda masukkan: FTP, FTPS, SFTP, WebDAV, SMB, NFS, penyimpanan kompatibel S3, atau Azure Blob Storage. Nama berkas, isi berkas, dan kredensial yang Anda masukkan dikirim ke server itu agar tugas dapat berjalan. Setiap layanan itu memiliki kebijakan privasi sendiri. Pengembang tidak menerima lalu lintas itu.

Berbagi folder melalui FTPS mendengarkan di perangkat ini agar klien FTPS yang Anda atur dapat membaca atau menulis folder yang Anda izinkan, dengan nama pengguna dan kata sandi yang Anda tetapkan. Lalu lintas itu tetap di antara perangkat Anda (dan siapa pun di jaringan yang memiliki kredensial itu). Pengembang bukan pihak dalam lalu lintas itu. Anda bertanggung jawab atas siapa yang dapat menjangkau port, folder mana yang Anda bagikan, dan seberapa kuat kata sandi itu.

Aplikasi tidak memeriksa pembaruan dan tidak menghubungi layanan pembaruan.

## Jadwal

Anda dapat menjalankan tugas saat perangkat mulai, atau pada hari yang Anda pilih. Aplikasi mendaftarkan hal itu ke jam alarm sistem. Hal itu hanya memulai aplikasi ini di perangkat Anda.

## Izin

Aplikasi hanya memakai izin berikut:

- **Internet** dan **status jaringan** untuk menjangkau server yang Anda atur dan untuk berbagi folder melalui FTPS.
- **Notifikasi** dan sebuah **layanan latar depan** untuk menjaga tugas yang sedang berjalan saat layar mati.
- **Alarm tepat** dan **jalan saat mulai** untuk memulai tugas yang dijadwalkan.
- **Kunci tetap terjaga** untuk mengurangi tidur selama transfer.

## Anak-anak

Aplikasi ini adalah alat cadangan dan sinkronisasi berkas. Aplikasi ini tidak ditujukan kepada anak di bawah 13 tahun.

## Pihak ketiga

Penyedia penyimpanan yang Anda atur memproses berkas dan kredensial yang dikirim tugas kepada mereka. Pengembang tidak menerima lalu lintas itu.

## Perubahan

Pembaruan kebijakan ini disertakan di dalam aplikasi.

## Kontak

Nama aplikasi: BUtil
Nama pengembang: Siarhei Kuchuk

Pertanyaan tentang aplikasi Android ini dapat dikirim melalui daftar di toko yang Anda pakai untuk memasangnya, atau di [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
