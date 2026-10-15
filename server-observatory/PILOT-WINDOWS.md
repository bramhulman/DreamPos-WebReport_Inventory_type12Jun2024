# Pilot Windows Server + IIS + MySQL

## Temuan dari script/log yang diberikan

- Windows Server 2025, RAM 16 GB; jumlah logical processor belum diketahui. Nama CPU bukan jumlah vCPU.
- MySQL disebut 8012: perlu SELECT VERSION() untuk memastikan, bukan mengasumsikan 8.0.12.
- Sekitar 20 schema, 5 aktif/besar, diakses web, webservice dan executable kalkulasi. Trafik IIS tidak meliputi pekerjaan desktop.
- Aborted_connects total 22 memicu restart pada sampel CPU 58,4% dan RAM 76,5%. Total counter bukan kegagalan aktif. Collector baru memakai delta dua snapshot dan mendeteksi reset/interval tidak valid. Tidak ada aksi restart.
- MySQL tercatat restart berulang. Jangan menyimpulkan semua restart memiliki penyebab sama tanpa log terkait. Restart membatalkan pekerjaan aktif dan dapat memicu recovery; perlu perubahan prosedur operasional terkontrol.
- Cabang CPU/RAM di script lama bisa masuk jalur restart MySQL walaupun jalur health sedang cooldown. Cooldown bukan satu pengaman terpusat.
- CPU dari Get-Process adalah waktu CPU kumulatif dalam detik, bukan persen. Gunakan delta waktu CPU / interval / logical processors untuk persentase seluruh mesin, dengan PID dan waktu mulai proses untuk mendeteksi reuse/restart.
- Mengembalikan 0 ketika pembacaan CPU/RAM gagal dapat menghasilkan status NORMAL palsu. Status koleksi gagal harus unknown/stale.
- Pemakai RAM terbesar belum tentu penyebab CPU tinggi atau gangguan. Buffer/cache MySQL tidak otomatis berarti memory leak.
- Disk C pada contoh sekitar 85,9%; belum diketahui ruang kosong absolut, kapasitas, pertumbuhan log, latency disk atau volume data MySQL. Jangan menghapus log/file database otomatis.
- Script lama memasukkan query mentah ke log/email dan password ke argumen proses; modul baru tidak melakukan itu. Password yang terpapar perlu dirotasi melalui inventaris dependensi, tanpa menyalinnya ke repo/chat lagi.

Rekomendasi operasional: tinjau penonaktifan cabang restart/kill otomatis pada task lama melalui change window dan pastikan jalur alarm pengganti tersedia. Proyek ini tidak mengubah task atau script lama di server.

## Pilot tahap 1 — lokal melalui RDP

1. Salin modul ke folder terpisah pada server. Jangan menaruhnya di folder publik aplikasi IIS.
2. Verifikasi Node.js versi yang masih didukung tersedia. Berikan akun collector akses baca terbatas pada CIM dan log IIS; write hanya pada direktori state yang diberi ACL khusus.
3. Generate dua token acak berbeda, minimal 32 karakter, untuk MONITOR_TOKEN (collector) dan MONITOR_VIEW_TOKEN (viewer). Jangan taruh token pada file yang di-commit atau task command line.
4. Jalankan `node server.cjs` dengan kedua environment variable. Akses dari browser di sesi RDP: http://127.0.0.1:8787. Gunakan token VIEW untuk membaca. Port tetap loopback.
5. Jalankan Collect-Server.ps1 manual, kemudian verifikasi snapshot. Belum ada penjadwalan otomatis.

## Pilot tahap 2 — trafik IIS

1. Jalankan `./Discover-IIS.ps1` manual untuk inventaris site id dan app pool, bukan untuk mengubah konfigurasi. Pastikan per-site logging W3C digunakan. LogDirectory mungkin memakai environment variable dan subfolder W3SVC<id>; verifikasi lokasi nyata.
2. Salin iis.example.json menjadi iis.local.json. Isi site id/nama/host alias dan folder log aktual. Jangan masukkan URL berisi parameter/token.
3. Pastikan W3C memuat date, time, sc-status; time-taken diperlukan untuk p95. Collector tidak mengaktifkan logging sendiri.
4. Jalankan `node collect-iis.cjs iis.local.json` dengan MONITOR_TOKEN. Pertama hanya membuat baseline di akhir log. Jalankan lagi setelah 60 detik untuk delta.
5. Start pilot 1–2 site, lalu paling banyak 5 site aktif sambil mengukur overhead. Baru perluas setelah budget sumber daya disetujui.

Batas: 1 MiB pembacaan tambahan/site/run. Backlog berlebih, rotasi file, truncation atau jeda >120 detik memperbarui baseline tanpa memalsukan RPS. Offset hanya maju sampai newline lengkap. Hanya file u_ex*.log terbaru di folder yang dibaca; permintaan sisa pada file lama saat rotasi bisa terlewat. Tidak ada replay histori. Direktori log harus diretasikan/diarsip melalui prosedur terpisah, bukan collector.

Jika tidak ada baris request selesai, collector tidak mengirim nol: belum bisa membedakan idle dari logging macet. Resource akan stale, bukan dianggap down. Heartbeat server adalah metrik terpisah. Belum ada active availability probe.

p95 adalah nearest-rank untuk time-taken dari baris selesai pada interval, bukan durasi query atau CPU aplikasi. time-taken IIS dapat meliputi waktu jaringan. Request lama baru tercatat setelah selesai; log buffering dan clock skew mempengaruhi kesegaran. HTTP error adalah 5xx, 4xx tidak disamakan dengan kegagalan aplikasi.

Satu IIS site dapat memuat beberapa aplikasi/webservice. Collector saat ini agregasi per site. Pemetaan app pool dan pemisahan prefix aplikasi merupakan pekerjaan lanjutan; tidak mengklaim CPU per website jika pool dipakai bersama.

## Pilot tahap 3 — MySQL instance

DBA membuat akun monitoring lokal tersendiri; jangan gunakan root atau grant pada tabel bisnis. Untuk collector otomatis ini, review pemberian SELECT hanya pada performance_schema.global_status. Query diagnostik per schema memerlukan SELECT tambahan pada events_statements_summary_by_digest, bukan hak tulis. Jangan memberikan PROCESS global atau akses data aplikasi tanpa kebutuhan yang dibuktikan.

Siapkan login path sebagai akun Windows yang sama dengan task collector. Contoh interaktif, password dimasukkan pada prompt:

```powershell
& 'C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql_config_editor.exe' set --login-path=observatory --host=127.0.0.1 --port=3306 --user=observatory_monitor --password
./Collect-MySQL.ps1
```

Login-path menghindari password plaintext di command line; file .mylogin.cnf memakai obfuscation dan bukan vault dengan enkripsi kuat. Lindungi profil/account dan ACL. Jangan salin login path ke repo atau berbagi akun Windows.

Query collector membaca 7 counter status global, sekali per run, connect timeout 3 detik dan process deadline 8 detik. Tidak mengubah instrumentation Performance Schema. Pemanggilan pertama hanya mengirim koneksi/thread/uptime. Rate dan aborted delta membutuhkan snapshot kedua, 5–120 detik setelahnya. Jadwal rekomendasi pilot 60 detik, tanpa overlap/retry otomatis; pilih 'Do not start a new instance' pada Task Scheduler.

Queries/s adalah tingkat instance, termasuk statement collector serta statement dari stored program sesuai definisi MySQL. Bukan traffic HTTP, jumlah transaksi, atau QPS tiap schema. Threads_connected termasuk koneksi pengambilan sampel; Threads_running bisa mencakup statement monitor yang sedang berjalan. Aborted_clients dan Aborted_connects berbeda dan tidak ditafsir sebagai pemicu restart.

State hanya berisi counter agregat dan waktu; ACL harus dibatasi ke akun collector/admin. MySQL lock adalah file handle eksklusif. IIS memakai lock file eksklusif; jika proses mati paksa, periksa bahwa proses sudah berhenti sebelum menghapus lock sisa. Tidak ada pembersihan lock otomatis yang berisiko overlap.

mysql-readonly-diagnostics.sql disediakan untuk pemeriksaan manual, bukan dijalankan otomatis. Agregat per schema bersifat kumulatif menurut default schema; query cross-schema bisa mengakses data schema lain. Instrumentation yang tidak aktif berarti data tidak lengkap, bukan beban nol. Atribusi web/webservice/desktop tidak dapat dipastikan jika semua memakai akun yang sama. Buat mapping workloads.local.json dari contoh, lalu rencanakan akun aplikasi terpisah dalam change window.

Jangan menghitung p95 dari rata-rata atau MAX_TIMER_WAIT. Latensi p95 database belum dikumpulkan. Jangan mengaktifkan general query log penuh pada server sibuk untuk memenuhi dashboard ini.

## Pilot tahap 4 — browser lewat internet

Arsitektur target:

```
Browser tim -> HTTPS + identity proxy dengan MFA -> reverse proxy IIS khusus -> 127.0.0.1:8787
Collector Windows/IIS/MySQL -> loopback /api/ingest
```

Tidak membuka port Node atau MySQL ke internet. Endpoint publik /api/ingest harus diblokir oleh proxy; metrik dikirim collector lokal. Proxy wajib memverifikasi login/MFA pada HTML, aset dan API, meneruskan header Host upstream 127.0.0.1:8787 (backend memeriksa Host), serta mempertahankan Authorization viewer bila memakai UI pilot ini. Token collector tidak boleh masuk browser. Identity proxy dan token viewer adalah dua lapis berbeda; token viewer saat ini masih dibagi manual, belum akun per pengguna. Jangan menonaktifkan pemeriksaan Host agar proxy sekadar berjalan.

Konfigurasi proxy belum dibuat atau diterapkan: masih membutuhkan subdomain/sertifikat dan pilihan identity provider/MFA. SSO per pengguna, pembatasan percobaan login di gateway, masa sesi, pencabutan akses dan audit perlu diverifikasi sebelum publikasi. RDP tidak perlu dipublikasikan tambahan untuk dashboard.

## Kriteria sebelum operasi produksi

- Collector diuji di target dengan akun minimal, tidak ada root/secret command line; credential yang terpapar sudah ditangani melalui prosedur internal.
- Ukur overhead pada jam sibuk dan saat sales_summary berjalan; tetapkan budget eksekusi, CPU dan I/O sebelum memperluas scope.
- Uji task overlap, service restart, rotasi log, MySQL reset, disk penuh pada direktori state, backend mati dan pemulihan.
- Tambahkan penyimpanan histori terpisah yang berbatas, retensi, backup/restore serta runbook disk penuh; jangan menambah histori ke MySQL bisnis sebelum mengukur dampaknya.
- Alert berdasarkan durasi/delta dan baseline, dengan alarm stale terpisah; kanal notifikasi harus disetujui. Tidak mengirim email dari pengembangan ini.
- Verifikasi MFA/TLS, endpoint ingest tidak bisa diakses publik, token VIEW tidak bisa POST dan token collector tidak bisa GET.
- Mulai tanpa auto-remediation. Perbaikan index/query, jadwal kalkulasi, pool/cache atau scaling berdasarkan korelasi, satu perubahan per pilot dan rollback yang jelas.

## Referensi primer

- Status counter MySQL: https://dev.mysql.com/doc/refman/8.0/en/server-status-variables.html
- Statement summaries: https://dev.mysql.com/doc/refman/8.0/en/performance-schema-statement-summary-tables.html
- Login path: https://dev.mysql.com/doc/refman/8.0/en/mysql-config-editor.html
- CPU kumulatif proses: https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.totalprocessortime
- IIS time-taken: https://learn.microsoft.com/en-us/troubleshoot/developer/webapps/iis/health-diagnostic-performance/time-taken-field-http-log
