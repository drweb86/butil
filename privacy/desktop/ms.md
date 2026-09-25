[Languages](README.md)

# Dasar Privasi

Kemas kini terakhir: 25 September 2026

**BUtil** by Siarhei Kuchuk

Nama aplikasi: BUtil
Nama pembangun: Siarhei Kuchuk

BUtil membuat sandaran, menyegerakkan dan memulihkan fail pada komputer ini. Ia juga boleh mengimport media, berkongsi folder atau memuat naik fail ke pelayan yang anda tetapkan. Ia tidak mencipta akaun pembangun. Pembangun tidak mengendalikan pelayan yang menerima fail, kata laluan atau data penggunaan anda.

## Data yang tidak dikumpul pembangun

Aplikasi ini tidak mengandungi iklan, analitik, laporan ranap atau SDK penjejakan. Pembangun tidak mengumpul, menjual atau berkongsi data peribadi.

## Data yang disimpan pada komputer anda

### Tugas dan tetapan

Takrif tugas disimpan hanya pada komputer ini. Sesuatu tugas boleh mengandungi laluan folder, jadual, tetapan storan serta kata laluan atau token yang anda taip. Kata laluan dan rahsia storan disulitkan pada komputer ini sebelum disimpan, dan hanya boleh dibaca pada komputer ini. Nilai itu tidak dimuat naik kepada pembangun.

- Tugas Windows: `%AppData%\BUtil Backup Tasks`
- Tugas Linux: `~/.config/BUtil Backup Tasks`
- Tetapan Windows (termasuk tema dan bahasa yang terakhir dipilih untuk Lesen atau Privasi): `%AppData%\BUtil\Settings\v1`
- Tetapan Linux: `~/.config/BUtil/Settings/v1`
- Keadaan tugas Windows: `%AppData%\BUtil\States`
- Keadaan tugas Linux: `~/.config/BUtil/States`
- Keadaan import media Windows: `%AppData%\BUtil Backup Tasks - States`
- Keadaan import media Linux: `~/.config/BUtil Backup Tasks - States`

### Fail yang anda pilih

Sandaran, penyegerakan, pemulihan dan import membaca serta menulis folder yang anda pilih. Fail itu kekal pada komputer ini atau di destinasi storan yang anda tetapkan. Aplikasi tidak memuat naiknya kepada pembangun.

### Log

Log diagnostik ditulis hanya pada komputer ini:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Fail itu tidak dihantar ke mana-mana.

Tiada pelayan pembangun digunakan untuk menyimpan data anda.

## Penggunaan rangkaian

### Destinasi yang anda tetapkan

Apabila tugas dijalankan, aplikasi hanya bersambung ke tempat yang anda tetapkan. Ia boleh menjadi folder tempatan atau pelayan yang anda masukkan: FTP, FTPS, SFTP, WebDAV, SMB, NFS, storan serasi S3 atau Azure Blob Storage. Nama fail, kandungan fail dan kelayakan yang anda masukkan dihantar ke pelayan itu supaya tugas dapat dijalankan. Setiap perkhidmatan itu mempunyai dasar privasinya sendiri. Pembangun tidak menerima trafik itu.

BUtil Server boleh mendengar pada komputer ini supaya klien BUtil yang anda tetapkan dapat menghantar fail. Trafik itu kekal antara komputer yang anda sediakan.

### Semakan kemas kini

Binaan di luar Store boleh meminta keluaran GitHub terkini:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) menerima permintaan HTTPS biasa (alamat IP, user-agent, masa). Pembangun tidak menerima trafik itu.

Pemasangan dari Microsoft Store tidak menggunakan semakan ini; Store menyampaikan kemas kini.

### Pautan yang anda buka

Aplikasi boleh membuka halaman ini dalam pelayar sistem. Tapak itu mempunyai dasar privasinya sendiri:

- Laman utama projek: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Keluaran terkini: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Bantuan corak fail: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Bantuan format tarikh: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Sumber ikon: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Lesen dan dasar privasi ini ditunjukkan dalam aplikasi. Ia tidak dibuka sebagai halaman web.

## Jadual

Pada Windows anda boleh menjalankan tugas semasa log masuk atau mengikut jadual mingguan. Aplikasi mendaftarkannya dalam Penjadual Tugas Windows dengan nama yang bermula dengan `BUtil`. Ini hanya melancarkan aplikasi ini pada komputer anda.

## Kanak-kanak

Aplikasi ini ialah alat sandaran dan penyegerakan fail. Ia tidak ditujukan kepada kanak-kanak di bawah 13 tahun.

## Pihak ketiga

GitHub memproses permintaan semakan kemas kini dan halaman yang anda buka, seperti yang diterangkan di atas. Microsoft Store memproses pemasangan dan kemas kini Store. Pembekal storan yang anda tetapkan memproses fail dan kelayakan yang dihantar tugas kepada mereka. Pembangun tidak menerima trafik itu.

## Perubahan

Kemas kini dasar ini akan disiarkan dalam fail ini di repositori projek.

## Hubungan

Nama aplikasi: BUtil
Nama pembangun: Siarhei Kuchuk

Soalan: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
