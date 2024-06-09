-- booking fee thieu status
alter table [BookingFee]
add Status int default 0 NOT NULL
    
    

-- deposit doi ten madedate thanh createdDate
EXEC sp_rename 'Deposit.MadeDate', 'CreatedDate', 'COLUMN';
    
-- deposit thieu status
alter table [Deposit]
add Status int default 0 NOT NULL

-- subcription thieu status
alter table [SubscriptionFee]
add Status int default 0 NOT NULL