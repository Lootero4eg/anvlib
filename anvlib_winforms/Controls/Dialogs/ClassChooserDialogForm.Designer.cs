namespace anvlib.Controls.Dialogs
{
    partial class ClassChooserDialogForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassChooserDialogForm));
            anvlib.Classes.TypedTreeViewDisplayMember typedTreeViewDisplayMember1 = new anvlib.Classes.TypedTreeViewDisplayMember();
            anvlib.Classes.TypedTreeViewDisplayMember typedTreeViewDisplayMember2 = new anvlib.Classes.TypedTreeViewDisplayMember();
            anvlib.Classes.TypedTreeViewDisplayMember typedTreeViewDisplayMember3 = new anvlib.Classes.TypedTreeViewDisplayMember();
            this.label1 = new System.Windows.Forms.Label();
            this.OkB = new System.Windows.Forms.Button();
            this.CancelB = new System.Windows.Forms.Button();
            this.MainIL = new System.Windows.Forms.ImageList(this.components);
            this.AssembliesTree = new anvlib.Controls.TypedTreeView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выберите класс:";
            // 
            // OkB
            // 
            this.OkB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OkB.Location = new System.Drawing.Point(111, 377);
            this.OkB.Name = "OkB";
            this.OkB.Size = new System.Drawing.Size(100, 31);
            this.OkB.TabIndex = 2;
            this.OkB.Text = "ОК";
            this.OkB.UseVisualStyleBackColor = true;
            this.OkB.Click += new System.EventHandler(this.OkB_Click);
            // 
            // CancelB
            // 
            this.CancelB.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CancelB.Location = new System.Drawing.Point(217, 377);
            this.CancelB.Name = "CancelB";
            this.CancelB.Size = new System.Drawing.Size(100, 31);
            this.CancelB.TabIndex = 3;
            this.CancelB.Text = "Отмена";
            this.CancelB.UseVisualStyleBackColor = true;
            // 
            // MainIL
            // 
            this.MainIL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MainIL.ImageStream")));
            this.MainIL.TransparentColor = System.Drawing.Color.Magenta;
            this.MainIL.Images.SetKeyName(0, "Assembly16.png");
            this.MainIL.Images.SetKeyName(1, "namespace.png");
            this.MainIL.Images.SetKeyName(2, "Class16.png");
            // 
            // AssembliesTree
            //             
            this.AssembliesTree.DataSource = null;
            typedTreeViewDisplayMember3.DataSourceName = "Classes";
            typedTreeViewDisplayMember3.DisplayMember = "Name";
            typedTreeViewDisplayMember3.DisplayСaption = null;
            typedTreeViewDisplayMember3.ImageIndex = 2;
            typedTreeViewDisplayMember3.ImageIndexByFieldName = null;
            typedTreeViewDisplayMember3.NodeName = "ClassName";
            typedTreeViewDisplayMember3.SelectedImageIndex = 2;
            typedTreeViewDisplayMember3.StateImageIndex = 2;
            typedTreeViewDisplayMember3.WithoutTag = false;
            typedTreeViewDisplayMember2.ChildDisplayMembers.Add(typedTreeViewDisplayMember3);
            typedTreeViewDisplayMember2.DataSourceName = "Namespaces";
            typedTreeViewDisplayMember2.DisplayMember = "NamespaceName";
            typedTreeViewDisplayMember2.DisplayСaption = null;
            typedTreeViewDisplayMember2.ImageIndex = 1;
            typedTreeViewDisplayMember2.ImageIndexByFieldName = null;
            typedTreeViewDisplayMember2.NodeName = "NamespaceName";
            typedTreeViewDisplayMember2.SelectedImageIndex = 1;
            typedTreeViewDisplayMember2.StateImageIndex = 1;
            typedTreeViewDisplayMember2.WithoutTag = false;
            typedTreeViewDisplayMember1.ChildDisplayMembers.Add(typedTreeViewDisplayMember2);
            typedTreeViewDisplayMember1.DataSourceName = "";
            typedTreeViewDisplayMember1.DisplayMember = "AssemblyName";
            typedTreeViewDisplayMember1.DisplayСaption = null;
            typedTreeViewDisplayMember1.ImageIndex = 0;
            typedTreeViewDisplayMember1.ImageIndexByFieldName = null;
            typedTreeViewDisplayMember1.NodeName = "AssemblyName";
            typedTreeViewDisplayMember1.SelectedImageIndex = 0;
            typedTreeViewDisplayMember1.StateImageIndex = 0;
            typedTreeViewDisplayMember1.WithoutTag = false;
            this.AssembliesTree.DataSourceDescription.Add(typedTreeViewDisplayMember1);            
            this.AssembliesTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AssembliesTree.ImageIndex = 0;
            this.AssembliesTree.ImageList = this.MainIL;
            this.AssembliesTree.Location = new System.Drawing.Point(15, 28);
            this.AssembliesTree.Name = "AssembliesTree";
            this.AssembliesTree.SelectedImageIndex = 0;
            this.AssembliesTree.Size = new System.Drawing.Size(407, 343);
            this.AssembliesTree.TabIndex = 1;
            this.AssembliesTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AssembliesTree_MouseDoubleClick);
            // 
            // ClassChooserDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 412);
            this.Controls.Add(this.CancelB);
            this.Controls.Add(this.OkB);
            this.Controls.Add(this.AssembliesTree);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ClassChooserDialogForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выбор класса";
            this.Load += new System.EventHandler(this.ClassChooserDialogForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private TypedTreeView AssembliesTree;
        private System.Windows.Forms.Button OkB;
        private System.Windows.Forms.Button CancelB;
        private System.Windows.Forms.ImageList MainIL;
    }
}