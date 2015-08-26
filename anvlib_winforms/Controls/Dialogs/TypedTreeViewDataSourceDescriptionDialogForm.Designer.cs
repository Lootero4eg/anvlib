namespace anvlib.Controls.Dialogs
{
    partial class TypedTreeViewDataSourceDescriptionDialogForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.PropertiesEd = new System.Windows.Forms.PropertyGrid();
            this.AddRootB = new System.Windows.Forms.Button();
            this.AddChildB = new System.Windows.Forms.Button();
            this.OkB = new System.Windows.Forms.Button();
            this.CancelB = new System.Windows.Forms.Button();
            this.DownB = new System.Windows.Forms.Button();
            this.RemoveB = new System.Windows.Forms.Button();
            this.UpB = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ClassNameEd = new System.Windows.Forms.TextBox();
            this.ClassBrowseB = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.MainTree = new anvlib.Controls.CommonTreeView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(9, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выберите узел для редактирования:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(385, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Свойства:";
            // 
            // PropertiesEd
            // 
            this.PropertiesEd.Location = new System.Drawing.Point(388, 79);
            this.PropertiesEd.Name = "PropertiesEd";
            this.PropertiesEd.Size = new System.Drawing.Size(349, 354);
            this.PropertiesEd.TabIndex = 3;
            this.PropertiesEd.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertiesEd_PropertyValueChanged);
            // 
            // AddRootB
            // 
            this.AddRootB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddRootB.Location = new System.Drawing.Point(12, 403);
            this.AddRootB.Name = "AddRootB";
            this.AddRootB.Size = new System.Drawing.Size(158, 30);
            this.AddRootB.TabIndex = 4;
            this.AddRootB.Text = "Добавить корень";
            this.AddRootB.UseVisualStyleBackColor = true;
            this.AddRootB.Click += new System.EventHandler(this.AddRootB_Click);
            // 
            // AddChildB
            // 
            this.AddChildB.Enabled = false;
            this.AddChildB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddChildB.Location = new System.Drawing.Point(176, 403);
            this.AddChildB.Name = "AddChildB";
            this.AddChildB.Size = new System.Drawing.Size(158, 30);
            this.AddChildB.TabIndex = 5;
            this.AddChildB.Text = "Добавить ветку";
            this.AddChildB.UseVisualStyleBackColor = true;
            this.AddChildB.Click += new System.EventHandler(this.AddChildB_Click);
            // 
            // OkB
            // 
            this.OkB.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OkB.Location = new System.Drawing.Point(582, 439);
            this.OkB.Name = "OkB";
            this.OkB.Size = new System.Drawing.Size(75, 30);
            this.OkB.TabIndex = 6;
            this.OkB.Text = "ОК";
            this.OkB.UseVisualStyleBackColor = true;
            // 
            // CancelB
            // 
            this.CancelB.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CancelB.Location = new System.Drawing.Point(663, 439);
            this.CancelB.Name = "CancelB";
            this.CancelB.Size = new System.Drawing.Size(75, 30);
            this.CancelB.TabIndex = 7;
            this.CancelB.Text = "Отмена";
            this.CancelB.UseVisualStyleBackColor = true;
            // 
            // DownB
            // 
            this.DownB.Location = new System.Drawing.Point(338, 115);
            this.DownB.Name = "DownB";
            this.DownB.Size = new System.Drawing.Size(30, 30);
            this.DownB.TabIndex = 9;
            this.DownB.UseVisualStyleBackColor = true;
            this.DownB.Click += new System.EventHandler(this.DownB_Click);
            this.DownB.Paint += new System.Windows.Forms.PaintEventHandler(this.DownB_Paint);
            // 
            // RemoveB
            // 
            this.RemoveB.Location = new System.Drawing.Point(338, 158);
            this.RemoveB.Name = "RemoveB";
            this.RemoveB.Size = new System.Drawing.Size(30, 30);
            this.RemoveB.TabIndex = 10;
            this.RemoveB.UseVisualStyleBackColor = true;
            this.RemoveB.Click += new System.EventHandler(this.RemoveB_Click);
            this.RemoveB.Paint += new System.Windows.Forms.PaintEventHandler(this.RemoveB_Paint);
            // 
            // UpB
            // 
            this.UpB.Location = new System.Drawing.Point(338, 79);
            this.UpB.Name = "UpB";
            this.UpB.Size = new System.Drawing.Size(30, 30);
            this.UpB.TabIndex = 8;
            this.UpB.UseVisualStyleBackColor = true;
            this.UpB.Click += new System.EventHandler(this.UpB_Click);
            this.UpB.Paint += new System.Windows.Forms.PaintEventHandler(this.UpB_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(9, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(498, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Выберите или введите имя класса для автоматической генерации дерева:";
            // 
            // ClassNameEd
            // 
            this.ClassNameEd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ClassNameEd.Location = new System.Drawing.Point(12, 25);
            this.ClassNameEd.Name = "ClassNameEd";
            this.ClassNameEd.Size = new System.Drawing.Size(683, 22);
            this.ClassNameEd.TabIndex = 12;
            this.ClassNameEd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ClassNameEd_KeyPress);
            // 
            // ClassBrowseB
            // 
            this.ClassBrowseB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ClassBrowseB.Location = new System.Drawing.Point(701, 24);
            this.ClassBrowseB.Name = "ClassBrowseB";
            this.ClassBrowseB.Size = new System.Drawing.Size(37, 23);
            this.ClassBrowseB.TabIndex = 13;
            this.ClassBrowseB.Text = "...";
            this.ClassBrowseB.UseVisualStyleBackColor = true;
            this.ClassBrowseB.Click += new System.EventHandler(this.ClassBrowseB_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(9, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(729, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "_________________________________________________________________________________" +
    "______________________";
            // 
            // MainTree
            // 
            this.MainTree.HideSelection = false;
            this.MainTree.Location = new System.Drawing.Point(12, 79);
            this.MainTree.Name = "MainTree";
            this.MainTree.Size = new System.Drawing.Size(320, 318);
            this.MainTree.TabIndex = 1;
            this.MainTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MainTree_AfterSelect);
            // 
            // DataSourceDescriptionDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 472);
            this.Controls.Add(this.ClassBrowseB);
            this.Controls.Add(this.ClassNameEd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RemoveB);
            this.Controls.Add(this.DownB);
            this.Controls.Add(this.UpB);
            this.Controls.Add(this.CancelB);
            this.Controls.Add(this.OkB);
            this.Controls.Add(this.AddChildB);
            this.Controls.Add(this.AddRootB);
            this.Controls.Add(this.PropertiesEd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MainTree);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DataSourceDescriptionDialogForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор описаний веток TreeView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CommonTreeView MainTree;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PropertyGrid PropertiesEd;
        private System.Windows.Forms.Button AddRootB;
        private System.Windows.Forms.Button AddChildB;
        private System.Windows.Forms.Button OkB;
        private System.Windows.Forms.Button CancelB;
        private System.Windows.Forms.Button UpB;
        private System.Windows.Forms.Button DownB;
        private System.Windows.Forms.Button RemoveB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ClassNameEd;
        private System.Windows.Forms.Button ClassBrowseB;
        private System.Windows.Forms.Label label4;

    }
}