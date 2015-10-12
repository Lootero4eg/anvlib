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
            _open_bracket='"';
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
            _conn = new OracleConnection(ConnectionString);
            _conn.Open();
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
            _cmd = new OracleCommand(CmdText, (OracleConnection)_conn);
            return _cmd;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(string SQLText)
        {
            _DA = new OracleDataAdapter(SQLText, (OracleConnection)_conn);
            return _DA;
        }

        /// <summary>
        /// Метод создание DbDataAdapter-а
        /// </summary>
        /// <param name="cmd">Команда которая будет заполнять DbDataAdapter</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            _DA = new OracleDataAdapter((OracleCommand)_cmd);
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
                    MessagePrinter.PrintMessage(e.Message, "Ошибка базы данных", 1, 1);
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
                    MessagePrinter.PrintMessage(e.Message, "Ошибка базы данных", 1, 1);
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
                    MessagePrinter.PrintMessage(e.Message, "Ошибка базы данных", 1, 1);
            }

            return null;
        }

        [Incomplete]
        public override void CreateTable(DataTable table) 
        {
            if (Connected)
            {
                base.CreateTable(table);

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


                    //--Придумать как сделать создание сиквенцов и привязку триггера
                    /*if (table.Columns[i].AutoIncrement)
                        sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";*/
                    if (!table.Columns[i].AllowDBNull)
                        sqlsc += " NOT NULL";
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
                    InsertDataToDb(table, _parameters_prefix);
            }
            else
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage("Не установлено соединение с базой данных!", "Ошибка", 1, 1);
            }
        }
                
        public override bool IsDBObjectExists(string obj_name, DataBaseObjects obj_type, bool CaseSensivity)
        {
            if (Connected)
            {
                string sql = "";
                if (CaseSensivity)
                    sql = "select 1 from all_objects where object_name='{0}' and object_type='{1}'";
                else
                    sql = "select 1 from all_objects where lower(object_name)=lower('{0}') and object_type='{1}'";

                sql = string.Format(sql, obj_name, GetObjectTypeCode(obj_type));

                var exec_res = ExecuteScalarCommand(CreateCommand(sql).ExecuteScalar);
                if (exec_res != null && exec_res != DBNull.Value)
                    return true;
            }
            else
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage("Не установлено соединение с базой данных!", "Ошибка", 1, 1);
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
    }
}
