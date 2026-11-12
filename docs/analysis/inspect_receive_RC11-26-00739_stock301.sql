-- Inspeksi saja: SET variabel sesi + SELECT. Tidak mengubah data transaksi.
-- Jalankan pada database SERVER yang berisi RC11/26-00739.
-- CustID 22 mengikuti kasus sebelumnya. CC diambil dari header, bukan diasumsikan Warehouse.
SET @audit_custid = 22;
SET @audit_doc = 'RC11/26-00739';
SET @audit_stock = 301;
SET @audit_old_date = '2026-11-09';
SET @audit_new_date = '2026-11-04';
SET @audit_from = '2026-11-03'; -- termasuk sehari sebelum tanggal tujuan
SET @audit_until = '2026-11-13'; -- eksklusif: sampai 12 November
SET @audit_ccid = (SELECT h.cc FROM cld_invlk_rcv h
 WHERE h.custid=@audit_custid AND h.id=@audit_doc LIMIT 1);

-- A. Identitas pemeriksaan dan header dokumen terkini.
SELECT DATABASE() AS database_name,NOW() AS snapshot_server_time,
 @audit_doc AS document_no,@audit_stock AS stock_id,@audit_ccid AS header_ccid;
SELECT h.* FROM cld_invlk_rcv h WHERE h.custid=@audit_custid AND h.id=@audit_doc;

-- B. SEMUA detail dokumen, tanpa filter tanggal/stock/flag: lihat item lain juga.
-- Periksa flagtrans, InvDate, ttlStock, CreatedDate dan Modifdate.
-- Detail pending/batal tetap ada dengan flagtrans=0 pada alur cancel saat ini.
SELECT t.* FROM cld_inv_trans t
WHERE t.custid=@audit_custid AND t.LinkID=@audit_doc
ORDER BY t.type,t.cc,t.stk_id,t.InvDate,t.ID;

-- C. Ringkasan posting dokumen khusus stock 301, di semua tanggal dan CC.
-- Bukan hanya tanggal 4/9: menangkap bila tanggal aktual bergeser lagi.
SELECT t.cc,t.InvDate,t.type,t.flagtrans,COUNT(*) AS detail_count,
 SUM(t.ttlStock) AS qty,SUM(t.ttlStock*t.Cost) AS total_cost,
 GROUP_CONCAT(t.ID ORDER BY t.ID) AS detail_ids,
 MIN(t.CreatedDate) AS first_created,MAX(t.Modifdate) AS last_modified
FROM cld_inv_trans t
WHERE t.custid=@audit_custid AND t.LinkID=@audit_doc AND t.Stk_Id=@audit_stock
GROUP BY t.cc,t.InvDate,t.type,t.flagtrans ORDER BY t.InvDate,t.cc,t.type,t.flagtrans;

-- D. Saldo harian versus SUM receive aktif, termasuk receive lain.
-- rcv_difference negatif = bucket lebih kecil dari ledger aktif: kandidat reverse berlebih/tanggal salah.
-- Selisih bukan bukti pasti double cancel; perlu history/audit untuk menetapkan penyebab.
SELECT q.*,q.rcvQty-q.active_receive_qty AS rcv_difference,
 q.ttlQty-q.formula_without_cwaste AS total_difference_without_cwaste,
 q.ttlQty-(q.formula_without_cwaste-COALESCE(q.cwasteqty,0)) AS total_difference_with_cwaste
FROM (
 SELECT s.*,
  (SELECT COALESCE(SUM(t.ttlStock),0) FROM cld_inv_trans t
   WHERE t.custid=s.custid AND t.cc=s.ccid AND t.Stk_Id=s.stkid
    AND t.type=3 AND t.flagtrans=1
    AND t.InvDate>=DATE(s.InvDate) AND t.InvDate<DATE_ADD(DATE(s.InvDate),INTERVAL 1 DAY)) AS active_receive_qty,
  (SELECT COALESCE(SUM(t.ttlStock),0) FROM cld_inv_trans t
   WHERE t.custid=s.custid AND t.cc=s.ccid AND t.Stk_Id=s.stkid
    AND t.LinkID=@audit_doc AND t.type=3 AND t.flagtrans=1
    AND t.InvDate>=DATE(s.InvDate) AND t.InvDate<DATE_ADD(DATE(s.InvDate),INTERVAL 1 DAY)) AS active_qty_this_document,
  COALESCE(s.openQty,0)+COALESCE(s.rcvQty,0)-COALESCE(s.rtnQty,0)
   +COALESCE(s.tinQty,0)-COALESCE(s.toutQty,0)-COALESCE(s.rwasteQty,0)
   -COALESCE(s.fwasteQty,0)-COALESCE(s.usageQty,0)-COALESCE(s.HoOutQty,0)
   +COALESCE(s.HoInQty,0)+COALESCE(s.BPIn,0)-COALESCE(s.BPout,0) AS formula_without_cwaste
 FROM cld_inv_transstockcls s
 WHERE s.custid=@audit_custid AND s.stkid=@audit_stock
  AND (@audit_ccid IS NULL OR s.ccid=@audit_ccid)
  AND s.InvDate>=@audit_from AND s.InvDate<@audit_until
) q ORDER BY q.ccid,q.InvDate;

-- E. Carry-forward: opening hari ini versus closeQty/ttlQty kemarin.
-- expected_open_from_yesterday mengikuti aturan bClose dalam kode.
-- Opname aktif hari ini dapat menjelaskan penyesuaian yang disengaja; lihat query F.
SELECT s.InvDate,s.ccid,s.openQty,s.ttlQty,p.InvDate AS previous_date,
 p.bClose AS previous_bclose,p.closeQty AS previous_close,p.ttlQty AS previous_total,
 CASE WHEN p.bClose=b'1' THEN p.closeQty ELSE p.ttlQty END AS expected_open_from_yesterday,
 s.openQty-(CASE WHEN p.bClose=b'1' THEN p.closeQty ELSE p.ttlQty END) AS carry_difference
FROM cld_inv_transstockcls s
LEFT JOIN cld_inv_transstockcls p ON p.custid=s.custid AND p.ccid=s.ccid AND p.stkid=s.stkid
 AND p.InvDate=DATE_SUB(s.InvDate,INTERVAL 1 DAY)
WHERE s.custid=@audit_custid AND s.stkid=@audit_stock
 AND (@audit_ccid IS NULL OR s.ccid=@audit_ccid)
 AND s.InvDate>=@audit_from AND s.InvDate<@audit_until
ORDER BY s.ccid,s.InvDate;

-- F. Semua ledger stock 301: receive, opname, transfer, waste, dll.
-- cc tidak difilter supaya posting ke CC lain juga terlihat.
SELECT t.* FROM cld_inv_trans t WHERE t.custid=@audit_custid AND t.Stk_Id=@audit_stock
 AND t.InvDate>=@audit_from AND t.InvDate<@audit_until
ORDER BY t.InvDate,t.cc,t.type,t.LinkID,t.ID;

-- G. Raw usage/batch dan ringkasan usage yang turut memengaruhi saldo.
SELECT u.* FROM cld_inv_transusage u WHERE u.custid=@audit_custid AND u.Stk_Id=@audit_stock
 AND (@audit_ccid IS NULL OR u.cc=@audit_ccid)
 AND u.InvDate>=@audit_from AND u.InvDate<@audit_until
ORDER BY u.InvDate,u.cc,u.type;
SELECT u.* FROM cld_inv_transusagesum u WHERE u.custid=@audit_custid AND u.stk_id=@audit_stock
 AND (@audit_ccid IS NULL OR u.cc=@audit_ccid)
 AND u.InvDate>=@audit_from AND u.InvDate<@audit_until ORDER BY u.InvDate,u.cc;

-- H. Kandidat detail receive ganda untuk stock ini. Bukan vonis duplikasi bisnis.
SELECT t.LinkID,t.cc,t.InvDate,t.flagtrans,COUNT(*) AS row_count,
 SUM(t.ttlStock) AS qty,GROUP_CONCAT(t.ID ORDER BY t.ID) AS ids
FROM cld_inv_trans t WHERE t.custid=@audit_custid AND t.Stk_Id=@audit_stock AND t.type=3
 AND t.InvDate>=@audit_from AND t.InvDate<@audit_until
GROUP BY t.LinkID,t.cc,t.InvDate,t.flagtrans HAVING COUNT(*)>1;

-- Catatan interpretasi:
-- Jika masih batal: dokumen ini tidak boleh memberi qty aktif pada tanggal 4 maupun 9.
-- Jika sudah Receive ulang tanggal 4: qty aktif harus di tanggal 4, bukan tanggal 9.
-- rcvQty pada tanggal 4/9 tetap boleh berisi RECEIVE LAIN yang sah: bandingkan SUM semua ledger aktif.
-- Berubahnya InvDate/Modifdate tidak merekam riwayat lengkap jumlah cancel/urutan perpindahan.
-- Snapshot sekarang tidak membuktikan jumlah cancel; bukti historis membutuhkan audit/binlog/log request.
