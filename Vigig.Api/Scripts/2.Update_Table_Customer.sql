ALTER TABLE [Customer]
ADD NormalizedUserName NVARCHAR(255) NOT NULL

ALTER TABLE [Customer]
ADD NormalizedEmail NVARCHAR(255) NOT NULL

ALTER TABLE [Customer]
ADD EmailConfirmed BIT DEFAULT 0