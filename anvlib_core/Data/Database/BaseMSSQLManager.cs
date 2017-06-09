using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using anvlib.Enums;
using anvlib.Interfaces;
using anvlib.Classes.Attributes;

namespace anvlib.Data.Database
{
    /// <summary>
    /// Базовый класс для MSSQL серверов
    /// </summary>    
    public class BaseMSSQLManager : BaseDbManager
    {
        /// <summary>
        /// Конструктор с дефолтными значениями
        /// </summary>
        public BaseMSSQLManager()
            : base()
        {
            _open_bracket = '[';
            _close_bracket = ']';
            _parameters_prefix = "@";
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="msg_system"></param>
        public BaseMSSQLManager(IPrintMessageSystem msg_system)
            : base(msg_system)
        {
            _open_bracket = '[';
            _close_bracket = ']';
            _parameters_prefix = "@";
            _owner = "dbo";
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="ConnectionString">Строка инициализации</param>
        public override void Connect(string ConnectionString)
        {
            _connectionString = ConnectionString;
            _conn = new SqlConnection(ConnectionString);
            try
            {
                _conn.Open();
            }
            catch (SqlException ex)
            {
                string msg = GetErrorMessage(ex);
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(msg, ErrorsManager.Messages.DBErrorMsg, 1, 1);
            }
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="srvname">Имя или апйпи сервера БД</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="database">База данных</param>        
        public void Connect(string srvname, string login, string password, string database)
        {
            Connect(GetConnectionString(srvname, login, password, database, false));
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="srvname">Имя или апйпи сервера БД</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="database">База данных</param>
        public void Connect(string srvname, string login, string password, string database, string app_name)
        {
            Connect(GetConnectionString(srvname, login, password, database, false, app_name));
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="srvname">Имя или апйпи сервера БД</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="database">База данных</param>
        /// <param name="WindowsLogin">Использовать аутентификацию Windows</param>
        public void ConnectAsWindowsUser(string srvname, string database)
        {
            Connect(GetConnectionString(srvname, "", "", database, true));
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="srvname">Имя или апйпи сервера БД</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="database">База данных</param>
        /// <param name="WindowsLogin">Использовать аутентификацию Windows</param>
        /// <param name="app_name">Отоьбражаемое имя приложения</param>
        public void ConnectAsWindowsUser(string srvname, string database, string app_name)
        {
            Connect(GetConnectionString(srvname, "", "", database, true, app_name));
        }

        /// <summary>
        /// Метод генерирующий строку подключения к серверу
        /// </summary>
        /// <param name="srvname"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        /// <param name="WindowsLogin"></param>
        /// <returns></returns>
        public static string GetConnectionString(string srvname, string login, string password, string database, bool WindowsLogin)
        {
            string connstr = "";
            if (!WindowsLogin)
                connstr = string.Format("server={0};uid={1};pwd={2};database={3};MultipleActiveResultSets=true",
                    srvname, login, password, database);
            else
                connstr = string.Format("server={0};Integrated Security=SSPI;database={2};MultipleActiveResultSets=true",
                    srvname, login, database);

            return connstr;
        }

        /// <summary>
        /// Метод генерирующий коннект стринг
        /// </summary>
        /// <param name="srvname"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        /// <param name="WindowsLogin"></param>
        /// <param name="app_name"></param>
        /// <returns></returns>
        public static string GetConnectionString(string srvname, string login, string password, string database, bool WindowsLogin, string app_name)
        {
            string connstr = "";
            if (!WindowsLogin)
                connstr = string.Format("server={0};uid={1};pwd={2};database={3};MultipleActiveResultSets=true;Application Name={4}",
                    srvname, login, password, database, app_name);
            else
                connstr = string.Format("server={0};Integrated Security=SSPI;database={2};Application Name={3}",
                    srvname, login, database, app_name);

            return connstr;
        }

        /// <summary>
        /// Коннекция
        /// </summary>
        public override DbConnection Connection
        {
            get
            {
                return (_conn as SqlConnection);
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
            _cmd = new SqlCommand(CmdText, (_conn as SqlConnection));
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
            _DA = new SqlDataAdapter(SQLText, (_conn as SqlConnection));
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
            _DA = new SqlDataAdapter((SqlCommand)cmd);
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
            //--если параметр типа VarBinary, то чтобы он заработал, надо преобразовать DbCommand к SqlCommand и сделать в Parameters.AddWithValue!!!!!!!
            _param = new SqlParameter(ParName, ParameterTypeConverter(ParType), ParSize);
            DbParameter tmpPar = _param;

            return tmpPar;
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
            catch (SqlException e)
            {
                string msg = GetErrorMessage(e);
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(msg, ErrorsManager.Messages.DBErrorMsg, 1, 1);
            }
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
            catch (SqlException e)
            {
                string msg = GetErrorMessage(e);
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(msg, ErrorsManager.Messages.DBErrorMsg, 1, 1);
            }

            return null;
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
            catch (SqlException e)
            {
                string msg = GetErrorMessage(e);
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(msg, ErrorsManager.Messages.DBErrorMsg, 1, 1);
            }

            return null;
        }

        /// <summary>
        /// Метод, создания таблицы в БД
        /// </summary>
        /// <param name="table">Переменная типа DataTable</param>
        [Incomplete]
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
                        sqlsc += " int";
                    else if (table.Columns[i].DataType.ToString().Contains("System.DateTime"))
                        sqlsc += " datetime";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Decimal"))
                        sqlsc += " decimal";//--возможны ошибки
                    else if (table.Columns[i].DataType.ToString().Contains("System.String"))
                    {
                        if (table.Columns[i].MaxLength <= 8000)
                            sqlsc += " varchar(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ")";
                        else
                            sqlsc += " text";
                    }
                    else if (table.Columns[i].DataType.ToString().Contains("System.Single"))
                        sqlsc += " single";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                        sqlsc += " float";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Guid"))
                        sqlsc += " uniqueidentifier";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Boolean"))
                        sqlsc += " bit";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte"))
                        sqlsc += " tinyint";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int16"))
                        sqlsc += " int";
                    else
                        sqlsc += " varchar(" + (table.Columns[i].MaxLength > -1 ? table.Columns[i].MaxLength.ToString() : "50") + ")";


                    if (table.Columns[i].AutoIncrement)
                        sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ")";
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
                        InsertDataToDbBulkMethod(table);
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

        /// <summary>
        /// Метод, проверяющий наличие объекта в базе данных по имени
        /// </summary>
        /// <param name="obj_name">Искомый объект</param>
        /// <param name="obj_type">Тип объекта(таблица, триггер и т.д.)</param>
        /// <param name="CaseSensivity">Чувствительность регистра</param>
        /// <returns></returns>
        public override bool IsDBObjectExists(string obj_name, DataBaseObjects obj_type, bool CaseSensivity)
        {
            if (Connected)
            {
                //--Тут надо переделывать, т.к. в объектах нет колонок, да и ф-ции тоже разных типов! так что пока только таблички можно искать
                string sql = "";
                if (CaseSensivity)
                {
                    sql = string.Format("select 1 from sys.objects where type='{0}' and name='{1}'",
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
                    MessagePrinter.PrintMessage(ErrorsManager.Messages.NotConnectedMsg, ErrorsManager.Messages.ErrorMsg, 1, 1);
            }

            return false;
        }

        private string GetErrorMessage(SqlException exp)
        {
            string msg = exp.Message;
            if (exp.Number == 201)
                msg = string.Format("{0}{1}", ErrorsManager.Messages.SQLSERVER_201, exp.Message);

            if (exp.Number == 229)
                msg = string.Format("{0}{1}", ErrorsManager.Messages.SQLSERVER_229, exp.Message);

            if (exp.Number == 547)
                msg = string.Format("{0}{1}", ErrorsManager.Messages.SQLSERVER_547, exp.Message);

            if (exp.Number == 18456)
                msg = string.Format("{0}{1}", ErrorsManager.Messages.SQLSERVER_18456, exp.Message);

            _last_error = exp.Number;            

            return msg;
        }

        //--Продумать работу с Пользователями/группами, а так же ролями и пермиженнами СУБД
        //--Как то незабыть сделать проверку на существование логинов и юзеров (sys.server_principa;s or sys.database_principals)
        protected override void CreateLogin(string LoginName, string Paswword, string AdditionalOptions)
        {
            string pass = "";
            if (!string.IsNullOrEmpty(Paswword))
                pass = string.Format("with password='{0}'", Paswword);
            string sql = string.Format(
                "create login {0} {1} {2}", LoginName, pass, AdditionalOptions);

            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        protected override void DeleteLogin(string LoginName, string AdditionalOptions)
        {
            string sql = string.Format("drop login {0}", LoginName);
            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        internal static class UserAdditionalOptions
        {
            public static string SetSqlUserDataBase(string DbName)
            {
                return string.Format(", DEFAULT_DATABASE = {0}", DbName);
            }

            public static string SetWindowsUser()
            {
                return "from windows ";
            }

            public static string SetWindowsUserDataBase(string DbName)
            {
                return string.Format("with DEFAULT_DATABASE = {0}", DbName);
            }
        }

        protected void CreateSQLLogin(string login, string password, string dbname)
        {
            CreateLogin(login, password, UserAdditionalOptions.SetSqlUserDataBase(dbname));
        }

        protected void CreateWindowsLogin(string login, string password, string dbname)
        {
            string adinfo = UserAdditionalOptions.SetWindowsUser();
            CreateLogin("[" + login + "]", "", adinfo + UserAdditionalOptions.SetWindowsUserDataBase(dbname));
        }

        protected void CreateUser(string UserName, string BindToLoginName)
        {
            string sql = string.Format("create user {0} for login {1}", UserName, BindToLoginName);
            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        protected void AddRoleToUser(string user_name, string role)
        {
            string sql = string.Format("sp_addrolemember  @rolename =  '{0}', @membername =  '{1}'", role, user_name);
            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        protected void DropRoleToUser(string user_name, string role)
        {
            string sql = string.Format("sp_droprolemember  @rolename =  '{0}', @membername =  '{1}'", role, user_name);
            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        protected void DeleteUser(string UserName)
        {
            string sql = string.Format("drop user {0}", UserName);
            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        protected string GetObjectTypeCode(DataBaseObjects db_object)
        {
            string res = "U";

            switch (db_object)
            {
                case DataBaseObjects.foreign_key:
                    res = "F";
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
                    break;

                case DataBaseObjects.table:
                    res = "U";
                    break;

                case DataBaseObjects.trigger:
                    res = "TR";
                    break;

                case DataBaseObjects.type:
                    res = "T";
                    break;
            }

            return res;
        }

        protected override DataTable GetTablePrimaryKey(DataTable table, bool CaseSensivity)
        {
            string sql = "";
            if (!CaseSensivity)
            {
                sql = string.Format("SELECT "
                            + "   schema_name(ta.schema_id)  SchemaName "
                            + "  ,ta.name  TableName "
                            + "  ,ind.name "
                            + "  ,indcol.key_ordinal Ord "
                            + "  ,col.name  ColumnName "
                            + "  ,ind.type_desc "
                            + "  ,ind.fill_factor "
                            + " from sys.tables ta "
                            + "  inner join sys.indexes ind "
                            + "   on ind.object_id = ta.object_id "
                            + "  inner join sys.index_columns indcol "
                            + "   on indcol.object_id = ta.object_id "
                            + "    and indcol.index_id = ind.index_id "
                            + "  inner join sys.columns col "
                            + "   on col.object_id = ta.object_id "
                            + "    and col.column_id = indcol.column_id "
                            + " where ind.is_primary_key = 1 and lower(ta.name)=lower('{0}')"
                            + " order by "
                            + "   ta.name "
                            + "  ,indcol.key_ordinal "
                            , table.TableName);
            }
            else
            {
                sql = string.Format("SELECT "
                            + "   schema_name(ta.schema_id)  SchemaName "
                            + "  ,ta.name  TableName "
                            + "  ,ind.name "
                            + "  ,indcol.key_ordinal Ord "
                            + "  ,col.name  ColumnName "
                            + "  ,ind.type_desc "
                            + "  ,ind.fill_factor "
                            + " from sys.tables ta "
                            + "  inner join sys.indexes ind "
                            + "   on ind.object_id = ta.object_id "
                            + "  inner join sys.index_columns indcol "
                            + "   on indcol.object_id = ta.object_id "
                            + "    and indcol.index_id = ind.index_id "
                            + "  inner join sys.columns col "
                            + "   on col.object_id = ta.object_id "
                            + "    and col.column_id = indcol.column_id "
                            + " where ind.is_primary_key = 1 and ta.name='{0}'"
                            + " order by "
                            + "   ta.name "
                            + "  ,indcol.key_ordinal "
                            , table.TableName);
            }

            _DR = ExecuteDataReader(CreateCommand(sql).ExecuteReader);

            List<DataColumn> cols = new List<DataColumn>();
            while (_DR.Read())
            {
                var col_name = _DR["ColumnName"].ToString();
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
                sql = string.Format("select is_nullable,max_length,precision,scale, dc.definition,c.system_type_id "
			        + "from sys.tables t inner join sys.columns c on c.object_id=t.object_id  "
			        + "left outer join sys.default_constraints dc on dc.parent_object_id=c.object_id and c.column_id=dc.parent_column_id "
			        + "where t.name='{0}' and c.name='{1}'"
			        , tablename, columnname);
            }
            else
                sql = string.Format("select is_nullable,max_length,precision,scale, dc.definition,c.system_type_id "
                    + "from sys.tables t inner join sys.columns c on c.object_id=t.object_id  "
                    + "left outer join sys.default_constraints dc on dc.parent_object_id=c.object_id and c.column_id=dc.parent_column_id "
                    + "where lower(t.name)=lower('{0}') and lower(c.name)=lower('{1}')"
                    , tablename, columnname);

            _DR = ExecuteDataReader(CreateCommand(sql).ExecuteReader);

            while (_DR.Read())
            {
                res.IsNullable = (bool)_DR["is_nullable"];
                res.MaxLength = Convert.ToInt32(_DR["max_length"]);
                res.Precision = Convert.ToInt32(_DR["precision"]);
                res.Sacale = Convert.ToInt32(_DR["scale"]);
                if (_DR["definition"] != DBNull.Value)
                {
                    res.DefaultValue = _DR["definition"];
                    res.DefaultValue = DefValuePostProcessing(res.DefaultValue);
                }
                else
                    res.DefaultValue = null;

                if (_DR["system_type_id"] != null && _DR["system_type_id"] != DBNull.Value)
                {
                    int coltype = Convert.ToInt32(_DR["system_type_id"]);
                    if (coltype == 35)
                        res.MaxLength = 0;
                }

                break;
            }

            _DR.Close();

            /*var result = ExecuteScalarCommand(CreateCommand(sql).ExecuteScalar);
            if (result != null && result != DBNull.Value)
                res.MaxLength = Convert.ToInt32(result);*/

            return res;
        }

        protected void InsertDataToDbBulkMethod(DataTable table)
        {
            SqlBulkCopy bcopy = new SqlBulkCopy(_conn as SqlConnection);
            bcopy.DestinationTableName = table.TableName;
            bcopy.WriteToServer(table);
            bcopy.Close();            
        }

        private object DefValuePostProcessing(object defval)
        {
            string tmpst = defval.ToString();

            while (true)
            {
                if (tmpst.IndexOf("(") < 0)
                    break;
                else
                {
                    while (tmpst[0] == '(')
                        tmpst = tmpst.Remove(0, 1);
                    while (tmpst[tmpst.Length - 1] == ')')
                        tmpst = tmpst.Remove(tmpst.Length - 1, 1);
                    if (tmpst.IndexOf("(") < 0)
                        return tmpst;
                    else
                        return DBNull.Value;
                }
            }

            return defval;
        }

        protected SqlDbType ParameterTypeConverter(DbType ParType)
        {            
            SqlDbType tmpType = SqlDbType.Int;
            switch (ParType)
            {
                case DbType.Binary:
                    tmpType = SqlDbType.VarBinary;
                    break;

                case DbType.String:
                    tmpType = SqlDbType.VarChar;
                    break;

                case DbType.Int16:
                    tmpType = SqlDbType.Int;
                    break;

                case DbType.Int32:
                    tmpType = SqlDbType.Int;
                    break;

                case DbType.Int64:
                    tmpType = SqlDbType.BigInt;
                    break;

                case DbType.Boolean:
                    tmpType = SqlDbType.Bit;
                    break;

                case DbType.DateTime:
                    tmpType = SqlDbType.DateTime;
                    break;

                case DbType.Double:
                    tmpType = SqlDbType.Float;
                    break;

                case DbType.Guid:
                    tmpType = SqlDbType.UniqueIdentifier;
                    break;

                case DbType.Xml:
                    tmpType = SqlDbType.Xml;
                    break;
            }

            return tmpType;
        }
    }
}
