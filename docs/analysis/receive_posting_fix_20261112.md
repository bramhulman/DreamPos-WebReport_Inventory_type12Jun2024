# Perbaikan cancel receive dan saldo quantity

## Perilaku

- Cancel button dikunci saat request berlangsung. Handler cancel memakai satu binding server (`Handles`), dengan guard JavaScript untuk double click dan aksi tombol lain selama request.
- Semua save/order, receive dan cancel untuk dokumen yang sama memakai named lock MySQL yang mencakup database/customer/nomor dokumen. Header/detail juga dikunci `FOR UPDATE`.
- Cancel mengambil qty, CC, tanggal dan status asli dari detail database yang dikunci. Nilai edit pada form tidak dipakai untuk reverse.
- Header harus Receive (flag=2), dan ada detail aktif. Cancel ulang atau quantity posting aktif nol ditolak dengan notifikasi dan reference ID, tanpa perubahan saldo.
- Acuan bukan semata `rcvQty=0`: bucket itu milik semua penerimaan hari tersebut. Setelah detail dokumen dinonaktifkan, rcvQty dibangun ulang dari SUM type=3 aktif. Bucket nol atau sudah negatif tidak dikurangi secara buta.
- Header, detail, bucket penerimaan, saldo akhir, carry-forward dan sinkronisasi master cost yang diaktifkan konfigurasi berada dalam satu MySQL transaction/koneksi. Semua tabel yang ditulis harus InnoDB.
- Pemindahan tanggal dilakukan setelah cancel, lalu Receive ulang; tanggal posting lama dan baru direkonsiliasi. Receive ulang dari halaman stale yang dokumennya sudah Receive ditolak.
- Saldo harian dihitung dengan Decimal dan komponen NULL dianggap nol. Hasil tersimpan diverifikasi pada presisi 4 desimal. Carry-forward mempertahankan closeQty opname sebagai anchor; movement lain tidak dihapus atau direkalkulasi oleh jalur receive ini.
- Aturan rumus closing-waste mengikuti jalur live receive/opname yang ada (tidak mengubah kebijakan cwasteqty dalam perbaikan ini).

## Diagnostik errLog

Semua event mempunyai `ref` unik. Reference yang sama ditampilkan pada notifikasi kegagalan. Cari reference itu pada errLog yang dikonfigurasi aplikasi; fallback logger tetap berlaku.

Event:
- `RECEIVE_STARTED`: permintaan dimulai.
- `RECEIVE_COMMITTED`: commit berhasil.
- `RECEIVE_REJECTED`: permintaan ditolak, misalnya sedang diproses, sudah batal, status stale atau posting qty nol.
- `RECEIVE_BUCKET_MISMATCH`: sebelum perubahan ledger, bucket tersimpan berbeda dari SUM penerimaan aktif. Ini dicatat walaupun proses berikutnya berhasil memperbaiki bucket.
- `RECEIVE_FAILED`: kegagalan SQL/koneksi atau exception lainnya.
- `RECEIVE_LOCK_RELEASE_FAILED` / `RECEIVE_ROLLBACK_FAILED`: kegagalan cleanup; koneksi lock yang tidak dapat dilepas dibuang dari pool.
- `RECEIVE_UI_FAILED`: kegagalan sebelum/di luar helper database.

Konteks mencakup database server thread ID, nomor dokumen, customer/user, aksi, tahap, durasi, status commit, hasil rollback, MySQL error number, SQL terakhir dan parameter, exception lengkap/inner exception/stack trace, identitas detail dan quantity/tanggal posting asli. Trace merekam tahap SQL, affected rows, serta snapshot seluruh bucket quantity dan cost sebelum/sesudah. Trace menggunakan timestamp UTC ISO; timestamp pembungkus errLog mengikuti logger server.

Notes/nomor surat jalan/free text pada command header disamarkan; connection string/password tidak ditulis. Diagnostic log berada di server, bukan ditampilkan ke pengguna.

Jika koneksi putus ketika commit dikonfirmasi, outcome dicatat `UNKNOWN_CHECK_DATABASE`. Pesan pengguna meminta memuat ulang dan memeriksa dokumen. Tidak ada retry write otomatis: hilangnya respons commit tidak membuktikan server membatalkan commit.

## Berkas dan rilis

Source utama:
- ConnectionMysql/clsReceivePosting.vb (baru, terdaftar dalam vbproj).
- ConnectionMysql/clsInventory.vb.
- Pages_InvRcvEdit.aspx.vb dan Pages_InvRcvEdit.aspx.
- Content/js/receive-posting.js (baru, masuk daftar Content deployment).

Build terverifikasi tersimpan di `tests/ReceivePosting/runtime/app-build/`. Untuk deployment compiled WebForms, rilis DLL aplikasi hasil build bersama ASPX dan JavaScript baru; menyalin file .vb saja tidak mengubah perilaku server. Gunakan proses rilis aplikasi yang biasa agar seluruh perubahan workspace yang sudah ada ikut ditinjau. Tidak ada migrasi skema baru untuk pengaman ini.

Database server tidak disentuh dalam implementasi/pengujian ini. Data lama tidak otomatis diperbaiki di luar stock/CC/tanggal yang disentuh transaksi; rekonsiliasi historis luas tetap merupakan pekerjaan terpisah. Jalur legacy transaksi lain dan proses opname yang dianalisa sebelumnya tidak semuanya dimigrasikan ke helper ini.

## Verifikasi

Build aplikasi lengkap menggunakan target WebApplication v14 dan compiler repository, dengan output terisolasi. Packaging arsip lama yang hilang dan prebuild penulisan versi dinonaktifkan hanya pada perintah verifikasi. Warning PdfSharp/variabel unused yang sudah ada bukan kegagalan compile.

12 skenario integrasi pada MySQL 5.7 terisolasi serta unit test JavaScript tersedia di `tests/ReceivePosting/`. Termasuk concurrency, cancel ulang, perpindahan tanggal, fault/timeout, rollback seluruh carry-forward, NULL, opname anchor dan ketidakpastian commit. Server produksi belum dideploy dan UI WebForms penuh belum diuji melalui browser/staging.
