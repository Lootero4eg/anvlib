using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using anvlib.Controls.Designers.Utils;

namespace anvlib.Controls.Designers
{
    internal class TypedListViewDesigner : ControlDesigner
    {        
        private DesignerActionListCollection _actionLists;        

        public override ICollection AssociatedComponents
        {
            get
            {
                ListView listView = this.Control as ListView;
                if (listView != null)
                    return (ICollection)listView.Columns;
                else
                    return base.AssociatedComponents;
            }
        }

        private bool OwnerDraw
        {
            get
            {
                return (bool)this.ShadowProperties["OwnerDraw"];
            }
            set
            {
                this.ShadowProperties["OwnerDraw"] = (object)(bool)(value ? true : false);
            }
        }

        private View View
        {
            get
            {
                return ((ListView)this.Component).View;
            }
            set
            {
                ((ListView)this.Component).View = value;
                if (value != View.Details)
                    return;                
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (this._actionLists == null)
                {
                    this._actionLists = new DesignerActionListCollection();
                    this._actionLists.Add((DesignerActionList)new ListViewActionList((ComponentDesigner)this));
                }
                return this._actionLists;
            }
        }        

        public override void Initialize(IComponent component)
        {
            ListView listView = (ListView)component;
            this.OwnerDraw = listView.OwnerDraw;
            listView.OwnerDraw = false;
            listView.UseCompatibleStateImageBehavior = false;
            this.AutoResizeHandles = true;
            base.Initialize(component);
            if (listView.View != View.Details)
                return;            
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            PropertyDescriptor oldPropertyDescriptor1 = (PropertyDescriptor)properties[(object)"OwnerDraw"];
            if (oldPropertyDescriptor1 != null)
                properties[(object)"OwnerDraw"] = (object)TypeDescriptor.CreateProperty(typeof(TypedListViewDesigner), oldPropertyDescriptor1, new Attribute[0]);
            PropertyDescriptor oldPropertyDescriptor2 = (PropertyDescriptor)properties[(object)"View"];
            if (oldPropertyDescriptor2 != null)
                properties[(object)"View"] = (object)TypeDescriptor.CreateProperty(typeof(TypedListViewDesigner), oldPropertyDescriptor2, new Attribute[0]);
            base.PreFilterProperties(properties);
        }        
    }
}
