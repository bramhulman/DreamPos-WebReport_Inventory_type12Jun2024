CREATE TABLE `cld_inv_trans` (

  `ID` int(11) NOT NULL,

  `CustID` int(11) DEFAULT NULL,

  `InvDate` datetime DEFAULT NULL,

  `type` int(11) NOT NULL COMMENT '1 Transfer, 2 waste, 3 receive, 4 return, 5 adjustment, 6 opening',

  `Cc` int(11) NOT NULL,

  `Stk_Id` int(11) NOT NULL COMMENT 'stock Id',

  `Stk_Name` varchar(30) DEFAULT NULL COMMENT 'Stock Name',

  `U1` decimal(10,4) DEFAULT NULL,

  `U2` decimal(10,4) DEFAULT NULL,

  `Factor1` decimal(10,4) DEFAULT NULL,

  `ttlStock` decimal(10,4) DEFAULT NULL,

  `CostU1` decimal(15,4) DEFAULT NULL,

  `Cost` decimal(15,4) DEFAULT NULL,

  `Linkid` varchar(18) NOT NULL,

  `flagtrans` int(11) DEFAULT NULL COMMENT 'Default isi 1',

  `U1_nm` varchar(5) DEFAULT NULL COMMENT 'Nama unit 1',

  `U2_nm` varchar(5) DEFAULT NULL COMMENT 'Name Unit 2',

  `CreatedBy` varchar(20) DEFAULT NULL,

  `CreatedDate` datetime DEFAULT NULL,

  `ModifBy` varchar(20) DEFAULT NULL,

  `ModifDate` datetime DEFAULT NULL,

  `Stk_no` varchar(15) DEFAULT NULL COMMENT 'Stock No',

  `Cc_name` varchar(15) DEFAULT NULL,

  PRIMARY KEY (`ID`,`type`,`Cc`,`Stk_Id`,`Linkid`),

  KEY `Gabung_trans` (`flagtrans`,`InvDate`),

  KEY `idx_trans_opt` (`type`,`InvDate`,`Stk_Id`,`Linkid`,`Cc`),

  KEY `idx_inv_trans_main` (`CustID`,`InvDate`,`Cc`,`Stk_Id`,`type`,`flagtrans`)

) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `cld_inv_transstockcls` (

  `invDate` datetime NOT NULL,

  `custId` int(11) NOT NULL,

  `stkId` int(11) NOT NULL,

  `stkNo` varchar(15) DEFAULT NULL,

  `stkName` varchar(30) DEFAULT NULL,

  `ccid` int(11) NOT NULL,

  `ccName` varchar(15) DEFAULT NULL,

  `OpenCost` decimal(13,4) DEFAULT NULL,

  `openQty` decimal(10,4) DEFAULT NULL COMMENT 'qty Opening',

  `Cost` decimal(13,4) DEFAULT NULL,

  `rcvQty` decimal(10,4) DEFAULT NULL,

  `rtnQty` decimal(10,4) DEFAULT NULL,

  `tinQty` decimal(10,4) DEFAULT NULL,

  `toutQty` decimal(10,4) DEFAULT NULL,

  `rwasteQty` decimal(10,4) DEFAULT NULL,

  `fwasteQty` decimal(10,4) NOT NULL,

  `usageQty` decimal(10,4) DEFAULT NULL,

  `ttlQty` decimal(10,4) DEFAULT NULL COMMENT 'Total Qty',

  `bClose` bit(1) DEFAULT NULL COMMENT 'melakukan stock opname atau tidak',

  `closeQty` decimal(10,4) DEFAULT NULL COMMENT 'Total StockOpname',

  `closeCost` decimal(13,4) DEFAULT NULL,

  `varianceQty` decimal(10,4) DEFAULT NULL COMMENT 'variance jika ada stockopname',

  `HoOutQty` decimal(10,4) DEFAULT NULL,

  `HoInQty` decimal(10,4) DEFAULT NULL,

  `BPIn` decimal(10,4) DEFAULT NULL,

  `BPout` decimal(10,4) DEFAULT NULL,

  `cwasteqty` decimal(10,4) DEFAULT '0.0000',

  PRIMARY KEY (`invDate`,`custId`,`stkId`,`ccid`),

  KEY `Gabung_transstockcls` (`bClose`,`closeQty`,`closeCost`),

  KEY `idx_stockcls_cost` (`invDate`,`ccid`,`stkId`),

  KEY `idx_stockcls_cost_cover` (`invDate`,`ccid`,`stkId`,`bClose`,`Cost`,`closeCost`,`OpenCost`),

  KEY `idx_transstockcls_invdate` (`invDate`),

  FULLTEXT KEY `Index` (`stkNo`,`stkName`,`ccName`)

) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `cld_invlk_rcv` (

  `ID` varchar(15) NOT NULL,

  `CustID` int(11) NOT NULL,

  `Do` varchar(15) DEFAULT NULL,

  `SupId` int(11) DEFAULT NULL,

  `Flag` int(11) DEFAULT NULL COMMENT '1 submit, 2 ud selesai',

  `DocTot` decimal(14,2) DEFAULT NULL,

  `Tipe` int(11) DEFAULT NULL COMMENT '1 receiving, 2 Return',

  `Cc` int(11) DEFAULT NULL,

  `CreatedBy` varchar(15) DEFAULT NULL,

  `CreatedDate` datetime DEFAULT NULL,

  `ModifBy` varchar(15) DEFAULT NULL,

  `ModifDate` datetime DEFAULT NULL,

  `InvDate` datetime DEFAULT NULL,

  `ID2` int(11) DEFAULT NULL,

  `exDelDate` datetime DEFAULT NULL,

  `Notes` varchar(100) DEFAULT '',

  `ID3` int(11) DEFAULT NULL,

  `InvoiceID` int(11) DEFAULT '0',

  `acc` int(11) DEFAULT '1',

  `p_tax` decimal(8,2) DEFAULT NULL,

  `n_tax` decimal(12,2) DEFAULT NULL,

  `pr_id` bigint(20) DEFAULT NULL,

  PRIMARY KEY (`ID`,`CustID`)

) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `cld_inv_transusage` (

  `CustID` int(11) NOT NULL,

  `InvDate` datetime DEFAULT NULL,

  `type` int(11) NOT NULL COMMENT '10 Usage',

  `Cc` int(11) NOT NULL,

  `Cc_name` varchar(15) DEFAULT NULL,

  `Stk_Id` int(11) NOT NULL COMMENT 'stock Id',

  `Stk_no` varchar(15) DEFAULT NULL COMMENT 'Stock No',

  `Stk_Name` varchar(30) DEFAULT NULL COMMENT 'Stock Name',

  `U2` decimal(10,4) DEFAULT NULL,

  `U3` decimal(10,4) DEFAULT NULL,

  `Factor2` decimal(10,4) DEFAULT NULL,

  `ttlStock` decimal(10,4) DEFAULT NULL COMMENT 'u2 + (U3/factor2)',

  `Cost` decimal(13,4) DEFAULT NULL,

  `Linkid` varchar(15) NOT NULL COMMENT 'SalesDetailID',

  `flagtrans` int(11) DEFAULT NULL COMMENT 'Default isi 1',

  `U2_nm` varchar(5) DEFAULT NULL COMMENT 'Nama unit 2',

  `U3_nm` varchar(5) DEFAULT NULL COMMENT 'Name Unit 3',

  `CreatedBy` varchar(15) DEFAULT NULL,

  `CreatedDate` datetime DEFAULT NULL,

  `ModifBy` varchar(15) DEFAULT NULL,

  `ModifDate` datetime DEFAULT NULL,

  `smNo` varchar(14) NOT NULL DEFAULT '',

  `Outletid` int(11) NOT NULL DEFAULT '0',

  `flag` int(11) NOT NULL DEFAULT '1',

  PRIMARY KEY (`CustID`,`type`,`Cc`,`Stk_Id`,`Linkid`,`flag`),

  KEY `Gabung_transusage` (`flagtrans`,`InvDate`),

  KEY `usageproc` (`type`,`Cc`,`Stk_Id`,`Linkid`),

  KEY `idx_transusage_flag_type` (`flag`,`type`),

  KEY `idx_usage_delete` (`Linkid`,`InvDate`,`Cc`,`Stk_Id`),

  KEY `idx_type_link_cust` (`type`,`Linkid`,`CustID`),

  KEY `idx_transusage_smno` (`smNo`,`Outletid`,`CustID`),

  KEY `idx_inv_transusage_main` (`CustID`,`InvDate`,`Cc`,`Stk_Id`,`type`,`flagtrans`)

) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `cld_inv_transusagesum` (

  `CustID` int(11) NOT NULL,

  `InvDate` datetime NOT NULL,

  `Cc` int(11) NOT NULL,

  `Stk_Id` int(11) NOT NULL,

  `ttlStock` decimal(10,4) DEFAULT NULL,

  `flag` int(11) DEFAULT NULL,

  PRIMARY KEY (`CustID`,`InvDate`,`Cc`,`Stk_Id`),

  KEY `InvDate` (`InvDate`,`Cc`,`Stk_Id`,`flag`),

  KEY `Gabung_transusagesum` (`Stk_Id`,`InvDate`,`Cc`,`flag`),

  KEY `idx_usagesum_cc_stk_date` (`Cc`,`Stk_Id`,`InvDate`)

) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `cld_inv_stk_cc` (
  `stkID` int(11) NOT NULL,
  `CustID` int(11) NOT NULL,
  `stkLimit` decimal(12,4) DEFAULT NULL,
  `ccid` int(11) NOT NULL,
  `cost` decimal(13,4) DEFAULT '0.0000',
  PRIMARY KEY (`stkID`,`CustID`,`ccid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;