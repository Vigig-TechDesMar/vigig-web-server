﻿IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ComplaintType')
BEGIN
    CREATE TABLE [ComplaintType](
        Id UNIQUEIDENTIFIER NOT NULL,
        NAME NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) ,
        IsActive BIT DEFAULT 1
    )
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Complaint')
BEGIN
CREATE TABLE [Complaint](
    Id UNIQUEIDENTIFIER NOT NULL,
    NAME NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) ,
    IsActive BIT DEFAULT 1
    )
END

ALTER TABLE [ComplaintType]
    ADD CONSTRAINT Pk_ComplaintType PRIMARY KEY (Id)

ALTER TABLE [Complaint]
    ADD CONSTRAINT Pk_Complaint PRIMARY KEY (Id)

ALTER TABLE [Complaint]
    ADD BookingId UNIQUEIDENTIFIER NOT NULL

ALTER TABLE [Complaint]
    ADD ComplaintTypeId UNIQUEIDENTIFIER NOT NULL

ALTER TABLE [Complaint]
    ADD FOREIGN KEY (BookingId) REFERENCES [Booking](Id)

ALTER TABLE [Complaint]
    ADD FOREIGN KEY (ComplaintTypeId) REFERENCES [ComplaintType](Id)