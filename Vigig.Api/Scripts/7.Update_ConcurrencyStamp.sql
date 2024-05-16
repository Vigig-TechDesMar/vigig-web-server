ALTER TABLE [ProviderKPI]
ADD ConcurrencyStamp NVARCHAR(MAX)

ALTER TABLE [LeaderBoard]
ADD ConcurrencyStamp NVARCHAR(MAX)

ALTER TABLE [Event]
ADD ConcurrencyStamp NVARCHAR(MAX)
