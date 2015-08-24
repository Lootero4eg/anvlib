using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace anvlib.Base
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
        public override DbCommand CreateCommand(string CmdText)
        {
            _cmd = new OracleCommand(CmdText, (OracleConnection)_conn);
            return _cmd;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        public override DbDataAdapter CreateDataAdapter(string SQLText)
        {
            _DA = new OracleDataAdapter(SQLText, (OracleConnection)_conn);
            return _DA;
        }

        /// <summary>
        /// Метод создание DbDataAdapter-а
        /// </summary>
        /// <param name="cmd">Команда которая будет заполнять DbDataAdapter</param>
        /// <returns></returns>
        public override DbDataAdapter CreateDataAdapter(DbCommand cmd)
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
        public override DbParameter CreateParameter(string ParName, DbType ParType, int ParSize)
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

            _param = new OracleParameter(ParName, tmpType, ParSize);
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
            catch (OracleException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
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
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

            return null;
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
