using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using anvlib.Base;
using anvlib.Classes;
using anvlib.Enums;
using anvlib.Interfaces;
using anvlib.Interfaces.Collections;
using anvlib.Presenters;
using anvlib.Presenters.Interfaces;

namespace anvlib.Presenters.Templates
{    
    public class UserGroupPresenterTemplate: BaseUserGroupPresenter
    {
        //--Ненужное или неиспользуемое нужно удалить, чтобы не захламлять презентатор!!!

        #region Main BaseUserGroupPresenter Methods

        protected override void FillGroups()
        {
            throw new NotImplementedException();
        }

        protected override void FillUsers()
        {
            throw new NotImplementedException();
        }

        public override void FillGroupControl()
        {
            throw new NotImplementedException();
        }

        public override void FillUserControl()
        {
            throw new NotImplementedException();
        }

        public override void DoInit()
        {
            throw new NotImplementedException();
        }

        public override object CreateNewGroupObject()
        {
            return base.CreateNewGroupObject();
        }

        public override object CreateNewUserObject()
        {
            return base.CreateNewUserObject();
        }

        #endregion

        #region Groups BaseUserGroupPresenter Methods

        public override void AddGroup(object Group)
        {
            base.AddGroup(Group);
        }

        public override void EditGroup(object Group)
        {
            base.EditGroup(Group);
        }        

        public override void RemoveGroup(object Group)
        {
            base.RemoveGroup(Group);
        }

        public override void GroupAddUser(object Group, object User, bool MoveFromUList)
        {
            base.GroupAddUser(Group, User, MoveFromUList);
        }

        public override void GroupRemoveUser(object Group, object User, bool RestoreInUserList)
        {
            base.GroupRemoveUser(Group, User, RestoreInUserList);
        }

        public override Type GetGroupType()
        {
            return base.GetGroupType();
        }

        #endregion

        #region Users BaseUserGroupPresenter Methods

        public override void AddUser(object User)
        {
            base.AddUser(User);
        }

        public override void EditUser(object User)
        {
            base.EditUser(User);
        }

        public override void RemoveUser(object User)
        {
            base.RemoveUser(User);
        }

        public override Type GetUserType()
        {
            return base.GetUserType();
        }

        #endregion                                
    }
}
