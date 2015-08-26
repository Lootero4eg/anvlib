using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using anvlib.Controls.Designers.Utils;

namespace anvlib.Controls.Designers
{
    internal class TypedTreeViewDesigner : ControlDesigner
    {        
        private DesignerActionListCollection _actionLists;
        private TreeView treeView;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (this._actionLists == null)
                {
                    this._actionLists = new DesignerActionListCollection();
                    this._actionLists.Add((DesignerActionList)new TreeViewActionList(this));
                }
                return this._actionLists;
            }
        }

        public TypedTreeViewDesigner()
        {
            this.AutoResizeHandles = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.treeView != null)
            {
                this.treeView.AfterExpand -= new TreeViewEventHandler(this.TreeViewInvalidate);
                this.treeView.AfterCollapse -= new TreeViewEventHandler(this.TreeViewInvalidate);
                this.treeView = (TreeView)null;
            }
            base.Dispose(disposing);
        }        

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this.treeView = component as TreeView;
            if (this.treeView == null)
                return;
            this.treeView.AfterExpand += new TreeViewEventHandler(this.TreeViewInvalidate);
            this.treeView.AfterCollapse += new TreeViewEventHandler(this.TreeViewInvalidate);
        }

        private void TreeViewInvalidate(object sender, TreeViewEventArgs e)
        {
            if (this.treeView == null)
                return;
            this.treeView.Invalidate();
        }
    }
}
