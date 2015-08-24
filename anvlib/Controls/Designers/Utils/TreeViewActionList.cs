// Type: System.Windows.Forms.Design.TreeViewActionList
// Assembly: System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: C1FDF7EE-5D71-4DDC-A656-8AC6988A7728
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Design.dll

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace anvlib.Controls.Designers.Utils
{
    internal class TreeViewActionList : DesignerActionList
    {
        private TypedTreeViewDesigner _designer;

        public ImageList ImageList
        {
            get
            {
                return ((TreeView)this.Component).ImageList;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)["ImageList"].SetValue((object)this.Component, (object)value);
            }
        }

        public TreeViewActionList(TypedTreeViewDesigner designer)
            : base(designer.Component)
        {
            this._designer = designer;
        }

        public void InvokeNodesDialog()
        {
            EditorServiceContext.EditValue((ComponentDesigner)this._designer, (object)this.Component, "Nodes");
        }

        public void InvokeDataSourceDescriptionDialog()
        {
            EditorServiceContext.EditValue(this._designer, (object)this.Component, "DataSourceDescription");
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            return new DesignerActionItemCollection()
      {
        (DesignerActionItem) new DesignerActionMethodItem((DesignerActionList) this, "InvokeDataSourceDescriptionDialog", "Edit DataSource Description", "Edit DataSource Description", "Edit DataSource Description", true),
        (DesignerActionItem) new DesignerActionMethodItem((DesignerActionList) this, "InvokeNodesDialog", "Edit Nodes...", "Edit Nodes...", "Edit Nodes...", true),
        (DesignerActionItem) new DesignerActionPropertyItem("ImageList", "ImageList", "ImageList","ImageList")
      };
        }
    }
}
