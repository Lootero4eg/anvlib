using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;

using anvlib.Enums;
using anvlib.Interfaces;
using anvlib.Classes.Attributes;

namespace anvlib.Data.Database
{
    /// <summary>
    /// Базовый класс для баз данных SQL Lite
    /// </summary>
    public class BaseSQLiteManager : BaseDbManager
    {
        /// <summary>
        /// Конструктор с дефолтными значениями
        /// </summary>
        public BaseSQLiteManager()
            : base()
        {
            _open_bracket = '"';
            _close_bracket = '"';
            _parameters_prefix = "@";
        }

        /// <summary>
        /// Метод, подключения к базе данных
        /// </summary>
        /// <param name="ConnectionString">Строка соединения</param>
        public override void Connect(string ConnectionString)
        {
            _connectionString = ConnectionString;
            _conn = new SQLiteConnection(ConnectionString);
            try
            {
                _conn.Open();
            }
            catch (SQLiteException ex)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ex.Message, MsgMgr.MessageText.DBErrorMsg, 1, 1);
            }
        }

        /// <summary>
        /// Метод, создает или открывает уже существующий файл базы данных
        /// </summary>
        /// <param name="Filename">Имя файла БД</param>
        public void OpenDbFile(string Filename)
        {
            Connect(string.Format("URI=file:{0}", Filename));
        }

        /// <summary>
        /// Метод, содает новую базу в памяти
        /// </summary>
        public void CreateDbInMemory()
        {
            Connect("Data Source=:memory:");
        }

        /// <summary>
        /// Коннекция
        /// </summary>
        public override DbConnection Connection
        {
            get
            {
                return (_conn as SQLiteConnection);
            }
            set
            {
                _conn = value;
            }
        }

        /// <summary>
        /// Метод создания DbCommand
        /// </summary>
        /// <param name="CmdText">Текст запроса или имя хранимой процедуры</param>
        /// <returns></returns>
        protected override DbCommand CreateCommand(string CmdText)
        {
            _cmd = new SQLiteCommand(CmdText, (_conn as SQLiteConnection));
            DbCommand tmpCmd = _cmd;
            tmpCmd.CommandType = CommandType.Text;

            if (_transaction != null)
                tmpCmd.Transaction = _transaction;

            return tmpCmd;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            _DA = new SQLiteDataAdapter((SQLiteCommand)cmd);
            DbDataAdapter tmpDA = _DA;

            return tmpDA;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(string SQLText)
        {
            _DA = new SQLiteDataAdapter(SQLText, (_conn as SQLiteConnection));
            DbDataAdapter tmpDA = _DA;

            return tmpDA;
        }

        protected override DbParameter CreateParameter(string ParName, DbType ParType, int ParSize)
        {
            //throw new Exception("В этой СУБД отсутствует понятия хранимых процедур, соответственно и параметры отсутствуют!");

            _param = new SQLiteParameter(ParName, ParType, ParSize);
            DbParameter tmpPar = _param;

            return tmpPar;
        }

        /// <summary>
        /// Метод создания таблицы из DataTable
        /// </summary>
        /// <param name="table">Таблица</param>
        /// <param name="insert_method">В этой СУБД этот параметр не работает, поэтому можно внести любое значение</param>
        [Experimental]
        public override void CreateTable(DataTable table, DataInsertMethod insert_method)
        {
            if (Connected)
            {
                base.CreateTable(table, insert_method);

                string sqlsc;
                sqlsc = "CREATE TABLE " + table.TableName + "(";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sqlsc += "\n" + table.Columns[i].ColumnName;
                    if (table.Columns[i].DataType.ToString().Contains("System.Int32"))
                        sqlsc += " integer";
                    else if (table.Columns[i].DataType.ToString().Contains("System.DateTime"))
                        sqlsc += " text";
                    else if (table.Columns[i].DataType.ToString().Contains("System.String"))
                        sqlsc += " text";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Single"))
                        sqlsc += " integer";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                        sqlsc += " real";
                    /*else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                        sqlsc += " integer";*/
                    else if (table.Columns[i].DataType.ToString().Contains("System.Guid"))
                        sqlsc += " text";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Boolean"))
                        sqlsc += " integer";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte"))
                        sqlsc += " integer";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int16"))
                        sqlsc += " integer";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte[]"))
                        sqlsc += " blob";
                    else
                        sqlsc += " text";

                    if (table.Columns[i].AutoIncrement)
                        sqlsc += " AUTOINCREMENT(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ")";
                    if (!table.Columns[i].AllowDBNull)
                        sqlsc += " NOT NULL";
                    if (table.Columns[i].DefaultValue != null && table.Columns[i].DefaultValue != DBNull.Value)
                        sqlsc += " DEFAULT " + table.Columns[i].DefaultValue.ToString();
                    sqlsc += ",";
                }

                if (table.PrimaryKey.Length > 0)
                {
                    string pks = "\nCONSTRAINT PK_" + table.TableName + " PRIMARY KEY (";
                    for (int i = 0; i < table.PrimaryKey.Length; i++)
                    {
                        pks += table.PrimaryKey[i].ColumnName + ",";
                    }
                    pks = pks.Substring(0, pks.Length - 1) + ")";
                    sqlsc += pks;
                    sqlsc = sqlsc + ")";
                }
                else
                    sqlsc = sqlsc.Substring(0, sqlsc.Length - 1) + ");";

                ExecuteCommand(CreateCommand(sqlsc).ExecuteNonQuery);

                if (_last_error == 0)//--Если табличка успешно создана, то надо ее заполнить 
                {
                    BeginTransaction();
                    InsertDataToDb(table, _parameters_prefix);
                    CommitTransaction();
                }
            }
            else
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(MsgMgr.MessageText.NotConnectedMsg, MsgMgr.MessageText.ErrorMsg, 1, 1);
            }
        }

        /// <summary>
        /// Метод, удаляющий таблицу из базы данных
        /// </summary>
        /// <param name="TableName"></param>
        public override void DropTable(string TableName)
        {
            string sql = string.Format("DROP TABLE IF EXISTS {0};", TableName);
            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        [Incomplete]
        public override bool IsDBObjectExists(string obj_name, DataBaseObjects obj_type, bool CaseSensivity)
        {
            //--надо смотреть табличку sqlite_master
            if (Connected)
            {
                string sql = "";
                if (CaseSensivity)
                {
                    sql = string.Format("select 1 from sqlite_master where type='{0}' and name='{1}'",
                            GetObjectTypeCode(obj_type),
                            obj_name);
                }
                else
                {
                    sql = string.Format("select 1 from sys.objects where type='{0}' and lower(name)=lower('{1}')",
                            GetObjectTypeCode(obj_type),
                            obj_name);
                }

                var exec_res = ExecuteScalarCommand(CreateCommand(sql).ExecuteScalar);
                if (exec_res != null && exec_res != DBNull.Value)
                    return true;
            }
            else
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(MsgMgr.MessageText.NotConnectedMsg, MsgMgr.MessageText.ErrorMsg, 1, 1);
            }

            return false;
        }

        [Incomplete]
        protected string GetObjectTypeCode(DataBaseObjects db_object)
        {
            string res = "table";

            switch (db_object)
            {
                /*case DataBaseObjects.foreign_key:
                    res = "foreing key";
                    break;

                case DataBaseObjects.function:
                    res = "FN";
                    break;

                case DataBaseObjects.index:
                    res = "UQ";
                    break;

                case DataBaseObjects.primary_key:
                    res = "PK";
                    break;

                case DataBaseObjects.procedure:
                    res = "P";
                    break;*/

                case DataBaseObjects.table:
                    res = "table";
                    break;

                /*case DataBaseObjects.trigger:
                    res = "TR";
                    break;

                case DataBaseObjects.type:
                    res = "T";
                    break;*/
            }

            return res;
        }

        protected override void CreateLogin(string LoginName, string Paswword, string AdditionalOptions)
        {
            throw new Exception(MsgMgr.MessageText.UsersManagmentIsNotSupported);
        }

        protected override void DeleteLogin(string LoginName, string AdditionalOptions)
        {
            throw new Exception(MsgMgr.MessageText.UsersManagmentIsNotSupported);
        }

        /// <summary>
        /// Обертка для выполнения DbCommand
        /// </summary>
        /// <param name="proc"></param>
        protected override void ExecuteCommand(ExecuteCmdDelegate proc)
        {
            try
            {
                _last_error = 0;
                proc.Invoke();
            }
            catch (SQLiteException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, MsgMgr.MessageText.DBErrorMsg, 1, 1);
            }
        }

        /// <summary>
        /// Обертка для выполнения DbCommand.ExecuteReader
        /// </summary>
        /// <param name="proc"></param>
        protected override DbDataReader ExecuteDataReader(ExecuteDataReaderCmdDelegate proc)
        {
            try
            {
                _last_error = 0;
                return proc.Invoke();
            }
            catch (SQLiteException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, MsgMgr.MessageText.DBErrorMsg, 1, 1);
            }

            return null;
        }

        /// <summary>
        /// Обертка для выполнения DbCommand.ExecuteScalar
        /// </summary>
        /// <param name="proc"></param>
        protected override object ExecuteScalarCommand(ExecuteScalarCmdDelegate proc)
        {
            try
            {
                _last_error = 0;
                return proc.Invoke();
            }
            catch (SQLiteException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, MsgMgr.MessageText.DBErrorMsg, 1, 1);
            }

            return null;
        }

        protected override DataTable GetTablePrimaryKey(DataTable table, bool CaseSensivity)
        {
            _DR = ExecuteDataReader(CreateCommand(string.Format("pragma table_info({0})", table.TableName)).ExecuteReader);
            List<DataColumn> cols = new List<DataColumn>();
            while (_DR.Read())
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (CaseSensivity)
                    {
                        if (_DR["name"].ToString() == col.ColumnName)
                        {
                            if (Convert.ToBoolean(_DR["pk"]))
                                cols.Add(col);
                        }
                    }
                    else
                    {
                        if (_DR["name"].ToString().ToLower() == col.ColumnName.ToLower())
                        {
                            if (Convert.ToBoolean(_DR["pk"]))
                                cols.Add(col);
                        }
                    }
                }
            }

            _DR.Close();

            if (cols.Count > 0)
                table.PrimaryKey = cols.ToArray();

            return table;
        }

        internal override DbColumnInformation GetDbColumnInformation(string tablename, string columnname, bool CaseSensivity)
        {
            DbColumnInformation res = new DbColumnInformation();

            string sql = "";
            if (!CaseSensivity)
                sql = string.Format("select max(length({0})) from {1}", columnname, tablename);
            else
                sql = string.Format("select max(length({0}{1}{2})) from {0}{3}{2}", _open_bracket, columnname, _close_bracket, tablename);
            var res2 = ExecuteScalarCommand(CreateCommand(sql).ExecuteScalar);
            if (res2 != null && res2 != DBNull.Value)
            {
                var mlen = Convert.ToInt32(res2);
                res.MaxLength = (mlen > 0 ? mlen : 50);
            }
            
            _DR = ExecuteDataReader(CreateCommand(string.Format("pragma table_info({0})",tablename)).ExecuteReader);
            while (_DR.Read())
            {
                if (CaseSensivity)
                {
                    if (_DR["name"].ToString() == columnname)
                    {
                        res.IsNullable = !Convert.ToBoolean(_DR["notnull"]);
                        if (_DR["dflt_value"] != DBNull.Value)
                            res.DefaultValue = _DR["dflt_value"];
                    }
                }
                else
                {
                    if (_DR["name"].ToString().ToLower() == columnname.ToLower())
                    {
                        res.IsNullable = !Convert.ToBoolean(_DR["notnull"]);
                        if (_DR["dflt_value"] != DBNull.Value)
                            res.DefaultValue = _DR["dflt_value"];
                    }
                }
            }

            _DR.Close();

            return res;
        }
    }
}
