using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Presenters;
using anvlib.Presenters.Wrappers;

namespace anvlib.Forms.Base
{
    public class BaseItemListForm : Form
    {                
        /// <summary>
        /// Устанавливается из наследуемой формы простым присваиванием
        /// </summary>
        protected BaseItemListPresenter Presenter
        {
            get;
            set;
        }

        public object EditableItem
        {
            get;
            set;
        }                   

        protected void OKClick()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        protected void CancelClick()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        protected void OKButtonClick(object sender, EventArgs e)
        {
            OKClick();
        }

        protected void CancelButtonClick(object sender, EventArgs e)
        {
            CancelClick();
        }
    }
}
