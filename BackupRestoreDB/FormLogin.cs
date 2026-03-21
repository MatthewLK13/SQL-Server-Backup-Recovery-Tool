using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupRestoreDB
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            cboAuthentication.SelectedIndex = 0;
        }

        private void cboAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAuthentication.Text.ToLower().Contains("windows")) // Windows Auth
            {
                txtUsername.Text = ""; 
                txtPassword.Text = "";
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
            }
            else // SQL Server Auth
            {
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            
            string ConnectionString = "";
            string serverName = txtServerName.Text.Trim();
            if (string.IsNullOrEmpty(serverName))
            {
                MessageBox.Show("Hãy nhập tên Server", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboAuthentication.SelectedIndex == 0)
                ConnectionString = $"Data Source={txtServerName.Text};Initial Catalog=master;Integrated Security=True;";
            else
                ConnectionString = $"Data Source={txtServerName.Text};Initial Catalog=master;User ID={txtUsername.Text};Password={txtPassword.Text};";

           
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open(); 

                    SqlCommand cmdTest = new SqlCommand("SELECT SUSER_SNAME()", conn);
                    string thanPhanHienTai = cmdTest.ExecuteScalar().ToString();

                    MessageBox.Show("Đăng nhập thành công!\nSQL Server nhận diện bạn là: " + thanPhanHienTai,
                                    "Kiểm tra đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BackupRestoreDB frmMain = new BackupRestoreDB(ConnectionString);

                    this.Hide();
                    frmMain.ShowDialog();

                    txtPassword.Text = "";
                    txtPassword.Focus();

                    this.Show(); 
                }
            }
            catch (Exception ex)
            {
                // Nếu sai pass
                MessageBox.Show("Đăng nhập thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    }

