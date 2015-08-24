using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

using anvlib.Classes;
using anvlib.Crypt;
using anvlib.Interfaces;

namespace anvlib.Forms
{        
    public partial class SqlConnectionForm : Form
    {
        private SqlConnection _conn;

        private bool isCheckconnectionPressed = false;

        private IConnectionFormSettings settings;

        public SqlConnectionForm(IConnectionFormSettings Settings)
        {
            settings = Settings;
            InitializeComponent();
        }

        private void GenerateConnection()
        {
            if (_conn == null)
                _conn = new SqlConnection();

            if (_conn.State == ConnectionState.Open)
                _conn.Close();

            if (SQLServeAuthRB.Checked)
            {
                _conn.ConnectionString = string.Format("Server={0};Uid={1};Pwd={2};database={3};MultipleActiveResultSets=true",
                    ServerName.Text, Login.Text, Password.Text, DBNames.Text);                
            }
            else 
            {
                _conn.ConnectionString = string.Format("Server={0};Integrated Security=SSPI;database={1};MultipleActiveResultSets=true",
                    ServerName.Text, DBNames.Text);
            }
        }

        private bool CheckConnection()
        {
            GenerateConnection();
            if (!string.IsNullOrEmpty(Password.Text))
                settings.SQLPassword = BaseEncryptor<System.Security.Cryptography.DESCryptoServiceProvider>.Encrypt(Password.Text);            
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }            
            _conn.Close();
            settings.SQLServAddr = ServerName.Text;
            settings.IsSQLTypeAuth = SQLServeAuthRB.Checked;
            if (SQLServeAuthRB.Checked)
                settings.SQLLogin = Login.Text;
            else
                settings.SQLLogin = null;
            settings.SQLDBName = DBNames.Text;
            settings.Save();
            isCheckconnectionPressed = true;
            return true;
        }

        public SqlConnection Connection
        {
            get
            {
                if (_conn != null)
                    return _conn;
                
                return null;
            }
        }

        private void CancelB_Click(object sender, EventArgs e)
        {
            if (isCheckconnectionPressed)
            {
                settings.SQLServAddr = "";
                settings.IsSQLTypeAuth = false;
                settings.SQLLogin = "";
                settings.SQLDBName = "";
                settings.Save();
            }
            Close();
        }

        private void TestConnB_Click(object sender, EventArgs e)
        {
            if (CheckConnection())
                MessageBox.Show("Соединение успешно установленно!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SQLServeAuthRB_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Login.Text))
                Login.Text = "";
            if (!string.IsNullOrEmpty(Password.Text))
                Password.Text = "";

            Login.Enabled = true;
            Password.Enabled = true;
        }

        private void WindowsAuthRB_CheckedChanged(object sender, EventArgs e)
        {
            Login.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;                
            Password.Text = "";
            Login.Enabled = false;
            Password.Enabled = false;
        }

        private void OkB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ServerName.Text))
            {
                MessageBox.Show("Необходимо указать имя сервера или его IP адрес!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SQLServeAuthRB.Checked && string.IsNullOrEmpty(Login.Text))
            {
                MessageBox.Show("Необходимо указать имя пользователя!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(DBNames.Text))
            {
                DialogResult dr = MessageBox.Show("Вы не выбрали базу данных на сервере."
                + "\r\nПо умолчанию выберется база данных назначенная данному пользователю."
                + "\r\nКак правило по умолчанию это база masters.\r\nВсе равно продолжать?",
                        "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == System.Windows.Forms.DialogResult.No)
                    return;
            }            

            if (CheckConnection())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void SqlConnectionForm_Load(object sender, EventArgs e)
        {
            if (settings != null)
            {
                if (settings.SQLServAddr != null)
                    ServerName.Text = settings.SQLServAddr;

                if (settings.IsSQLTypeAuth)
                {
                    SQLServeAuthRB.Checked = true;
                    Login.Text = settings.SQLLogin;
                }
                else
                    WindowsAuthRB.Checked = true;
                if (settings.SQLDBName != null)
                    DBNames.Text = settings.SQLDBName;

                if (settings.SQLPassword != null)
                {
                    Password.Text = BaseEncryptor<System.Security.Cryptography.DESCryptoServiceProvider>.Decrypt(settings.SQLPassword);
                }
            }
        }

        private void ServerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((byte)e.KeyChar == 13)
                OkB_Click(this, new EventArgs());
        }

        private void Login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((byte)e.KeyChar == 13)
                OkB_Click(this, new EventArgs());
        }

        private void Password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((byte)e.KeyChar == 13)
                OkB_Click(this, new EventArgs());
        }

        private void DBName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((byte)e.KeyChar == 13)
                OkB_Click(this, new EventArgs());
        }

        private void DBNames_DropDown(object sender, EventArgs e)
        {
            GenerateConnection();
            try
            {
                if (_conn.State != ConnectionState.Open)
                    _conn.Open();
            }
            catch { return; }

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("select * from sysdatabases", _conn));
                DataTable dt = new DataTable();
                da.Fill(dt);
                DBNames.DataSource = dt;
                DBNames.DisplayMember = "name";
            }
            catch { return; }
        }
    }
}
