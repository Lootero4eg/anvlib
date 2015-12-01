using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using anvlib.Classes.Attributes;
using anvlib.Enums;
using anvlib.Interfaces;

namespace anvlib.Data.Database
{
    /// <summary>
    /// Базовый класс для серверов на базе Oracle 8-11
    /// </summary>
    public class BaseOracleManager : BaseDbManager
    {
        /// <summary>
        /// Конструктор с дефолтными значениями
        /// </summary>
        public BaseOracleManager()
            : base()
        {
            _open_bracket = '"';
            _close_bracket = '"';
            _parameters_prefix = ":";
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="msg_system"></param>
        public BaseOracleManager(IPrintMessageSystem msg_system)
            : base(msg_system)
        {
            _open_bracket = '"';
            _close_bracket = '"';
            _parameters_prefix = ":";
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="srvname">Имя или айпи сервера</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>                
        public void Connect(string srvname, string login, string password)
        {
            string connstr = string.Format("data source={0};user id={1};password={2}",
                srvname, login, password);

            Connect(connstr);
        }

        public string GetConnectionString(string srvname, string login, string password)
        {
            string connstr = string.Format("data source={0};user id={1};password={2}",
                srvname, login, password);

            return connstr;
        }


        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="ConnectionString">Строка инициализации</param>
        public override void Connect(string ConnectionString)
        {
            try
            {
                _conn = new OracleConnection(ConnectionString);
                _conn.Open();
            }
            catch (OracleException ex)
            {                
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ex.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = ex.ErrorCode;
            }
        }

        /// <summary>
        /// Коннекция
        /// </summary>
        public override DbConnection Connection
        {
            get
            {
                return (_conn as OracleConnection);
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
            _cmd = new OracleCommand(CmdText, (_conn as OracleConnection));
            DbCommand tmpCmd = _cmd;
            if (CmdText != null && CmdText.Length > 0)
                if (CmdText.Split(' ').Length == 1)
                    tmpCmd.CommandType = CommandType.StoredProcedure;

            if (_transaction != null)
                tmpCmd.Transaction = _transaction;

            return tmpCmd;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(string SQLText)
        {
            _DA = new OracleDataAdapter(SQLText, (_conn as OracleConnection));            
            DbDataAdapter tmpDA = _DA;            

            return tmpDA;            
        }

        /// <summary>
        /// Метод создание DbDataAdapter-а
        /// </summary>
        /// <param name="cmd">Команда которая будет заполнять DbDataAdapter</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            _DA = new OracleDataAdapter((OracleCommand)cmd);            
            DbDataAdapter tmpDA = _DA;
            //tmpDA.SelectCommand.Connection = (_conn as OracleConnection); //-- по странной причине само не проставляется из конструктора

            return tmpDA;
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
            OracleDbType tmpType = OracleDbType.Int32;
            switch (ParType)
            {
                case DbType.String:
                    tmpType = OracleDbType.Varchar2;
                    break;

                case DbType.Int16:
                    tmpType = OracleDbType.Int16;
                    break;

                case DbType.Int32:
                    tmpType = OracleDbType.Int32;
                    break;

                case DbType.Int64:
                    tmpType = OracleDbType.Int64;
                    break;

                case DbType.Boolean:
                    tmpType = OracleDbType.Int16;
                    break;

                case DbType.DateTime:
                    tmpType = OracleDbType.Date;
                    break;
            }
            #endregion

            _param = new OracleParameter(ParName, tmpType, (ParSize > -1 ? ParSize : 0));
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
                    MessagePrinter.PrintMessage(e.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
            }
        }

        protected override object ExecuteScalarCommand(ExecuteScalarCmdDelegate proc)
        {
            try
            {
                return proc.Invoke();
            }
            catch (OracleException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
            }

            return null;
        }

        protected override DbDataReader ExecuteDataReader(ExecuteDataReaderCmdDelegate proc)
        {
            try
            {
                return proc.Invoke();
            }
            catch (OracleException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
            }

            return null;
        }

        [Incomplete]
        public override void CreateTable(DataTable table, DataInsertMethod insert_method)
        {
            if (Connected)
            {
                base.CreateTable(table, insert_method);

                string sqlsc;
                sqlsc = "CREATE TABLE " + table.TableName + "(";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sqlsc += "" + table.Columns[i].ColumnName;
                    if (table.Columns[i].DataType.ToString().Contains("System.Int32"))
                        sqlsc += " number";
                    else if (table.Columns[i].DataType.ToString().Contains("System.DateTime"))
                        sqlsc += " datetime";
                    else if (table.Columns[i].DataType.ToString().Contains("System.String"))
                        sqlsc += " varchar2(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ")";
                    /*else if (table.Columns[i].DataType.ToString().Contains("System.Single"))
                        sqlsc += " single ";*/
                    else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                        sqlsc += " float";                    
                    else if (table.Columns[i].DataType.ToString().Contains("System.Guid"))
                        sqlsc += " raw(32)";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Boolean"))
                        sqlsc += " number(1)";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte"))
                        sqlsc += " number(1)";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int16"))
                        sqlsc += " number";
                    else
                        sqlsc += " varchar2(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ")";


                    #warning Придумать как сделать создание сиквенцов и привязку триггера
                    /*if (table.Columns[i].AutoIncrement)
                        sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";*/
                    if (!table.Columns[i].AllowDBNull)
                        sqlsc += " NOT NULL";
                    if (table.Columns[i].DefaultValue != null && table.Columns[i].DefaultValue != DBNull.Value)
                        sqlsc += " DEFAULT " + table.Columns[i].DefaultValue.ToString();
                    sqlsc += ",";
                }

                if (table.PrimaryKey.Length > 0)
                {
                    string pks = "CONSTRAINT PK_" + table.TableName + " PRIMARY KEY (";
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
                    if (insert_method == DataInsertMethod.Normal)
                        InsertDataToDb(table, _parameters_prefix);
                    if (insert_method == DataInsertMethod.FastIfPossible)
                        InsertDataToDbBulkMethod(table);                    
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
            if (Connected)
            {
                string sql = "";
                if (CaseSensivity)
                {                 
                    sql = "select 1 from all_objects where object_name='{0}' and object_type='{1}'";
                    if (!string.IsNullOrEmpty(_owner))
                        sql += " and owner='{2}'";
                }
                else
                {
                    sql = "select 1 from all_objects where upper(object_name)=upper('{0}') and object_type='{1}'";
                    if (!string.IsNullOrEmpty(_owner))
                        sql += " and upper(owner)=upper('{2}')";
                }

                if (string.IsNullOrEmpty(_owner))
                    sql = string.Format(sql, obj_name, GetObjectTypeCode(obj_type));
                else
                    sql = string.Format(sql, obj_name, GetObjectTypeCode(obj_type), _owner);
                
                var exec_res = ExecuteScalarCommand(CreateCommand(sql).ExecuteScalar);
                if (exec_res != null && exec_res != DBNull.Value)
                    return true;
            }
            else
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ErrorsManager.Messages.NotConnectedMsg, ErrorsManager.Messages.ErrorMsg, 1, 1);
            }

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

        [Incomplete]
        protected string GetObjectTypeCode(DataBaseObjects db_object)
        {
            string res = "TABLE";

            switch (db_object)
            {
                case DataBaseObjects.foreign_key:
                    res = "";
                    break;

                case DataBaseObjects.function:
                    res = "";
                    break;

                case DataBaseObjects.index:
                    res = "";
                    break;

                case DataBaseObjects.primary_key:
                    res = "";
                    break;

                case DataBaseObjects.procedure:
                    res = "";
                    break;

                case DataBaseObjects.table:
                    res = "TABLE";
                    break;

                case DataBaseObjects.trigger:
                    res = "TRIGGER";
                    break;

                case DataBaseObjects.type:
                    res = "";
                    break;
            }

            return res;
        }
        
        protected override DataTable GetTablePrimaryKey(DataTable table, bool CaseSensivity)
        {
            string sql = null;
            if (CaseSensivity)
            {
                sql = "select column_name "
                + "from all_constraints ac inner join all_cons_columns c on c.CONSTRAINT_NAME=ac.CONSTRAINT_NAME "
                + "where ac.constraint_type='P' and ac.table_name='{0}'";
                if (!string.IsNullOrEmpty(_owner))
                    sql += " and c.OWNER='{1}'";
            }
            else
            {
                sql = "select column_name "
                + "from all_constraints ac inner join all_cons_columns c on c.CONSTRAINT_NAME=ac.CONSTRAINT_NAME "
                + "where ac.constraint_type='P' and upper(ac.table_name)=upper('{0}')";
                if (!string.IsNullOrEmpty(_owner))
                    sql += " and upper(c.OWNER)=upper('{1}')";
            }

            if (string.IsNullOrEmpty(_owner))
                sql = string.Format(sql, table.TableName);
            else
                sql = string.Format(sql, table.TableName, _owner);

            _DR = ExecuteDataReader(CreateCommand(sql).ExecuteReader);

            List<DataColumn> cols = new List<DataColumn>();
            while (_DR.Read())
            {
                var col_name = _DR["column_name"].ToString();
                cols.Add(table.Columns[col_name]);
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
            if (CaseSensivity)
            {
                sql = "select TABLE_NAME,COLUMN_NAME,DATA_TYPE,DATA_LENGTH, "
                + "data_precision,data_scale,NULLABLE,DATA_DEFAULT "
                + " from all_tab_columns "
                + "where  "
                + "table_name='{0}' and column_name='{1}'";
                if (!string.IsNullOrEmpty(_owner))
                    sql += " and owner='{2}'";
            }
            else
            {
                sql = "select TABLE_NAME,COLUMN_NAME,DATA_TYPE,DATA_LENGTH, "
                + "data_precision,data_scale,NULLABLE,DATA_DEFAULT "
                + " from all_tab_columns "
                + "where  "
                + "upper(table_name)=upper('{0}') and upper(column_name)=upper('{1}')";
                if (!string.IsNullOrEmpty(_owner))
                    sql += " and upper(owner)=upper('{2}')";
            }

            if (string.IsNullOrEmpty(_owner))
                sql = string.Format(sql, tablename, columnname);
            else
                sql = string.Format(sql, tablename, columnname,_owner);

            _DR = ExecuteDataReader(CreateCommand(sql).ExecuteReader);

            while (_DR.Read())
            {
                if (!string.IsNullOrEmpty(_DR["nullable"].ToString()))
                {
                    if (_DR["nullable"].ToString().ToUpper() == "Y")
                        res.IsNullable = true;
                    if (_DR["nullable"].ToString().ToUpper() == "N")
                        res.IsNullable = false;
                }
                if (_DR["data_length"] != DBNull.Value)
                    res.MaxLength = Convert.ToInt32(_DR["data_length"]);
                if (_DR["data_precision"] != DBNull.Value)
                    res.Precision = Convert.ToInt32(_DR["data_precision"]);
                if (_DR["data_scale"] != DBNull.Value)
                    res.Sacale = Convert.ToInt32(_DR["data_scale"]);

                if (_DR["DATA_DEFAULT"] != DBNull.Value)
                {
                    res.DefaultValue = _DR["DATA_DEFAULT"];
                    //res.DefaultValue = DefValuePostProcessing(res.DefaultValue);
                }
                else
                    res.DefaultValue = null;

                break;
            }

            _DR.Close();

            return res;
        }

        protected void InsertDataToDbBulkMethod(DataTable table)
        {            
            OracleBulkCopy bcopy = new OracleBulkCopy(_conn as OracleConnection);
            bcopy.DestinationTableName = table.TableName;
            bcopy.WriteToServer(table);
            bcopy.Close();
        }

        internal override void PrepareTableSchemeBeforeFill(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
            {
                string sql = "select DATA_TYPE, DATA_PRECISION"
                + " from all_tab_columns "
                + "where  "
                + "upper(table_name)=upper('{0}') and upper(column_name)=upper('{1}')";
                if (!string.IsNullOrEmpty(_owner))
                    sql += " and upper(owner)=upper('{2}')";

                if (string.IsNullOrEmpty(_owner))
                    sql = string.Format(sql, col.Table.TableName, col.ColumnName);
                else
                    sql = string.Format(sql, col.Table.TableName, col.ColumnName, _owner);

                _DR = ExecuteDataReader(CreateCommand(sql).ExecuteReader);
                while (_DR.Read())
                {
                    if (_DR["DATA_TYPE"] != DBNull.Value)
                    {
                        if (_DR["DATA_TYPE"].ToString().ToLower() == "number")
                        {
                            /*var prec = _DR["DATA_PRECISION"];
                            if (prec != DBNull.Value)
                            {
                                int precision = Convert.ToInt32(prec);
                                if (precision == 1)
                                    col.DataType = typeof(bool);
                                else
                                    col.DataType = typeof(int);
                            }
                            else*/
                                col.DataType = typeof(int);
                        }

                        if (_DR["DATA_TYPE"].ToString().ToLower() == "float")
                            col.DataType = typeof(double);
                    }
                }
                _DR.Close();
            }
        }
    }
}
