using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace anvlib.Data
{
    public class DTC_Column
    {
        public string ColumnName { get; set; }
        public DbType ColumnType { get; set; }
        public bool Autoincrement { get; set; }
        public bool PrimaryKey { get; set; }

        public DTC_Column() { }

        public DTC_Column(string colname, DbType coltype)
        {
            ColumnName = colname;
            ColumnType = coltype;
        }

        public DTC_Column(string colname, DbType coltype, bool primary_key)
        {
            ColumnName = colname;
            ColumnType = coltype;
            PrimaryKey = primary_key;
        }

        public DTC_Column(string colname, DbType coltype, bool autoincrement, bool primary_key)
        {
            ColumnName = colname;
            ColumnType = coltype;
            Autoincrement = autoincrement;
            PrimaryKey = primary_key;
        }
    }
}
