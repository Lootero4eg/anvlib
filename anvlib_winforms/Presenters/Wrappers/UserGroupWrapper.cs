using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using anvlib.Presenters;
using anvlib.Presenters.Interfaces;
using anvlib.Interfaces;
using anvlib.Controls;

namespace anvlib.Presenters.Wrappers
{
    public class UserGroupWrapper : BasePresenterWrapper<BaseUserGroupPresenter>, IUserGroupControl, IUserGroupPresenter
    {
        private object selectedGroup;
        private object selectedUser;
        
        private bool _movingUser = true;

        public bool MoveUsers
        {
            get { return _movingUser; }
            set { _movingUser = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object SelectedGroup
        {
            get { return selectedGroup; }
            set { selectedGroup = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value; }
        }

        public void IsPresenterPrepared()
        {
            if (Presenter == null)
                throw new Exception("Working with null Presenter not possible!!!");
        }

        public void SetGroupControl(Control GrControl)
        {
            IsPresenterPrepared();
            Presenter.SetGroupControl(GrControl);
        }

        public void SetUserControl(Control UsrControl)
        {
            IsPresenterPrepared();
            Presenter.SetUserControl(UsrControl);
        }

        #region Groups Methods

        public void AddGroup(object Group)
        {
            IsPresenterPrepared();
            Presenter.AddGroup(Group);
        }

        public void EditGroup(object Group)
        {
            IsPresenterPrepared();
            Presenter.EditGroup(Group);
        }

        public void RemoveGroup(object Group)
        {
            IsPresenterPrepared();
            Presenter.RemoveGroup(Group);
        }

        public void GroupAddUser(object Group, object User, bool MoveFromUList)
        {
            IsPresenterPrepared();
            Presenter.GroupAddUser(Group, User, MoveFromUList);
        }

        public void GroupRemoveUser(object Group, object User, bool RestoreInUserList)
        {
            IsPresenterPrepared();
            Presenter.GroupRemoveUser(Group, User, RestoreInUserList);
        }

        #endregion

        #region Users Methods

        public void AddUser(object User)
        {
            IsPresenterPrepared();
            Presenter.AddUser(User);
        }

        public void EditUser(object User)
        {
            IsPresenterPrepared();
            Presenter.EditUser(User);
        }

        public void RemoveUser(object User)
        {
            IsPresenterPrepared();
            Presenter.RemoveUser(User);
        }

        public object CreateNewGroupObject()
        {
            return Presenter.CreateNewGroupObject();
        }
        
        public object CreateNewUserObject()
        {
            return Presenter.CreateNewUserObject();
        }

        #endregion

        #region Misc Methods

        public virtual AddEditContextMenuStrip CreateGroupContextMenu(string add_caption, string edit_caption, string remove_caption, bool is_addb_disabled)
        {
            AddEditContextMenuStrip res = new AddEditContextMenuStrip(is_addb_disabled);
            res.CreateAddMenuItem(add_caption, AddGroupButtonClick);
            res.CreateEditMenuItem(edit_caption, EditGroupButtonClick);
            res.CreateRemoveMenuItem(remove_caption, RemoveGroupButtonClick);

            return res;
        }

        public virtual AddEditContextMenuStrip CreateUserContextMenu(string add_caption, string edit_caption, string remove_caption, bool is_addb_disabled)
        {
            AddEditContextMenuStrip res = new AddEditContextMenuStrip(is_addb_disabled);
            res.CreateAddMenuItem(add_caption, AddUserButtonClick);
            res.CreateEditMenuItem(edit_caption, EditUserButtonClick);
            res.CreateRemoveMenuItem(remove_caption, RemoveUserButtonClick);

            return res;
        }

        public void SetGroupsAddEditControl(Control AddEdCrtl)
        {
            Presenter.SetGroupsAddEditControl(AddEdCrtl);
        }
        
        public void SetUsersAddEditControl(Control AddEdCrtl)
        {
            Presenter.SetUsersAddEditControl(AddEdCrtl);
        }

        public Type GetGroupType()
        {
            return Presenter.GetGroupType();
        }

        public Type GetUserType()
        {
            return Presenter.GetUserType();
        }

        #endregion

        #region Event handlers

        public void AddGroupButtonClick(object sender, EventArgs e)
        {
            AddGroup(Presenter.CreateNewGroupObject());
        }

        public void EditGroupButtonClick(object sender, EventArgs e)
        {
            EditGroup(SelectedGroup);
        }

        public void RemoveGroupButtonClick(object sender, EventArgs e)
        {
            RemoveGroup(SelectedGroup);
        }

        public void AddUserButtonClick(object sender, EventArgs e)
        {
            AddUser(Presenter.CreateNewUserObject());
        }

        public void EditUserButtonClick(object sender, EventArgs e)
        {
            EditUser(SelectedUser);
        }

        public void RemoveUserButtonClick(object sender, EventArgs e)
        {
            RemoveUser(SelectedUser);
        }

        public void GroupAddUserButtonClick(object sender, EventArgs e)
        {
            GroupAddUser(selectedGroup, selectedUser, MoveUsers);
        }

        public void GroupRemoveUserButtonClick(object sender, EventArgs e)
        {
            GroupRemoveUser(selectedGroup, selectedUser, MoveUsers);
        }

        #endregion
    }
}
