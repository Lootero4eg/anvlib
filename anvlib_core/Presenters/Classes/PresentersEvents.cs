using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Presenters.Classes
{
    public delegate void FillViewData(object sender);
    public delegate void ClearView();
    public delegate void ViewChanged(object sender);
}
