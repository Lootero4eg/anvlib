namespace anvlib.Forms
{
    partial class SqlConnectionFormSimple
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
            this.ServerNameE = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PasswordE = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LoginE = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CancelB = new System.Windows.Forms.Button();
            this.OkB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ServerName
            // 
            this.ServerNameE.Location = new System.Drawing.Point(2, 94);
            this.ServerNameE.Name = "ServerName";
            this.ServerNameE.Size = new System.Drawing.Size(276, 20);
            this.ServerNameE.TabIndex = 3;
            this.ServerNameE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ServerName_KeyPress);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Введите имя сервера или его IP адрес:";
            // 
            // Password
            // 
            this.PasswordE.Location = new System.Drawing.Point(2, 53);
            this.PasswordE.Name = "Password";
            this.PasswordE.PasswordChar = '*';
            this.PasswordE.Size = new System.Drawing.Size(276, 20);
            this.PasswordE.TabIndex = 2;
            this.PasswordE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Password_KeyPress);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(278, 14);
            this.label3.TabIndex = 13;
            this.label3.Text = "Введите пароль:";
            // 
            // Login
            // 
            this.LoginE.Location = new System.Drawing.Point(2, 16);
            this.LoginE.Name = "Login";
            this.LoginE.Size = new System.Drawing.Size(276, 20);
            this.LoginE.TabIndex = 1;
            this.LoginE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Login_KeyPress);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(278, 14);
            this.label2.TabIndex = 12;
            this.label2.Text = "Введите имя пользователя:";
            // 
            // CancelB
            // 
            this.CancelB.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelB.Location = new System.Drawing.Point(141, 121);
            this.CancelB.Name = "CancelB";
            this.CancelB.Size = new System.Drawing.Size(75, 23);
            this.CancelB.TabIndex = 15;
            this.CancelB.Text = "Отмена";
            this.CancelB.UseVisualStyleBackColor = true;
            // 
            // OkB
            // 
            this.OkB.Location = new System.Drawing.Point(65, 121);
            this.OkB.Name = "OkB";
            this.OkB.Size = new System.Drawing.Size(75, 23);
            this.OkB.TabIndex = 14;
            this.OkB.Text = "ОК";
            this.OkB.UseVisualStyleBackColor = true;
            this.OkB.Click += new System.EventHandler(this.OkB_Click);
            // 
            // SqlConnectionFormSimple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 148);
            this.Controls.Add(this.CancelB);
            this.Controls.Add(this.OkB);
            this.Controls.Add(this.PasswordE);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LoginE);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ServerNameE);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SqlConnectionFormSimple";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Вход в систему";
            this.Load += new System.EventHandler(this.SqlConnectionFromSimple_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ServerNameE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PasswordE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox LoginE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CancelB;
        private System.Windows.Forms.Button OkB;
    }
}