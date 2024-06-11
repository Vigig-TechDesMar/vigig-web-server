ALTER TABLE [ClaimedVoucher]
DROP COLUMN StartDate
     
ALTER TABLE [ClaimedVoucher]
DROP COLUMN EndDate
     
ALTER TABLE [ClaimedVoucher]
ADD UsedDate DATETIME NULL 

ALTER TABLE [ClaimedVoucher]
ADD IsUsed BIT DEFAULT 0