using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Base;
using anvlib.Classes;

namespace anvlib.Presenters.Interfaces
{
    /// <summary>
    /// Интерфейс для презентеров работающих с базой данных
    /// Сильно устарел, оставил его для совместимости. Сейчас в место него надо использовать ISetConnectionPresenter или ISetConnectionStringPresenter
    /// </summary>    
    public interface IDBCompatiblePresenter
    {
        /// <summary>
        /// Установить Менеджер БД
        /// </summary>
        /// <param name="manager"></param>
        void SetDBManager(BaseDbManager manager);        
    }
}
