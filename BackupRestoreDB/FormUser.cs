using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using BackupRestoreDB.Services;

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
        private RestoreService restoreService;

        ToolStripButton btnBackupDB;
        ToolStripButton btnRestoreDB;
        ToolStripButton btnCreateDevice;
        ToolStripButton btnExit;

        public BackupRestoreDB(string cnn)
        {
            this.connectionString = cnn;
            this.restoreService = new RestoreService(cnn);
            InitializeComponent();
            initializeUI();
            loadDanhSachDB();
        }

        private void initializeUI()
        {
            // MAU SAC CHU DAO
            Color primaryColor = Color.FromArgb(30, 58, 95);      // Dark blue header
            Color accentColor = Color.FromArgb(41, 128, 185);    // Light blue accent
            Color whiteColor = Color.White;
            Color lightGray = Color.FromArgb(245, 245, 245);
            Color textDark = Color.FromArgb(52, 73, 94);

            // FONT CHUNG
            Font segoeUI = new Font("Segoe UI", 9F);
            Font segoeUIBold = new Font("Segoe UI", 9F, FontStyle.Bold);

            // ===== TOOLSTRIP BUTTONS =====
            btnBackupDB = new ToolStripButton("Sao luu");
            btnBackupDB.Click += btnBackupDB_Click;
            btnBackupDB.ForeColor = whiteColor;
            btnBackupDB.Font = segoeUIBold;
            btnBackupDB.Padding = new Padding(8, 0, 8, 0);
            toolStripMain.Items.Add(btnBackupDB);

            toolStripMain.Items.Add(new ToolStripSeparator() { Margin = new Padding(4, 0, 4, 0) });

            btnRestoreDB = new ToolStripButton("Phuc hoi");
            btnRestoreDB.Click += btnRestore_Click;
            btnRestoreDB.ForeColor = whiteColor;
            btnRestoreDB.Font = segoeUIBold;
            btnRestoreDB.Padding = new Padding(8, 0, 8, 0);
            toolStripMain.Items.Add(btnRestoreDB);

            toolStripMain.Items.Add(new ToolStripSeparator() { Margin = new Padding(4, 0, 4, 0) });

            btnCreateDevice = new ToolStripButton("Tao Device");
            btnCreateDevice.Click += btnCreateDevice_Click;
            btnCreateDevice.ForeColor = whiteColor;
            btnCreateDevice.Font = segoeUIBold;
            btnCreateDevice.Padding = new Padding(8, 0, 8, 0);
            toolStripMain.Items.Add(btnCreateDevice);

            toolStripMain.Items.Add(new ToolStripSeparator() { Margin = new Padding(4, 0, 4, 0) });

            // CHECKBOX THOI GIAN
            chkTime = new CheckBox()
            {
                Text = "Phuc hoi theo thoi gian",
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = whiteColor,
                Font = segoeUIBold,
                Padding = new Padding(6, 0, 0, 0)
            };
            toolStripMain.Items.Add(new ToolStripControlHost(chkTime));
            chkTime.CheckedChanged += (s, e) => {
                if (pnlThoiGian != null)
                {
                    pnlThoiGian.Visible = chkTime.Checked;
                }
            };

            toolStripMain.Items.Add(new ToolStripSeparator() { Margin = new Padding(4, 0, 4, 0) });

            btnExit = new ToolStripButton("Thoat");
            btnExit.Click += BtnExit_Click;
            btnExit.ForeColor = Color.FromArgb(231, 76, 60);
            btnExit.Font = segoeUIBold;
            btnExit.Padding = new Padding(8, 0, 8, 0);
            toolStripMain.Items.Add(btnExit);

            // ===== CHECKBOX XOA BACKUP CU =====
            chkDeleteOldBackups = new CheckBox()
            {
                Text = "Xoa cac ban sao luu cu truoc khi sao luu moi",
                AutoSize = true,
                BackColor = lightGray,
                ForeColor = textDark,
                Font = segoeUI,
                Location = new Point(20, 12)
            };

            // ===== PANEL THOI GIAN PHUC HOI =====
            pnlThoiGian = new Panel()
            {
                Location = new Point(0, 42),
                Size = new Size(800, 48),
                Visible = false,
                BackColor = lightGray
            };

            Label lblText = new Label()
            {
                Text = "Ngay & Gio phuc hoi:",
                Location = new Point(20, 14),
                AutoSize = true,
                Font = segoeUIBold,
                ForeColor = textDark
            };

            dtpNgay = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(160, 10),
                Width = 120,
                Font = segoeUI,
                CalendarForeColor = textDark
            };

            dtpGio = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Location = new Point(290, 10),
                Width = 100,
                Font = segoeUI
            };

            Label lblHuongDan = new Label()
            {
                Text = "He thong tu dong chon ban backup moi nhat truoc thoi diem ban nhap. Thoi diem phai TRUOC hien tai it nhat 1 phut.",
                Location = new Point(410, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.FromArgb(127, 140, 141)
            };

            pnlThoiGian.Controls.Add(lblText);
            pnlThoiGian.Controls.Add(dtpNgay);
            pnlThoiGian.Controls.Add(dtpGio);
            pnlThoiGian.Controls.Add(lblHuongDan);

            // ===== PANEL CHUA CONTROLS PHIA DUOI =====
            Panel pnlRightBottom = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 93,
                BackColor = lightGray
            };

            pnlRightBottom.Controls.Add(chkDeleteOldBackups);
            pnlRightBottom.Controls.Add(pnlThoiGian);

            splitContainer1.Panel2.Controls.Add(pnlRightBottom);

            // ===== DATAGRIDVIEW BEN TRAI =====
            dgvDatabases.AllowUserToAddRows = false;
            dgvDatabases.ReadOnly = true;
            dgvDatabases.BackgroundColor = lightGray;
            dgvDatabases.RowHeadersVisible = false;
            dgvDatabases.Columns.Add("dbName", "Co so du lieu");
            dgvDatabases.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDatabases.Columns[0].HeaderCell.Style.Font = segoeUIBold;
            dgvDatabases.Columns[0].DefaultCellStyle.Font = segoeUI;

            // ===== DATAGRIDVIEW BEN PHAI =====
            dgvBackups.AllowUserToAddRows = false;
            dgvBackups.ReadOnly = true;
            dgvBackups.BackgroundColor = whiteColor;
            dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBackups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBackups.RowHeadersWidth = 25;
            dgvBackups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBackups.Height = 210;
            dgvBackups.SelectionChanged += DgvBackups_SelectionChanged;

            // Tuy chinh header style cho dgvBackups
            foreach (DataGridViewColumn col in dgvBackups.Columns)
            {
                col.HeaderCell.Style.Font = segoeUIBold;
                col.DefaultCellStyle.Font = segoeUI;
            }
        }

        private void DgvBackups_SelectionChanged(object sender, EventArgs e)
        {
            // Kiem tra xem co dong nao dang duoc chon khong
            if (dgvBackups.CurrentRow != null && dgvBackups.CurrentRow.Index >= 0)
            {
                // Truy xuat dong hien tai
                DataGridViewRow row = dgvBackups.CurrentRow;


                var cellValue = row.Cells["Ban sao luu thu"].Value;

                if (cellValue != null)
                {
                    lblBackupCount.Text = cellValue.ToString();
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Ban co chan chan muon dang xuat va quay lai trang Login?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            // 1. Rang buoc: phai chon CSDL tu danh sach ben trai
            if (dgvDatabases.CurrentRow == null || dgvDatabases.CurrentRow.Cells[0].Value == null)
            {
                MessageBox.Show("Vui long chon co so du lieu o danh sach ben trai!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenDB = dgvDatabases.CurrentRow.Cells[0].Value.ToString();

            // 1.1. Validate database name - chi cho phep ky tu an toan
            if (!Regex.IsMatch(tenDB, @"^[a-zA-Z0-9_]+$"))
            {
                MessageBox.Show("Ten database khong hop le! Chi cho phep chu cai, so va dau gach duoi.", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. Xac dinh position dua theo che do phuc hoi
            //    - PIT mode: tu dong query max(position) truoc thoi diem T
            //    - Non-PIT: user chon tu grid
            int position;
            DateTime? stopAt = null;

            if (chkTime.Checked)
            {
                // === Che do Point-in-Time Recovery ===
                stopAt = dtpNgay.Value.Date + dtpGio.Value.TimeOfDay;

                // Validate: thoi diem phuc hoi phai TRUOC thoi diem hien tai it nhat 1 phut
                DateTime minAllowedTime = DateTime.Now.AddMinutes(-1);
                if (stopAt.Value >= minAllowedTime)
                {
                    MessageBox.Show($"Thoi diem phuc hoi phai TRUOC thoi diem hien tai it nhat 1 phut!\n\n" +
                                   $"• Thoi diem ban nhap: {stopAt.Value:dd/MM/yyyy HH:mm:ss}\n" +
                                   $"• Thoi diem hien tai: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                                   "Loi thoi gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tu dong tim max(position) trong msdb.dbo.backupset co backup_start_date <= T
                try
                {
                    position = restoreService.GetMaxPositionBefore(tenDB, stopAt.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Khong the truy van backup history:\n" + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (position == 0)
                {
                    MessageBox.Show($"Khong co ban backup nao truoc thoi diem {stopAt.Value:dd/MM/yyyy HH:mm:ss}!\n\n" +
                                   "Vui long chay Full Backup truoc hoac chon thoi diem khac.",
                                   "Khong tim thay backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                // === Che do Restore thuong ===
                if (dgvBackups.CurrentRow == null)
                {
                    MessageBox.Show("Vui long chon mot ban sao luu tu bang ben phai!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                position = Convert.ToInt32(dgvBackups.CurrentRow.Cells[0].Value);
            }

            string deviceName = "DEVICE_" + tenDB;

            // 3. Build safe path - tran path traversal attack
            string logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQLBackup", "Log");
            Directory.CreateDirectory(logFolder);
            string logPath = Path.Combine(logFolder, $"Tail_{tenDB}.trn");

            // 4. Xac nhan voi nguoi dung
            string confirmMsg = stopAt.HasValue
                ? $"Xac nhan phuc hoi database [{tenDB}] ve thoi diem {stopAt.Value:dd/MM/yyyy HH:mm:ss}?"
                : $"Xac nhan phuc hoi database [{tenDB}]?";
            if (MessageBox.Show(confirmMsg, "Xac nhan", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            // 5. QUAN TRONG: Ket noi vao database 'master' de thuc hien phuc hoi
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master";

            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Buoc 1 & 2: Ngat ket noi nguoi dung va Backup Log duoi (Tail-Log)
                    // NORECOVERY dua DB vao trang thai cho phuc hoi, ngat moi ket noi moi
                    string sqlTailLog = $@"
                ALTER DATABASE [{tenDB}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                BACKUP LOG [{tenDB}] TO DISK = '{logPath}' WITH INIT, NORECOVERY;";
                    new SqlCommand(sqlTailLog, conn).ExecuteNonQuery();

                    // Buoc 3: Phuc hoi ban Full Backup (Diem xuat phat)
                    // NORECOVERY: Giu DB o trang thai cho de co the nap tiep file Log
                    string sqlRestoreFull = $@"RESTORE DATABASE [{tenDB}] FROM [{deviceName}] WITH FILE = {position}, REPLACE, NORECOVERY;";
                    new SqlCommand(sqlRestoreFull, conn).ExecuteNonQuery();

                    // Buoc 4: Phuc hoi Log (Nap cac hanh dong tu file Log vua backup)
                    string sqlRestoreLog;
                    if (stopAt.HasValue)
                    {
                        // STOPAT: Thuc hien lai cac hanh dong trong Log nhung dung lai dung thoi diem t
                        sqlRestoreLog = $@"RESTORE LOG [{tenDB}] FROM DISK = '{logPath}' WITH STOPAT = '{stopAt.Value:yyyy-MM-dd HH:mm:ss}', RECOVERY;";
                    }
                    else
                    {
                        // RECOVERY: Hoan tat qua trinh va dua Database hoat dong tro lai
                        sqlRestoreLog = $@"RESTORE LOG [{tenDB}] FROM DISK = '{logPath}' WITH RECOVERY;";
                    }
                    new SqlCommand(sqlRestoreLog, conn).ExecuteNonQuery();

                    // Buoc 5: Chuyen lai database sang che do hoat dong binh thuong
                    new SqlCommand($"ALTER DATABASE [{tenDB}] SET MULTI_USER;", conn).ExecuteNonQuery();

                    MessageBox.Show("Khoi phuc du lieu thanh cong!", "Thong bao");
                }
                catch (Exception ex)
                {
                    // Co gang mo lai quyen truy cap neu xay ra loi giua chung
                    try { new SqlCommand($"ALTER DATABASE [{tenDB}] SET MULTI_USER;", conn).ExecuteNonQuery(); } catch { }
                    MessageBox.Show("Loi: " + ex.Message, "Loi Server");
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
                MessageBox.Show("Loi ket noi SQL Server:\n" + ex.Message, "Bao loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBackupDB_Click(object sender, EventArgs e) {
            if(dgvDatabases.CurrentRow == null || dgvDatabases.CurrentRow.Cells[0].Value == null)
            {
                MessageBox.Show("Vui long chon CSDL de sao luu!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tenDB = dgvDatabases.CurrentRow.Cells[0].Value.ToString();
            if (string.IsNullOrEmpty(tenDB) || tenDB.Contains("...")) return;
            string deviceName = "DEVICE_" + tenDB;
            string dienGiai = "Ban sao luu tao luc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

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
                    MessageBox.Show("Sao luu co so du lieu [" + tenDB + "] thanh cong!", "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadLichSuSaoLuu(tenDB);

                    // Tu dong tao Task Scheduler cho backup log sau khi backup thanh cong
                    CreateBackupLogScheduledTask();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Loi khi sao luu:\n" + ex.Message, "Bao loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tu dong tao Windows Task Scheduler de backup log dinh ky sau khi co Full Backup
        /// Chu y: Can chay app voi quyen Administrator de tao Task Scheduler thanh cong
        /// </summary>
        private void CreateBackupLogScheduledTask()
        {
            const string taskName = "BackupRestoreDB_AutoBackupLog";
            const string taskDescription = "Tu dong backup log dinh ky - tao boi BackupRestoreDB app";

            try
            {
                // Buoc 1: Tim duong dan sqlcmd
                string sqlcmdPath = FindSqlcmdPath();
                if (string.IsNullOrEmpty(sqlcmdPath))
                {
                    // Neu khong tim thay sqlcmd, thong bao nhung khong chap loi
                    System.Diagnostics.Debug.WriteLine("Khong tim thay sqlcmd. Task Scheduler khong duoc tao.");
                    return;
                }

                // Buoc 2: Trich xuat server name tu connectionString
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                string serverName = builder.DataSource;
                string additionalParams = "";

                // Neu la Windows Auth thi khong can user/password
                if (!builder.IntegratedSecurity)
                {
                    // SQL Auth - them tham so tuong ung
                    additionalParams = $"-U {builder.UserID} -P {builder.Password}";
                }

                // Buoc 3: Xoa task cu neu co (de update lai)
                DeleteScheduledTask(taskName);

                // Buoc 4: Tao task moi
                // Chu y: /RL HIGH = chay voi quyen cao nhat
                string arguments = $"/create /tn \"{taskName}\" " +
                                   $"/tr \"\\\"{sqlcmdPath}\\\" {additionalParams} -S {serverName} -d master -Q \\\"EXEC sp_JobBackupLog\\\"\" " +
                                   $"/sc minute /mo 15 " +  // Chay moi 15 phut
                                   $"/f"; // Ghi de neu da ton tai

                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "schtasks.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Task Scheduler '{taskName}' da tao thanh cong.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Loi tao Task Scheduler: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Khong hien thi loi vi task scheduler la tinh nang phu
                System.Diagnostics.Debug.WriteLine($"Loi khi tao Task Scheduler: {ex.Message}");
            }
        }

        /// <summary>
        /// Tim duong dan sqlcmd.exe tren may
        /// </summary>
        private string FindSqlcmdPath()
        {
            // Thu tim trong PATH
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
            foreach (string path in pathEnv.Split(';'))
            {
                string sqlcmdPath = Path.Combine(path.Trim(), "sqlcmd.exe");
                if (File.Exists(sqlcmdPath))
                {
                    return sqlcmdPath;
                }
            }

            // Thu cac duong dan pho bien cua sqlcmd
            string[] commonPaths = new string[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    @"Microsoft SQL Server\Client SDK\SQLServer2012\Tools\Binn\sqlcmd.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    @"Microsoft SQL Server\90\Tools\Binn\sqlcmd.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\Client SDK\SQLServer2012\Tools\Binn\sqlcmd.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\110\Tools\Binn\sqlcmd.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\120\Tools\Binn\sqlcmd.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\130\Tools\Binn\sqlcmd.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\140\Tools\Binn\sqlcmd.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\150\Tools\Binn\sqlcmd.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\160\Tools\Binn\sqlcmd.exe")
            };

            foreach (string path in commonPaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        /// <summary>
        /// Xoa Task Scheduler neu ton tai
        /// </summary>
        private void DeleteScheduledTask(string taskName)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "schtasks.exe",
                    Arguments = $"/delete /tn \"{taskName}\" /f",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi))
                {
                    process.WaitForExit();
                }
            }
            catch
            {
                // Bo qua loi neu khong xoa duoc
            }
        }
        private void btnCreateDevice_Click(object sender, EventArgs e)
        {
            string tenDB = lblDBName.Text;
            if(string.IsNullOrEmpty(tenDB) || tenDB.Contains("..."))
            {
                MessageBox.Show("Vui long chon co so du lieu o danh sach ben trai!","Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string deviceName = "DEVICE_" + tenDB;
            string thuMucBackup = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQLBackup", "Full");
            if(!Directory.Exists(thuMucBackup))
            {
                Directory.CreateDirectory(thuMucBackup);
            }

            string physicalPath = Path.Combine(thuMucBackup, deviceName + ".bak");

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
                        MessageBox.Show("Device cho CSDL [" + tenDB + "] da ton tai roi! Khong can tao lai.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string createQuery = "EXEC sp_addumpdevice  'disk', @devName, @path";
                    SqlCommand cmdCreate = new SqlCommand(createQuery, conn);
                    cmdCreate.Parameters.AddWithValue("@devName", deviceName);
                    cmdCreate.Parameters.AddWithValue("@path", physicalPath);
                    cmdCreate.ExecuteNonQuery();

                    MessageBox.Show("Tao Device thanh cong! Tra Device ve: " + physicalPath, "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Loi khi tao Device:\n" + ex.Message, "Bao loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    lblBackupCount.Text = dgvBackups.Rows[0].Cells["Ban sao luu thu"].Value.ToString();
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
                MessageBox.Show("Khong the tai lich su sao luu:\n" + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
