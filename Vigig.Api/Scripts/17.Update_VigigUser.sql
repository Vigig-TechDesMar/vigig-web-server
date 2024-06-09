IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VigigUser' AND COLUMN_NAME = 'BuildingId')
BEGIN
	DECLARE @ConstraintName AS NVARCHAR(256)
	DECLARE @Sql AS NVARCHAR(MAX)

	SELECT @ConstraintName = fk.name
	FROM sys.foreign_keys fk
	JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
	JOIN sys.tables ref ON fk.referenced_object_id = ref.object_id
	WHERE ref.name = 'BuildingId'

	SELECT @Sql = 'ALTER TABLE [VigigUser] DROP CONSTRAINT ' + @ConstraintName
	EXEC sp_executesql @Sql

	ALTER TABLE [VigigUser]
	ALTER COLUMN BuildingId UNIQUEIDENTIFIER NULL;
END