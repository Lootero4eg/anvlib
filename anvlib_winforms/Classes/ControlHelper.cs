using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

using anvlib.Utilities;

namespace anvlib.Classes
{
    public static class ControlHelper
    {
        public static void SetSelectedListControlItem(ListControl ctrl, string PropertyName, object ValueToSelect)
        {
            bool iscbox = false;
            bool islbox = false;
            if ((ctrl as ComboBox) != null)
                iscbox = true;
            if ((ctrl as ListBox) != null)
                islbox = true;
            
            if (iscbox)
            {
                foreach (var item in (ctrl as ComboBox).Items)
                {
                    if (ObjectInspector.GetObjectPropertyValue(item, PropertyName).Equals(ValueToSelect))
                    {
                        (ctrl as ComboBox).SelectedItem = item;
                        break;
                    }
                }
            }

            if (islbox)
            {
                foreach (var item in (ctrl as ListBox).Items)
                {
                    if (ObjectInspector.GetObjectPropertyValue(item, PropertyName).Equals(ValueToSelect))
                    {
                        (ctrl as ComboBox).SelectedItem = item;
                        break;
                    }
                }
            }
        }
        
        public static Control GetControlByName(Control[] ctrls, string ctrlname, bool casesensivity)
        {
            for (int i = 0; i < ctrls.Length; i++)
            {
                if (casesensivity)
                {
                    if (ctrls[i].Name == ctrlname)
                        return ctrls[i];
                }
                else
                {
                    if (ctrls[i].Name.ToLower() == ctrlname.ToLower())
                        return ctrls[i];
                }
            }

            return null;
        }
    }
}
