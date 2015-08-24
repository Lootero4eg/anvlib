using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace anvlib.Classes
{
    //--Если в сабитемах DataSource = null - это значит, что данные надо искать в датасоурце основного элемента
    /// <summary>
    /// Базовый класс для инициализации ListView через ControlFiller
    /// </summary>
    public class BaseListViewItem
    {
        protected bool _defaultStyles = true;
        private Font _font;
        private Color _foreColor;
        private Color _backColor;
        
        /// <summary>
        /// Имя свойства класса выводимое на экран
        /// </summary>
        public string DisplayMember { get; set; }

        /// <summary>
        /// Источник данных
        /// </summary>
        public string DataSourceName { get; set; }

        /// <summary>
        /// Имя элемента в списке
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Шрифт
        /// </summary>
        public Font Font 
        {
            get
            {
                return _font;
            }

            set
            {
                _font = value;
                _defaultStyles = false;
            }
        }

        /// <summary>
        /// Основной цвет
        /// </summary>
        public Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                _defaultStyles = false;
            }
        }

        /// <summary>
        /// Цвет фона
        /// </summary>
        public Color BackColor 
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                _defaultStyles = false;
            }
        }

        /// <summary>
        /// Метод проверки стиля по умолчанию
        /// </summary>
        /// <returns></returns>
        public bool IsDefaultSttyle()
        {            
            return _defaultStyles;
        }

        /// <summary>
        /// Тип для аттрибута который надо возвращать вместо самого Property
        /// </summary>
        public Type CustomAttributeType { get; set; }

        /// <summary>
        /// Имя переменной аттрибута которое надо возвращать
        /// </summary>
        public string CustomAttributeValueName { get; set; }

        /// <summary>
        /// Имя поля из которго будет браться индекс картинки
        /// Поле обязательно должно быть типа "int"!
        /// </summary>
        public string ImageIndexByFieldName { get; set; }

        /// <summary>
        /// Пустой базовый конструктор
        /// </summary>
        public BaseListViewItem()
        { 
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="type">Тип класса(у сабов можно сделать нулл)</param>
        /// <param name="DispMember">Какое поля класса выводить на экран</param>
        /// <param name="dsource_name">Источник данных</param>
        public BaseListViewItem(string DispMember, string dsource_name)
        {            
            DisplayMember = DispMember;
            DataSourceName = dsource_name;
        }
    }

    //--Придумать как привязать сюда еще и группы, пока просто небыло такой необходимости, да и не работал с ними никогда...
    public class TypedListViewDisplayMember: BaseListViewItem
    {
        private BaseListViewItemsCollection _subitems = new BaseListViewItemsCollection();

        /// <summary>
        /// Поле только для внутреннего использования. Сколько не прятал его, оно не прячется...как сделать пока не знаю...
        /// </summary>
        [Bindable(false)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object AssembliesList { get; set; }

        /// <summary>
        /// Крыжик у ListViewItem-а
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Индекс картинки из ImageList
        /// </summary>
        public int ImageIndex { get; set; }        

        /// <summary>
        /// Тултип, тока что-то не работал когда я проверял...
        /// </summary>
        public string ToolTipText { get; set; }

        /// <summary>
        /// Внутренние элементы объекта, если режим "Details" и настроены Columns
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BaseListViewItemsCollection SubItems
        {
            get { return _subitems; }
            set { _subitems = value; }
        }

        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public TypedListViewDisplayMember()//: base()
        {
            ImageIndex = -1;
            if (SubItems == null)
                SubItems = new BaseListViewItemsCollection();
            AssembliesList = null;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type">Тип класса(у сабов можно сделать нулл)</param>
        /// <param name="DispMember">Какое поля класса выводить на экран</param>
        /// <param name="dsource_name">Источник данных</param>
        public TypedListViewDisplayMember(string DispMember, string dsource_name):base(DispMember, dsource_name)
        {            
            ImageIndex = -1;
            if (SubItems == null)
                SubItems = new BaseListViewItemsCollection();
        }

        /// <summary>
        /// Метод установки индекса картинки, хотя можно и вручную поставить...
        /// </summary>
        /// <param name="index">Индекс картинки</param>
        public void SetImageIndex(int index)
        {
            ImageIndex = index;            
            _defaultStyles = false;
        }

        public TypedListViewDisplayMember Clone()
        {
            TypedListViewDisplayMember res = new TypedListViewDisplayMember();
            res._defaultStyles = this._defaultStyles;
            res.BackColor = this.BackColor;
            res.Checked = this.Checked;
            res.CustomAttributeType = this.CustomAttributeType;
            res.CustomAttributeValueName = this.CustomAttributeValueName;
            res.DataSourceName = this.DataSourceName;
            res.DisplayMember = this.DisplayMember;
            res.Caption = this.Caption;
            //res.SubItems = this.SubItems;
            res.SubItems = new BaseListViewItemsCollection();
            foreach (var item in this.SubItems)
            {
                BaseListViewItem newitem = new BaseListViewItem(item.DisplayMember, item.DataSourceName);
                newitem.Caption = item.Caption;
                newitem.BackColor = item.BackColor;
                newitem.CustomAttributeType = item.CustomAttributeType;
                newitem.CustomAttributeValueName = item.CustomAttributeValueName;
                newitem.Font = item.Font;
                newitem.ForeColor = item.ForeColor;
                newitem.ImageIndexByFieldName = item.ImageIndexByFieldName;
                res.SubItems.Add(newitem);
            }

            return res;
        }
    }

    /// <summary>
    /// Опеределение коллекции сабитемов
    /// </summary>
    public class BaseListViewItemsCollection : Collection<BaseListViewItem> { }

    /// <summary>
    /// Опеределение коллекции основных итемов, правда я думаю она никогда не будет использоваться, 
    /// поэтому пока полежит в комментариях
    /// </summary>
    /*public class TypedListViewDisplayMemberList : Collection<TypedListViewDisplayMember> 
    { 
    }*/
}
