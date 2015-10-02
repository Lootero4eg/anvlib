using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Classes.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class BaseInfoAttribute: Attribute
    {
        protected string info = "";

        public string Information { get { return info; } }        
    }
}
