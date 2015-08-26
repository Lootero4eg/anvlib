using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using anvlib.Interfaces;
using anvlib.Presenters;
using anvlib.Presenters.Wrappers;
using anvlib.Presenters.Interfaces;

namespace anvlib.Controls
{    
    [DesignTimeVisibleAttribute(true)]    
    public partial class BaseUserGroupControl : UserControl//, IUserGroupControl
    {              
        protected static UserGroupWrapper _usergroup_presenter = new UserGroupWrapper();        

        public bool MoveUsers
        {
            get { return _usergroup_presenter.MoveUsers; }
            set { _usergroup_presenter.MoveUsers = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]        
        public object SelectedGroup 
        {
            get { return _usergroup_presenter.SelectedGroup; }
            set { _usergroup_presenter.SelectedGroup = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]        
        public object SelectedUser 
        {
            get { return _usergroup_presenter.SelectedUser; }
            set { _usergroup_presenter.SelectedUser = value; }
        }               

        #region Misc Methods        

        protected virtual AddEditContextMenuStrip CreateGroupContextMenu(string add_caption,string edit_caption,string remove_caption, bool is_addb_disabled)
        {
            AddEditContextMenuStrip res = new AddEditContextMenuStrip(is_addb_disabled);
            res.CreateAddMenuItem(add_caption, AddGroupButtonClick);
            res.CreateEditMenuItem(edit_caption, EditGroupButtonClick);
            res.CreateRemoveMenuItem(remove_caption, RemoveGroupButtonClick);

            return res;
        }

        protected virtual AddEditContextMenuStrip CreateUserContextMenu(string add_caption, string edit_caption, string remove_caption, bool is_addb_disabled)
        {
            AddEditContextMenuStrip res = new AddEditContextMenuStrip(is_addb_disabled);
            res.CreateAddMenuItem(add_caption, AddUserButtonClick);
            res.CreateEditMenuItem(edit_caption, EditUserButtonClick);
            res.CreateRemoveMenuItem(remove_caption, RemoveUserButtonClick);

            return res;
        }

        #endregion

        #region Event handlers

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                _usergroup_presenter.DoInit();
                _usergroup_presenter.Presenter.FillUserControl();
                _usergroup_presenter.Presenter.FillGroupControl();
            }
            base.OnLoad(e);
        } 

        protected void AddGroupButtonClick(object sender, EventArgs e)
        {
            _usergroup_presenter.AddGroup(_usergroup_presenter.CreateNewGroupObject());
        }

        protected void EditGroupButtonClick(object sender, EventArgs e)
        {
            _usergroup_presenter.EditGroup(SelectedGroup);
        }

        protected void RemoveGroupButtonClick(object sender, EventArgs e)
        {
            _usergroup_presenter.RemoveGroup(SelectedGroup);
        }

        protected void AddUserButtonClick(object sender, EventArgs e)
        {
            _usergroup_presenter.AddUser(_usergroup_presenter.CreateNewUserObject());
        }

        protected void EditUserButtonClick(object sender, EventArgs e)
        {
            _usergroup_presenter.EditUser(SelectedUser);
        }

        protected void RemoveUserButtonClick(object sender, EventArgs e)
        {
            _usergroup_presenter.RemoveUser(SelectedUser);
        }

        protected void GroupAddUserButtonClick(object sender, EventArgs e)
        {
            _usergroup_presenter.GroupAddUser(SelectedGroup, SelectedUser, MoveUsers);
        }

        protected void GroupRemoveUserButtonClick(object sender, EventArgs e)
        {
            _usergroup_presenter.GroupRemoveUser(SelectedGroup, SelectedUser, MoveUsers);
        }        

        #endregion
    }
}
