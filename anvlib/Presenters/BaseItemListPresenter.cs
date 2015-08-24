using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using anvlib.Classes;

namespace anvlib.Presenters
{
    /// <summary>
    /// Базовый презентер для Представления данных в списках
    /// </summary>
    public abstract class BaseItemListPresenter: BasePresenter
    {
        /// <summary>
        /// Ссылка на лист контрол, просто для полной совместимости
        /// </summary>
        protected object view { get { return listControl; } }

        /// <summary>
        /// Данные для представления
        /// </summary>
        protected object view_data = null;

        /// <summary>
        /// Выбранный элемент списка
        /// </summary>
        public object SelectedItem { get; protected set; }

        /// <summary>
        /// Контролс управляющий списком
        /// </summary>
        protected Control listControl { get; private set; }

        /// <summary>
        /// Метод проверяющий установлен ли управляющий контролс
        /// </summary>
        public void IsPresenterPrepared()
        {            
            if (listControl == null)
                throw new Exception("Working with null ListControl not possible!!!");
        }

        public abstract void FillList();

        /// <summary>
        /// Метод заполняющий контролсы типа ListControl
        /// </summary>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="DisplayMember">Отображаемый элемент</param>
        /// <param name="GenerateItems">Генерировать элементы или брать их из источника данных</param>
        protected virtual void FillListBoxLikeControl(object DataSource, string DisplayMember, bool GenerateItems)
        {
            ControlFiller.FillListControl((listControl as ListControl), DataSource, DisplayMember, GenerateItems);
        }

        /// <summary>
        /// Метод заполняющий контролсы типа TreeView
        /// </summary>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="TreeLeafsInfo">Описание дерева</param>
        protected virtual void FillTreeView(object DataSource, TypedTreeViewDisplayMemberList TreeLeafsInfo)
        {
            ControlFiller.FillTreeViewControl((listControl as TreeView), DataSource, TreeLeafsInfo);
        }

        /// <summary>
        /// Метод заполняющий контролсы типа ListView
        /// </summary>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="ListViewInfo">Описание списка выводимых объектов</param>
        protected virtual void FillListView(object DataSource, TypedListViewDisplayMember ListViewInfo)
        {
            ControlFiller.FillListView((listControl as ListView), DataSource, ListViewInfo);
        }

        protected abstract void ClearItems();

        public virtual void SetEditableItem(object Item)
        {
            SelectedItem = Item;
        }

        public abstract object CreateNewListItem();

        /// <summary>
        /// Метод установки рабочего контролса
        /// </summary>
        /// <param name="ctrl"></param>
        public void SetListControl(Control ctrl)
        {
            listControl = ctrl;
        }
    }
}
