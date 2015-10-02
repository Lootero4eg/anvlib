using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Reflection;

namespace anvlib.Classes.Base
{
    /// <summary>
    /// Класс опций для PropertyGrid-а
    /// </summary>
    public class BaseOptions
    {
        protected ReadOnlyAttribute GetReadOnlyAttribute(string obj)
        {
            try
            {
                AttributeCollection attributes = TypeDescriptor.GetProperties(this)[obj].Attributes;
                foreach (var attr in attributes)
                {
                    if (attr is ReadOnlyAttribute)
                        return (attr as ReadOnlyAttribute);
                }
            }
            catch
            {
                return new ReadOnlyAttribute(false);
            }

            return new ReadOnlyAttribute(false);
        }

        public void EnableAttribute(string AttrName, bool EnableFlag)
        {
            //AttributeCollection attributes = TypeDescriptor.GetProperties(this)["NetLogin"].Attributes;            
            var attr = GetReadOnlyAttribute(AttrName);
            FieldInfo field = attr.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                field.SetValue(attr, EnableFlag, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
        }

        //--написать методы для категоризации и прочего
    }
}
