using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using MySql.Data.MySqlClient;
using MySql.Data.Types;

using anvlib.Classes.Attributes;
using anvlib.Data.Database;
using anvlib.Enums;

namespace anvlib.Data.Database
{
    public class BaseMySQLManager : BaseDbManager
    {
        /// <summary>
        /// Конструктор с дефолтными значениями
        /// </summary>
        public BaseMySQLManager()
            : base()
        {
            _open_bracket = '`';
            _close_bracket = '`';
            _parameters_prefix = "?";
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="ConnectionString">Строка инициализации</param>
        public override void Connect(string ConnectionString)
        {
            _connectionString = ConnectionString;
            _conn = new MySqlConnection(ConnectionString);
            try
            {
                _conn.Open();
            }
            catch (MySqlException ex)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ex.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);
            }
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="srvname">Имя или айпи сервера</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>         
        /// <param name="database">База данных</param>         
        public void Connect(string srvname, string login, string password, string database)
        {
            string connstr = string.Format("server={0};uid={1};password={2};database={3}",
                srvname, login, password, database);

            Connect(connstr);
        }

        /// <summary>
        /// Коннекция
        /// </summary>
        public override DbConnection Connection
        {
            get
            {
                return (_conn as MySqlConnection);
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
            _cmd = new MySqlCommand(CmdText, (_conn as MySqlConnection));
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
            _DA = new MySqlDataAdapter(SQLText, (_conn as MySqlConnection));
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
            _DA = new MySqlDataAdapter((MySqlCommand)cmd);
            DbDataAdapter tmpDA = _DA;

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
            MySqlDbType tmpType = MySqlDbType.Int32;
            switch (ParType)
            {
                case DbType.String:
                    tmpType = MySqlDbType.VarChar;
                    break;

                case DbType.Int16:
                    tmpType = MySqlDbType.Int16;
                    break;

                case DbType.Int32:
                    tmpType = MySqlDbType.Int32;
                    break;

                case DbType.Int64:
                    tmpType = MySqlDbType.Int64;
                    break;

                case DbType.Boolean:
                    tmpType = MySqlDbType.Bit;
                    break;

                case DbType.DateTime:
                    tmpType = MySqlDbType.DateTime;
                    break;

                case DbType.Double:
                    tmpType = MySqlDbType.Float;
                    break;

                case DbType.Guid:
                    tmpType = MySqlDbType.VarChar;                    
                    break;
            }
            #endregion

            _param = new MySqlParameter(ParName, tmpType, ParSize);
            DbParameter tmpPar = _param;

            return tmpPar;
        }        

        protected override void CreateLogin(string LoginName, string Paswword, string AdditionalOptions)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteLogin(string LoginName, string AdditionalOptions)
        {
            throw new NotImplementedException();
        }

        public override void CreateTable(DataTable table, DataInsertMethod insert_method, bool PrepareTableForInsert)
        {
            if (Connected)
            {
                base.CreateTable(table, insert_method, PrepareTableForInsert);

                string sqlsc;
                sqlsc = "CREATE TABLE " + table.TableName + "(";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sqlsc += "\n" + table.Columns[i].ColumnName;
                    if (table.Columns[i].DataType.ToString().Contains("System.Int32"))
                        sqlsc += " int";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int64"))//--если будут ошибки, то заменить на bigint
                        sqlsc += " bigint";
                    else if (table.Columns[i].DataType.ToString().Contains("System.DateTime"))
                        sqlsc += " datetime";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Decimal"))
                        sqlsc += " decimal";//--возможны ошибки
                    else if (table.Columns[i].DataType.ToString().Contains("System.String"))
                    {
                        if (table.Columns[i].MaxLength <= 255)
                            sqlsc += " varchar(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ")";
                        else
                            sqlsc += " text";
                    }
                    /*else if (table.Columns[i].DataType.ToString().Contains("System.Single")) //--незнаю какой тип его подменяет
                        sqlsc += " single";*/
                    else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                        sqlsc += " double";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Guid"))
                        sqlsc += " char(36)";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Boolean"))
                        sqlsc += " tinyint(1)";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte"))
                        sqlsc += " tinyint";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int16"))
                        sqlsc += " smallint";
                    else
                        sqlsc += " varchar(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ")";


                    if (table.Columns[i].AutoIncrement)
                        sqlsc += " AUTO_INCREMENT";// +table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ")";
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
                        InsertDataToDb(table, _parameters_prefix, PrepareTableForInsert);
                        CommitTransaction();
                    }
                }
            }
            else
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ErrorsManager.Messages.NotConnectedMsg, ErrorsManager.Messages.ErrorMsg, 1, 1);
            }
        }

        public override void DropTable(string TableName)
        {
            base.DropTable(TableName);
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
            catch (MySqlException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
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
            catch (MySqlException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
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
            catch (MySqlException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);

                _last_error = e.ErrorCode;
            }

            return null;
        }        

        /// <summary>
        /// Метод, проверяющий наличие объекта в базе данных по имени
        /// </summary>
        /// <param name="obj_name">Искомый объект</param>
        /// <param name="obj_type">Тип объекта(таблица, триггер и т.д.)</param>
        /// <param name="CaseSensivity">Чувствительность регистра</param>
        /// <returns></returns>
        public override bool IsDBObjectExists(string obj_name, DataBaseObjects obj_type, bool CaseSensivity)
        {
            string sql = "";            
            switch (obj_type)
            { 
                case DataBaseObjects.table:
                    if (CaseSensivity)
                        sql = string.Format("select 1 from information_schema.TABLES where table_name='{0}'", obj_name);
                    else
                        sql = string.Format("select 1 from information_schema.TABLES where lower(table_name)=lower('{0}')", obj_name);
                    break;
            }

            var res = ExecuteScalarCommand(CreateCommand(sql).ExecuteScalar);

            if (res != null && res != DBNull.Value)
                return true;

            return false;
        }

        protected override DataTable GetTablePrimaryKey(DataTable table, bool CaseSensivitye)
        {
            return table;
        }

        //--У данного драйвера DataAdapter сам вытаскивает Primary Keys,Nullable, MaxLength and Precision. Жаль что Дефолты еще не вытаскивает!
        public override DataTable GetTableFromDb(string tablename, bool with_primary_key, bool with_max_string_length, bool with_default_values, bool CaseSensivity)
        {
            return base.GetTableFromDb(tablename, with_primary_key, with_max_string_length, with_default_values, CaseSensivity);
        }

        internal override DbColumnInformation GetDbColumnInformation(string tablename, string columnname, bool CaseSensivity)
        {
            DbColumnInformation res = base.GetDbColumnInformation(tablename, columnname, CaseSensivity);
            string sql = "";
            if (!CaseSensivity)
            {
                sql = "select * from information_schema.columns where lower(table_name)=lower('{0}') and lower(column_name)=lower('{1}')";
                if (!string.IsNullOrEmpty(_owner))
                    sql += " and lower(table_schema)=lower('{2}')";
            }
            else
            {
                sql = "select * from information_schema.columns where table_name='{0}' and column_name='{1}'";
                if (!string.IsNullOrEmpty(_owner))
                    sql += " and table_schema='{2}'";
            }

            if (string.IsNullOrEmpty(_owner))
                sql = string.Format(sql, tablename, columnname);
            else
                sql = string.Format(sql, tablename, columnname, _owner);

            _DR = ExecuteDataReader(CreateCommand(sql).ExecuteReader);
            while (_DR.Read())
            {
                if (_DR["column_default"] != null && _DR["column_default"] != DBNull.Value)
                    res.DefaultValue = _DR["column_default"];
                break;
            }
            _DR.Close();

            return res;
        }
    }
}
