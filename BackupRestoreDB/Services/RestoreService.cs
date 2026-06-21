using System;
using System.Data;
using System.Data.SqlClient;

namespace BackupRestoreDB.Services
{
    /// <summary>
    /// Service phụ trách logic phục hồi database SQL Server.
    /// </summary>
    /// <remarks>
    /// Lý do đặt logic restore trong C# source code thay vì stored procedure:
    /// khi phục hồi, target database bị xóa/sửa — stored procedure sống trong
    /// database đó sẽ bị xóa theo, không thể gọi lại. Đặt SQL trong service C#
    /// đảm bảo logic luôn khả dụng bất kể trạng thái database.
    /// </remarks>
    public class RestoreService
    {
        private readonly string _connectionString;
        private readonly Func<string, DateTime, int> _positionQuery;

        /// <summary>Constructor production — truy vấn SQL Server thật.</summary>
        public RestoreService(string connectionString)
            : this(connectionString, null)
        {
        }

        /// <summary>Constructor cho phép inject query function (dùng trong test).</summary>
        /// <param name="connectionString">SQL Server connection string.</param>
        /// <param name="positionQuery">
        /// Delegate thực thi truy vấn position. Nếu null, dùng truy vấn mặc định
        /// vào msdb.dbo.backupset.
        /// </param>
        public RestoreService(string connectionString, Func<string, DateTime, int> positionQuery)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
            _positionQuery = positionQuery ?? DefaultMaxPositionQuery;
        }

        /// <summary>
        /// Tìm vị trí (position) của bản backup mới nhất có thời điểm thực hiện
        /// TRƯỚC hoặc BẰNG thời điểm target.
        /// </summary>
        /// <param name="dbName">Tên database cần phục hồi.</param>
        /// <param name="targetTime">Thời điểm muốn phục hồi tới.</param>
        /// <returns>
        /// Position lớn nhất trong msdb.dbo.backupset thỏa điều kiện;
        /// trả về 0 nếu không tìm thấy bản backup nào trước thời điểm target.
        /// </returns>
        /// <remarks>
        /// Position 0 không phải là một vị trí backup hợp lệ trong SQL Server —
        /// giá trị này được dùng làm "không tìm thấy" để caller xử lý lỗi rõ ràng.
        /// Lọc type IN ('D','L') nghĩa là chỉ tính Full Backup (D) và Log Backup (L),
        /// bỏ qua Differential Backup (I) — đơn giản hóa theo yêu cầu đồ án.
        /// </remarks>
        public int GetMaxPositionBefore(string dbName, DateTime targetTime)
        {
            if (string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentException("Ten database khong duoc rong.", nameof(dbName));

            return _positionQuery(dbName, targetTime);
        }

        /// <summary>Truy vấn thật vào msdb.dbo.backupset — closure capture _connectionString.</summary>
        private int DefaultMaxPositionQuery(string dbName, DateTime targetTime)
        {
            // Chuẩn hoá kết nối về master vì msdb thuộc server-level metadata
            var builder = new SqlConnectionStringBuilder(_connectionString) { InitialCatalog = "master" };

            const string sql = @"
                SELECT ISNULL(MAX(position), 0)
                FROM msdb.dbo.backupset
                WHERE database_name = @dbName
                  AND backup_start_date <= @targetTime
                  AND type IN ('D', 'L');";

            using (var conn = new SqlConnection(builder.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@dbName", SqlDbType.NVarChar, 128).Value = dbName;
                cmd.Parameters.Add("@targetTime", SqlDbType.DateTime).Value = targetTime;

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }
    }
}
