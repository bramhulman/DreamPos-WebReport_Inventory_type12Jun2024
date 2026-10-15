# Observatory — pilot monitoring internal

Modul mandiri, tidak mengubah aplikasi DreamPos. Memerlukan Node.js yang didukung di lingkungan Anda; menggunakan modul bawaan, tanpa npm install.

## Menjalankan demo lokal

Dari direktori ini: `node server.cjs --demo`, lalu buka http://127.0.0.1:8787.
Semua data demo adalah sintetis. Grafik mulai setelah dua sampel (~30 detik).

## Menjalankan pilot data nyata

1. Buat token acak dengan `node -e "console.log(require('node:crypto').randomBytes(32).toString('hex'))"` dan simpan melalui mekanisme secret internal. Jangan commit token.
2. Buat dua token acak berbeda: `MONITOR_TOKEN` untuk collector dan `MONITOR_VIEW_TOKEN` untuk viewer; set keduanya pada proses server, lalu `node server.cjs`.
3. Buka dashboard dan masukkan MONITOR_VIEW_TOKEN. Token browser hanya di memori; reload meminta login kembali.
4. Set secret yang sama di sesi collector, jalankan `./Collect-Server.ps1` secara manual terlebih dahulu. Script mengirim satu snapshot baca-saja, bukan daemon.
5. Setelah izin akun dan overhead diverifikasi, jadwalkan setiap 60 detik dengan akun berizin minimal yang memadai untuk CIM. Script tidak mendaftarkan task otomatis.

Server hanya bind 127.0.0.1. Remote collector memerlukan reverse proxy HTTPS internal dengan pembatasan jaringan, batas request, dan autentikasi organisasi sebelum pilot lintas server. Jangan mengekspos port atau mengganti bind tanpa review. Token collector dan viewer sudah dipisahkan; SSO/MFA tetap perlu dipasang sebelum akses internet. Tidak ada koneksi database atau perubahan konfigurasi server otomatis.

## Yang tersedia

- UI Indonesia responsif, pencarian, filter jenis, ringkasan, grafik trafik website.
- API `GET /api/metrics` dan `POST /api/ingest`, header `Authorization: Bearer <token>` dengan token viewer untuk GET dan collector untuk POST.
- Validasi payload, batas 16 KiB/request, maksimum 1.000 resource, timeout, security headers, tanpa aset eksternal.
- Metrik kedaluwarsa setelah 120 detik; riwayat snapshot website 240 titik di memori (~60 menit), resource dihapus setelah 24 jam tanpa pembaruan. Restart menghapus semua data.
- Collector Windows: CPU rata-rata processor, RAM OS, persentase penggunaan drive lokal tertinggi. Tidak mengukur IOPS atau pemakaian CPU per website.
- Peringatan berbasis snapshot: CPU >=80%, RAM/disk >=85%, p95 >=1000 ms, error >=1%. Ini ambang pilot, bukan bukti insiden, SLA, atau alasan otomatis menambah server. Peringatan hanya di layar.

## Kontrak agregat

Satu POST per resource per interval, contoh:

```json
{"id":"inventory-web","name":"Inventory","server":"IIS-01","kind":"website","rps":12.5,"p95":320,"errorRate":0.4}
```

`kind`: server / website / database. Metrik opsional: cpu, memory, disk (persen), rps (request/s untuk website atau query/s untuk database), p95 (ms), errorRate (persen), connections (jumlah). Nama/id hanya karakter ASCII alfanumerik, spasi, titik, garis bawah, titik dua, tanda minus; maksimal 100 karakter. Snapshot menggantikan snapshot resource sebelumnya; field hilang ditampilkan kosong. `receivedAt` ditentukan server; pipeline wajib hanya mengirim agregat terbaru, jangan replay backlog tanpa perubahan kontrak waktu observasi.

Jangan kirim URL berisi query string, IP pengunjung, cookies, header Authorization, SQL mentah, connection string, atau data pelanggan. Backend membuang field tambahan. Nama resource/server tetap informasi internal.

## Strategi bertahap

1. **Inventarisasi** — konfirmasi OS, IIS/Nginx/Apache, pemetaan website/app pool, lokasi log, DB engine, jumlah layanan, login internal dan batas kapasitas. Pelajari script monitor lama yang sudah disamarkan.
2. **Baseline server** — pilot collector di satu server, interval 60 detik, ukur waktu eksekusi, CPU, RAM dan I/O collector saat normal serta saat sibuk. Pastikan gagal kirim tidak memblokir aplikasi. Tentukan budget overhead bersama tim sebelum perluasan.
3. **Trafik setiap website** — adapter log incremental dengan checkpoint offset, rotasi file, pembacaan berbatas dan waktu UTC. Untuk IIS W3C, baca header #Fields; agregasikan count/durasi interval, p95 dari time-taken, dan error HTTP 5xx per site. Jangan rata-ratakan p95 antar interval. Bedakan error aplikasi 5xx dan client 4xx. Jangan membaca ulang seluruh file pada setiap interval.
4. **Database** — adapter sesuai engine menggunakan akun baca dengan akses metrik terbatas, timeout query dan interval konservatif. Ambil query throughput, koneksi, blocking dan latensi bila engine menyediakan sumber yang tepat. Hindari tracing penuh/query text. Collector IIS incremental dan MySQL tingkat instance sudah disertakan; analisis per schema dan atribusi aplikasi masih memerlukan mapping serta pilot.
5. **Akses dan penyimpanan** — reverse proxy TLS, LAN/VPN allowlist, SSO/RBAC, token collector terpisah dan rotasi, penyimpanan time-series dengan kebijakan retensi serta backup/restore teruji. Implementasi saat ini in-memory, bukan histori audit.
6. **Alert operasional** — ambang berbasis baseline, durasi pelanggaran, hysteresis, maintenance window, deduplikasi, serta notifikasi ke kanal yang disetujui. Alarm collector hilang harus terpisah dari layanan down. Uptime perlu probe tersendiri; dashboard ini tidak mengklaim availability.
7. **Optimasi berbasis bukti** — korelasikan RPS/error/p95 dengan CPU/RAM/disk dan DB. Periksa query/index, caching, thread/connection pool, dan endpoint lambat sebelum memutuskan scale. Uji satu perubahan, ukur ulang, siapkan rollback.

## Batas produksi

Belum ada SSO/RBAC, durable storage, audit trail, deduplikasi/replay protection, token terpisah per collector, alarm eksternal, probe uptime, atau atribusi workload per database/aplikasi. Jangan gunakan mode demo untuk keputusan infrastruktur. Jangan memublikasikan data internal ke hosting eksternal. Deployment produksi belum dilakukan.

## Verifikasi

`node --check server.cjs`, `node --check public/app.js`, dan `node smoke-test.cjs`.
Smoke test memakai proses server lokal terpisah dan token acak, memeriksa akses anonim ditolak, payload invalid ditolak, metrik masuk/terbaca, CSP dan halaman, serta mode demo read-only. Collector perlu verifikasi pada Windows server target sebelum dijadwalkan.

Referensi adapter IIS: https://learn.microsoft.com/en-us/iis/configuration/system.applicationhost/sites/site/logfile/


## Integrasi Windows IIS dan MySQL

Lihat [PILOT-WINDOWS.md](PILOT-WINDOWS.md). Collector IIS membaca W3C incremental dengan batas 1 MiB/site/run, tidak mengekspor URL/IP. Collector MySQL membaca tujuh status global memakai login-path, tidak mengakses tabel bisnis.

Pengujian tambahan: `node iis-test.cjs` dan `./mysql-interval-test.ps1`.
