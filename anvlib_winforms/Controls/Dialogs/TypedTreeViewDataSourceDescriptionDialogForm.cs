using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Classes;
using anvlib.Controls.Extensions;

namespace anvlib.Controls.Dialogs
{
    public partial class TypedTreeViewDataSourceDescriptionDialogForm : Form
    {        
        private int nodescount = 0;
        private TypedTreeViewDisplayMemberList _tvdml;

        public TypedTreeViewDisplayMemberList TypedTreeViewDisplayMemberList
        {
            get { return _tvdml; }
        }

        public TypedTreeViewDataSourceDescriptionDialogForm(TypedTreeViewDisplayMemberList tvdml)
        {
            _tvdml = tvdml;
            InitializeComponent();
            
            if (_tvdml.Count > 0)
                GenerateTree();
        }

        private void AddRootB_Click(object sender, EventArgs e)
        {
            nodescount++;
            TreeNode tr = new TreeNode(string.Format("Node{0}", nodescount.ToString()));            
            TypedTreeViewDisplayMember tvdm = new TypedTreeViewDisplayMember("", "");
            tr.Tag = tvdm;
            MainTree.Nodes.Add(tr);
            tvdm.NodeName = tr.Text;            
            _tvdml.Add(tvdm);
            MainTree.SelectedNode = tr;
            AddChildB.Enabled = true;
            HiglightNode();
        }

        private void MainTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (MainTree.SelectedNode != null)
                PropertiesEd.SelectedObject = MainTree.SelectedNode.Tag;
        }

        private void AddChildB_Click(object sender, EventArgs e)
        {
            if (MainTree.SelectedNode != null)
            {
                nodescount++;
                TreeNode tr = new TreeNode(string.Format("Node{0}", nodescount.ToString()));
                TypedTreeViewDisplayMember tvdm = new TypedTreeViewDisplayMember("", "");
                tr.Tag = tvdm;
                tvdm.NodeName = tr.Text;                
                MainTree.SelectedNode.Nodes.Add(tr);
                (MainTree.SelectedNode.Tag as TypedTreeViewDisplayMember).ChildDisplayMembers.Add(tvdm);                
                MainTree.SelectedNode = tr;
                HiglightNode();
            }
        }

        private void GenerateTree()
        {
            if (_tvdml != null && _tvdml.Count > 0)
            {
                foreach (var item in _tvdml)
                {
                    TreeNode tr = new TreeNode(!string.IsNullOrEmpty(item.NodeName) ? item.NodeName : item.DisplayMember);
                    tr.Tag = item;
                    MainTree.Nodes.Add(tr);
                    nodescount++;
                    if (item.ChildDisplayMembers.Count > 0)
                    {
                        foreach (var subitem in item.ChildDisplayMembers)
                            MakeBranch(tr, subitem);
                    }
                }

                MainTree.ExpandAll();
                MainTree.SelectedNode = MainTree.Nodes[0];
                HiglightNode();
                AddChildB.Enabled = true;
            }
        }

        private void MakeBranch(TreeNode parentnode, TypedTreeViewDisplayMember tvdm)
        {
            TreeNode node = new TreeNode(tvdm.NodeName);
            node.Tag = tvdm;
            parentnode.Nodes.Add(node);
            nodescount++;

            if (tvdm.ChildDisplayMembers.Count > 0)
            {
                foreach (var subitem in tvdm.ChildDisplayMembers)
                    MakeBranch(node, subitem);
            }
        }

        private void PropertiesEd_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {            
            if (e.ChangedItem.Label == "NodeName")
                MainTree.SelectedNode.Text = e.ChangedItem.Value.ToString();

            if (e.ChangedItem.Label == "ImageIndex")
                PropertiesEd.Refresh();
        }

        private void RemoveB_Click(object sender, EventArgs e)
        {
            if (MainTree.SelectedNode != null)
            {
                var parentnode = MainTree.SelectedNode.Parent;
                _tvdml.Remove((MainTree.SelectedNode.Tag as TypedTreeViewDisplayMember));
                MainTree.SelectedNode.Remove();
                if (MainTree.Nodes.Count > 0)
                {
                    if (parentnode != null)
                        MainTree.SelectedNode = parentnode;
                    else
                        MainTree.SelectedNode = MainTree.Nodes[0];
                    HiglightNode();
                }
            }

            if (MainTree.Nodes.Count == 0)
                nodescount = 0;
        }

        private void HiglightNode()
        {
            if (MainTree.SelectedNode != null)
                MainTree.Select();
        }

        private void UpB_Paint(object sender, PaintEventArgs e)
        {            
            MakeTransparent(e.Graphics, Properties.Resources.BuilderDialog_moveup, UpB);
        }

        private void DownB_Paint(object sender, PaintEventArgs e)
        {
            MakeTransparent(e.Graphics, Properties.Resources.BuilderDialog_movedown, DownB);
        }

        private void MakeTransparent(Graphics gr, Bitmap image, Control ctrl)
        {
            Bitmap bmp = image;
            bmp.MakeTransparent();
            int x = (ctrl.Width - image.Width) / 2;
            int y = (ctrl.Height - image.Height) / 2;
            gr.DrawImage(bmp, x, y);
        }

        private void RemoveB_Paint(object sender, PaintEventArgs e)
        {
            MakeTransparent(e.Graphics, Properties.Resources.Delete, RemoveB);
        }

        private void UpB_Click(object sender, EventArgs e)
        {
            if (MainTree.SelectedNode != null)
                TreeViewExtensions.MoveUp(MainTree.SelectedNode); 
        }

        private void DownB_Click(object sender, EventArgs e)
        {
            if (MainTree.SelectedNode != null)
                TreeViewExtensions.MoveBranchDown(MainTree.SelectedNode); 
        }

        private void ClassBrowseB_Click(object sender, EventArgs e)
        {
            List<AssemblyDllInfo> assemblies = new List<AssemblyDllInfo>();            
            if (_tvdml.AssembliesList != null)
                assemblies = (_tvdml.AssembliesList as List<AssemblyDllInfo>);
            ClassChooserDialogForm ccdf = new ClassChooserDialogForm();
            ccdf.LoadedAssemblies = assemblies;
            DialogResult dr = ccdf.ShowDialog();            
            if (dr == System.Windows.Forms.DialogResult.OK)
            {                
                var eitem = ccdf.EditableItem;
                ClassNameEd.Text = eitem.ToString();
                try
                {
                    MainTree.Nodes.Clear();
                    PropertiesEd.SelectedObject = null;
                    object obj = (ccdf.EditableItem as Type).Assembly.CreateInstance((ccdf.EditableItem as Type).FullName);                    
                    _tvdml = anvlib.Classes.TypedTreeViewDisplayMemberList.GenerateTreeDescriptionFromObject(obj, null);
                    GenerateTree();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }            
            _tvdml.AssembliesList = ccdf.LoadedAssemblies;
        }

        private void ClassNameEd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                try
                {
                    var type = Type.GetType(ClassNameEd.Text);
                    object obj = Activator.CreateInstance(type);
                    if (obj != null)
                    {
                        _tvdml = anvlib.Classes.TypedTreeViewDisplayMemberList.GenerateTreeDescriptionFromObject(obj, null);
                        GenerateTree();
                    }
                    else
                        MessageBox.Show("Null");
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
        }        
    }
}
