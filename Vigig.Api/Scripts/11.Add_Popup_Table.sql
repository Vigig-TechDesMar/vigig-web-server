IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PopUp')
BEGIN
    CREATE TABLE [PopUp](
        Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
        Title NVARCHAR(MAX) NOT NULL,
        SubTitle NVARCHAR(MAX),
        StartDate DATETIME NOT NULL,
        EndDate DATETIME NOT NULL,
        EventId UNIQUEIDENTIFIER NOT NULL,
    )
END

ALTER TABLE [PopUp]
ADD FOREIGN KEY (EventId) REFERENCES [Event](Id)
