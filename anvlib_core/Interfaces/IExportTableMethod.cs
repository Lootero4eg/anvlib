using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace anvlib.Interfaces
{
    public interface IExportTableMethod
    {
        /// <summary>
        /// Экпорт таблицы
        /// </summary>
        /// <param name="table"></param>
        void Export(DataTable table);

        /// <summary>
        /// Событие успешного выполнения экспорта
        /// </summary>
        event EventHandler ExportComplete;

        /// <summary>
        /// Событие неудачного выполнения экспорта
        /// </summary>
        event EventHandler ExportException;
    }
}
