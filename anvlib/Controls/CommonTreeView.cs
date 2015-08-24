using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib.Controls
{
    public class CommonTreeView: TreeView
    {
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                this.SelectedNode = this.GetNodeAt(e.Location);

            base.OnMouseClick(e); //anvlib.Controls.Designers.TypedTreeViewDesigner;
        }        
    }    
}
