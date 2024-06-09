IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VigigUser' AND COLUMN_NAME = 'BuildingId')
BEGIN
	DECLARE @ConstraintName AS NVARCHAR(256)
	DECLARE @Sql AS NVARCHAR(MAX)

SELECT @ConstraintName = fk.name
FROM sys.foreign_keys fk
         INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
         INNER JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
WHERE fk.parent_object_id = OBJECT_ID('VigigUser') AND c.name = 'BuildingId';

SELECT @Sql = 'ALTER TABLE [VigigUser] DROP CONSTRAINT ' + @ConstraintName
    EXEC sp_executesql @Sql

ALTER TABLE [VigigUser]
ALTER COLUMN BuildingId UNIQUEIDENTIFIER NULL;
END