using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using anvlib.Controls;
using anvlib.Interfaces;
using anvlib.Presenters;
using anvlib.Presenters.Wrappers;
using anvlib.Presenters.Interfaces;

namespace anvlib.Forms.Base
{
    [DesignTimeVisibleAttribute(true)]    
    public class BaseUserGroupForm : Form//, IUserGroupControl
    {
        protected static UserGroupWrapper _usergroup_presenter = new UserGroupWrapper();        

        /*
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
        }        */

        #region Event handlers            

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                _usergroup_presenter.DoInit();
                _usergroup_presenter.Presenter.FillUserControl();
                _usergroup_presenter.Presenter.FillGroupControl();
                base.OnLoad(e);
            }
        } 

        protected void OkClick()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        protected void CancelClick()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        protected void OKButtonClick(object sender, EventArgs e)
        {
            OkClick();
        }

        protected void CancelButtonClick(object sender, EventArgs e)
        {
            CancelClick();
        }

        #endregion
    }
}
