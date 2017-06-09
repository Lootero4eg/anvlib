using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Utilities
{
    public static class TypesHelper
    {
        public static bool IsStandardType(Type type)
        {
            if ((type == typeof(string)) || (type == typeof(int)) || (type == typeof(float)) || (type == typeof(double))
                || (type == typeof(decimal)) || (type == typeof(bool)) || (type == typeof(long)) || (type == typeof(short))
                || (type == typeof(DateTime)))
                return true;

            return false;
        }
        
        //public static object GetGenericType()
    }
}
