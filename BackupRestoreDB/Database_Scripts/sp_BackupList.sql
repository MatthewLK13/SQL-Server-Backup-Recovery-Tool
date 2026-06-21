CREATE PROCEDURE sp_BackupList
	@dbName NVARCHAR(100)
AS
BEGIN
	SELECT
		position AS [Bản sao lưu thứ],
		name AS [Diễn giải],
		backup_start_date AS [Thời gian thực hiện],
		user_name AS [người sao lưu]
	FROM msdb.dbo.backupset
	WHERE database_name = @dbName AND type = 'D'
	ORDER BY position DESC;
END