-- Diagnosis only: SET session variables + SELECT. No application data is changed.
-- Run on the SERVER database containing the incident, preferably while the three items are idle.
-- custid=22, ccid=2 and stock IDs confirmed by the supplied type=12 rows.
SET @audit_custid = 22;
SET @audit_date = '2026-11-12';
SET @audit_ccid = 2;

-- Expected quantities from ledger IDs 1176617/1176618/1176619, saved 12:56:39-12:56:51.
-- Compare with CURRENT snapshot; later transactions/recalculations may have changed it.
SELECT e.stkid,e.expected_open,s.openQty,s.ttlQty,
 s.openQty-e.expected_open AS opening_difference,
 s.ttlQty-e.expected_open AS total_minus_opname,
 s.rcvQty,s.rtnQty,s.tinQty,s.toutQty,s.rwasteQty,s.fwasteQty,
 s.usageQty,s.HoOutQty,s.HoInQty,s.BPIn,s.BPout,s.cwasteqty
FROM (SELECT 205 AS stkid,100.0000 AS expected_open
 UNION ALL SELECT 206,188.0000 UNION ALL SELECT 373,352.0000) e
LEFT JOIN cld_inv_transstockcls s ON s.stkid=e.stkid AND s.custid=@audit_custid
 AND s.ccid=@audit_ccid AND s.invDate=@audit_date;

-- 1. Snapshot: previous close / today's opening / movement buckets / closing total.
-- NULL raw_formula means at least one operand is NULL.
SELECT x.*, x.ttlQty - x.formula_qty AS stored_minus_formula
FROM (
 SELECT s.*,
   s.openQty+s.rcvQty-s.rtnQty+s.tinQty-s.toutQty-s.rwasteQty-s.fwasteQty
      -s.usageQty-s.HoOutQty+s.HoInQty-s.BPout+s.BPIn AS raw_formula,
   COALESCE(s.openQty,0)+COALESCE(s.rcvQty,0)-COALESCE(s.rtnQty,0)
      +COALESCE(s.tinQty,0)-COALESCE(s.toutQty,0)-COALESCE(s.rwasteQty,0)
      -COALESCE(s.fwasteQty,0)-COALESCE(s.usageQty,0)-COALESCE(s.HoOutQty,0)
      +COALESCE(s.HoInQty,0)-COALESCE(s.BPout,0)+COALESCE(s.BPIn,0) AS formula_qty
 FROM cld_inv_transstockcls s
 WHERE s.custId=@audit_custid
   AND s.invDate>=DATE_SUB(@audit_date,INTERVAL 1 DAY)
   AND s.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
   AND s.ccid=@audit_ccid
   AND s.stkid IN (205,206,373)
) x ORDER BY x.stkId,x.ccid,x.invDate;

-- 2. All ledger types (do not assume 'no receive' means no usage/transfer).
-- Shows full detail including flags, old/new dates and modification timestamps.
SELECT t.* FROM cld_inv_trans t
WHERE t.custid=@audit_custid
 AND t.invDate>=DATE_SUB(@audit_date,INTERVAL 1 DAY)
 AND t.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
 AND EXISTS (SELECT 1 FROM cld_inv_transstockcls s
   WHERE s.custid=t.custid AND s.stkid=t.stk_id AND s.ccid=t.cc
    AND s.invDate>=@audit_date AND s.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
    AND s.ccid=@audit_ccid
    AND s.stkid IN (205,206,373))
ORDER BY t.cc,t.stk_id,t.invDate,t.type,t.ID;

-- 3. Does the receive bucket match ACTIVE type=3 ledger rows on the same day?
SELECT s.invDate,s.custid,s.ccid,s.stkid,s.stkName,s.rcvQty,
 (SELECT COALESCE(SUM(t.ttlStock),0) FROM cld_inv_trans t
   WHERE t.custid=s.custid AND t.cc=s.ccid AND t.stk_id=s.stkid
     AND t.type=3 AND t.flagtrans=1
     AND t.invDate>=DATE(s.invDate) AND t.invDate<DATE_ADD(DATE(s.invDate),INTERVAL 1 DAY)) AS active_receive_qty
FROM cld_inv_transstockcls s
WHERE s.custid=@audit_custid
 AND s.invDate>=@audit_date AND s.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
 AND s.ccid=@audit_ccid
 AND s.stkid IN (205,206,373);

-- 4. Usage is stored separately and can affect total even without ledger rows today.
SELECT u.* FROM cld_inv_transusagesum u
WHERE u.custid=@audit_custid
 AND u.invDate>=@audit_date AND u.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
 AND EXISTS (SELECT 1 FROM cld_inv_transstockcls s
   WHERE s.custid=u.custid AND s.ccid=u.cc AND s.stkid=u.stk_id
    AND s.invDate>=@audit_date AND s.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
    AND s.ccid=@audit_ccid AND s.stkid IN (205,206,373));

-- 5. Duplicate receive lines / duplicate opname candidates for the incident day.
-- Multiple lines are candidates for review, not automatic proof of double processing.
SELECT t.custid,t.cc,t.stk_id,t.invDate,t.type,t.LinkID,t.flagtrans,
 COUNT(*) AS row_count,SUM(t.ttlStock) AS qty,GROUP_CONCAT(t.ID ORDER BY t.ID) AS ids
FROM cld_inv_trans t
WHERE t.custid=@audit_custid AND t.type IN (3,12)
 AND t.invDate>=@audit_date AND t.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
GROUP BY t.custid,t.cc,t.stk_id,t.invDate,t.type,t.LinkID,t.flagtrans
HAVING COUNT(*)>1;

-- 6. Headers versus detail flags, quantity and dates, for the three items.
SELECT t.ID,t.LinkID,t.custid,t.cc,t.stk_id,t.invDate,t.flagtrans,t.ttlStock,
 t.CreatedDate,t.Modifdate,h.flag AS header_flag,h.InvDate AS header_date
FROM cld_inv_trans t LEFT JOIN cld_invlk_rcv h ON h.id=t.LinkID AND h.custid=t.custid
WHERE t.custid=@audit_custid AND t.type=3
 AND t.invDate>=DATE_SUB(@audit_date,INTERVAL 1 DAY)
 AND t.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
 AND EXISTS (SELECT 1 FROM cld_inv_transstockcls s
   WHERE s.custid=t.custid AND s.ccid=t.cc AND s.stkid=t.stk_id
    AND s.invDate>=@audit_date AND s.invDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
    AND s.ccid=@audit_ccid AND s.stkid IN (205,206,373))
ORDER BY t.LinkID,t.stk_id;

-- 7. Actual server engine, nullable fields, indexes and triggers (not assumptions from local backup).
SELECT TABLE_NAME,ENGINE FROM information_schema.TABLES
 WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('cld_inv_trans','cld_inv_transstockcls','cld_invlk_rcv');
SELECT COLUMN_NAME,COLUMN_TYPE,IS_NULLABLE,COLUMN_DEFAULT FROM information_schema.COLUMNS
 WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='cld_inv_transstockcls' ORDER BY ORDINAL_POSITION;
SELECT TABLE_NAME,INDEX_NAME,NON_UNIQUE,SEQ_IN_INDEX,COLUMN_NAME FROM information_schema.STATISTICS
 WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('cld_inv_trans','cld_inv_transstockcls','cld_invlk_rcv')
 ORDER BY TABLE_NAME,INDEX_NAME,SEQ_IN_INDEX;
SELECT TRIGGER_NAME,EVENT_OBJECT_TABLE,ACTION_TIMING,EVENT_MANIPULATION,ACTION_STATEMENT
 FROM information_schema.TRIGGERS WHERE TRIGGER_SCHEMA=DATABASE()
 AND EVENT_OBJECT_TABLE IN ('cld_inv_trans','cld_inv_transstockcls','cld_invlk_rcv');

-- 8. Raw usage / batch usage for Minced Chicken and the two comparison items.
-- These transactions are NOT necessarily in cld_inv_trans.
SELECT u.* FROM cld_inv_transusage u
WHERE u.CustID=@audit_custid AND u.Cc=@audit_ccid AND u.Stk_Id IN (205,206,373)
 AND u.InvDate>=@audit_date AND u.InvDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
ORDER BY u.Stk_Id,u.type,u.InvDate;

SELECT u.Stk_Id,u.type,u.flagtrans,COUNT(*) AS row_count,SUM(u.ttlStock) AS qty
FROM cld_inv_transusage u
WHERE u.CustID=@audit_custid AND u.Cc=@audit_ccid AND u.Stk_Id IN (205,206,373)
 AND u.InvDate>=@audit_date AND u.InvDate<DATE_ADD(@audit_date,INTERVAL 1 DAY)
GROUP BY u.Stk_Id,u.type,u.flagtrans;
