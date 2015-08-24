using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Presenters;
using anvlib.Interfaces;

namespace anvlib.Presenters.Wrappers
{
    public sealed class ItemListWrapper: BasePresenterWrapper<BaseItemListPresenter>
    {       
        public object EditableItem
        {
            get { return Presenter.SelectedItem; }            
        }

        public void IsAllPrepared()
        {
            if (Presenter == null)
                throw new Exception("Working with null Presenter not possible!!!");

            Presenter.IsPresenterPrepared();
        }

        public override void DoInit()
        {
            IsAllPrepared();
            Presenter.DoInit();
        }

        public void FillList()
        {
            IsAllPrepared();
            Presenter.FillList();// (ListControl);
        }

        public void SetListControl(Control Ctrl)
        {            
            Presenter.SetListControl(Ctrl);
        }

        public void SetEditableItem(object Item)
        {
            IsAllPrepared();
            Presenter.SetEditableItem(Item);
        }

        public object CreateNewListItem()
        {
            return Presenter.CreateNewListItem();
        }           
    }
}
