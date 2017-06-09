using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using anvlib.Data;
using anvlib.Data.Database;
using anvlib.Interfaces;

namespace anvlib.Data.ExportTableMethods
{
    /// <summary>
    /// Класс прослойка между реальным экспортером и интерфесом
    /// </summary>
    public class CSV_TXT_EportMethod: IExportTableMethod
    {
        private CSV_TXT_TableWriter writer;
        private string _fname = "";
        private char _delimeter = ',';
        private bool _is_string_quoted = false;

        public event EventHandler ExportComplete;
        public event EventHandler ExportException;

        public CSV_TXT_EportMethod(string filename)
        {
            _fname = filename;
        }

        /// <summary>
        /// Метод, устанавливающий разделитель м/у колонками
        /// </summary>
        /// <param name="Delimeter"></param>
        public void SetDelimeter(char Delimeter)
        {
            _delimeter = Delimeter;
        }

        /// <summary>
        /// Флаг, позволяющий записывать поля с текстовым содержанием обрамленные двойными каовычками ("")
        /// </summary>
        public void SetStringColumnsQuotedFlag(bool flag)
        {
            _is_string_quoted = flag;
        }

        /// <summary>
        /// Реализация метода экспорта таблицы
        /// </summary>
        /// <param name="table"></param>
        /// <param name="additionaldata"></param>
        public void Export(DataTable table, DataInsertMethod InsertMethod, bool CaseSensivity, bool PrepareTableForInsert)
        {
            try
            {
                writer = new CSV_TXT_TableWriter(_fname);
                writer.SetDelimeter(_delimeter);
                writer.IsStringColumnsQuoted = _is_string_quoted;
                writer.WriteTableToFile(table);

                if (ExportComplete != null)
                    ExportComplete(this, new EventArgs());
            }
            catch (Exception e)
            {
                if (ExportException != null)
                    ExportException(e, new EventArgs());
                else
                    throw e;
            }
        }
    }
}
