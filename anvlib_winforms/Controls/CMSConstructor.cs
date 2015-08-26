using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace anvlib.Controls
{
    public class CMSConstructor : ContextMenuStrip
    {
        public void CreateNewMenuItem(string Caption, EventHandler ClickHandler)
        {
            ToolStripMenuItem newTSMI = new ToolStripMenuItem(Caption);
            newTSMI.Click += ClickHandler;
            this.Items.Add(newTSMI);
        }
    }
}
