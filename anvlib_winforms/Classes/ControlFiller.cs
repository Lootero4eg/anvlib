using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace anvlib.Classes
{    
    /// <summary>
    /// Вспомогательный класс по заполнению различных контролсов, информацией из любых классов
    /// </summary>
    public static class ControlFiller
    {
        /// <summary>
        /// Метод заполняющий контролсы типа ListControl(ListBox,ComboBox,CheckedListBox)
        /// </summary>
        /// <param name="ctrl">Заполняемый контролс</param>
        /// <param name="DataSource">Источник данных. Коллекция или объект</param>
        /// <param name="DisplayMember">Выводимый элемент объекта на экран</param>
        /// <param name="GenerateItems">Флаг генерации элементов или считывание из объекта.</param>
        public static void FillListControl(ListControl ctrl, object DataSource, string DisplayMember, bool GenerateItems)
        {            
            if (ctrl is CheckedListBox)
            {
                var clb = (ctrl as CheckedListBox);

                if (GenerateItems)
                {
                    if (DataSource is System.Collections.IEnumerable)
                    {
                        System.Collections.IEnumerable col = (DataSource as System.Collections.IEnumerable);
                        ListControlGenerateItem(clb, col, DisplayMember);
                    }
                }
                else
                {
                    clb.DataSource = null;
                    clb.DataSource = DataSource;
                    clb.DisplayMember = DisplayMember;
                }

                return;
            }

            if (ctrl is ComboBox)
            {
                var cbox = (ctrl as ComboBox);
                if (!GenerateItems)
                {
                    cbox.DataSource = null;
                    if (DataSource is DataTable)
                    {
                        cbox.DisplayMember = DisplayMember;
                        cbox.DataSource = DataSource;
                    }
                    else
                    {
                        cbox.DataSource = DataSource;
                        cbox.DisplayMember = DisplayMember;                        
                    }
                }
                else
                {
                    if (DataSource is System.Collections.IEnumerable)
                    {
                        System.Collections.IEnumerable col = (DataSource as System.Collections.IEnumerable);
                        ListControlGenerateItem(cbox, col, DisplayMember);
                    }
                }
            }

            if (ctrl is ListBox)
            {
                var lbox = (ctrl as ListBox);
                if (!GenerateItems)
                {
                    lbox.DataSource = null;
                    if (DataSource is DataTable)
                    {
                        lbox.DisplayMember = DisplayMember;
                        lbox.DataSource = DataSource;
                    }
                    else
                    {
                        //--чето какие то косяки...то показывает, то не показывает правильно...
                        lbox.DisplayMember = DisplayMember;
                        lbox.DataSource = DataSource;
                        //lbox.DisplayMember = DisplayMember;
                    }
                }
                else
                {
                    if (DataSource is System.Collections.IEnumerable)
                    {
                        System.Collections.IEnumerable col = (DataSource as System.Collections.IEnumerable);
                        ListControlGenerateItem(lbox, col, DisplayMember);
                    }
                }
            }
        }

        private static void ListControlGenerateItem(object ctrl, System.Collections.IEnumerable col, string DisplayMember)
        {
            foreach (var item in col)
            {
                if (item.GetType() != typeof(string))
                {
                    PropertyInfo pinfo = item.GetType().GetProperty(DisplayMember);
                    object dispitem = pinfo.GetValue(item, null);
                    if (dispitem != null)
                    {
                        if (ctrl is ComboBox)
                            (ctrl as ComboBox).Items.Add(dispitem);
                        if (ctrl is ListBox)
                            (ctrl as ListBox).Items.Add(dispitem);
                        /*if (ctrl is CheckedListBox)
                            (ctrl as CheckedListBox).Items.Add(dispitem);*/
                    }
                }
                else
                {
                    if (ctrl is ComboBox)
                        (ctrl as ComboBox).Items.Add(item);
                    if (ctrl is ListBox)
                        (ctrl as ListBox).Items.Add(item);
                    /*if (ctrl is CheckedListBox)
                        (ctrl as CheckedListBox).Items.Add(item);*/
                }
            }
        }

        /// <summary>
        /// Альтернативный метод по заполнению контролсов типа ListControl
        /// </summary>
        /// <param name="ctrl">Сам контрол</param>
        /// <param name="Items">Набор строк, которые мы будем отображать</param>
        public static void FillListControl(ListControl ctrl, params string[] Items)
        {
            if (ctrl is CheckedListBox)
            {
                (ctrl as CheckedListBox).Items.Clear();
                foreach (var item in Items)
                    (ctrl as CheckedListBox).Items.Add(item);
            }

            if (ctrl is ComboBox)
            {
                (ctrl as ComboBox).Items.Clear();
                foreach (var item in Items)
                    (ctrl as ComboBox).Items.Add(item);
            }

            if (ctrl is ListBox)
            {
                (ctrl as ListBox).Items.Clear();
                foreach (var item in Items)
                    (ctrl as ListBox).Items.Add(item);
            }
        }

        /// <summary>
        /// Метод заполняющий контрол типа TreeView
        /// </summary>
        /// <param name="treeview">Контролс, в котором мы будем работать</param>
        /// <param name="DataSource">Коллекция или объект из которого мы будем строить дерево.
        /// Точнее даже это и есть само дерево, только не построенное в тривью</param>
        /// <param name="TreeLeafsInfo">Описание структуры дерева.
        /// Можно делать несколько веток парента, но только один уровень вложенности, паернт.парент не продуман пока</param>
        public static void FillTreeViewControl(TreeView treeview, object DataSource, TypedTreeViewDisplayMemberList TreeLeafsInfo)
        {
            TreeNode tr = new TreeNode();
            tr = CreateLeafs(tr, DataSource, TreeLeafsInfo);
            foreach (TreeNode item in tr.Nodes)
                treeview.Nodes.Add(item);
        }
        
        /// <summary>
        /// Добавочный метод к FillTreeViewControl, рекурсивно заполняющий TreeView
        /// </summary>
        /// <param name="tr">TreeView контролс</param>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="TreeLeafsInfo">Описание дерева</param>
        /// <returns></returns>
        private static TreeNode CreateLeafs(TreeNode tr, object DataSource, TypedTreeViewDisplayMemberList TreeLeafsInfo)
        {
            //--Сделать разбор атрибутов типа DisplayName!!!
            foreach (var Leaf in TreeLeafsInfo)
            {
                //--Если датасорц - Коллекция. Не понимаю зачем я сделал проверку на строку...поизучай этот вопрос на досуге
                if ((DataSource is System.Collections.IEnumerable) && DataSource.GetType() != typeof(string))
                {
                    #region Цикл по коллекции
                    var collection = (DataSource as System.Collections.IEnumerable);
                    foreach (var item in collection)
                    {                        
                        bool to_be_continue = false;
                        object text;

                        //--если строка, то идем к следующему элементу коллекции
                        if (item.GetType() == typeof(string))
                            to_be_continue = true;

                        #region Если есть только DataSource
                        if (item.GetType().GetProperty(Leaf.DisplayMember) == null && !string.IsNullOrEmpty(Leaf.DataSourceName)/* && !to_be_continue*/)
                        {
                            //--если это примитивное значение
                            if (!item.GetType().IsClass)
                                text = item;
                            else//--иначе это Класс
                            {
                                //--переделать на DisplayCaption
                                if (!string.IsNullOrEmpty(Leaf.DisplayСaption))
                                    text = Leaf.DisplayСaption;
                                else
                                {
                                    text = "";//--проверить нужен ли вообще этот участок кода!!!
                                    if (!(item is System.Collections.IEnumerable))
                                    {
                                        var val = item.GetType().GetProperty(Leaf.DataSourceName).GetValue(item, null);
                                        text = val.GetType().GetProperty(Leaf.DisplayMember).GetValue(val, null);
                                    }
                                }
                            }
                        }                         
                        else
                        {                            
                            if (string.IsNullOrEmpty(Leaf.DisplayMember))
                                to_be_continue = true;
                            //--если элемент не строка и DisplayMember != null
                            if (!to_be_continue)
                            {                                
                                if (item is System.Collections.IEnumerable)
                                {
                                    text = "";
                                    if (!string.IsNullOrEmpty(Leaf.DisplayСaption))
                                        text = Leaf.DisplayСaption;
                                }
                                else
                                    text = item.GetType().GetProperty(Leaf.DisplayMember).GetValue(item, null);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Leaf.DisplayСaption))
                                    text = Leaf.DisplayСaption;
                                else
                                    text = item;
                            }
                        }
                        #endregion

                        #region Если мы считали какоето значение в переменную text
                        if (text != null)
                        {
                            TreeNode mnode = new TreeNode(text.ToString());
                            if (!Leaf.WithoutTag)
                                mnode.Tag = item;
                            //--если индекс картинки мы берем из какого то поля в классе
                            if (string.IsNullOrEmpty(Leaf.ImageIndexByFieldName))
                            {
                                mnode.ImageIndex = Leaf.ImageIndex;
                                mnode.SelectedImageIndex = Leaf.SelectedImageIndex;
                                mnode.StateImageIndex = Leaf.StateImageIndex;
                            }
                            else
                            {
                                object im_idx=null;
                                if (Leaf.ImageIndexByFieldName.IndexOf('.') == -1)
                                    im_idx = item.GetType().GetProperty(Leaf.ImageIndexByFieldName).GetValue(item, null);
                                else
                                    im_idx = anvlib.Utilites.ObjectInspector.GetObjectPropertyValueRecursive(item, Leaf.ImageIndexByFieldName);
                                if (im_idx != null && im_idx.GetType() == typeof(int))
                                {
                                    mnode.ImageIndex = (int)im_idx;
                                    mnode.SelectedImageIndex = (int)im_idx;
                                    mnode.StateImageIndex = (int)im_idx;
                                }
                            }
                            tr.Nodes.Add(mnode);
                            
                            #region Если есть дочерние ветки
                            if (Leaf.ChildDisplayMembers != null)
                            {
                                foreach (var SubLeaf in Leaf.ChildDisplayMembers)
                                {
                                    if (SubLeaf.DataSourceName != null)
                                    {
                                        if (Leaf.DataSourceName != SubLeaf.DataSourceName || SubLeaf.IsRecusiveCollection)
                                        {
                                            //--переделать на DisplayCaption
                                            if (!string.IsNullOrEmpty(SubLeaf.DisplayMember) && !string.IsNullOrEmpty(SubLeaf.DisplayСaption))
                                            {
                                                TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                                tdml.Add(SubLeaf);
                                                CreateLeafs(mnode, item, tdml);
                                            }
                                            else
                                            {
                                                var innercol = item.GetType().GetProperty(SubLeaf.DataSourceName).GetValue(item, null);
                                                TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                                tdml.Add(SubLeaf);
                                                CreateLeafs(mnode, innercol, tdml);
                                            }
                                        }
                                        else
                                        {
                                            TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                            tdml.Add(SubLeaf);
                                            CreateLeafs(mnode, item, tdml);
                                        }
                                    }
                                    else
                                    {
                                        TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                        tdml.Add(SubLeaf);
                                        CreateLeafs(mnode, SubLeaf.DisplayMember, tdml);
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    //--если DataSource = null, но является не коллекцией 
                    if (DataSource != null)
                    {                        
                        object text;
                        if (!string.IsNullOrEmpty(Leaf.DisplayMember) && !string.IsNullOrEmpty(Leaf.DisplayСaption))
                            text = Leaf.DisplayMember.Remove(0, 7);
                        else
                        {
                            text = "";
                            if (!(DataSource is ValueType) && DataSource.GetType() != typeof(string))
                            {
                                if (!string.IsNullOrEmpty(Leaf.DisplayMember))
                                    text = DataSource.GetType().GetProperty(Leaf.DisplayMember).GetValue(DataSource, null);
                                else if (!string.IsNullOrEmpty(Leaf.DisplayСaption))
                                    text = Leaf.DisplayСaption;
                                else
                                    text = DataSource;
                            }
                        }
                        if (text != null)
                        {
                            TreeNode mnode = new TreeNode(text.ToString());
                            if (!Leaf.WithoutTag)
                                mnode.Tag = DataSource;
                            if (string.IsNullOrEmpty(Leaf.ImageIndexByFieldName))
                            {
                                mnode.ImageIndex = Leaf.ImageIndex;
                                mnode.SelectedImageIndex = Leaf.SelectedImageIndex;
                                mnode.StateImageIndex = Leaf.StateImageIndex;
                            }
                            else
                            {
                                var im_idx = DataSource.GetType().GetProperty(Leaf.ImageIndexByFieldName).GetValue(DataSource, null);
                                if (im_idx != null && im_idx.GetType() == typeof(int))
                                {
                                    mnode.ImageIndex = (int)im_idx;
                                    mnode.SelectedImageIndex = (int)im_idx;
                                    mnode.StateImageIndex = (int)im_idx;
                                }
                            }

                            tr.Nodes.Add(mnode);
                            if (Leaf.ChildDisplayMembers != null)
                            {
                                foreach (var SubLeaf in Leaf.ChildDisplayMembers)
                                {
                                    if (DataSource.GetType() != typeof(string))
                                    {
                                        var innercol = DataSource.GetType().GetProperty(SubLeaf.DataSourceName).GetValue(DataSource, null);
                                        TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                        tdml.Add(SubLeaf);
                                        CreateLeafs(mnode, innercol, tdml);
                                    }
                                    else
                                    {                                        
                                        TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                        tdml.Add(SubLeaf);                                        
                                        CreateLeafs(mnode, SubLeaf.DataSourceName, tdml);
                                    }
                                }
                            }
                        }                        
                    }
                }
            }

            return tr;
        }

        /// <summary>
        /// Метод заполняющий контролс типа ListView
        /// </summary>
        /// <param name="ctrl">Сам контрол</param>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="ItemDesc">Описание элементов листвью</param>
        public static void FillListView(ListView ctrl, object DataSource, TypedListViewDisplayMember ItemDesc)
        {
            if (ItemDesc != null)
            {
                ListViewItem li = new ListViewItem();

                #region IEnumerable Case
                if ((DataSource is System.Collections.IEnumerable) && DataSource.GetType() != typeof(string))
                {
                    var collection = (DataSource as System.Collections.IEnumerable);
                    foreach (var item in collection)
                    {
                        object text;                        
                        if (/*item.GetType().GetProperty(*/ItemDesc.DisplayMember == null && ItemDesc.DataSourceName != null)
                        {
                            if (!item.GetType().IsClass)
                                text = item;
                            else
                            {
                                var val = item.GetType().GetProperty(ItemDesc.DataSourceName).GetValue(item, null);
                                text = val.GetType().GetProperty(ItemDesc.DisplayMember).GetValue(val, null);
                            }
                        }
                        else
                        {
                            if (item.GetType() != typeof(string))
                                text = item.GetType().GetProperty(ItemDesc.DisplayMember).GetValue(item, null);
                            else
                                text = item.ToString();
                            //--тут надо передалть и убрать тип аттрибута!!!
                            if (ItemDesc.CustomAttributeType != null)
                            {
                                var custattrs = ItemDesc.CustomAttributeType.GetField(text.ToString()).GetCustomAttributes(false);
                                if (custattrs != null)
                                {
                                    foreach (var attr in custattrs)
                                    {
                                        var text2 = attr.GetType().GetProperty(ItemDesc.CustomAttributeValueName).GetValue(attr, null);
                                        if (text2 != null)
                                        {
                                            text = text2;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (text != null)
                        {
                            li = new ListViewItem(text.ToString());
                            li.Checked = ItemDesc.Checked;
                            if (!ItemDesc.IsDefaultSttyle())
                            {
                                li.Font = ItemDesc.Font;
                                li.ForeColor = ItemDesc.ForeColor;
                                li.BackColor = ItemDesc.BackColor;
                            }
                            if (string.IsNullOrEmpty(ItemDesc.ImageIndexByFieldName))
                                li.ImageIndex = ItemDesc.ImageIndex;
                            else
                            {
                                var im_idx = item.GetType().GetProperty(ItemDesc.ImageIndexByFieldName).GetValue(item, null);
                                if (im_idx != null && im_idx.GetType() == typeof(int))
                                    li.ImageIndex = (int)im_idx;
                            }
                            li.Tag = item;
                        }

                        if (ItemDesc.SubItems.Count > 0)
                        {
                            foreach (var subitem in ItemDesc.SubItems)
                            {
                                object text2 = null;
                                if (subitem.DataSourceName != null)
                                {
                                    if (item.GetType().GetProperty(subitem.DataSourceName) != null)
                                    {
                                        if (!item.GetType().IsClass)
                                            text2 = item;
                                        else
                                        {
                                            var val = item.GetType().GetProperty(subitem.DataSourceName).GetValue(item, null);
                                            if (val != null)
                                                text2 = val.GetType().GetProperty(subitem.DisplayMember).GetValue(val, null);
                                        }
                                    }
                                    else //--экспириметально
                                    {
                                        if (!item.GetType().IsClass)
                                            text2 = item;
                                        else
                                        {
                                            var val = item.GetType().GetProperty(subitem.DisplayMember).GetValue(item, null);
                                            if (val != null && (val.GetType() != typeof(string) & val.GetType() != typeof(DateTime) & val.GetType() != typeof(int)))
                                                text2 = val.GetType().GetProperty(subitem.DisplayMember).GetValue(val, null);
                                            else
                                                text2 = val;
                                        }
                                    }
                                }
                                else
                                {                                    
                                    text2 = item.GetType().GetProperty(subitem.DisplayMember).GetValue(item, null);
                                    if (subitem.CustomAttributeType != null)
                                    {
                                        var custattrs = subitem.CustomAttributeType.GetField(text2.ToString()).GetCustomAttributes(false);
                                        if (custattrs != null)
                                        {
                                            foreach (var attr in custattrs)
                                            {
                                                var text3 = attr.GetType().GetProperty(subitem.CustomAttributeValueName).GetValue(attr, null);
                                                if (text3 != null)
                                                {
                                                    text2 = text3;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (text2 != null)
                                {
                                    ListViewItem.ListViewSubItem si = new ListViewItem.ListViewSubItem();
                                    si.Text = text2.ToString();
                                    if (!subitem.IsDefaultSttyle())
                                    {
                                        si.Font = ItemDesc.Font;
                                        si.ForeColor = ItemDesc.ForeColor;
                                        si.BackColor = ItemDesc.BackColor;
                                    }
                                    li.SubItems.Add(si);
                                }
                            }
                        }

                        ctrl.Items.Add(li);
                    }
                }
                #endregion

                //--надо сделать еще классами, а не только коллекции!!!

                #region DataTable Case
                if (DataSource is DataTable)
                {
                    var table = (DataSource as DataTable);
                    //--captions:cap1;cap2;cap3...
                }
                #endregion
            }
        }

        /*public static void FillListControl(Control listControl, object view_data, string p)
        {
            throw new NotImplementedException();
        }*/
    }
}
