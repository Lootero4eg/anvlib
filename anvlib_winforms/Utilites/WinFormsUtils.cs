using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.Windows.Forms;

namespace anvlib.Utilites
{
    /// <summary>
    /// Вспомогательный класс для работы с Windows Forms
    /// </summary>
    public static class WinFormsUtils
    {
        public static void SetFormCaption(string Caption, ref Form frm)
        {
            frm.Text = Caption;
        }

        public static void SetFormIcon(Icon icon, ref Form frm)
        {
            frm.Icon = icon;
        }

        public static void SetBoolProperties(bool value, string PropertyName, params object[] ctrls)
        {
            foreach (var item in ctrls)
                ObjectInspector.SetObjectPropertyValue(item, PropertyName, value);
        }
    }
}
