alter table [Banner]
add IsActive bit default 1

-- drop constraint PK of ClaimedVoucher
DECLARE @ConstraintName NVARCHAR(256);
DECLARE @TableName NVARCHAR(256) = 'ClaimedVoucher'; 
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

-- add composite PK
ALTER TABLE [ClaimedVoucher]
ADD CONSTRAINT PK_ClaimedVoucher PRIMARY KEY (VoucherId, CustomerId);


