using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Classes.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class Experimental: BaseInfoAttribute
    {
        public Experimental()
        {
            info = "This is Experimental item.";
        }
    }
}
