using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

namespace anvlib.Classes
{    
    /// <summary>
    /// Вспомогательный класс по заполнению различных контролсов, информацией из любых классов
    /// </summary>
    public static class ControlFillerWPF
    {
        /// <summary>
        /// Метод заполняющий контролсы типа ListControl(ListBox,ComboBox,CheckedListBox)
        /// </summary>
        /// <param name="ctrl">Заполняемый контролс</param>
        /// <param name="DataSource">Источник данных. Коллекция или объект</param>
        /// <param name="DisplayMember">Выводимый элемент объекта на экран</param>
        /// <param name="GenerateItems">Флаг генерации элементов или считывание из объекта.</param>
        public static void FillListControl(object ctrl, object DataSource, string DisplayMember, bool GenerateItems)
        {                        
            if (ctrl is ComboBox)
            {
                var cbox = (ctrl as ComboBox);
                if (!GenerateItems)
                {
                    cbox.ItemsSource = null;
                    cbox.ItemsSource = (DataSource as System.Collections.IEnumerable);
                    cbox.DisplayMemberPath = DisplayMember;
                }
                else
                {
                    if (DataSource is System.Collections.IEnumerable)
                    {
                        System.Collections.IEnumerable col = (DataSource as System.Collections.IEnumerable);
                        foreach (var item in col)
                        {
                            PropertyInfo pinfo = item.GetType().GetProperty(DisplayMember);
                            object dispitem = pinfo.GetValue(item, null);
                            if (dispitem != null)
                                cbox.Items.Add(dispitem);
                        }
                    }
                }
            }

            if (ctrl is ListBox)
            {
                var lbox = (ctrl as ListBox);
                if (!GenerateItems)
                {
                    lbox.ItemsSource = null;
                    lbox.ItemsSource = (DataSource as System.Collections.IEnumerable);
                    lbox.DisplayMemberPath = DisplayMember;
                }
                else
                {
                    if (DataSource is System.Collections.IEnumerable)
                    {
                        System.Collections.IEnumerable col = (DataSource as System.Collections.IEnumerable);
                        foreach (var item in col)
                        {
                            PropertyInfo pinfo = item.GetType().GetProperty(DisplayMember);
                            object dispitem = pinfo.GetValue(item, null);
                            if (dispitem != null)
                                lbox.Items.Add(dispitem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Альтернативный метод по заполнению контролсов типа ListControl
        /// </summary>
        /// <param name="ctrl">Сам контрол</param>
        /// <param name="Items">Набор строк, которые мы будем отображать</param>
        public static void FillListControl(object ctrl, params string[] Items)
        {            
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
            TreeViewItem tr = new TreeViewItem();
            tr = CreateLeafs(tr, DataSource, TreeLeafsInfo);            
            treeview.ItemsSource = tr.Items;
        }
        
        /// <summary>
        /// Добавочный метод к FillTreeViewControl, рекурсивно заполняющий TreeView
        /// </summary>
        /// <param name="tr">TreeView контролс</param>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="TreeLeafsInfo">Описание дерева</param>
        /// <returns></returns>
        private static TreeViewItem CreateLeafs(TreeViewItem tr, object DataSource, TypedTreeViewDisplayMemberList TreeLeafsInfo)
        {
            foreach (var Leaf in TreeLeafsInfo)
            {
                if ((DataSource is System.Collections.IEnumerable) && DataSource.GetType() != typeof(string))
                {
                    var collection = (DataSource as System.Collections.IEnumerable);
                    foreach (var item in collection)
                    {
                        object text;
                        if (item.GetType().GetProperty(Leaf.DisplayMember) == null && Leaf.DataSourceName != null)
                        {
                            if (!item.GetType().IsClass)
                                text = item;
                            else
                            {
                                var val = item.GetType().GetProperty(Leaf.DataSourceName).GetValue(item, null);
                                text = val.GetType().GetProperty(Leaf.DisplayMember).GetValue(val, null);
                            }
                        }
                        else
                            text = item.GetType().GetProperty(Leaf.DisplayMember).GetValue(item, null);
                        if (text != null)
                        {
                            TreeViewItem mnode = new TreeViewItem();
                            mnode.Header = text.ToString();
                            mnode.Tag = item;
                            /*mnode.ImageIndex = Leaf.ImageIndex;
                            mnode.SelectedImageIndex = Leaf.SelectedImageIndex;
                            mnode.StateImageIndex = Leaf.StateImageIndex;*/
                            tr.Items.Add(mnode);
                            if (Leaf.ChildDisplayMembers != null)
                            {
                                foreach (var SubLeaf in Leaf.ChildDisplayMembers)
                                {
                                    if (Leaf.DataSourceName != SubLeaf.DataSourceName)
                                    {
                                        var innercol = item.GetType().GetProperty(SubLeaf.DataSourceName).GetValue(item, null);
                                        TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                        tdml.Add(SubLeaf);
                                        CreateLeafs(mnode, innercol, tdml);
                                    }
                                    else
                                    {
                                        TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                        tdml.Add(SubLeaf);
                                        CreateLeafs(mnode, item, tdml);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (DataSource != null)
                    {
                        object text;
                        if (!(DataSource is ValueType))
                            text = DataSource.GetType().GetProperty(Leaf.DisplayMember).GetValue(DataSource, null);
                        else
                            text = DataSource;
                        if (text != null)
                        {
                            TreeViewItem mnode = new TreeViewItem();
                            mnode.Header = text.ToString();
                            mnode.Tag = DataSource;
                            /*mnode.ImageIndex = Leaf.ImageIndex;
                            mnode.SelectedImageIndex = Leaf.SelectedImageIndex;
                            mnode.StateImageIndex = Leaf.StateImageIndex;*/
                            tr.Items.Add(mnode);
                            if (Leaf.ChildDisplayMembers != null)
                            {
                                foreach (var SubLeaf in Leaf.ChildDisplayMembers)
                                {
                                    var innercol = DataSource.GetType().GetProperty(SubLeaf.DataSourceName).GetValue(DataSource, null);
                                    TypedTreeViewDisplayMemberList tdml = new TypedTreeViewDisplayMemberList();
                                    tdml.Add(SubLeaf);
                                    CreateLeafs(mnode, innercol, Leaf.ChildDisplayMembers);
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
        /*public static void FillListView(ListView ctrl, object DataSource, TypedListViewDisplayMember ItemDesc)
        {
            if (ItemDesc != null)
            {
                ListViewItem li = new ListViewItem();

                if ((DataSource is System.Collections.IEnumerable) && DataSource.GetType() != typeof(string))
                {
                    var collection = (DataSource as System.Collections.IEnumerable);
                    foreach (var item in collection)
                    {
                        if (item.GetType() == ItemDesc.DisplayMemberType)
                        { 
                            object text;
                            if (item.GetType().GetProperty(ItemDesc.DisplayMember) == null && ItemDesc.DataSourceName != null)
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
                                text = item.GetType().GetProperty(ItemDesc.DisplayMember).GetValue(item, null);
                            if (text != null)
                            {
                                li = new ListViewItem(text.ToString());
                                li.Checked = ItemDesc.Checked;
                                if (!ItemDesc.IsDefaultSttyle())
                                {
                                    li.Font = ItemDesc.Font;
                                    li.ForeColor = ItemDesc.ForeColor;
                                    li.BackColor = ItemDesc.BackColor;
                                    li.ImageIndex = ItemDesc.ImageIndex;
                                }
                                li.Tag = item;
                            }

                            if (ItemDesc.SubItems.Count > 0)
                            {
                                foreach (var subitem in ItemDesc.SubItems)
                                {
                                    object text2 = null;
                                    if (item.GetType().GetProperty(subitem.DisplayMember) == null && subitem.DataSourceName != null)
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
                                    else
                                        text2 = item.GetType().GetProperty(subitem.DisplayMember).GetValue(item, null);
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
                }
            }
        }       */
    }
}
