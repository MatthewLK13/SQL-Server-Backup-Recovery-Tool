CREATE OR ALTER PROCEDURE sp_JobBackupLog
    @baseBackupPath NVARCHAR(500) = 'C:\SQLBackup\Log\' 
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @dbName NVARCHAR(100);
    DECLARE @finalPath NVARCHAR(600);
    DECLARE @sql NVARCHAR(MAX);

    -- 1. Dùng Cursor để duyệt qua tất cả DB của người dùng
    DECLARE db_cursor CURSOR FOR 
    SELECT name 
    FROM sys.databases 
     -- Loại bỏ các DB hệ thống và các DB đang Offline và phải ở chế độ FULL
    WHERE name NOT IN ('master', 'model', 'msdb', 'tempdb') 
      AND state_desc = 'ONLINE' 
      AND recovery_model_desc <> 'SIMPLE'; 

    OPEN db_cursor;  
    FETCH NEXT FROM db_cursor INTO @dbName;  

    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        -- Check xem đã có bản sao lưu hay chưa
        IF EXISTS (
            SELECT 1 FROM msdb.dbo.backupset 
            WHERE database_name = @dbName AND type = 'D'
        )
        BEGIN
            -- 3. Tạo đường dẫn file riêng cho từng DB: LogChain_TenDB.trn
            SET @finalPath = @baseBackupPath + 'LogChain_' + @dbName + '.trn';
            
            -- 4. Tiến hành gom Log
            SET @sql = 'BACKUP LOG [' + @dbName + '] TO DISK = ''' + @finalPath + ''' WITH NOINIT';
            
            BEGIN TRY
                EXEC sp_executesql @sql;
                PRINT 'SUCCESS: Đã gom Log cho DB [' + @dbName + ']';
            END TRY
            BEGIN CATCH
                PRINT 'ERROR: Lỗi khi gom Log cho [' + @dbName + ']: ' + ERROR_MESSAGE();
            END CATCH
        END
        ELSE
        BEGIN
            PRINT 'SKIP: DB [' + @dbName + '] chưa có bản Full Backup, bỏ qua.';
        END

        FETCH NEXT FROM db_cursor INTO @dbName;  
    END  

    CLOSE db_cursor;  
    DEALLOCATE db_cursor;
END