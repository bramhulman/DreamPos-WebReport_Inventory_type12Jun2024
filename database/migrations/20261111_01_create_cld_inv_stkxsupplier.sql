-- =========================================================================
-- Migration: Create Table cld_inv_stkxsupplier
-- Date: 2026-11-11
-- Purpose: Menyimpan relasi pemetaan antara Supplier dan Master Barang Stok
-- =========================================================================

CREATE TABLE IF NOT EXISTS `cld_inv_stkxsupplier` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `stk_id` INT NOT NULL,
  `supplier_id` INT NOT NULL,
  `custid` INT NOT NULL,
  `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `idx_stk_supp_cust` (`stk_id`, `supplier_id`, `custid`),
  KEY `idx_supp_cust` (`supplier_id`, `custid`),
  KEY `idx_stk_cust` (`stk_id`, `custid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
