﻿ALTER TABLE [Voucher] 
DROP COLUMN Quantity
     
ALTER TABLE [Voucher]
ADD VoucherTitle NVARCHAR(450)

ALTER TABLE [Voucher]
ADD IconUrl NVARCHAR(MAX) NULL

ALTER TABLE [Voucher]
ADD Amount INT NULL