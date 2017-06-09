using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using anvlib.Utilities;

namespace anvlib.Forms.Base
{
    public class BaseItemsChooserForm: Form
    {
        private object _dscontrol;
        private object _selectedItem;
        private object _selectedItems;

        public object SelectedItem
        {
            get { return _selectedItem; }
        }

        public object SelectedItems
        {
            get { return _selectedItems; }
        }

        public int SelectedItemsCount
        {
            get 
            {
                if (_selectedItems != null)
                {
                    if (ObjectInspector.HasObjectPropertyByName(_selectedItems, "Count"))
                        return (int)ObjectInspector.GetObjectPropertyValue(_selectedItems, "Count");
                    else
                        return 1;
                }

                return 0;
            }
        }

        protected void SetMainControl(object control)
        {
            _dscontrol = control;
        }

        public void SetDataSource(object datasource)
        {
            if (_dscontrol != null)
            {
                if (ObjectInspector.HasObjectDataSourceProperty(_dscontrol))
                    ObjectInspector.SetDataSource(_dscontrol, datasource);
            }
            else
                throw new Exception("You must initialize list control first!!!");
        }

        public void SetDisplayMember(string dispmember)
        {
            if (_dscontrol != null)
            {
                if (ObjectInspector.HasObjectDisplayMemberProperty(_dscontrol))
                    ObjectInspector.SetDisplayMember(_dscontrol, dispmember);
            }
            else
                throw new Exception("You must initialize list control first!!!");
        }

        public void SetDisplayMemberAndDataSource(object datasource, string dispmember)
        {
            SetDataSource(datasource);
            SetDisplayMember(dispmember);
        }

        /// <summary>
        /// Базовый метод с выбранными элементами.
        /// Попримеру можно взять этот хэндлер и создать на его основе выборку из грида
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OkBClick(object sender, EventArgs e)
        {
            if (_dscontrol != null)
            {
                if (ObjectInspector.HasObjectPropertyByName(_dscontrol, "SelectedItems"))
                {
                    _selectedItems = ObjectInspector.GetObjectPropertyValue(_dscontrol, "SelectedItems");
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }

                if (ObjectInspector.HasObjectPropertyByName(_dscontrol, "SelectedItem"))
                {
                    _selectedItem = ObjectInspector.GetObjectPropertyValue(_dscontrol, "SelectedItem");
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
