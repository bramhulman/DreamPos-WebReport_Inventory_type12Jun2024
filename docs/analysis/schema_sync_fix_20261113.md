# Perbaikan master schema: FULLTEXT terbaca sebagai kolom

Pesan server menunjukkan entri JSON palsu type KEY, definition KEY NULL DEFAULT NULL, diperlakukan sebagai ADD COLUMN FULLTEXT. FULLTEXT KEY merupakan deklarasi indeks, bukan kolom. Tabel cld_inv_ordercust sudah berhasil dibuat; tidak perlu dihapus karena error ini.

Perubahan lokal:
- apply_server_update.ps1 memvalidasi seluruh master sebelum DDL master pertama. Entri bertipe indeks/constraint dikeluarkan dari daftar kolom dengan pesan DB SYNC SKIP. Kolom sungguhan bernama FULLTEXT dengan tipe varchar tetap diperbolehkan.
- Tipe/definition terpotong, tanda kutip/kurung tidak tertutup, atau statement majemuk ditolak sebelum DDL master berjalan.
- Pathdata/schema_master.json diperbaiki: 5 entri indeks palsu dihapus; 308 definisi kolom yang rusak dipulihkan dari backup_fast/schema.sql. Hanya definisi rusak yang diganti, dengan pemeriksaan kecocokan awalan tipe; definisi master yang valid dipertahankan. Tidak ada presisi decimal yang ditebak.
- Perbaikan dapat direproduksi dengan tools/repair_schema_master.py. Detail lama/baru ada pada schema_master_repairs_20261113.json; salinan master hasil perbaikan ada pada schema_master.repaired.json.

Ini tidak menambahkan/menghapus indeks FULLTEXT database yang sudah ada. Sinkronisasi master ini menangani kolom/primary key; indeks tambahan tetap dikelola lewat migrasi indeks tersendiri.

Langkah server:
1. Simpan salinan apply_server_update.ps1 dan Pathdata/schema_master.json server sebelum diganti.
2. Salin skrip update dan master JSON yang sudah diperbaiki dari workspace ke lokasi yang sama pada folder aplikasi server. JSON harus tetap berada di Pathdata/schema_master.json, bukan root aplikasi.
3. Jalankan ulang apply_server_update.ps1 dari folder aplikasi yang sama. Tabel yang sudah ada akan diperiksa sebagai tabel eksisting; jangan DROP tabel cld_inv_ordercust.
4. Bila ada error baru, gunakan UpdateLog dari proses terbaru untuk diagnosis. Perubahan ini belum dijalankan terhadap database produksi.

Verifikasi:
- Tes SQL generation/quoting sebelumnya tetap lulus.
- Test-SchemaMasterColumn: 1146 kolom / 104 tabel valid, indeks palsu ditolak sebagai kolom, kolom FULLTEXT sungguhan diterima, decimal terpotong dan statement majemuk ditolak.
- Windows PowerShell 5.1: validasi lulus.
- Seluruh 104 CREATE TABLE hasil master perbaikan berhasil dieksekusi pada database baru dalam instance MySQL 5.7.18 lokal dengan datadir terisolasi, tanpa menggunakan Web.config atau database aplikasi. Instance uji telah dihentikan.

DDL MySQL bisa commit per statement; keberhasilan membuat tabel sebelum error berikutnya tidak berarti perlu menghapus tabel atau memulihkan seluruh database. Validasi baru mencegah bagian master schema memulai perubahan jika masih ada definisi terpotong, tetapi bukan rollback terhadap migrasi yang sudah dijalankan sebelumnya.
