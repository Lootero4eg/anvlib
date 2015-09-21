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

        public DTC_Column() { }

        public DTC_Column(string colname, DbType coltype)
        {
            ColumnName = colname;
            ColumnType = coltype;
        }
    }
}
