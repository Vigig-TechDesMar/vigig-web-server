DECLARE @ConstraintName NVARCHAR(256);
DECLARE @TableName NVARCHAR(256) = 'Transaction'; 
DECLARE @SchemaName NVARCHAR(256) = 'dbo';
DECLARE @Sql NVARCHAR(MAX);

SELECT @ConstraintName = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_NAME = @TableName
  AND CONSTRAINT_TYPE = 'PRIMARY KEY'
  AND TABLE_SCHEMA = @SchemaName;
IF @ConstraintName IS NOT NULL
BEGIN
    SET @Sql = 'ALTER TABLE ' + QUOTENAME(@SchemaName) + '.' + QUOTENAME(@TableName) + ' DROP CONSTRAINT ' + QUOTENAME(@ConstraintName);
EXEC sp_executesql @Sql;
    PRINT 'Primary key constraint dropped: ' + @ConstraintName;
END
ELSE
BEGIN
    PRINT 'Primary key constraint not found for table ' + @SchemaName + '.' + @TableName;
END
ALTER TABLE [Transaction]
    DROP COLUMN Id
        
ALTER TABLE [Transaction]
    ADD Id INT IDENTITY(1,1) PRIMARY KEY