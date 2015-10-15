using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using anvlib.Interfaces;

namespace anvlib.Data
{
    /// <summary>
    /// Вспомогательный класс для экспорта таблицы в базу данных или формат
    /// </summary>
    public static class ExportTableTo
    {
        /// <summary>
        /// Метод вызывающи у "Способа" экспорта, метод экспорта.
        /// </summary>
        /// <param name="ExportMethod"></param>
        /// <param name="table"></param>
        /// <param name="additionaldata">Здесь передаются такие параметры как имя файла или база данных в которую писать</param>
        public static void Export(IExportTableMethod ExportMethod, DataTable table, bool CaseSensivity)
        {
            if (ExportMethod != null)
                ExportMethod.Export(table, CaseSensivity);
        }
    }
}
