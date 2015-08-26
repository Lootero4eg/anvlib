using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

using anvlib.Utilites;

namespace anvlib.Classes
{  
    /// <summary>
    /// Базовый класс для инициализации TreeView через ControlFiller
    /// </summary>
    //[TypeConverter(typeof(TypedTreeViewMemberTypeConverter))]     
    public class TypedTreeViewDisplayMember
    {        
        private bool _withoutTag = false;
        private int _imageIndex = -1;
        private int _selectedImageIndex = -1;
        private int _stateImageIndex = -1;

        /// <summary>
        /// Нужен для визуализации, чтобы не пересоздавать название нодов заного
        /// </summary>
        //[Browsable(false)]
        [CategoryAttribute("Дизайн")]
        [Description("Имя ветки")]
        public string NodeName { get; set; }

        /// <summary>
        /// Имя ветки
        /// </summary>
        [CategoryAttribute("Источник данных и отображение")]
        [Description("Наименование ветки, отображаемое в дереве. Шаблон вида:\"<Имя ветки>: {value}\"")]
        public string DisplayСaption { get; set; }

        /// <summary>
        /// Какое поля класса выводить на экран
        /// Если надо выводить просто строку, как отдельную ветку дерева, то в это свойство надо писать
        /// "string:<Выводимая строка>"
        /// </summary>
        [CategoryAttribute("Источник данных и отображение")]
        [Description("Свойство класса, которое будет отображаться в Treeview как значение")]
        public string DisplayMember { get; set; }

        /// <summary>
        /// Источник данных
        /// </summary>
        [CategoryAttribute("Источник данных и отображение")]
        [Description("Источник данных: имя коллекции в классе или имя таблицы в датасете. Если сам источник является таблицей, то оставить пустым")]        
        public string DataSourceName { get; set; }

        /// <summary>
        /// Флаг указывающий контрол филлеру, чтобы он не заполнял поле тэг
        /// </summary>
        [CategoryAttribute("Разное")]
        [Description("Флаг указывающий контрол филлеру, чтобы он не заполнял поле тэг")] 
        public bool WithoutTag { get { return _withoutTag; } set { _withoutTag = value; } }

        /// <summary>
        /// Описание дочерних веток дерева
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]//--Без этого атрибута не будет работать запись в дизайнер!!!!!!!
        public TypedTreeViewDisplayMemberList ChildDisplayMembers { get; set; }

        /// <summary>
        /// Индекс картинки из ImaeList
        /// </summary>
        [CategoryAttribute("Отображение иконок")]
        [Description("Индекс картинки из ImaeList")]
        public int ImageIndex
        {
            get { return _imageIndex; }
            set 
            { 
                _imageIndex = value;
                SelectedImageIndex = _imageIndex;                
            }
        }
        
        /// <summary>
        /// Индекс картинки, когда элемент выделен
        /// </summary>
        [CategoryAttribute("Отображение иконок")]
        [Description("Индекс картинки, когда элемент выделен")]
        public int SelectedImageIndex
        {
            get { return _selectedImageIndex; }
            set { _selectedImageIndex = value; }
        }

        /// <summary>
        /// Индекс картинки из ImaeList
        /// </summary>
        [CategoryAttribute("Отображение иконок")]
        [Description("Индекс картинки состояния из ImaeList")]
        public int StateImageIndex
        {
            get { return _stateImageIndex; }
            set { _stateImageIndex = value; }
        }

        /// <summary>
        /// Имя поля из которго будет браться индекс картинки
        /// Поле обязательно должно быть типа "int"!
        /// Для всех имиджей State, Selected и обычный ImageIndex будет один номер!
        /// </summary>
        [CategoryAttribute("Отображение иконок")]
        [Description("Поле обязательно должно быть типа \"int\"!\r\nДля всех имиджей State, Selected и обычный ImageIndex будет один номер!")] 
        public string ImageIndexByFieldName { get; set; }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="type">Тип класса паррента в котором будем искать нашего дисплей мембера</param>
        /// <param name="DispMember">Какое поля класса выводить на экран</param>
        /// <param name="dsource_name">Источник данных</param>
        public TypedTreeViewDisplayMember(string DispMember, string dsource_name)
        {            
            DisplayMember = DispMember;
            DataSourceName = dsource_name;
            ChildDisplayMembers = new TypedTreeViewDisplayMemberList();
        }

        /// <summary>
        /// Базовый конструктор 2
        /// </summary>
        /// <param name="type">Тип класса паррента в котором будем искать нашего дисплей мембера</param>
        /// <param name="DispMember">Какое поля класса выводить на экран</param>
        /// <param name="dsource_name">Источник данных</param>
        /// <param name="childs">Описание дочерних веток дерева</param>
        public TypedTreeViewDisplayMember(string DispMember, string dsource_name, TypedTreeViewDisplayMemberList childs)
        {            
            DisplayMember = DispMember;
            DataSourceName = dsource_name;
            ChildDisplayMembers = childs;
        }
        
        public TypedTreeViewDisplayMember()
        {
            ChildDisplayMembers = new TypedTreeViewDisplayMemberList();
        }

        /// <summary>
        /// Метод установки индекса картинки, хотя можно и вручную поставить...
        /// </summary>
        /// <param name="index">Индекс картинки</param>
        public void SetSameImageIndex(int index)
        {
            ImageIndex = index;
            SelectedImageIndex = index;
            StateImageIndex = index;
        }

        public TypedTreeViewDisplayMember Clone()
        {
            TypedTreeViewDisplayMember res = new TypedTreeViewDisplayMember("", "");
            //res = (TypedTreeViewDisplayMemberNew)this.MemberwiseClone();
            res.DataSourceName = this.DataSourceName;
            res.DisplayMember = this.DisplayMember;
            res.DisplayСaption = this.DisplayСaption;
            res.ImageIndex = this.ImageIndex;
            res.ImageIndexByFieldName = this.ImageIndexByFieldName;
            res.NodeName = this.NodeName;
            res.SelectedImageIndex = this.SelectedImageIndex;
            res.StateImageIndex = this.StateImageIndex;
            res.WithoutTag = this.WithoutTag;
            foreach (var item in this.ChildDisplayMembers)            
                res.ChildDisplayMembers.Add(item.Clone());            

            return res;
        }
    }

    /// <summary>
    /// Коллекция веток дерева
    /// </summary>    
    public class TypedTreeViewDisplayMemberList : Collection<TypedTreeViewDisplayMember> 
    { 
        //написать методы добавления по одному дисплей мемберу, и если повторяется то записывать листья в массив
        //ну и для чайлодв тоже надо сделать!
        //public TypedTreeViewDisplayMemberNew GetTypedTreeViewDisplayMemberBy

        private bool _isItemAlreadyRemoved = false;
        private static int _nodescount;

        private object _parent;
        
        /// <summary>
        /// Эксперименатльное поле, наверное уберу
        /// </summary>
        public object Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        
        /// <summary>
        /// Поле только для внутреннего использования. Сколько не прятал его, оно не прячется...как сделать пока не знаю...
        /// </summary>
        [Bindable(false)]
        [Browsable(false)]                
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object AssembliesList { get; set; }

        public TypedTreeViewDisplayMemberList()
        { 
        }

        public TypedTreeViewDisplayMemberList Clone()
        {
            TypedTreeViewDisplayMemberList res = new TypedTreeViewDisplayMemberList();            
            res = new TypedTreeViewDisplayMemberList();
            res.Parent = this.Parent;
            res.AssembliesList = this.AssembliesList;

            foreach (var item in this.Items)
                res.Add(item.Clone());

            return res;
        }

        public new void Remove(TypedTreeViewDisplayMember item)
        {
            bool removed = false;

            if (this.Contains(item))
                removed = base.Remove(item);

            if (!removed)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    bool found = IsNeededItem(this.Items[i], item);
                    if (found)
                        this.Items[i].ChildDisplayMembers.Remove(item.ChildDisplayMembers[i]);
                    if (_isItemAlreadyRemoved)
                    {
                        _isItemAlreadyRemoved = false;
                        break;
                    }
                }
            }
        }
        
        private bool IsNeededItem(TypedTreeViewDisplayMember child, TypedTreeViewDisplayMember searchitem)
        { 
            if (child == searchitem)
                return true;
            else if (child.ChildDisplayMembers.Count > 0)
            {
                for (int i = 0; i < child.ChildDisplayMembers.Count; i++)
                {
                    bool found = IsNeededItem(child.ChildDisplayMembers[i],searchitem);
                    if (found && !_isItemAlreadyRemoved)
                    {
                        child.ChildDisplayMembers.Remove(child.ChildDisplayMembers[i]);
                        _isItemAlreadyRemoved = true;
                        return false;
                    }

                    if (_isItemAlreadyRemoved)
                        break;
                }
            }

            return false;
        }

        //--максимум 2 вложенности для всего!
        public static TypedTreeViewDisplayMemberList GenerateTreeDescriptionFromObject(object obj,object parent)
        {
            TypedTreeViewDisplayMemberList res = new TypedTreeViewDisplayMemberList();
#warning Надо сделать правильную проверку и работу с коллецкиями!!!
            /*Type tp = anvlib.Utilites.TypeSystem.GetElementType(obj.GetType());
            if (tp != obj.GetType())
                obj = Activator.CreateInstance(tp);*/
            if (parent == null)
                _nodescount = 0;
            //--пока работает только по свойствам
            foreach (var item in obj.GetType().GetProperties())
            {
                bool standard = false;
                bool generic = false;
                bool array = false;

                //--Standard type
                if (TypesHelper.IsStandardType(item.PropertyType))
                {
                    if (parent == null)
                        res.Add(GetStandardTypeDescription(item.Name));
                    else
                    {
                        var tvdm = GetStandardTypeDescription(item.Name);
                        tvdm.DataSourceName = parent.ToString();
                        res.Add(tvdm);
                    }
                    standard = true;
                }

                #region Generic type
                if (item.PropertyType.IsGenericType)
                {
                    TypedTreeViewDisplayMember tvdm2 = GetStandardTypeDescription("");
                    tvdm2.DisplayСaption = item.Name;
                    TypedTreeViewDisplayMember tvdm = GetGenericChild(tvdm2, item, item.PropertyType);
                    res.Add(tvdm);
                    generic = true;
                }
                #endregion

                #region Array Type
                if (item.PropertyType.IsArray)
                {
                    Type eltype = item.PropertyType.GetElementType();
                    if (TypesHelper.IsStandardType(eltype))
                    {
                        var ntvdm = GetStandardTypeDescription("");
                        ntvdm.DisplayСaption = item.Name;
                        ntvdm.ChildDisplayMembers = new TypedTreeViewDisplayMemberList();
                        var child = new TypedTreeViewDisplayMember("", item.Name);
                        child.NodeName = string.Format("Node{0}", _nodescount);
                        _nodescount++;
                        ntvdm.ChildDisplayMembers.Add(child);                        
                        res.Add(ntvdm);
                    }
                    
                    if (eltype.IsGenericType)
                    {
                        TypedTreeViewDisplayMember tvdm2 = GetStandardTypeDescription("");
                        tvdm2.DisplayСaption = item.Name;
                        TypedTreeViewDisplayMember tvdm3 = new TypedTreeViewDisplayMember("", item.Name);
                        tvdm3.DisplayСaption = "Collection";
                        tvdm3.NodeName = string.Format("Node{0}", _nodescount);
                        _nodescount++;
                        tvdm2.ChildDisplayMembers.Add(tvdm3);
                        TypedTreeViewDisplayMember tvdm = GetGenericChild(tvdm3, item, eltype);
                        res.Add(tvdm2);
                    }

                    array = true;
                }
                #endregion

                #region Class type
                if (!standard && !generic && !array)
                {
                    TypedTreeViewDisplayMember tvdm = GetStandardTypeDescription("");
                    tvdm.DisplayСaption = item.Name;
                    try
                    {
                        var obj_instance = item.GetValue(obj, null);
                        tvdm.ChildDisplayMembers = GenerateTreeDescriptionFromObject(obj_instance, item.Name);
                        res.Add(tvdm);
                    }
                    catch 
                    {                        
                    }
                }
                #endregion
            }

            return res;
        }        

        private static TypedTreeViewDisplayMember GetGenericChild(TypedTreeViewDisplayMember tvdm, object obj, Type propType)
        {            
            Type[] types;
            if (propType.IsGenericType)
                types = (propType as Type).GetGenericArguments();
            else//--это вообще не используется! убери это при первой же возможности
            {
                types = new Type[1];
                types[0] = propType;
            }
            
            foreach (var type in types)
            {
                if (type.IsGenericType)
                {                    
                    TypedTreeViewDisplayMember tvdm2 = new TypedTreeViewDisplayMember("", (obj as PropertyInfo).Name);
                    tvdm2.DisplayСaption = "Collection";
                    tvdm2.NodeName = string.Format("Node{0}", _nodescount);
                    _nodescount++;
                    tvdm2 = GetGenericChild(tvdm2, obj, type);
                    if (tvdm.ChildDisplayMembers == null)
                        tvdm.ChildDisplayMembers = new TypedTreeViewDisplayMemberList();                    
                    tvdm.ChildDisplayMembers.Add(tvdm2);
                }
                if (TypesHelper.IsStandardType(type))
                {
                    if (tvdm.ChildDisplayMembers == null)
                        tvdm.ChildDisplayMembers = new TypedTreeViewDisplayMemberList();
                    TypedTreeViewDisplayMember tvdm2 = new TypedTreeViewDisplayMember("", (obj as PropertyInfo).Name);
                    tvdm2.NodeName = string.Format("Node{0}", _nodescount);
                    _nodescount++;
                    tvdm.ChildDisplayMembers.Add(tvdm2);
                }
            }

            return tvdm;
        }

        private static TypedTreeViewDisplayMember GetStandardTypeDescription(string Name)
        {
            TypedTreeViewDisplayMember tvdm = new TypedTreeViewDisplayMember(Name, null);
            tvdm.NodeName = string.Format("Node{0}", _nodescount);
            _nodescount++;
            return tvdm;
        }
    }
}
