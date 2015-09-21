using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace anvlib.Data
{
    /// <summary>
    /// Вспомогательный класс для записи объекта типа DataTable в тектовый файл формата CSV/TXT
    /// </summary>
    public class CSV_TXT_TableWriter
    {
        private StreamWriter csv_writer;
        private char _delimeter = ',';
        private string _fname = "";
        private bool _is_string_quoted = false;

        public CSV_TXT_TableWriter(string Filename)
        {
            _fname = Filename;
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
        public bool IsStringColumnsQuoted { get { return _is_string_quoted; } set { _is_string_quoted = value; } }        

        /// <summary>
        /// Метод, записывающий данные из таблицы в файл
        /// </summary>
        /// <param name="table"></param>
        public void WriteTableToFile(DataTable table)
        {
            if (table != null && table.Columns.Count > 0 && table.Rows.Count > 0)
            {
                csv_writer = new StreamWriter(_fname);
                string columns_format_string = "";

                for (int i = 0; i < table.Columns.Count;i++ )
                {
                    if (i != table.Columns.Count - 1)
                    {
                        csv_writer.Write(string.Format("{0}{1}", table.Columns[i].ColumnName, _delimeter));
                        columns_format_string += QuoteStringColumn(table.Columns[i], i) + _delimeter;
                    }
                    else
                    {
                        csv_writer.Write(table.Columns[i].ColumnName);
                        columns_format_string += QuoteStringColumn(table.Columns[i], i);
                    }
                }
                csv_writer.WriteLine();

                foreach (DataRow dr in table.Rows)
                {
                    csv_writer.WriteLine(columns_format_string, dr.ItemArray);
                }
                csv_writer.Close();
            }
            else
                throw new Exception("Can not write table to file. Table is empty.");
        }

        /// <summary>
        /// Метод, обрамляющий в ковычки строковые поля
        /// </summary>
        /// <param name="col"></param>
        /// <param name="col_idx"></param>
        /// <returns></returns>
        private string QuoteStringColumn(DataColumn col, int col_idx)
        {
            string res = new StringBuilder().Append("{").Append(col_idx.ToString()).Append("}").ToString();
            if (_is_string_quoted && col.DataType == typeof(string))            
                res = string.Format("\"{0}\"", res);            

            return res;
        }
    }
}
