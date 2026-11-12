# Analisa inventory: opname type 12 dan receive supplier type 3

Tanggal pemeriksaan: 12 November 2026. Lingkup: kode lokal saat pemeriksaan, errLog.txt dari server, serta pending_queries.log dan slow_queries.log lokal yang tersedia. Asal/kelengkapan dua log tambahan belum dikonfirmasi. Tidak ada koneksi atau perubahan data server; tidak ada perubahan kode aplikasi dalam analisa ini. Versi server belum diverifikasi cocok dengan source lokal.

## Pembaruan: hasil query database dari pengguna

Snapshot hasil query yang dikirim setelah pemeriksaan log menunjukkan tiga saldo hari ini sudah konsisten:

| Stock | Opname | openQty | ttlQty | Selisih rumus |
|---|---:|---:|---:|---:|
| 205 Beef 65cl | 100 | 100 | 100 | 0 |
| 206 Beef 85cl | 188 | 188 | 188 | 0 |
| 373 Chicken | 352 | 352 | 352 | 0 |

Semua movement bucket hari ini nol, termasuk BPout, usageQty dan cwasteqty. Query usage summary serta raw usage/batch usage kosong. Tidak ada kandidat ledger ganda pada query duplikasi hari kejadian. Ketiga tabel utama dikonfirmasi InnoDB. Ini memungkinkan transaksi atomik, tetapi tidak otomatis menyatukan perintah yang saat ini memakai koneksi berbeda.

CloseQty 11 November masing-masing 100/188/352 dan bClose aktif, sehingga benar menjadi opening 12 November. ttlQty 11 November 208.4394/325.4816/557.8886 adalah saldo buku sebelum penyesuaian fisik, bukan angka yang harus menggantikan closeQty. Data receive 11 November RC11/26-00792 berstatus aktif dan header Receive; quantity 50/80/150 cocok dengan rcvQty tanggal 11 November. Snapshot ini tidak menunjukkan bukti reverse ganda pada tiga stok ini.

Dengan data ini, selisih Chicken 217,26 pada screenshot sudah tidak ada pada snapshot terbaru. Tidak ada dasar menyimpulkan 134,74 merupakan penggunaan atau batch out hari ini. Kemungkinan saldo sempat tertinggal lalu dihitung ulang tetap terbuka, tetapi tidak dapat dipastikan kapan/oleh proses mana tanpa snapshot sebelum/sesudah atau audit/binlog. Rekalkulasi 13:26 adalah kandidat berdasarkan urutan log, bukan penyebab/perbaikan yang telah terbukti. Waktu pengambilan screenshot dan apakah pengguna menjalankan Calculate Inventory di antara dua pengambilan data masih belum diketahui.

Cacat konsistensi dan idempotency cancel yang ditemukan di kode tetap berlaku sebagai risiko; hasil data saat ini bukan verifikasi bahwa kode aman terhadap double cancel. Analisa tidak memperbaiki kode aplikasi maupun database. Hasil snapshot tidak mencakup seluruh schema/index/trigger pada query 7, hanya engine.

## Pembaruan: data transaksi dan log server terbaru

Bagian ini menggantikan batas log/kesimpulan cakupan pada pemeriksaan awal di bawah. Log terbaru dari server sudah mencakup waktu opname dan berakhir 12 November 2026 13:48:01. Ketiga file terbaru telah dibaca ulang.

Data yang dikonfirmasi pengguna: custid=22, Warehouse ccid=2. ID 1176617 / stock 205 / 100 Kg / 12:56:39; ID 1176618 / stock 206 / 188 Kg / 12:56:42; ID 1176619 / stock 373 / 352 Kg / 12:56:51. Semua type=12, flagtrans=1. Dua beef cocok dengan screenshot (100 dan 188); Chicken berbeda: 352 vs tampilan 217,26, selisih sekitar 134,74 Kg. Screenshot tidak memiliki timestamp, sehingga belum diketahui apakah tampil sebelum atau sesudah rekalkulasi 13:24-13:26.

Daily Opname terkonfirmasi memakai insertStockOpname(..., U1=0, U2=0, ..., opnameVal, ..., adjustnow=2): Pages_DailyOpname.aspx.vb:211 dan 535. Ini konsisten dengan data pengguna yang U1/U2 nol tetapi ttlStock terisi. Jadi U1/U2 nol pada tiga row tersebut bukan dengan sendirinya bukti quantity gagal dikirim. Jalur ini tetap menuju proses non-atomik yang telah dibahas. AJAX menetapkan success=True setelah helper kembali, sedangkan helper dapat menelan exception.

Timeline bukti baru:
- 12:53:33-12:53:40: redirect login berulang dari Pages_DailyOpname.aspx karena NO_SESSION.
- 12:55:27: login ridwan1 tercatat.
- 12:56:39/42/51: simpan tiga opname berdasarkan row database yang diberikan.
- errLog.txt:5128, 12:56:40: logout=timeout / NO_SESSION. Dekat waktunya tetapi tidak ada request ID yang menghubungkan event ini ke request save tertentu; bukan bukti SQL timeout atau pembatalan commit.
- Tidak ditemukan entri error SQL yang mengidentifikasi stok 205/206/373 sekitar simpan pukul 12:56. Pending log juga tidak menyediakan jejak SQL opname tersebut. Tidak adanya entri gagal bukan bukti seluruh langkah sukses.
- errLog.txt:5202 dan slow_queries.log:119, 13:24:54: CREATE saldo harian melalui INSERT SELECT gagal dengan Unknown column 'stk_no' in field list. Kode sumber: ConnectionMysql/cls_recalculate.vb:851-864 memakai stk_no/stk_name/cc_name pada tabel saldo, sedangkan referensi skema memakai stkNo/stkName/ccName. Query juga menggunakan nama yang salah pada alias yest.
- errLog.txt:5203 dan slow_queries.log:120, 13:25:36: SQL Timeout expired yang nyata pada UPDATE komponen rcvqty/rtnqty/tinqty/.../usageqty/bpout/cwasteqty. Pending token 4302d11125c3493986646876680cc92f mulai 13:24:54. Ini error query database, berbeda dengan logout sesi. Jeda timestamp sekitar 42 detik tidak harus sama dengan timeout eksekusi SQL karena pencatatan dimulai sebelum seluruh operasi selesai.
- errLog.txt:5204 dan slow_queries.log:121, 13:26:36: INSERT saldo gagal lagi dengan Unknown column 'stk_no'.
- slow_queries.log:122, 13:26:40: UPDATE komponen berikutnya tercatat sukses tetapi lambat (4366ms). Ini tidak membuktikan seluruh urutan rekalkulasi berhasil, karena langkah INSERT sebelumnya masih gagal.

Cakupan UPDATE yang timeout adalah seluruh row custid=22 tanggal 12 November; tidak ada filter ccid/stkid. Dengan demikian ada proses rekalkulasi luas setelah opname, yang dapat mengubah snapshot stok. cls_recalculate.InvCaclculate_ActiveTransOnly menjalankan INSERT, cascade opening, UPDATE komponen dan UPDATE total secara terpisah serta mengabaikan hasil Boolean executeNonQuery. Kegagalan satu langkah tidak menghentikan langkah berikutnya. Ini memperkuat risiko saldo parsial, tetapi kejadian pukul 13:25 tidak boleh dianggap penyebab saldo yang sudah salah pada 12:56.

Ada ketidakkonsistenan rumus tambahan: total pada cls_recalculate.vb sekitar baris 895 mengurangi cwasteqty, sedangkan formula clsInventoryProcess.vb:1123 tidak. Jika cwasteqty bukan nol, hasil dapat berbeda menurut jalur terakhir yang menghitung. Perlu menetapkan aturan bisnis closing-waste sebelum memperbaikinya.

Query diagnosis sekarang difokuskan ke ID 205/206/373, ccid=2 dan menambahkan perbandingan input serta transaksi mentah cld_inv_transusage (termasuk batch out type=11). Selisih sekitar 134,74 dapat berasal dari saldo total lama, usage/bpout/movement lain atau rekalkulasi sesudah opname; belum dapat memilih satu penyebab tanpa snapshot bucket. Pemeriksaan yang diperlukan berikutnya adalah hasil query tersebut, terutama row stock 373 tanggal 11/12 November.

## Kesimpulan pemeriksaan awal

Ada cacat konsistensi nyata pada kode: perubahan ledger/dokumen dan saldo stok tidak atomik, error update saldo dapat ditelan, dan cancel receive belum idempotent. Karena itu belum bisa menjamin cancel hanya mengurangi satu kali. Kasus tiga item Warehouse belum bisa dipastikan akar penyebabnya tanpa snapshot database, jumlah input, jam simpan, dan versi server.

Screenshot menunjukkan saldo 100 Kg, 188 Kg dan 217,26 Kg, tetapi tidak memuat quantity opname yang diharapkan atau movement buckets. Screenshot saja tidak cukup menentukan selisih.

## 1. Opname type 12: opening benar, ttlQty lama (P1)

Lokasi: ConnectionMysql/clsInventory.vb:1476-1606 dan clsInventoryProcess.vb:1006-1158.

Alur adjustnow=2: simpan/update ledger type=12 pada tanggal hari ini; set closing kemarin; panggil insertStockOpname untuk kemarin; panggil executeNonQuery_Inv_Processstock(..., True) untuk membawa closeQty kemarin menjadi openQty hari ini. Pada row hari ini yang sudah ada, UPDATE openQty terjadi sekitar baris 1075, lalu UPDATE ttlQty pada baris 1123. Kedua perintah melalui clsDbo dengan koneksi/autocommit terpisah, bukan satu MySqlTransaction. Di antaranya ada update cost.

Jika update opening sukses lalu update total gagal/koneksi terputus/request berhenti, openQty benar sementara ttlQty tetap lama. Ini sesuai pola keluhan, tetapi bukan bukti bahwa kejadian tiga item disebabkan kegagalan tersebut.

Nilai balik insertStockOpname() dan executeNonQuery_Inv_Processstock() tidak diperiksa pemanggil. clsDbo.executeNonQuery2 menelan exception, clsDbo.executeNonQuery mengembalikan False, dan halaman opname juga memiliki Catch kosong. Pengguna dapat diarahkan kembali tanpa mendapat pesan gagal walaupun proses hanya selesai sebagian. executeNonQuery_Inv_Processstock juga tidak selalu mengubah hasil menjadi False saat update total gagal.

Rumus yang dipakai:
`ttlQty = openQty + rcvQty - rtnQty + tinQty - toutQty - rwasteQty - fwasteQty - usageQty - HoOutQty + HoInQty - BPout + BPIn`.

Jika semua movement nol, ttlQty harus sama dengan openQty. Namun 'tidak ada transaksi di cld_inv_trans' belum membuktikan semua movement nol: usage berasal dari cld_inv_transusagesum, dan bucket lama bisa tertinggal. Query diagnosis membandingkan keduanya.

## 2. Receive type 3: transaksi hanya melindungi sebagian proses (P1)

Lokasi: clsInventory.vb:175, 256-268, 441, 481-493, 498-530, 562.

Header/detail menggunakan conn/trans. Tetapi semua pemanggilan executeNonQuery_Inv_ProcessCls() pada insertInvRcv memakai overload tanpa parameter trans. Overload itu membuka clsDbo sendiri; insertStockCLosing mengubah bucket secara langsung lalu menghitung ttlQty dengan perintah terpisah.

Akibat: rollback header/detail tidak membatalkan perubahan saldo stok yang sudah commit di koneksi lain. Sebaliknya, kegagalan saldo dapat ditelan lalu header/detail tetap commit. Pembacaan cost dari koneksi lain juga tidak melihat perubahan ledger yang belum commit. Memiliki BeginTransaction di insertInvRcv saja belum membuat seluruh transaksi inventory atomik.

Overload yang menerima MySqlTransaction sudah ada di clsInventoryProcess, tetapi belum dipakai pada jalur receive ini. Mengganti overload saja belum menyelesaikan idempotency, tanggal, dan locking.

## 3. Cancel receive dapat mengurangi dua kali (P1)

Lokasi: clsInventory.vb:310-324, 417-421, 498-530; clsInventoryProcess.vb:516.

Cancel mereset flagtrans=0 pada ledger, kemudian selalu mengirim Invrcvqty=-row.ttlStock. insertStockCLosing menjalankan `rcvQty = rcvQty + Invrcvqty`, bukan membangun ulang bucket dari SUM ledger aktif. Komentar di blok cancel yang menyatakan agregasi SUM tidak sesuai implementasi bucket: SUM pada executeNonQuery_Inv_ProcessCls dipakai untuk cost, bukan mengganti rcvQty.

Guard shouldReverse=False saat xcancel mencegah reverse dari blok normal dalam satu panggilan. Jadi satu panggilan cancel normal tidak otomatis mengurangi dua kali lewat dua blok itu. Namun panggilan cancel berikutnya tetap mengurangi lagi: tidak ada cek status terbaru/lock header atau detail untuk memastikan transisi 1 ke 0 hanya terjadi sekali. dtExisting hanya mengambil id dan stk_id. Kondisi UPDATE flag tidak dibatasi flagtrans=1 dan jumlah row transisi tidak dijadikan guard.

Contoh logika: penerimaan 10 -> rcvQty=10; cancel pertama -> 0; cancel ulang -> -10 meski ledger tetap flagtrans=0. Dua tab, klik ulang, refresh/retry atau pengiriman ulang request dapat memicu ini. Ini demonstrasi alur kode, bukan reproduksi terhadap database server.

Jumlah reverse berasal dari ViewState/dt yang dapat berisi quantity lama/yang diedit, bukan selalu quantity persisten yang benar-benar sudah diposting. Guard finish juga memakai row.flagtrans dari ViewState; request finish yang sama dengan ViewState pending dapat menambahkan lagi. Status harus dibaca dan dikunci di database.

## 4. Tanggal saldo dan tanggal transaksi dapat berbeda (P1)

Receive baru mengisi InvDate saldo memakai Date.Now (baris 266), sementara tanggal detail/dokumen menggunakan usedDate. Cancel menggunakan usedDate dan baris detail di-update ke tanggal itu. Jika pengguna mengganti tanggal atau transaksi lintas hari, saldo yang dikurangi dapat berada di hari berbeda dari saldo yang dahulu ditambah. Pembetulan harus mempertahankan identitas posting lama (custid, cc, stk, tanggal, qty), membalik posting lama tepat satu kali, lalu memproses tanggal yang benar dan carry-forward yang terpengaruh.

## 5. NULL dan pembatalan opname (P2)

Referensi backup_fast/schema.sql:711 menunjukkan banyak kolom saldo boleh NULL. Rumus tidak menggunakan COALESCE; satu NULL membuat ttlQty menjadi NULL tanpa SQL error. Verifikasi struktur/data server diperlukan, bukan menganggap backup pasti sama.

CancelStockOpname (clsInventory.vb:1443-1473) menghapus ledger type 11/12 dan mereset closing kemarin dalam transaksi, tetapi tidak memulihkan openQty/ttlQty hari berikutnya. Pemanggil Pages_StockOpname.aspx.vb:175 tidak memanggil rekalkulasi sesudahnya. Ini masalah tambahan pada cancel opname, terpisah dari cancel supplier.

## Bukti log

- errLog.txt:3780: 02/11/26 16:23:39, UPDATE ttlQty gagal: Unable to connect to any of the specified MySQL hosts; stok 148, CC 6, tanggal saldo 2026-10-15, custid 22.
- errLog.txt:3787: 02/11/26 16:44:59, perintah total gagal untuk stok 148, CC 5, tanggal saldo 2026-10-29.
- Total 109 baris mengandung pesan gagal koneksi tersebut; 21 baris memuat UPDATE rumus ttlQty (sebagian mungkin baris error multiline).
- 41 baris mengandung Reading from the stream; ini bukti gangguan baca/koneksi, bukan bukti otomatis terjadi double posting.
- Tidak ditemukan string Timeout expired, TimeoutException, Deadlock, atau Lock wait dalam errLog ini. logout=timeout adalah timeout sesi, bukan bukti timeout query MySQL.
- slow_queries.log:113-115 mengonfirmasi gangguan stream/koneksi pada 2 November. Label TIMEOUT/CRASH dipakai juga untuk error koneksi; jangan dibaca sebagai diagnosis SQL timeout pasti. Ada query cost 13,740ms dan 24,191ms pada 14/15 Oktober; itu bukan bukti durasi opname 12 November.
- 12 November sekitar 09:33 ada redirect login berulang dari halaman opname karena NO_SESSION. Ada error ViewState/client disconnected. Keduanya tidak membuktikan commit/rollback atau double posting.
- errLog yang diperiksa berakhir 12 November 10:06:31. Kejadian setelah jam itu tidak dapat dicocokkan dengan log ini.
- Pending log bukan bukti query aman diulang dan belum ditemukan mekanisme replay otomatis pada jalur ini. Jangan mengeksekusi ulang SQL tambah/kurang dari pending log tanpa rekonsiliasi.

## Diagnosis server yang masih diperlukan

Jalankan inventory_qty_audit_20261112.sql di database server; hanya SET variabel sesi dan SELECT. Default custid=22 berdasar log, tanggal 12 November, Warehouse dan nama mengandung minc; sesuaikan jika identitas berbeda. Query belum dijalankan terhadap server.

Interpretasi:
1. stored_minus_formula bukan nol: total tersimpan tidak mengikuti bucket, mengarah ke update total gagal/tidak dijalankan/ditimpa.
2. stored_minus_formula nol tetapi saldo salah: komponen movement/opening yang salah; bandingkan active_receive_qty dan rcvQty serta usage terpisah.
3. raw_formula NULL: cek operand NULL dan skema aktual.
4. rcvQty lebih kecil dari SUM aktif setelah cancel: kandidat reverse berlebih/rollback parsial/tanggal berbeda; perlu riwayat request atau audit/binlog untuk memastikan dua cancel.
5. Query duplikasi hanya menandai kandidat. Bucket bisa double tanpa ada dua ledger row.

Mohon cocokkan quantity input tiap item, ID stock/CC, waktu simpan, log terbaru termasuk pending_queries.log/slow_queries.log, dan versi/DLL server. Timestamp Modifdate saja tidak menyimpan seluruh sejarah pembatalan.

## Rencana perbaikan

Prioritas: (1) satukan ledger/header/saldo dalam koneksi dan transaksi yang sama; (2) lock dokumen dan saldo dalam urutan konsisten, baca status aktual, posting hanya saat status berubah; (3) cancel kedua menjadi no-op atau ditolak tanpa mengubah qty; (4) hitung opening dan total dalam update atomik, atau rekalkulasi bucket dari sumber aktif secara terkontrol; (5) propagasikan error dan tampilkan hasil gagal; (6) simpan identitas posting lama dan proses carry-forward; (7) normalisasi NULL sesuai aturan data; (8) audit request ID/transaction ID, affected rows, commit/rollback, tanggal dan quantity lama/baru.

Uji wajib sebelum rilis: opname tanpa movement dan dengan movement, fault setelah update opening, cancel sekali/ulang/dua request serentak, finish dikirim ulang, rollback setelah saldo berubah, perubahan qty/tanggal dan transaksi backdate, serta cancel opname. Rekonsiliasi data lama dilakukan terpisah setelah akar selisih diketahui. Jangan sekadar menyamakan ttlQty dengan openQty untuk semua item karena movement sah harus tetap dihitung.

Catatan di luar lingkup: schema_master.json lokal memuat definisi terpotong seperti decimal(10 dan key palsu FULLTEXT KEY. Gunakan SHOW CREATE TABLE/metadata server untuk validasi skema; perbaikan tanda kutip skrip sebelumnya belum memvalidasi seluruh isi DDL master. Temuan ini tidak membuktikan penyebab saldo Warehouse.
