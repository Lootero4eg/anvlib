using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace anvlib.Utilites
{
    public static class SQLServerTableCreator
    {
        public static string CreateTableFromDT(string connectionString, string tableName, DataTable table)
        {
            string sqlsc;            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                sqlsc = "CREATE TABLE " + tableName + "(";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sqlsc += "\n" + table.Columns[i].ColumnName;
                    if (table.Columns[i].DataType.ToString().Contains("System.Int32"))
                        sqlsc += " int ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.DateTime"))
                        sqlsc += " datetime ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.String"))
                        sqlsc += " varchar(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ") ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Single"))
                        sqlsc += " single ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                        sqlsc += " double ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Guid"))
                        sqlsc += " uniqueidentifier ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Boolean"))
                        sqlsc += " bit ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte"))
                        sqlsc += " tinyint ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int16"))
                        sqlsc += " tinyint ";
                    else
                        sqlsc += " varchar(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ") ";



                    if (table.Columns[i].AutoIncrement)
                        sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";
                    if (!table.Columns[i].AllowDBNull)
                        sqlsc += " NOT NULL ";
                    sqlsc += ",";
                }

                string pks = "\nCONSTRAINT PK_" + tableName + " PRIMARY KEY (";
                for (int i = 0; i < table.PrimaryKey.Length; i++)
                {
                    pks += table.PrimaryKey[i].ColumnName + ",";
                }
                pks = pks.Substring(0, pks.Length - 1) + ")";

                sqlsc += pks;
                connection.Close();

            }
            return sqlsc + ")";
        }

    }
}
