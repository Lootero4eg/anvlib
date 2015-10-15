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
                    MessagePrinter.PrintMessage(ex.Message, MsgMgr.MessageText.DBErrorMsg, 1, 1);
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
            throw new NotImplementedException();
        }

        protected override void CreateLogin(string LoginName, string Paswword, string AdditionalOptions)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteLogin(string LoginName, string AdditionalOptions)
        {
            throw new NotImplementedException();
        }


        public override void CreateTable(DataTable table, DataInsertMethod insert_method)
        {
            base.CreateTable(table, insert_method);
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
            catch (MySqlException e)
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
            catch (MySqlException e)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, MsgMgr.MessageText.DBErrorMsg, 1, 1);
            }

            return null;
        }

        /*protected override void InsertDataToDb(DataTable table, string parameters_prefix)
        {
            base.InsertDataToDb(table, parameters_prefix);
        }*/

        /// <summary>
        /// Метод, проверяющий наличие объекта в базе данных по имени
        /// </summary>
        /// <param name="obj_name">Искомый объект</param>
        /// <param name="obj_type">Тип объекта(таблица, триггер и т.д.)</param>
        /// <param name="CaseSensivity">Чувствительность регистра</param>
        /// <returns></returns>
        public override bool IsDBObjectExists(string obj_name, Enums.DataBaseObjects obj_type, bool CaseSensivity)
        {
            throw new NotImplementedException();
        }

        protected override DataTable GetTablePrimaryKey(DataTable table, bool CaseSensivitye)
        {
            return table;
        }
    }
}
