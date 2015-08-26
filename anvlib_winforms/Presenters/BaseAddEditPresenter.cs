using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using anvlib.Classes;
using anvlib.Utilites;

namespace anvlib.Presenters
{
    /// <summary>
    /// Базовый презентер для работы формами или контролсами редакторами
    /// </summary>
    public abstract class BaseAddEditPresenter: BaseFormPresenter
    {
        /// <summary>
        /// Редактируемый или новый объект
        /// </summary>
        protected object editableItem = null;

        /// <summary>
        /// Набор контролсов, для работы с данными объекта
        /// </summary>
        protected Control[] view_controls;

        /// <summary>
        /// Метод установки объекта редактирования
        /// </summary>
        /// <param name="Item"></param>
        public void SetItemForEditing(object Item)
        {
            editableItem = Item;
        }

        public void SetFormControls(params Control[] controls)
        {
            view_controls = controls;
        }

        //--будет небольшая потеря производительности, но компы вроде мощные. зато читабельность кода будет выше
        protected object GetControlByName(string ControlName, bool CaseSensivity)
        {
            return ControlHelper.GetObjectByName(view_controls, ControlName, CaseSensivity);
        }

        //--будет небольшая потеря производительности, но компы вроде мощные. зато читабельность кода будет выше
        protected void SetControlPropertyValue(object ctrl, string property, object value)
        {
            ObjectInspector.SetObjectPropertyValue(ctrl, property, value);
        }

        //--будет небольшая потеря производительности, но компы вроде мощные. зато читабельность кода будет выше
        protected object GetControlPropertyValue(object ctrl, string property)
        {
            return ObjectInspector.GetObjectPropertyValue(ctrl, property);
        }

        public abstract void DisplayEditableItem();

        public abstract object ReadEditableItemFromDisplay();        

        public abstract object CreateNewItem();        
    }
}
