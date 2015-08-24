using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace anvlib
{
    /// <summary>
    /// Вспомогательный класс для Опеределения версии библиотеки
    /// </summary>
    public static class LibraryInformation
    {
        public static System.Version Version
        {
            get
            {
                string tmpst = Assembly.GetExecutingAssembly().FullName;
                string res = tmpst.Split(',')[1];                
                res = res.Replace(" Version=", "");
                return System.Version.Parse(res);
            }
        }
    }
}
