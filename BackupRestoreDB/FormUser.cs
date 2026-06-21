using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace BackupRestoreDB
{
    public partial class BackupRestoreDB : Form
    {
        private Panel pnlThoiGian;
        private DateTimePicker dtpNgay;
        private DateTimePicker dtpGio;
        private CheckBox chkTime;
        private CheckBox chkDeleteOldBackups;
        private string connectionString;

        ToolStripButton btnBackupDB;
        ToolStripButton btnRestoreDB;
        ToolStripButton btnCreateDevice;
        ToolStripButton btnExit;

        public BackupRestoreDB(string cnn)
        {
            this.connectionString = cnn;
            InitializeComponent();
            initializeUI();
            loadDanhSachDB();
        }

        private void initializeUI()
        {
            // CÁC NÚT CHO TOOLSTRIP
            btnBackupDB = new ToolStripButton("💿 Sao lưu");
            btnBackupDB.Enabled = true;
            btnBackupDB.Click += btnBackupDB_Click;
            toolStripMain.Items.Add(btnBackupDB);

            toolStripMain.Items.Add(new ToolStripSeparator());
            btnRestoreDB = new ToolStripButton("💾 Phục hồi");
            btnRestoreDB.Click += btnRestore_Click;
            toolStripMain.Items.Add(btnRestoreDB);

            btnCreateDevice = new ToolStripButton("💾 Tạo device sao lưu");
            btnCreateDevice.Enabled = true;
            btnCreateDevice.Click += btnCreateDevice_Click;
            toolStripMain.Items.Add(btnCreateDevice);

            toolStripMain.Items.Add(new ToolStripSeparator());

            chkTime = new CheckBox() { Text = "⏰ Tham số phục hồi theo thời gian", AutoSize = true, BackColor = Color.Transparent };
            toolStripMain.Items.Add(new ToolStripControlHost(chkTime));
            chkTime.CheckedChanged += (s, e) => {
                if (pnlThoiGian != null)
                {
                    pnlThoiGian.Visible = chkTime.Checked; 
                }
            };

            btnExit = new ToolStripButton("❌ Thoát");
            btnBackupDB.Enabled = true;
            btnExit.Click += BtnExit_Click;
            toolStripMain.Items.Add(btnExit);

            chkDeleteOldBackups = new CheckBox();
            chkDeleteOldBackups.Text = "Xóa tất cả các bản sao lưu cũ trong File trước khi sao lưu bản mới";
            chkDeleteOldBackups.AutoSize = true;
            chkDeleteOldBackups.Location = new Point(150, 10);

            pnlThoiGian = new Panel();
            pnlThoiGian.Location = new Point(0, 40); // Tọa độ đẩy nó xuống dưới cái Checkbox phía trên
            pnlThoiGian.Size = new Size(800, 80);
            pnlThoiGian.Visible = false; // MẶC ĐỊNH 
            Label lblText = new Label() { Text = "Ngày giờ để phục hồi tới thời điểm đó", Location = new Point(150, 5), AutoSize = true };
            dtpNgay = new DateTimePicker() { Format = DateTimePickerFormat.Short, Location = new Point(370, 2), Width = 100 };
            dtpGio = new DateTimePicker() { Format = DateTimePickerFormat.Time, ShowUpDown = true, Location = new Point(480, 2), Width = 100 };

            Label lblHuongDan = new Label();
            lblHuongDan.Text = "Hướng dẫn: Ngày giờ ta nhập vào là thời điểm ta muốn phục hồi cơ sở dữ liệu\nvề đó. Thời điểm này phải sau thời điểm của bản sao lưu mà ta đã\nchọn trên lưới, và trước thời điểm hiện tại ít nhất là 1 phút";
            lblHuongDan.Location = new Point(150, 30);
            lblHuongDan.AutoSize = true;
            lblHuongDan.BorderStyle = BorderStyle.FixedSingle;

            // Lắp ráp các chi tiết con vào pnlThoiGian
            pnlThoiGian.Controls.Add(lblText);
            pnlThoiGian.Controls.Add(dtpNgay);
            pnlThoiGian.Controls.Add(dtpGio);
            pnlThoiGian.Controls.Add(lblHuongDan);

            

            Panel pnlRightBottom = new Panel();
            pnlRightBottom.Dock = DockStyle.Bottom;
            pnlRightBottom.Height = 130; 
            pnlRightBottom.BackColor = Color.White;

            pnlRightBottom.Controls.Add(chkDeleteOldBackups);
            pnlRightBottom.Controls.Add(pnlThoiGian);

            splitContainer1.Panel2.Controls.Add(pnlRightBottom);

            // CÁC CỘT CHO BẢNG BÊN TRÁI
            dgvDatabases.AllowUserToAddRows = false;
            dgvDatabases.ReadOnly = true;
            dgvDatabases.BackgroundColor = Color.White;
            dgvDatabases.RowHeadersVisible = false;
            dgvDatabases.Columns.Add("dbName", "Cơ sở dữ liệu");
            dgvDatabases.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // TẠO NHANH VÀ CĂN CHỈNH KÍCH THƯỚC CỘT BẢNG BÊN PHẢI
            dgvBackups.AllowUserToAddRows = false;
            dgvBackups.ReadOnly = true;
            dgvBackups.BackgroundColor = Color.White;
            dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBackups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvBackups.RowHeadersWidth = 25;
            dgvBackups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBackups.Height = 210;
            dgvBackups.SelectionChanged += DgvBackups_SelectionChanged;
        }

        private void DgvBackups_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào đang được chọn không
            if (dgvBackups.CurrentRow != null && dgvBackups.CurrentRow.Index >= 0)
            {
                // Truy xuất dòng hiện tại
                DataGridViewRow row = dgvBackups.CurrentRow;

        
                var cellValue = row.Cells["Bản sao lưu thứ"].Value;

                if (cellValue != null)
                {
                    lblBackupCount.Text = cellValue.ToString();
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất và quay lại trang Login?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close(); 
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            // 1. Ràng buộc: phải chọn CSDL từ danh sách bên trái
            if (dgvDatabases.CurrentRow == null || dgvDatabases.CurrentRow.Cells[0].Value == null)
            {
                MessageBox.Show("Vui lòng chọn cơ sở dữ liệu ở danh sách bên trái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenDB = dgvDatabases.CurrentRow.Cells[0].Value.ToString();
            string deviceName = "DEVICE_" + tenDB;
            string logPath = $@"C:\SQLBackup\Log\Tail_{tenDB}.trn";

            int position;
            DateTime? stopAt = null;

            // 2. Xác định position và chế độ phục hồi
            if (chkTime.Checked)
            {
                // === Chế độ Point-in-Time Recovery ===
                stopAt = dtpNgay.Value.Date + dtpGio.Value.TimeOfDay;

                // Validate: thời điểm phục hồi phải TRƯỚC thời điểm hiện tại ít nhất 1 phút
                if (stopAt.Value >= DateTime.Now.AddMinutes(-1))
                {
                    MessageBox.Show("Thời điểm phục hồi phải TRƯỚC thời điểm hiện tại ít nhất 1 phút!", "Lỗi thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tự động tìm bản backup mới nhất có backup_start_date <= thời điểm target
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra recovery model của database
                    string queryRecovery = @"
                        SELECT recovery_model_desc
                        FROM sys.databases
                        WHERE name = @dbName";
                    SqlCommand cmdRecovery = new SqlCommand(queryRecovery, conn);
                    cmdRecovery.Parameters.AddWithValue("@dbName", tenDB);
                    string recoveryMode = cmdRecovery.ExecuteScalar().ToString();

                    if (recoveryMode != "FULL")
                    {
                        MessageBox.Show($"Database [{tenDB}] đang ở chế độ [{recoveryMode}].\n" +
                                       "Để phục hồi theo thời gian, database cần ở chế độ FULL.\n" +
                                       "Vui lòng chuyển sang FULL recovery model hoặc chọn chế độ phục hồi thông thường.",
                                       "Lỗi recovery mode", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string queryMaxPos = @"
                        SELECT ISNULL(MAX(position), 0)
                        FROM msdb.dbo.backupset
                        WHERE database_name = @dbName
                          AND backup_start_date <= @targetTime
                          AND type IN ('D', 'L')";
                    SqlCommand cmdMax = new SqlCommand(queryMaxPos, conn);
                    cmdMax.Parameters.AddWithValue("@dbName", tenDB);
                    cmdMax.Parameters.AddWithValue("@targetTime", stopAt.Value);
                    position = Convert.ToInt32(cmdMax.ExecuteScalar());
                }

                if (position == 0)
                {
                    MessageBox.Show($"Không có bản backup nào trước thời điểm {stopAt.Value:dd/MM/yyyy HH:mm:ss}!\nVui lòng chạy Full Backup trước hoặc chọn thời điểm khác.", "Không tìm thấy backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                // === Chế độ restore thông thường: yêu cầu user chọn bản sao lưu từ lưới ===
                if (dgvBackups.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn một bản sao lưu từ danh sách bên phải!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                position = Convert.ToInt32(dgvBackups.CurrentRow.Cells[0].Value);
            }

            if (MessageBox.Show($"Xác nhận phục hồi database [{tenDB}]?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            // QUAN TRỌNG: Kết nối vào database 'master' để thực hiện phục hồi
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master";

            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Bước 1 & 2: Ngắt kết nối người dùng và Backup Log đuôi (Tail-Log)
                    // NORECOVERY đưa DB vào trạng thái chờ phục hồi, ngắt mọi kết nối mới
                    string sqlTailLog = $@"
                ALTER DATABASE [{tenDB}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                BACKUP LOG [{tenDB}] TO DISK = '{logPath}' WITH INIT, NORECOVERY;";
                    new SqlCommand(sqlTailLog, conn).ExecuteNonQuery();

                    // Bước 3: Phục hồi bản Full Backup (Điểm xuất phát)
                    // NORECOVERY: Giữ DB ở trạng thái chờ để có thể nạp tiếp file Log
                    string sqlRestoreFull = $@"RESTORE DATABASE [{tenDB}] FROM [{deviceName}] WITH FILE = {position}, REPLACE, NORECOVERY;";
                    new SqlCommand(sqlRestoreFull, conn).ExecuteNonQuery();

                    // Bước 4: Phục hồi Log (Nạp các hành động từ file Log vừa backup)
                    string sqlRestoreLog;
                    if (stopAt.HasValue)
                    {
                        // STOPAT: Thực hiện lại các hành động trong Log nhưng dừng lại đúng thời điểm t
                        sqlRestoreLog = $@"RESTORE LOG [{tenDB}] FROM DISK = '{logPath}' WITH STOPAT = '{stopAt.Value:yyyy-MM-dd HH:mm:ss}', RECOVERY;";
                    }
                    else
                    {
                        // RECOVERY: Hoàn tất quá trình và đưa Database hoạt động trở lại
                        sqlRestoreLog = $@"RESTORE LOG [{tenDB}] FROM DISK = '{logPath}' WITH RECOVERY;";
                    }
                    new SqlCommand(sqlRestoreLog, conn).ExecuteNonQuery();

                    // Bước 5: Chuyển lại database sang chế độ hoạt động bình thường
                    new SqlCommand($"ALTER DATABASE [{tenDB}] SET MULTI_USER;", conn).ExecuteNonQuery();

                    MessageBox.Show("Khôi phục dữ liệu thành công!", "Thông báo");
                }
                catch (Exception ex)
                {
                    // Cố gắng mở lại quyền truy cập nếu xảy ra lỗi giữa chừng
                    try { new SqlCommand($"ALTER DATABASE [{tenDB}] SET MULTI_USER;", conn).ExecuteNonQuery(); } catch { }
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi Server");
                }
            }
        }



        private void loadDanhSachDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT name AS dbName FROM sys.databases WHERE database_id > 4 ORDER BY name";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvDatabases.AutoGenerateColumns = false;
                    dgvDatabases.Columns["dbName"].DataPropertyName = "dbName";
                    dgvDatabases.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối SQL Server:\n" + ex.Message, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBackupDB_Click(object sender, EventArgs e) {
            if(dgvDatabases.CurrentRow == null || dgvDatabases.CurrentRow.Cells[0].Value == null)
            {
                MessageBox.Show("Vui lòng chọn CSDL để sao lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tenDB = dgvDatabases.CurrentRow.Cells[0].Value.ToString();
            if (string.IsNullOrEmpty(tenDB) || tenDB.Contains("...")) return;
            string deviceName = "DEVICE_" + tenDB;
            string dienGiai = "Bản sao lưu tạo lúc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            int ghiDe = chkDeleteOldBackups.Checked ? 1 : 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_BackupDatabase", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@dbName", tenDB);
                    cmd.Parameters.AddWithValue("@deviceName", deviceName);
                    cmd.Parameters.AddWithValue("@dienGiai", dienGiai);
                    cmd.Parameters.AddWithValue("@ghiDe", ghiDe);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sao lưu cơ sở dữ liệu [" + tenDB + "] thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadLichSuSaoLuu(tenDB);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi khi sao lưu:\n" + ex.Message, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCreateDevice_Click(object sender, EventArgs e)
        {
            string tenDB = lblDBName.Text;
            if(string.IsNullOrEmpty(tenDB) || tenDB.Contains("..."))
            {
                MessageBox.Show("Vui lòng chọn cơ sở dữ liệu ở danh sách bên trái!","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string deviceName = "DEVICE_" + tenDB;
            string thuMucBackup = @"c:\SQLBackup\Full";
            if(!System.IO.Directory.Exists(thuMucBackup))
            {
                System.IO.Directory.CreateDirectory(thuMucBackup);
            }

            string physicalPath = thuMucBackup + @"\" + deviceName + ".bak";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM sys.backup_devices WHERE name = @devName";
                    SqlCommand cmdCheck = new SqlCommand(checkQuery, conn);
                    cmdCheck.Parameters.AddWithValue("@devName", deviceName);

                    if((int)cmdCheck.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Device cho CSDL [" + tenDB + "] đã tồn tại rồi! Không cần tạo lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string createQuery = "EXEC sp_addumpdevice  'disk', @devName, @path";
                    SqlCommand cmdCreate = new SqlCommand(createQuery, conn);
                    cmdCreate.Parameters.AddWithValue("@devName", deviceName);
                    cmdCreate.Parameters.AddWithValue("@path", physicalPath);
                    cmdCreate.ExecuteNonQuery();

                    MessageBox.Show("Tạo Device thành công! Trả Device về: " + physicalPath, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo Device:\n" + ex.Message, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dgvDatabases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDatabases.CurrentRow != null && dgvDatabases.CurrentRow.Cells[0].Value != null)
            {
                string tenDB = dgvDatabases.CurrentRow.Cells[0].Value.ToString();
                lblDBName.Text = tenDB;
                loadLichSuSaoLuu(tenDB);

                if (dgvBackups.Rows.Count > 0)
                {
                    lblBackupCount.Text = dgvBackups.Rows[0].Cells["Bản sao lưu thứ"].Value.ToString();
                }
                else
                {
                    lblBackupCount.Text = "N/A";
                }
            }
        }

        private void loadLichSuSaoLuu(string dbName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                   


                    SqlCommand cmd = new SqlCommand("sp_BackupList", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dbName", dbName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtHistory = new DataTable();
                    da.Fill(dtHistory);

                    dgvBackups.DataSource = dtHistory;

                    dgvBackups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải lịch sử sao lưu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
       
    }
}
