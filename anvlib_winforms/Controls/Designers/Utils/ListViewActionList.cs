using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace anvlib.Controls.Designers.Utils
{
    internal class ListViewActionList : DesignerActionList
    {
        private ComponentDesigner _designer;

        public View View
        {
            get
            {
                return ((ListView)this.Component).View;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)["View"].SetValue((object)this.Component, (object)value);
            }
        }

        public ImageList LargeImageList
        {
            get
            {
                return ((ListView)this.Component).LargeImageList;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)["LargeImageList"].SetValue((object)this.Component, (object)value);
            }
        }

        public ImageList SmallImageList
        {
            get
            {
                return ((ListView)this.Component).SmallImageList;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)["SmallImageList"].SetValue((object)this.Component, (object)value);
            }
        }

        public ListViewActionList(ComponentDesigner designer)
            : base(designer.Component)
        {
            this._designer = designer;
        }

        public void InvokeItemsDialog()
        {
            EditorServiceContext.EditValue(this._designer, (object)this.Component, "Items");
        }

        public void InvokeColumnsDialog()
        {
            EditorServiceContext.EditValue(this._designer, (object)this.Component, "Columns");
        }

        public void InvokeGroupsDialog()
        {
            EditorServiceContext.EditValue(this._designer, (object)this.Component, "Groups");
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
        (DesignerActionItem) new DesignerActionMethodItem((DesignerActionList) this, "InvokeItemsDialog", "Edit Items...", "Edit Items...", "Edit Items...", true),
        (DesignerActionItem) new DesignerActionMethodItem((DesignerActionList) this, "InvokeColumnsDialog", "Edit Columns...", "Edit Columns...", "Edit Columns...", true),
        (DesignerActionItem) new DesignerActionMethodItem((DesignerActionList) this, "InvokeGroupsDialog", "Edit Groups...", "Edit Groups...", "Edit Groups...", true),
        (DesignerActionItem) new DesignerActionPropertyItem("View", "View", "View", "View"),
        (DesignerActionItem) new DesignerActionPropertyItem("SmallImageList", "SmallImageList", "SmallImageList", "SmallImageList"),
        (DesignerActionItem) new DesignerActionPropertyItem("LargeImageList", "LargeImageList", "LargeImageList", "LargeImageList")
      };
        }
    }
}
