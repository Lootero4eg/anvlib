using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using anvlib.Enums;
using anvlib.Interfaces;
using anvlib.Classes;

namespace anvlib.Presenters
{
    /// <summary>
    /// Базовый класс для презентеров Пользователей и Групп
    /// </summary>
    public abstract class BaseUserGroupPresenter : BaseFormPresenter
    {
        /// <summary>
        /// Группы
        /// </summary>
        protected object Groups;

        /// <summary>
        /// Пользователи
        /// </summary>
        protected object Users;

        //--могут быть просто индексы

        /// <summary>
        /// Выбранная группа или ее индекс
        /// </summary>
        protected object SelectedGroup;

        /// <summary>
        /// Выбранный пользователь или его индекс
        /// </summary>
        protected object SelectedUser;

        /// <summary>
        /// Представление данных для групп
        /// </summary>
        protected Control GroupControl;

        /// <summary>
        /// Представление данных для пользователей
        /// </summary>
        protected Control UserControl;

        /// <summary>
        /// Форма или контролс редактор групп
        /// </summary>
        protected Control _groupsAddEditControl;

        /// <summary>
        /// Форма или контролс редактор польщователей
        /// </summary>
        protected Control _usersAddEditControl;

        /// <summary>
        /// Тип презентера(одиночный или сдвоенный)
        /// </summary>
        protected PresenterEnums.UserGroupView presenterViewType = PresenterEnums.UserGroupView.SingleView;

        /// <summary>
        /// Тип презентера(одиночный или сдвоенный)
        /// </summary>
        public PresenterEnums.UserGroupView PresenterViewType
        {
            get { return presenterViewType; }
        }

        /// <summary>
        /// Возвращае тип конкретной группы
        /// </summary>
        /// <returns></returns>
        public virtual Type GetGroupType()
        {
            if (SelectedGroup != null)
                return SelectedGroup.GetType();
            else
                return typeof(object);
        }

        /// <summary>
        /// Возвращает тип конкретного пользователя
        /// </summary>
        /// <returns></returns>
        public virtual Type GetUserType()
        {
            if (SelectedUser != null)
                return SelectedUser.GetType();
            else
                return typeof(object);
        }

        #region Presenter Init Methods

        /// <summary>
        /// Метод проверяющий подготовленности для дальнейшей работы
        /// </summary>
        protected void IsAllPrepared()
        {
            if (presenterViewType == PresenterEnums.UserGroupView.SingleView)
            {
                if (GroupControl == null || UserControl == null)
                    throw new Exception("You must intialize one of controls");
            }

            if (presenterViewType == PresenterEnums.UserGroupView.MultiView)
            {
                if (GroupControl == null && UserControl == null)
                    throw new Exception("You must intialize one of controls");
            }
        }

        /// <summary>
        /// Метод устанавливающий контролс для групп
        /// </summary>
        /// <param name="GrCrtl"></param>
        public void SetGroupControl(Control GrCrtl)
        {
            GroupControl = GrCrtl;
        }

        /// <summary>
        /// Метод устанавливающий контролс для групп
        /// </summary>
        /// <param name="UsrCrtl"></param>
        public void SetUserControl(Control UsrCrtl)
        {
            UserControl = UsrCrtl;
        }

        /// <summary>
        /// Метод устанавливающий Редактируемую форму или контролс для группы
        /// </summary>
        /// <param name="AddEdCrtl"></param>
        public void SetGroupsAddEditControl(Control AddEdCrtl)
        {
            _groupsAddEditControl = AddEdCrtl;
        }

        /// <summary>
        /// Метод устанавливающий Редактируемую форму или контролс для ползователя
        /// </summary>
        /// <param name="AddEdCrtl"></param>
        public void SetUsersAddEditControl(Control AddEdCrtl)
        {
            _usersAddEditControl = AddEdCrtl;
        }

        /// <summary>
        /// Метод создающий Новую Группу
        /// </summary>
        /// <returns></returns>
        public virtual object CreateNewGroupObject()
        {
            return null;
        }

        /// <summary>
        /// Метод создающий Нового пользователя
        /// </summary>
        /// <returns></returns>
        public virtual object CreateNewUserObject()
        {
            return null;
        }

        #endregion

        protected abstract void FillGroups();
        protected abstract void FillUsers();

        /// <summary>
        /// Метод добавление новой группы с вызовом формы или контролса
        /// </summary>
        /// <param name="Group"></param>
        public virtual void AddGroup(object Group)
        {
            EditGroup(Group);
        }

        /// <summary>
        /// Метод редактирования группы с вызовом формы или контролса
        /// </summary>
        /// <param name="Group"></param>
        public virtual void EditGroup(object Group)
        {
            if (_groupsAddEditControl is Form)
            {
                if (_groupsAddEditControl is IAddEditCommonForm)
                {
                    (_groupsAddEditControl as IAddEditCommonForm).SetItemForEditing(Group);
                    (_groupsAddEditControl as IAddEditCommonForm).ShowDialog();
                }
                else
                    (_groupsAddEditControl as Form).ShowDialog();
            }
        }

        /// <summary>
        /// Метод удаления группы
        /// Пока не реализован
        /// </summary>
        /// <param name="Group"></param>
        public virtual void RemoveGroup(object Group)
        {
        }

        /// <summary>
        /// Метод добавление нового пользователя с вызовом формы или контролса
        /// </summary>
        /// <param name="User"></param>
        public virtual void AddUser(object User)
        {
            EditUser(User);
        }

        /// <summary>
        /// Метод редактирования пользователя с вызовом формы или контролса
        /// </summary>
        /// <param name="User"></param>
        public virtual void EditUser(object User)
        {
            if (_usersAddEditControl is Form)
            {
                if (_usersAddEditControl is IAddEditCommonForm)
                {
                    (_usersAddEditControl as IAddEditCommonForm).SetItemForEditing(User);
                    (_usersAddEditControl as IAddEditCommonForm).ShowDialog();
                }
                else
                    (_usersAddEditControl as Form).ShowDialog();
            }
        }

        /// <summary>
        /// Метод удаления пользователя.
        /// Не реализован
        /// </summary>
        /// <param name="User"></param>
        public virtual void RemoveUser(object User) { }

        /// <summary>
        /// Метод добавления пользователя в группу
        /// </summary>
        /// <param name="Group">Группа, в которую будем добавлять</param>
        /// <param name="User">Добавляемый пользователь</param>
        /// <param name="MoveFromUList">Перенести из пользовательского списка и удалить его от туда, оставив только в группе, иначе будет копирование</param>
        public virtual void GroupAddUser(object Group, object User, bool MoveFromUList) { }

        /// <summary>
        /// Метод удаления пользователя из группы
        /// </summary>
        /// <param name="Group">Группа из которой будем удалять</param>
        /// <param name="User">Удаляемый пользователь</param>
        /// <param name="RestoreInUserList">После удаления, вернуть пользователя в пользовательский лист</param>
        public virtual void GroupRemoveUser(object Group, object User, bool RestoreInUserList) { }

        /// <summary>
        /// Метод установки Вида презентера, одно или двух модульый
        /// </summary>
        /// <param name="ViewType"></param>
        public void SetPresenterView(PresenterEnums.UserGroupView ViewType)
        {
            presenterViewType = ViewType;
        }

        public abstract void FillGroupControl();

        /// <summary>
        /// Метод заполняющий контролсы типа ListControl
        /// </summary>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="DisplayMember">Отображаемый элемент</param>
        /// <param name="GenerateItems">Генерировать элементы или брать их из источника данных</param>
        protected void FillListControl(ListControl ctrl, object DataSource, string DisplayMember, bool GenerateItems)
        {
            ControlFiller.FillListControl(ctrl, DataSource, DisplayMember, GenerateItems);
        }

        /// <summary>
        /// Метод заполняющий контролсы типа TreeView
        /// </summary>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="TreeLeafsInfo">Описание дерева</param>
        protected virtual void FillTreeView(TreeView ctrl, object DataSource, TypedTreeViewDisplayMemberList TreeLeafsInfo)
        {
            ControlFiller.FillTreeViewControl(ctrl, DataSource, TreeLeafsInfo);
        }

        /// <summary>
        /// Метод заполняющий контролсы типа ListView
        /// </summary>
        /// <param name="DataSource">Источник данных</param>
        /// <param name="ListViewInfo">Описание списка выводимых объектов</param>
        public virtual void FillListView(ListView ctrl, object DataSource, TypedListViewDisplayMember ListViewInfo)
        {
            ControlFiller.FillListView(ctrl, DataSource, ListViewInfo);
        }

        public abstract void FillUserControl();
    }
}
