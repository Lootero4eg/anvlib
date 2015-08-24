using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using Npgsql;
using NpgsqlTypes;

namespace anvlib.Base
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
        public override DbCommand CreateCommand(string CmdText)
        {
            _cmd = new NpgsqlCommand(CmdText, (NpgsqlConnection)_conn);
            return _cmd;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        public override DbDataAdapter CreateDataAdapter(string SQLText)
        {
            _DA = new NpgsqlDataAdapter(SQLText, (NpgsqlConnection)_conn);
            return _DA;
        }

        /// <summary>
        /// Метод создание DbDataAdapter-а
        /// </summary>
        /// <param name="cmd">Команда которая будет заполнять DbDataAdapter</param>
        /// <returns></returns>
        public override DbDataAdapter CreateDataAdapter(DbCommand cmd)
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
        public override DbParameter CreateParameter(string ParName, DbType ParType, int ParSize)
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
                System.Windows.Forms.MessageBox.Show(e.Message);
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
                System.Windows.Forms.MessageBox.Show(e.Message);
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
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

            return null; ;
        }

        protected override void CreateLogin(string UserName, string Paswword, string AdditionalOptions)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteLogin(string UserName, string AdditionalOptions)
        {
            throw new NotImplementedException();
        }
    }
}
