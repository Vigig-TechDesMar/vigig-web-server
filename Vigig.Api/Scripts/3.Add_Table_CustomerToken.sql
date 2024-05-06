-- Initialize table
CREATE TABLE CustomerToken(
  CustomerId UNIQEIDENTIFIER NOT NULL,
  LoginProvider NVARCHAR(255) NOT NULL,
  Name NVARCHAR(255) NOT NULL,
  Value NVARCHAR(MAX) NOT NULL
)
-- add primary key
ALTER TABLE CustomerToken
    ADD CONSTRAINT PK_CustomerToken PRIMARY KEY (UserId, LoginProvider, Name);
-- add foreign key
ALTER TABLE CustomerToken
    ADD FOREIGN KEY CustomerId REFERENCES Customer(Id)