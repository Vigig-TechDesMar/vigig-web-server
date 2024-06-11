IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Transaction' AND COLUMN_NAME = 'DepositId')
BEGIN
	DECLARE @ConstraintName AS NVARCHAR(256)
	DECLARE @Sql AS NVARCHAR(MAX)

SELECT @ConstraintName = fk.name
FROM sys.foreign_keys fk
         INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
         INNER JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
WHERE fk.parent_object_id = OBJECT_ID('Transaction') AND c.name = 'DepositId';

SELECT @Sql = 'ALTER TABLE [Transaction] DROP CONSTRAINT ' + @ConstraintName
    EXEC sp_executesql @Sql

ALTER TABLE [Transaction]
ALTER COLUMN DepositId UNIQUEIDENTIFIER NULL;
END