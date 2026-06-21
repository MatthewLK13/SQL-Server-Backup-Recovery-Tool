using System;
using BackupRestoreDB.Services;
using Xunit;

namespace BackupRestoreDB.Tests
{
    /// <summary>
    /// Unit tests cho RestoreService.GetMaxPositionBefore.
    /// Sử dụng constructor injection để mock SQL query, không cần SQL Server thật.
    /// </summary>
    public class RestoreServiceTests
    {
        // Connection string giả — không được dùng vì ta inject query delegate
        private const string FakeConnectionString = "Data Source=fake;Initial Catalog=master;Integrated Security=True;";

        #region Constructor validation

        [Fact]
        public void Constructor_ThrowArgumentNullException_WhenConnectionStringIsNull()
        {
            // Arrange
            Func<string, DateTime, int> query = (db, t) => 0;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RestoreService(null, query));
        }

        [Fact]
        public void Constructor_AcceptNullQuery_UseDefaultProductionQuery()
        {
            // Act — không ném exception vì null query sẽ fallback về default
            var service = new RestoreService(FakeConnectionString, null);

            // Assert
            Assert.NotNull(service);
        }

        #endregion

        #region GetMaxPositionBefore — input validation

        [Fact]
        public void GetMaxPositionBefore_ThrowArgumentException_WhenDbNameIsNull()
        {
            // Arrange
            var service = new RestoreService(FakeConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.GetMaxPositionBefore(null, DateTime.Now));
        }

        [Fact]
        public void GetMaxPositionBefore_ThrowArgumentException_WhenDbNameIsEmpty()
        {
            var service = new RestoreService(FakeConnectionString);

            Assert.Throws<ArgumentException>(() => service.GetMaxPositionBefore("", DateTime.Now));
        }

        [Fact]
        public void GetMaxPositionBefore_ThrowArgumentException_WhenDbNameIsWhitespace()
        {
            var service = new RestoreService(FakeConnectionString);

            Assert.Throws<ArgumentException>(() => service.GetMaxPositionBefore("   ", DateTime.Now));
        }

        #endregion

        #region GetMaxPositionBefore — happy path với mock query

        [Fact]
        public void GetMaxPositionBefore_ReturnsMaxPosition_WhenBackupExistsBeforeTarget()
        {
            // Arrange
            var targetTime = new DateTime(2026, 6, 21, 10, 0, 0);
            Func<string, DateTime, int> fakeQuery = (db, t) =>
            {
                Assert.Equal("MyDB", db);
                Assert.Equal(targetTime, t);
                return 5; // giả sử position 5 là bản mới nhất
            };
            var service = new RestoreService(FakeConnectionString, fakeQuery);

            // Act
            int position = service.GetMaxPositionBefore("MyDB", targetTime);

            // Assert
            Assert.Equal(5, position);
        }

        [Fact]
        public void GetMaxPositionBefore_ReturnsZero_WhenNoBackupBeforeTarget()
        {
            // Arrange — mock trả về 0 (không có bản backup nào)
            var service = new RestoreService(FakeConnectionString, (db, t) => 0);

            // Act
            int position = service.GetMaxPositionBefore("EmptyDB", DateTime.Now);

            // Assert
            Assert.Equal(0, position);
        }

        [Fact]
        public void GetMaxPositionBefore_PropagatesException_WhenQueryFails()
        {
            // Arrange — mock ném exception giả lập SQL lỗi
            Func<string, DateTime, int> failingQuery = (db, t) =>
                throw new InvalidOperationException("SQL connection failed");
            var service = new RestoreService(FakeConnectionString, failingQuery);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(
                () => service.GetMaxPositionBefore("MyDB", DateTime.Now));
            Assert.Contains("SQL connection failed", ex.Message);
        }

        [Fact]
        public void GetMaxPositionBefore_PassesTargetTimeToQuery()
        {
            // Arrange — verify delegate nhận đúng targetTime
            DateTime expectedTime = new DateTime(2025, 12, 31, 23, 59, 59);
            DateTime actualTime = DateTime.MinValue;
            Func<string, DateTime, int> capturingQuery = (db, t) =>
            {
                actualTime = t;
                return 1;
            };
            var service = new RestoreService(FakeConnectionString, capturingQuery);

            // Act
            service.GetMaxPositionBefore("DB", expectedTime);

            // Assert
            Assert.Equal(expectedTime, actualTime);
        }

        #endregion
    }
}
