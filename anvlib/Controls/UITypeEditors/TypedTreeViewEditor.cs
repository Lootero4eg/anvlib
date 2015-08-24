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
    internal class TypedTreeViewEditor : UITypeEditor
    {
        public TypedTreeViewEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            /*IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null)
                MessageBox.Show("1");
            else
                MessageBox.Show(edSvc.GetType().ToString());*/
            if (value == null)
                value = new TypedTreeViewDisplayMemberList();
            //MessageBox.Show(context.GetType().ToString());
            TypedTreeViewDisplayMemberList obj_state = (value as TypedTreeViewDisplayMemberList).Clone();            
            TypedTreeViewDataSourceDescriptionDialogForm dsddf = new TypedTreeViewDataSourceDescriptionDialogForm(obj_state);
            DialogResult dr = dsddf.ShowDialog();
            if (dr == DialogResult.OK)
                value = dsddf.TypedTreeViewDisplayMemberList;

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
