# Dokumentasi Cross-App SSO & Error Logging (DreamPos-WebReport Inventory)

## 1. Ringkasan Alur Perpindahan Sesi (Sidebar: Web Report Home)
Ketika user mengklik tombol sidebar **Web Report Home** di aplikasi **DreamPos-WebReport Inventory** (`SiteInventoryHome.Master.vb` / `SiteHome.Master.vb`), aplikasi memanggil helper terpusat `mHelper.RedirectToWebReportHome(Context)` untuk mengarahkan user ke aplikasi **DreamPos-WebReport (`DreamPos-WebReport_usesession`)** tanpa harus login ulang.

### Parameter yang Dikirim ke URL Target (`My.Settings.webreport`)
Format URL tujuan:
`<webreport>/Default.aspx?login=<user>&username=<user>&custid=<custid>&modulepriv=<mpriv>&rptpriv=<rpriv>&modul=<modul>&rptf=<rptf>&invpriv=<invpriv>&time=<sha512_token>`

- `time`: Hash `SHA-512` dari format tanggal jam `ddMMMyyyyHH` (menggunakan `GenerateSHA512String`).
- Nilai parameter diambil dari cookie aktif `UserLogin_inv` pada sisi Inventory.

---

## 2. Sifat Opsional / Standalone
Integrasi ini bersifat **loose-coupled (longgar)**:
- Aplikasi Inventory hanya membaca setting `My.Settings.webreport` saat tombol **Web Report Home** diklik.
- Aplikasi target (`DreamPos-WebReport_usesession`) tetap dapat berjalan dan diakses secara **standalone** (berdiri sendiri) kapan pun tanpa memerlukan aplikasi Inventory.

---

## 3. Pencatatan Error Log (`ErrLog.txt`)
Setiap kegagalan saat menyiapkan parameter atau redirect dari sisi Inventory akan dicatat ke `ErrLog.txt` melalui `mHelper.LogRequestFailure` (dilengkapi Incident GUID, Path, Status Auth, dan detail penyebab):

| Stage di `ErrLog.txt` | Penyebab | Tindakan Perbaikan |
| :--- | :--- | :--- |
| `WebReportSso.MissingTargetUrl` | `My.Settings.webreport` di `Web.config` kosong. | Isi setting `webreport` di `Web.config` dengan URL Web Report (contoh: `http://dreamposweb.com:7357`). |
| `WebReportSso.MissingCookie` | Cookie `UserLogin_inv` tidak ditemukan / sesi sudah habis. | User otomatis diarahkan ke `~/Default.aspx` (login ulang). |
| `WebReportSso.IncompleteCookie` | Nilai `userName` atau `custid` di dalam cookie `UserLogin_inv` kosong. | Cek proses login / `Default.aspx.vb`; user diarahkan ke `~/Default.aspx`. |
| `WebReportSso.RedirectException` | Terjadi exception tak terduga saat membangun URL atau redirect. | Cek detail `Exception`, `Message`, dan `Stack` pada Incident ID terkait di `ErrLog.txt`. |

> [!NOTE]
> Jika redirect berhasil dibuat, URL target juga dicatat dengan tag `url` (atau dapat dicek di `ErrLog.txt` sisi target `DreamPos-WebReport_usesession` apabila validasi di server tujuan gagal).
