CREATE PROCEDURE DropForeignKeyAndAlterColumn
    @TableName NVARCHAR(128),
    @ColumnName NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT * 
        FROM INFORMATION_SCHEMA.COLUMNS 
        WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName
    )
BEGIN
        DECLARE @ConstraintName NVARCHAR(256)
        DECLARE @Sql NVARCHAR(MAX)

SELECT @ConstraintName = fk.name
FROM sys.foreign_keys fk
         INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
         INNER JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
WHERE fk.parent_object_id = OBJECT_ID(@TableName) AND c.name = @ColumnName;

IF @ConstraintName IS NOT NULL
BEGIN
            SET @Sql = 'ALTER TABLE ' + QUOTENAME(@TableName) + ' DROP CONSTRAINT ' + QUOTENAME(@ConstraintName)
            EXEC sp_executesql @Sql
END

        SET @Sql = 'ALTER TABLE ' + QUOTENAME(@TableName) + ' ALTER COLUMN ' + QUOTENAME(@ColumnName) + ' UNIQUEIDENTIFIER NULL'
        EXEC sp_executesql @Sql
END
END
