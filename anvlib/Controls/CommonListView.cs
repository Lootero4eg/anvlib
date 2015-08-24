using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib.Controls
{
    public class CommonListView : ListView
    {
        public int SelectedItemIndex
        {
            get
            {
                if (SelectedIndices.Count > 0)
                    return SelectedIndices[0];
                else
                    return -1;
            }
        }
    }
}
