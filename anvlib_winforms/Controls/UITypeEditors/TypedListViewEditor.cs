using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.ComponentModel.Design;
using System.Runtime;

using anvlib.Classes;
using anvlib.Controls.Dialogs;

namespace anvlib.Controls.UITypeEditors
{
    internal class TypedListViewEditor : UITypeEditor
    {
        public TypedListViewEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {                       
            TypedListViewDisplayMember obj_state = new TypedListViewDisplayMember();
            //MessageBox.Show((value as TypedListViewDisplayMember).SubItems.Count.ToString());
            if (value != null)
                obj_state = (value as TypedListViewDisplayMember).Clone();
            
            TypedListViewDataSourceDescriptionDialogForm dsddf = new TypedListViewDataSourceDescriptionDialogForm(obj_state);
            DialogResult dr = dsddf.ShowDialog();
            if (dr == DialogResult.OK)
                value = dsddf.TypedListViewDisplayMember;
            
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
