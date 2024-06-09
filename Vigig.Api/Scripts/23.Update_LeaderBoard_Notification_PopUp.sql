-- alter table leaderboard
ALTER TABLE [LeaderBoard]
ADD Description NVARCHAR(MAX) NULL 

ALTER TABLE [LeaderBoard]
ADD IsActive BIT DEFAULT 1

-- alter table PopUp
ALTER TABLE [PopUp]
ADD IsActive BIT DEFAULT 1

select * from SubscriptionFee
