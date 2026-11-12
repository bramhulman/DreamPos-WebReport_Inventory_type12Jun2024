# Hasil inspeksi RC11/26-00739, stock 301

Sumber: hasil query yang diberikan pengguna; database cldmemberbarburger, customer 22, Warehouse CC 2. Query identitas mencatat waktu server 12 November 2026 15:39:12. Hasil adalah beberapa SELECT, bukan snapshot transaksi tunggal: ada detail lain ModifDate 15:40:08, sehingga tidak boleh menyimpulkan semua tabel dibaca persis bersamaan.

## Kesimpulan kondisi terkini

Detail ID 1169627 aktif (type 3, flagtrans=1), tanggal 4 November 2026, qty 55 Pcs. Hanya satu detail dokumen untuk item ini pada hasil yang diberikan. Tidak ada kontribusi aktif dokumen pada 9 November; rcvQty Warehouse tanggal 9 adalah 0. rcvQty tanggal 4 adalah 55, sama dengan SUM ledger receive aktif. Semua rcv_difference tanggal 3-12 November nol; total_difference dan carry_difference juga nol. Tidak terlihat sisa pengurangan berlebih, duplikasi posting quantity, atau putus carry-forward pada snapshot terkini ini.

Perhitungan saldo Warehouse:
- 3 November: opening/ending 5.
- 4 November: 5 + receive 55 - HOout 10 = 50.
- 5 November: 50.
- 6 November: 50 - HOout 3 = 47.
- 7 November: 47.
- 8 November: 47 - HOout 5 = 42.
- 9-11 November: 42, tanpa receive 55 lagi.
- 12 November: 42 - HOout 3 = 39.

Total distribusi keluar 21 (10+3+5+3); 5+55-21=39 Pcs. Baris type 8 pada CC 8/14 adalah receipt outlet; tidak dihitung kembali sebagai penambahan stok Warehouse. Raw usage/batch dan summary usage kosong. Query kandidat detail ganda kosong.

## Batas kesimpulan

Hasil header lengkap tidak disertakan, hanya header_ccid=2, jadi status header dan kesesuaiannya dengan detail belum bisa diverifikasi dari lampiran ini. Detail kini AKTIF; jika pengguna bermaksud dokumen tetap dibatalkan, kondisi ini tidak sesuai maksud tersebut. Jika dibatalkan lalu diterima ulang tanggal 4, kondisi quantity sekarang sesuai.

CreatedDate detail 4 November 14:11:11 dan ModifDate 9 November 10:32:04. InvDate saat ini 4 November, tetapi timestamp ini bukan riwayat perubahan lengkap dan tidak membuktikan kapan cancel/perpindahan tanggal berlangsung. Belum ada bukti bahwa dua cancel tidak pernah terjadi: rekalkulasi sesudahnya dapat membuat saldo kembali cocok. Hasil ini hanya memastikan tidak ada selisih bersih pada data sekarang untuk lingkup yang diperiksa.

Bug risiko cancel tidak idempotent dan update saldo beda koneksi yang telah ditemukan dalam kode tetap perlu diperbaiki; pemeriksaan satu dokumen dengan saldo cocok bukan pembuktian keamanan semua jalur cancellation. Tidak ada data server atau kode aplikasi yang diubah dalam pemeriksaan hasil ini.
