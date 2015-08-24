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
    public partial class TypedListViewDataSourceDescriptionDialogForm : Form
    {                
        private TypedListViewDisplayMember _tlvdm;
        private int _itemsCount = 0;

        public TypedListViewDisplayMember TypedListViewDisplayMember
        {
            get { return _tlvdm; }
        }

        public TypedListViewDataSourceDescriptionDialogForm(TypedListViewDisplayMember tlvdm)
        {
            _tlvdm = tlvdm;
            InitializeComponent();

            GenerateListViewItems();
        }

        private void AddMainItemB_Click(object sender, EventArgs e)
        {
            if (MainLV.Items.Count == 0)
            {
                ListViewItem li = new ListViewItem(MainLV.Groups[0]);
                li.Text = string.Format("Item{0}", _itemsCount.ToString());
                _itemsCount++;
                if (_tlvdm != null)
                    li.Tag = _tlvdm;
                else
                {
                    _tlvdm = new Classes.TypedListViewDisplayMember();
                    _tlvdm.Caption = li.Text;
                    li.Tag = _tlvdm;
                }
                MainLV.Items.Add(li);
                AddMainItemB.Enabled = false;
            }
        }        

        private void AddChildB_Click(object sender, EventArgs e)
        {
            ListViewItem li = new ListViewItem(MainLV.Groups[1]);
            li.Text = string.Format("Item{0}", _itemsCount.ToString());
            _itemsCount++;
            if (_tlvdm != null)
            {
                //MessageBox.Show(_tlvdm.SubItems.Count.ToString());
                Classes.BaseListViewItem _stlvdm = new Classes.BaseListViewItem();
                _stlvdm.Caption = li.Text;                
                _tlvdm.SubItems.Add(_stlvdm);
                li.Tag = _stlvdm;  
            }
            
            MainLV.Items.Add(li);
        }                

        private void PropertiesEd_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {            
            if (e.ChangedItem.Label == "Caption")
                MainLV.SelectedItems[0].Text = e.ChangedItem.Value.ToString();
        }

        private void RemoveB_Click(object sender, EventArgs e)
        {
            if (MainLV.SelectedItems[0] != MainLV.Items[0])
            {
                _tlvdm.SubItems.Remove(MainLV.SelectedItems[0].Tag as Classes.BaseListViewItem);
                MainLV.SelectedItems[0].Remove();
            }
            else if (_tlvdm.SubItems.Count == 0)
            {
                /*_tlvdm.SubItems.Remove(MainLV.SelectedItems[0].Tag as Classes.BaseListViewItem);
                MainLV.SelectedItems[0].Tag = new Classes.TypedListViewDisplayMember();
                MainLV.SelectedItems[0].Text = "Item0";*/
                MainLV.Items.Clear();
                _tlvdm = new TypedListViewDisplayMember();
                _tlvdm.Caption = "Item0";
            }
        }

        private void HiglightItem()
        {            
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
        }

        private void DownB_Click(object sender, EventArgs e)
        {         
        }

        private void ClassBrowseB_Click(object sender, EventArgs e)
        {
            List<AssemblyDllInfo> assemblies = new List<AssemblyDllInfo>();
            if (_tlvdm.AssembliesList != null)
                assemblies = (_tlvdm.AssembliesList as List<AssemblyDllInfo>);
            ClassChooserDialogForm ccdf = new ClassChooserDialogForm();
            ccdf.LoadedAssemblies = assemblies;
            DialogResult dr = ccdf.ShowDialog();            
            if (dr == System.Windows.Forms.DialogResult.OK)
            {                
                var eitem = ccdf.EditableItem;
                ClassNameEd.Text = eitem.ToString();
                try
                {
                    MainLV.Items.Clear();
                    PropertiesEd.SelectedObject = null;
                    object obj = (ccdf.EditableItem as Type).Assembly.CreateInstance((ccdf.EditableItem as Type).FullName);
                    //_tlvdm = anvlib.Classes.TypedTreeViewDisplayMemberList.GenerateTreeDescriptionFromObject(obj, null);
                    //--пересоздать items
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            _tlvdm.AssembliesList = ccdf.LoadedAssemblies;
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
                        //_tlvdm = anvlib.Classes.TypedTreeViewDisplayMemberList.GenerateTreeDescriptionFromObject(obj, null);
                        //--вызов процедуры генерации итемов
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

        private void MainLV_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (MainLV.SelectedItems.Count > 0)            
                PropertiesEd.SelectedObject = MainLV.SelectedItems[0].Tag;
        }

        private void GenerateListViewItems()
        {
            MainLV.Items.Clear();
            if (_tlvdm != null)
            {
                ListViewItem li = new ListViewItem(MainLV.Groups[0]);
                li.Text = (!string.IsNullOrEmpty(_tlvdm.Caption) ? _tlvdm.Caption : _tlvdm.DisplayMember);
                li.Tag = _tlvdm;
                MainLV.Items.Add(li);
                foreach (var item in _tlvdm.SubItems)
                {
                    li = new ListViewItem(MainLV.Groups[1]);
                    li.Text = (!string.IsNullOrEmpty(item.Caption) ? item.Caption : item.DisplayMember);
                    li.Tag = item;
                    MainLV.Items.Add(li);
                }
            }
        }        
    }
}
