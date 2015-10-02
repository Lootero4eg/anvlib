using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Classes.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class Incomplete : BaseInfoAttribute
    {
        public Incomplete()
        {
            info = "This item is in Incomplete state.";
        }
    }
}
