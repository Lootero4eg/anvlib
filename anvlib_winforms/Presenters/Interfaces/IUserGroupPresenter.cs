using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib.Presenters.Interfaces
{
    /// <summary>
    /// Интерфейс пользователей и групп.
    /// </summary>
    public interface IUserGroupPresenter
    {
        object CreateNewGroupObject();
        object CreateNewUserObject();
        void SetGroupControl(Control GrControl);
        void SetUserControl(Control GrControl);
        void SetGroupsAddEditControl(Control AddEdCrtl);
        void SetUsersAddEditControl(Control AddEdCrtl);
        Type GetGroupType();
        Type GetUserType();
    }
}
