using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using Npgsql;
using NpgsqlTypes;

using anvlib.Enums;
using anvlib.Interfaces;

namespace anvlib.Data.Database
{
    /// <summary>
    /// Базовый класс для Postgres SQL
    /// </summary>
    public class BasePostgresSQLManager: BaseDbManager
    {
        /// <summary>
        /// Конструктор с дефолтными значениями
        /// </summary>
        public BasePostgresSQLManager()
            : base()
        { 
            _open_bracket='"';
            _close_bracket = '"';
            _parameters_prefix = "@";
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="msg_system">Система оповещения об ошибке</param>
        public BasePostgresSQLManager(IPrintMessageSystem msg_system)
            : base(msg_system)
        {
            _open_bracket = '"';
            _close_bracket = '"';
            _parameters_prefix = "@";
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="srvname">Имя или айпи сервера</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>                
        public void Connect(string srvname, string login, string password,string database)
        {
            string connstr = string.Format("server={0};uid={1};pwd={2};database={3};",
                srvname, login, password,database);

            Connect(connstr);
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="ConnectionString">Строка инициализации</param>
        public override void Connect(string ConnectionString)
        {            
            _conn = new NpgsqlConnection(ConnectionString);
            _conn.Open();
        }

        /// <summary>
        /// Коннекция
        /// </summary>
        public override DbConnection Connection
        {
            get
            {
                return (_conn as NpgsqlConnection);
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
            _cmd = new NpgsqlCommand(CmdText, (NpgsqlConnection)_conn);
            return _cmd;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(string SQLText)
        {
            _DA = new NpgsqlDataAdapter(SQLText, (NpgsqlConnection)_conn);
            return _DA;
        }

        /// <summary>
        /// Метод создание DbDataAdapter-а
        /// </summary>
        /// <param name="cmd">Команда которая будет заполнять DbDataAdapter</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            _DA = new NpgsqlDataAdapter((NpgsqlCommand)_cmd);
            return _DA;
        }

        /// <summary>
        /// Метод создание параметра для хранимых процедур
        /// </summary>
        /// <param name="ParName">Имя параметра</param>
        /// <param name="ParType">Тип параметра</param>
        /// <param name="ParSize">Размер параметра. Для строковых параметров и параметров с плавающей точкой</param>
        /// <returns></returns>
        protected override DbParameter CreateParameter(string ParName, DbType ParType, int ParSize)
        {
            #region Types Convert
            NpgsqlDbType tmpType = NpgsqlDbType.Integer;
            switch (ParType)
            {
                case DbType.String:
                    tmpType = NpgsqlDbType.Varchar;
                    break;

                case DbType.Int16:
                    tmpType = NpgsqlDbType.Smallint;
                    break;

                case DbType.Int32:
                    tmpType = NpgsqlDbType.Integer;
                    break;

                case DbType.Int64:
                    tmpType = NpgsqlDbType.Bigint;
                    break;

                case DbType.Boolean:
                    tmpType = NpgsqlDbType.Boolean;
                    break;

                case DbType.DateTime:
                    tmpType = NpgsqlDbType.Date;
                    break;
            }
            #endregion

            _param = new NpgsqlParameter(ParName, tmpType, ParSize);
            DbParameter tmpPar = _param;

            return tmpPar;
        }        

        protected override void ExecuteCommand(ExecuteCmdDelegate proc)
        {
            try
            {
                proc.Invoke();
            }
            catch (DbException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message,ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
            }
        }

        protected override object ExecuteScalarCommand(ExecuteScalarCmdDelegate proc)
        {
            try
            {
                return proc.Invoke();
            }
            catch (Npgsql.NpgsqlException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message,ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
            }

            return null;
        }

        protected override DbDataReader ExecuteDataReader(ExecuteDataReaderCmdDelegate proc)
        {
            try
            {
                var res = proc.Invoke();
                return res;
            }
            catch (Npgsql.NpgsqlException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message,ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
            }

            return null; ;
        }

        public override void CreateTable(DataTable table, DataInsertMethod insert_method, bool PrepareTableForInsert) 
        {
            //insert_method = DataInsertMethod.Normal;
            if (Connected)
            {
                base.CreateTable(table, insert_method, PrepareTableForInsert);

                string sqlsc;
                sqlsc = "CREATE TABLE " + table.TableName + "(";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sqlsc += "\n" + table.Columns[i].ColumnName;
                    if (table.Columns[i].DataType.ToString().Contains("System.Int32") && !table.Columns[i].AutoIncrement)                    
                        sqlsc += " integer";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int32") && table.Columns[i].AutoIncrement)                    
                        sqlsc += " serial";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int64"))//--если будут ошибки, то заменить на bigint
                        sqlsc += " integer";
                    else if (table.Columns[i].DataType.ToString().Contains("System.DateTime"))
                        sqlsc += " date";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Decimal"))
                        sqlsc += " decimal";//--возможны ошибки
                    else if (table.Columns[i].DataType.ToString().Contains("System.String"))
                    {
                        if (table.Columns[i].MaxLength <= 8000)
                            sqlsc += " varchar(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ")";
                        else
                            sqlsc += " text";
                    }
                    /*else if (table.Columns[i].DataType.ToString().Contains("System.Single"))
                        sqlsc += " single";*/
                    else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                        sqlsc += " real";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Guid"))
                        sqlsc += " UUID";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Boolean"))
                        sqlsc += " boolean";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte"))
                        sqlsc += " smallint";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int16"))
                        sqlsc += " smallint";
                    else
                        sqlsc += " varchar(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ")";


                    //if (table.Columns[i].AutoIncrement)//--будут проблемы....
                    //    sqlsc += " serial";// +table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ")";
                    if (!table.Columns[i].AllowDBNull)
                        sqlsc += " NOT NULL ";
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
                    sqlsc = sqlsc + ");";
                }
                else
                    sqlsc = sqlsc.Substring(0, sqlsc.Length - 1) + ");";

                ExecuteCommand(CreateCommand(sqlsc).ExecuteNonQuery);

                if (_last_error == 0)//--Если табличка успешно создана, то надо ее заполнить       
                {                    
                    if (insert_method == DataInsertMethod.Normal)
                        InsertDataToDb(table, _parameters_prefix, PrepareTableForInsert);
                    if (insert_method == DataInsertMethod.FastIfPossible)
                    {
                        BeginTransaction();
                        InsertDataToDbBulkMethod(table);
                        CommitTransaction();
                    }
                }
                else
                {
                    if (MessagePrinter != null)
                        MessagePrinter.PrintMessage(ErrorsManager.Messages.DBErrorMsg, ErrorsManager.Messages.ErrorMsg, 1, 1);
                }
            }
            else
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ErrorsManager.Messages.NotConnectedMsg, ErrorsManager.Messages.ErrorMsg, 1, 1);
            }
        }

        public override bool IsDBObjectExists(string obj_name, DataBaseObjects obj_type, bool CaseSensivity)
        {
            string search_tablename = "";
            string search_pars = "";

            switch (obj_type)
            { 
                case DataBaseObjects.table:
                    search_tablename = "information_schema.tables";
                    if (!CaseSensivity)
                    {
                        search_pars = string.Format("lower(table_name)=lower('{0}')", obj_name);
                        if (!string.IsNullOrEmpty(_owner))
                            search_pars = string.Format("{0} and lower(table_scheam)=lower('{0}')", search_pars, _owner);
                    }
                    else
                    {
                        search_pars = string.Format("table_name='{0}'", obj_name);
                        if (!string.IsNullOrEmpty(_owner))
                            search_pars = string.Format("{0} and table_scheam='{0}'", search_pars, _owner);
                    }
                    break;
            }

            string sql = string.Format("select 1 from {0} where {1}", search_tablename, search_pars);
            var res = ExecuteScalarCommand(CreateCommand(sql).ExecuteScalar);
            if (res != null && res != DBNull.Value)
                return true;

            return false;
        }

        protected override void CreateLogin(string UserName, string Paswword, string AdditionalOptions)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteLogin(string UserName, string AdditionalOptions)
        {
            throw new NotImplementedException();
        }

        protected override DataTable GetTablePrimaryKey(DataTable table, bool CaseSensivity)
        {
            return table;
        }

        //--Желательно отключать максоендж в постгре, а то будет 50...
        internal override DbColumnInformation GetDbColumnInformation(string tablename, string columnname, bool CaseSensivity)
        {
            var colinfo = base.GetDbColumnInformation(tablename, columnname, CaseSensivity);
            string sql= null;
            if (!CaseSensivity)
            {
                sql = string.Format("select column_default from information_schema.columns where lower(table_name)=lower('{0}') and lower(column_name)=lower('{1}')", tablename, columnname);
                if (!string.IsNullOrEmpty(_owner))
                    sql = string.Format("{0} and lower(table_schema)=lower({1})", sql, _owner);
            }
            else
            {
                sql = string.Format("select column_default from information_schema.columns where table_name='{0}' and column_name='{1}'", tablename, columnname);
                if (!string.IsNullOrEmpty(_owner))
                    sql = string.Format("{0} and table_schema={1}", sql, _owner);
            }

            var res = ExecuteScalarCommand(CreateCommand(sql).ExecuteScalar);
            if (res != null && res != DBNull.Value)
            {
                if (res.ToString().IndexOf(":") > -1)
                {
                    string def_val = res.ToString();                    
                    def_val = def_val.Substring(0, res.ToString().IndexOf(':'));
                    colinfo.DefaultValue = def_val;
                }
                else colinfo.DefaultValue = res;
            }

            return colinfo;
        }

        protected void InsertDataToDbBulkMethod(DataTable table)
        {
            List<string> columns_names = new List<string>();
            for (int i = 0; i < table.Columns.Count; i++)
                columns_names.Add(table.Columns[i].ColumnName);
            string sql = string.Format("COPY {0}({1}) FROM STDIN", table.TableName, string.Join(",", columns_names.ToArray()));
            
            _cmd = CreateCommand(sql);
            _cmd.CommandType = CommandType.Text;
            var serializer = new NpgsqlCopySerializer(_conn as NpgsqlConnection);
            NpgsqlCopyIn copyIn = new NpgsqlCopyIn((_cmd as NpgsqlCommand), (_conn as NpgsqlConnection), serializer.ToStream);            
            try
            {
                copyIn.Start();
                foreach (DataRow dr in table.Rows)
                {
                    for (int i = 0; i < table.Columns.Count; i++)                                            
                        AddValueToSerializer(serializer, dr[i]);                    

                    serializer.EndRow();
                    serializer.Flush();                    
                }
                copyIn.End();
                serializer.Close();
            }
            catch (Exception e)
            {
                try
                {
                    copyIn.Cancel("Exception has occured!");
                }
                catch (NpgsqlException ex)
                {
                    if (ex.BaseMessage.Contains("Exception has occured!"))
                        throw new Exception(string.Format("Copy was uncanceled. exception1: {0};exception2: {1}", e.Message, ex.Message));
                }
            }
        }

        private void AddValueToSerializer(NpgsqlCopySerializer serializer, object value)
        {
            if (value.GetType() == typeof(Int32))            
                serializer.AddInt32((int)value);
            if (value.GetType() == typeof(Int64))
                serializer.AddInt64((Int64)value);
            if (value.GetType() == typeof(string))
                serializer.AddString(value.ToString());
            if (value.GetType() == typeof(float))
                serializer.AddNumber((float)value);
            if (value.GetType() == typeof(double))
                serializer.AddNumber((double)value);
            if (value.GetType() == typeof(bool))
                serializer.AddBool((bool)value);
            if (value.GetType() == typeof(DateTime))
                serializer.AddDateTime((DateTime)value);
            if (value == null || value == DBNull.Value)
                serializer.AddNull();
            if (value.GetType() == typeof(Guid))
                serializer.AddString(value.ToString());
        }
    }
}
