using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib.Controls
{
    public class CommonCheckedListBox: CheckedListBox
    {
        private bool _lostFocusIfEmptySpaceClicked = true;

        protected bool LostFocusIfEmptySpaceClicked
        {
            get { return _lostFocusIfEmptySpaceClicked; }
            set { _lostFocusIfEmptySpaceClicked = value; }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                int idx = this.IndexFromPoint(e.Location);
                if (idx >= 0)
                    this.SelectedIndex = idx;
                if (idx == -1 && _lostFocusIfEmptySpaceClicked)
                    this.SelectedIndex = -1;
            }

            base.OnMouseDown(e);
        }  
    }
}
