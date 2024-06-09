-- alter complaint
DECLARE @DefaultConstraintName NVARCHAR(128);

SELECT @DefaultConstraintName = dc.name
FROM sys.default_constraints dc
         JOIN sys.columns c ON dc.parent_object_id = c.object_id
WHERE c.object_id = OBJECT_ID('Complaint') AND c.name = 'IsActive';

IF @DefaultConstraintName IS NOT NULL
BEGIN
    DECLARE @DropDefaultSql NVARCHAR(MAX);
    SET @DropDefaultSql = 'ALTER TABLE Complaint DROP CONSTRAINT ' + QUOTENAME(@DefaultConstraintName);
EXEC sp_executesql @DropDefaultSql;
    PRINT 'Default constraint ' + @DefaultConstraintName + ' dropped successfully.';
END
ELSE
BEGIN
    PRINT 'No default constraint found for column IsActive in table Complaint.';
END
     
     
ALTER TABLE [Complaint]
DROP COLUMN IsActive
     
ALTER TABLE [Complaint]
ADD Status int default 0

ALTER TABLE [Complaint]
ADD Content NVARCHAR(MAX) NOT NULL

-- alter event image
-- drop FK eventId
DECLARE @ForeignKeyName NVARCHAR(MAX);
SELECT @ForeignKeyName = fk.name
FROM sys.foreign_keys fk
         INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
         INNER JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
WHERE fk.parent_object_id = OBJECT_ID('EventImage') AND c.name = 'EventId';

IF @ForeignKeyName IS NOT NULL
BEGIN
    DECLARE @DropSql NVARCHAR(MAX);
    SET @DropSql = 'ALTER TABLE EventImage DROP CONSTRAINT ' + QUOTENAME(@ForeignKeyName);
EXEC sp_executesql @DropSql;
    PRINT 'Foreign key constraint dropped: ' + @ForeignKeyName;
END
ELSE
BEGIN
    PRINT 'No foreign key constraint found on column EventId in table EventImage';
END

-- drop old FK
ALTER TABLE [EventImage]
DROP COLUMN EventId
     
-- add new FK
ALTER TABLE [EventImage]
ADD BannerId UNIQUEIDENTIFIER REFERENCES [Banner](Id)

ALTER TABLE [EventImage]
ADD PopUpId UNIQUEIDENTIFIER REFERENCES [PopUp](Id)
        
