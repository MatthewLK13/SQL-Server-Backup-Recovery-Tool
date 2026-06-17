CREATE PROCEDURE sp_BackupList
	@dbName NVARCHAR(100)
AS
BEGIN
	SELECT
		position AS [Ban sao luu thu],
		name AS [Dien giai],
		backup_start_date AS [Thoi gian thuc hien],
		user_name AS [nguoi sao luu]
	FROM msdb.dbo.backupset
	WHERE database_name = @dbName AND type = 'D'
	ORDER BY position DESC;
END