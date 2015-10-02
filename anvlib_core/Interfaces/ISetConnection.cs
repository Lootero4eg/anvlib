using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Интерфейс для презентеров работающих с базой данных
    /// Сильно устарел, оставил его для совместимости. Сейчас в место него надо использовать ISetConnectionPresenter или ISetConnectionStringPresenter
    /// </summary>
    public interface ISetConnection
    {
        /// <summary>
        /// Передача строки соединения в Презентатор
        /// </summary>
        /// <param name="manager"></param>
        void SetConnectionString(DbConnection Connection);
    }
}
