ALTER PROCEDURE sp_BackupDatabase
    @dbName NVARCHAR(100),      -- Tên CSDL cần sao lưu
    @deviceName NVARCHAR(100),  -- Tên Device đã tạo 
    @dienGiai NVARCHAR(200),    -- Mô tả bản sao lưu 
    @ghiDe BIT                  -- 1 là Xóa hết bản cũ (INIT), 0 là Nối tiếp (NOINIT)
AS
BEGIN
    DECLARE @sqlQuery NVARCHAR(MAX);

    -- @ghiDe 
    IF @ghiDe = 1
    BEGIN
		EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = @dbName;
        -- Ghi đè (INIT): Xóa sạch các bản backup cũ trong file, chỉ giữ lại bản mới này
        SET @sqlQuery = 'BACKUP DATABASE [' + @dbName + '] TO [' + @deviceName + '] WITH INIT, NAME = N''' + @dienGiai + '''';
    END
    ELSE
    BEGIN
        -- Nối tiếp (NOINIT): Giữ nguyên các bản cũ, nhét thêm bản mới này vào cuối file
        SET @sqlQuery = 'BACKUP DATABASE [' + @dbName + '] TO [' + @deviceName + '] WITH NOINIT, NAME = N''' + @dienGiai + '''';
    END

    -- Thực thi câu lệnh
    EXEC (@sqlQuery);
END
