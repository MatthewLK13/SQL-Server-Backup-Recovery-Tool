-- =============================================================================
-- KHONG SU DUNG (DEPRECATED) — Logic restore da duoc chuyen vao C# source code
-- File: BackupRestoreDB/Services/RestoreService.cs va FormUser.cs:btnRestore_Click
--
-- Ly do: khi phuc hoi, target database bi xoa/sua — stored procedure song trong
-- database do se bi xoa theo, khong the goi lai. Dat SQL trong service C# dam bao
-- logic luon kha dung bat ke trang thai database.
--
-- File nay duoc giu lai lam tai lieu tham khao cho logic restore ban dau.
-- KHONG chay CREATE PROCEDURE nay tren moi truong production.
-- =============================================================================

-- CREATE OR ALTER PROCEDURE sp_RestoreDatabase
    @dbName NVARCHAR(100),
    @deviceName NVARCHAR(100),
    @position INT,
    @isPointInTime BIT,
    @stopAtTime DATETIME = NULL,
    @logFolderPath NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @sqlQuery NVARCHAR(MAX);
    DECLARE @tempLogFile NVARCHAR(500) = @logFolderPath + 'TailLog_' + @dbName + '.trn';

    -- 1. single user
    IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @dbName)
    BEGIN
        SET @sqlQuery = 'ALTER DATABASE [' + @dbName + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE';
        EXEC (@sqlQuery);

        -- tách log vào 1 file
        IF @isPointInTime = 1
        BEGIN
            BEGIN TRY
                SET @sqlQuery = 'BACKUP LOG [' + @dbName + '] TO DISK = ''' + @tempLogFile + ''' WITH INIT';
                EXEC (@sqlQuery);
            END TRY BEGIN CATCH END CATCH
        END
    END

    --  NORECOVERY cho 2 trường hợp
    SET @sqlQuery = 'RESTORE DATABASE [' + @dbName + '] FROM [' + @deviceName + 
                    '] WITH FILE = ' + CAST(@position AS NVARCHAR(10)) + ', REPLACE, NORECOVERY';
    EXEC (@sqlQuery);

    -- tham số phục hồi
    IF @isPointInTime = 1
    BEGIN
        BEGIN TRY
            -- Nạp tail log
            SET @sqlQuery = 'RESTORE LOG [' + @dbName + '] FROM DISK = ''' + @tempLogFile + 
                            ''' WITH STOPAT = ''' + CONVERT(VARCHAR, @stopAtTime, 120) + ''', RECOVERY';
            EXEC (@sqlQuery);
        END TRY
        BEGIN CATCH
            SET @sqlQuery = 'RESTORE DATABASE [' + @dbName + '] WITH RECOVERY';
            EXEC (@sqlQuery);
        END CATCH
    END
    ELSE
    BEGIN
        -- Phục hồi Full 
        SET @sqlQuery = 'RESTORE DATABASE [' + @dbName + '] WITH RECOVERY';
        EXEC (@sqlQuery);
    END

    -- 4. quay về chế độ cũ
    IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @dbName)
    BEGIN
        SET @sqlQuery = 'ALTER DATABASE [' + @dbName + '] SET MULTI_USER';
        EXEC (@sqlQuery);
    END
END