using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using anvlib.Utilities;

namespace anvlib.Data
{
    /// <summary>
    /// Впомогательный класс для создания объектов типа DataTable
    /// </summary>
    public class DataTableCreator
    {
        private string _tabName;
        private List<DTC_Column> _columns;

        public DataTableCreator(string TableName)
        {
            _tabName = TableName;
            _columns = new List<DTC_Column>();
        }

        public DataTableCreator(string TableName, List<DTC_Column> Columns)
        {
            _tabName = TableName;
            _columns = Columns;
        }

        /// <summary>
        /// Метод добавления нового столбца в таблицу
        /// </summary>
        /// <param name="ColumnName">Имя столбца</param>
        /// <param name="ColumnType">Тип столбца</param>
        public void AddColumn(string ColumnName, DbType ColumnType)
        {
            _columns.Add(new DTC_Column(ColumnName, ColumnType));
        }

        /// <summary>
        /// Метод добавления нового столбца в таблицу
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnType"></param>
        /// <param name="primary_key"></param>
        public void AddColumn(string ColumnName, DbType ColumnType, bool primary_key)
        {
            _columns.Add(new DTC_Column(ColumnName, ColumnType, primary_key));
        }

        /// <summary>
        /// Метод генерирующий таблицу по заданным столбцам и имени таблицы
        /// </summary>
        /// <returns>Возвращает объет типа DataTable</returns>
        public DataTable CreateTable()
        {
            DataTable res = new DataTable(_tabName);
            DataColumn[] pkeys = new DataColumn[0];
            foreach (var col in _columns)
            {
                DataColumn dcol = new DataColumn(col.ColumnName,
                    DbTypeToSystemTypeConverter.Convert(col.ColumnType));                                    
                res.Columns.Add(dcol);
                if (col.PrimaryKey)
                {
                    Array.Resize<DataColumn>(ref pkeys, pkeys.Length + 1);
                    pkeys[pkeys.Length - 1] = dcol; 
                }
                //--Надо написать код добавления счетчиков res.AutoIncremen;
            }

            if (pkeys.Length > 0)
                res.PrimaryKey = pkeys;

            return res;
        }
    }
}
