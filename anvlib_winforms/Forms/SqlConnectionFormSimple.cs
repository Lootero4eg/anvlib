using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using anvlib.Interfaces;

namespace anvlib.Forms
{
    public partial class SqlConnectionFormSimple : Form, ISimpleSqlConnectionString
    {
        private SqlConnection _conn;
        private string _conn_str;
        private bool _returnConnection;

        private string _dbcatalog;
        private string _app_name;

        public SqlConnectionFormSimple(bool ReturnConnectionaAfterConnect, string ApplicationName)
        {
            _returnConnection = ReturnConnectionaAfterConnect;
            _app_name = ApplicationName;
            InitializeComponent();
        }

        public string ServerName
        {
            get { return ServerNameE.Text; }
            set { ServerNameE.Text = value; }
        }

        public string Login
        {
            get { return LoginE.Text; }
            set { LoginE.Text = value; }
        }

        public string Password
        {
            get { return PasswordE.Text; }
            set { PasswordE.Text = value; }
        }

        public string DBCatalog
        {
            get { return _dbcatalog; }
            set { _dbcatalog = value; }
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

        public string ConnectionString { get; set; }

        public void SetFormIcon(Icon icon)
        {
            this.Icon = icon;
        }

        private void OkB_Click(object sender, EventArgs e)
        {
            _conn_str = anvlib.Data.Database.BaseMSSQLManager.GetConnectionString(ServerNameE.Text, LoginE.Text, PasswordE.Text, _dbcatalog, false, _app_name);
            SqlConnection conn = new SqlConnection(_conn_str);
            try
            {
                conn.Open();
                if (_returnConnection)
                {
                    _conn = conn;                    
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    return;
                }
                conn.Close();
                ConnectionString = _conn_str;
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к серверу, проверте правильность данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void Login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                PasswordE.Focus();
                e.Handled = true;
            }
        }

        private void Password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 && ServerNameE.Text.Length > 0)
            {
                OkB_Click(null, null);
                e.Handled = true;
            }
            if (e.KeyChar == (char)13 && ServerNameE.Text.Length == 0)
            {
                ServerNameE.Focus();
                e.Handled = true;
            }            
        }

        private void ServerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                OkB_Click(null, null);
                e.Handled = true;
            }
        }

        private void SqlConnectionFromSimple_Load(object sender, EventArgs e)
        {
            /*Login.Text = Properties.Settings.Default.SQLLogin;
            ServerName.Text = Properties.Settings.Default.SQLServ;*/
            if (LoginE.Text.Length > 0)
                PasswordE.Select();
        }
    }
}
