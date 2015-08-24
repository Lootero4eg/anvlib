using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;

using anvlib.Interfaces;

namespace anvlib.Base
{
    /// <summary>
    /// Этот класс нужен только для других базовых классов
    /// так что используй базовые классы 
    /// типа BaseMSSQLManager и BaseOracleManager
    /// </summary>
    public abstract class BaseDbManager
    {
        protected DbConnection _conn;
        protected DbCommand _cmd;
        protected DbParameter _param;
        protected DbDataAdapter _DA;
        protected DbDataReader _DR;
        protected DataSet _ds;
        protected DataTable _dt;
        protected DataRow _row;
        protected DataColumn _column;
        protected DbTransaction _transaction;
        protected string _systables_prefix; //--префикс системных таблиц
        protected string _parameters_prefix;//--префикс для параметров, в MySQL и MSSQL отличаются, возможно в оракле тоже
        protected string _TransactionStartCmd;//--команда начала транзакции
        protected string _TransactionEndCmd;//--комнад окончания транзакции
        protected char _open_bracket;//--для избежания проблем с системными полями
        protected char _close_bracket;//--для избежания проблем с системными полями

        protected int _last_error = 0;

        protected IPrintMessageSystem MessagePrinter;

        #region BaseDbManager Properties & Methods

        /// <summary>
        /// Конструктор класс
        /// </summary>
        public BaseDbManager(){ }        

        /// <summary>
        /// Состоянние "Соединения с сервером"
        /// </summary>
        public bool Connected
        {
            get
            {
                if(_conn != null)
                    if( _conn.State == System.Data.ConnectionState.Open)
                        return true;

                return false;
            }
        }

        /// <summary>
        /// Событие возникающее, при ошибке запуска команды, ридера или датаадаптера
        /// К сожалениею, пока вызывается только в потомках!
        /// </summary>
        public event EventHandler ExecutionException;

        /// <summary>
        /// Флаг показывающий, есть ли незавершенные транзанкции
        /// </summary>
        public bool HasUncommitedTransaction
        {
            get 
            {
                if (_transaction != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Установить соединение с сервером
        /// </summary>
        /// <param name="ConnectionString">Строка соединения с сервером</param>
        public abstract void Connect(string ConnectionString);

        /// <summary>
        /// Коннекция. Устанавливается в наследниках класса
        /// </summary>
        public abstract DbConnection Connection { get; set; }

        /// <summary>
        /// Код ошибки выполнения запроса. Если значнеие 0 - значит ошибки нет.
        /// </summary>
        public int LastError { get { return _last_error; } }

        /// <summary>
        /// Установить соединение с сервером, если заранее заполнили свойство Connection
        /// </summary>
        public void Connect()
        {
            Reconnect();
        }

        /// <summary>
        /// Разъединить соединение с сервером
        /// </summary>
        public void Disconnect()
        {
            if (Connected)
            {
                if (_transaction != null)
                    throw new Exception("Необходимо завершить начатую Транзанкцию!");

                _conn.Close();
            }
        }

        /// <summary>
        /// Переоткрыть коннекцию или открыть закрытую коннецию
        /// </summary>
        public void Reconnect()
        {
            if (_conn != null)
            {
                if (_conn.State == ConnectionState.Open)
                    Disconnect();

                try
                {
                    _conn.Open();
                }
                catch (Exception e)
                {
                    if (MessagePrinter != null)
                        MessagePrinter.PrintMessage(e.Message, "Ошибка базы данных", 1, 1);
                }
            }
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        public abstract DbDataAdapter CreateDataAdapter(string SQLText);
        
        /// <summary>
        /// Метод создание DbDataAdapter-а
        /// </summary>
        /// <param name="cmd">Команда которая будет заполнять DbDataAdapter</param>
        /// <returns></returns>
        public abstract DbDataAdapter CreateDataAdapter(DbCommand cmd);

        /// <summary>
        /// Метод создания DbCommand
        /// </summary>
        /// <param name="CmdText">Текст запроса или имя хранимой процедуры</param>
        /// <returns></returns>
        public abstract DbCommand CreateCommand(string CmdText);

        /// <summary>
        /// Метод создание параметра для хранимых процедур
        /// </summary>
        /// <param name="ParName">Имя параметра</param>
        /// <param name="ParType">Тип параметра</param>
        /// <param name="ParSize">Размер параметра. Для строковых параметров и параметров с плавающей точкой</param>
        /// <returns></returns>
        public abstract DbParameter CreateParameter(string ParName, DbType ParType, int ParSize);

        /// <summary>
        /// Метод начинающий транзакцию.       
        /// </summary>
        /// <returns></returns>
        public void BeginTransaction()
        {
            if (Connected)
                _transaction = _conn.BeginTransaction();
            else
                throw new Exception("Соединение с базой данных не инициализировано!");
        }

        /// <summary>
        /// Метод завершающий транзанкцию с сохранением всех ранее выполненных действий
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction = null;
            }
            else
                throw new Exception("Для того чтобы завершить транзанкцию, вам необходимо ее начать!");
        }

        /// <summary>
        /// Метод завершающий транзанкцию с отменой всех ранее выполненных действий
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }
            else
                throw new Exception("Для того чтобы завершить транзанкцию, вам необходимо ее начать!");
        }

        public void SetPrintMessageSystem(IPrintMessageSystem MsgSystem)
        {
            MessagePrinter = MsgSystem;
        }

        protected delegate int ExecuteCmdDelegate();

        protected delegate object ExecuteScalarCmdDelegate();

        protected delegate DbDataReader ExecuteDataReaderCmdDelegate();

        //--незабудь сделать процедуру
        protected delegate int DataAdapterFill1Delegate(DataSet dset);

        //--незабудь сделать процедуру
        protected delegate int DataAdapterFill2Delegate(DataTable table);

        protected abstract void ExecuteCommand(ExecuteCmdDelegate proc);

        protected abstract object ExecuteScalarCommand(ExecuteScalarCmdDelegate proc);

        protected abstract DbDataReader ExecuteDataReader(ExecuteDataReaderCmdDelegate proc);

        protected void RaiseExecuteException(int err_code)
        {
            if (ExecutionException != null)
                ExecutionException(err_code, new EventArgs());
        }

        protected abstract void CreateLogin(string LoginName, string Paswword, string AdditionalOptions);

        protected abstract void DeleteLogin(string LoginName, string AdditionalOptions);

        #endregion
    }
}
