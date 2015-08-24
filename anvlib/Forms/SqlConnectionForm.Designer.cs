namespace anvlib.Forms
{
    partial class SqlConnectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.ServerName = new System.Windows.Forms.TextBox();
            this.OkB = new System.Windows.Forms.Button();
            this.CancelB = new System.Windows.Forms.Button();
            this.TestConnB = new System.Windows.Forms.Button();
            this.Login = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SQLServeAuthRB = new System.Windows.Forms.RadioButton();
            this.WindowsAuthRB = new System.Windows.Forms.RadioButton();
            this.Password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DBNames = new System.Windows.Forms.ComboBox();
            this.SavePass = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите имя сервера или его IP адрес:";
            // 
            // ServerName
            // 
            this.ServerName.Location = new System.Drawing.Point(4, 16);
            this.ServerName.Name = "ServerName";
            this.ServerName.Size = new System.Drawing.Size(276, 20);
            this.ServerName.TabIndex = 1;
            this.ServerName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ServerName_KeyPress);
            // 
            // OkB
            // 
            this.OkB.Location = new System.Drawing.Point(130, 242);
            this.OkB.Name = "OkB";
            this.OkB.Size = new System.Drawing.Size(75, 23);
            this.OkB.TabIndex = 8;
            this.OkB.Text = "ОК";
            this.OkB.UseVisualStyleBackColor = true;
            this.OkB.Click += new System.EventHandler(this.OkB_Click);
            // 
            // CancelB
            // 
            this.CancelB.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelB.Location = new System.Drawing.Point(206, 242);
            this.CancelB.Name = "CancelB";
            this.CancelB.Size = new System.Drawing.Size(75, 23);
            this.CancelB.TabIndex = 9;
            this.CancelB.Text = "Отмена";
            this.CancelB.UseVisualStyleBackColor = true;
            this.CancelB.Click += new System.EventHandler(this.CancelB_Click);
            // 
            // TestConnB
            // 
            this.TestConnB.Location = new System.Drawing.Point(130, 216);
            this.TestConnB.Name = "TestConnB";
            this.TestConnB.Size = new System.Drawing.Size(150, 23);
            this.TestConnB.TabIndex = 7;
            this.TestConnB.Text = "Проверить соединение";
            this.TestConnB.UseVisualStyleBackColor = true;
            this.TestConnB.Click += new System.EventHandler(this.TestConnB_Click);
            // 
            // Login
            // 
            this.Login.Location = new System.Drawing.Point(5, 96);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(276, 20);
            this.Login.TabIndex = 4;
            this.Login.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Login_KeyPress);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(278, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Введите имя пользователя:";
            // 
            // SQLServeAuthRB
            // 
            this.SQLServeAuthRB.AutoSize = true;
            this.SQLServeAuthRB.Checked = true;
            this.SQLServeAuthRB.Location = new System.Drawing.Point(16, 40);
            this.SQLServeAuthRB.Name = "SQLServeAuthRB";
            this.SQLServeAuthRB.Size = new System.Drawing.Size(217, 17);
            this.SQLServeAuthRB.TabIndex = 2;
            this.SQLServeAuthRB.TabStop = true;
            this.SQLServeAuthRB.Text = "Вход под учетной записью SQL Server";
            this.SQLServeAuthRB.UseVisualStyleBackColor = true;
            this.SQLServeAuthRB.CheckedChanged += new System.EventHandler(this.SQLServeAuthRB_CheckedChanged);
            // 
            // WindowsAuthRB
            // 
            this.WindowsAuthRB.AutoSize = true;
            this.WindowsAuthRB.Location = new System.Drawing.Point(16, 58);
            this.WindowsAuthRB.Name = "WindowsAuthRB";
            this.WindowsAuthRB.Size = new System.Drawing.Size(247, 17);
            this.WindowsAuthRB.TabIndex = 3;
            this.WindowsAuthRB.Text = "Вход по встроенной безопасности Windows";
            this.WindowsAuthRB.UseVisualStyleBackColor = true;
            this.WindowsAuthRB.CheckedChanged += new System.EventHandler(this.WindowsAuthRB_CheckedChanged);
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(5, 133);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(276, 20);
            this.Password.TabIndex = 5;
            this.Password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Password_KeyPress);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(278, 14);
            this.label3.TabIndex = 9;
            this.label3.Text = "Введите пароль:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(278, 14);
            this.label4.TabIndex = 11;
            this.label4.Text = "Введите имя базы данных:";
            // 
            // DBNames
            // 
            this.DBNames.FormattingEnabled = true;
            this.DBNames.Location = new System.Drawing.Point(5, 188);
            this.DBNames.Name = "DBNames";
            this.DBNames.Size = new System.Drawing.Size(276, 21);
            this.DBNames.TabIndex = 12;
            this.DBNames.DropDown += new System.EventHandler(this.DBNames_DropDown);
            this.DBNames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DBName_KeyPress);
            // 
            // SavePass
            // 
            this.SavePass.AutoSize = true;
            this.SavePass.Checked = true;
            this.SavePass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SavePass.Enabled = false;
            this.SavePass.Location = new System.Drawing.Point(16, 155);
            this.SavePass.Name = "SavePass";
            this.SavePass.Size = new System.Drawing.Size(118, 17);
            this.SavePass.TabIndex = 13;
            this.SavePass.Text = "Сохранить пароль";
            this.SavePass.UseVisualStyleBackColor = true;
            // 
            // SqlConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 267);
            this.Controls.Add(this.SavePass);
            this.Controls.Add(this.DBNames);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.WindowsAuthRB);
            this.Controls.Add(this.SQLServeAuthRB);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TestConnB);
            this.Controls.Add(this.CancelB);
            this.Controls.Add(this.OkB);
            this.Controls.Add(this.ServerName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SqlConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройка соединения с сервером";
            this.Load += new System.EventHandler(this.SqlConnectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ServerName;
        private System.Windows.Forms.Button OkB;
        private System.Windows.Forms.Button CancelB;
        private System.Windows.Forms.Button TestConnB;
        private System.Windows.Forms.TextBox Login;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton SQLServeAuthRB;
        private System.Windows.Forms.RadioButton WindowsAuthRB;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox DBNames;
        private System.Windows.Forms.CheckBox SavePass;
    }
}