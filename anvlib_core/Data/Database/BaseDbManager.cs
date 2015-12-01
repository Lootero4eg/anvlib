using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;

using anvlib.Enums;
using anvlib.Interfaces;
using anvlib.Classes.PrintMessageSystems;
using anvlib.Classes.Attributes;
using anvlib.Data.Database.Messages;

namespace anvlib.Data.Database
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
        protected string _connectionString;//--строка соединения
        protected string _owner;//--владелец объекта
        protected char _open_bracket;//--для избежания проблем с системными полями
        protected char _close_bracket;//--для избежания проблем с системными полями

        protected int _last_error = 0;

        //--Может использоваться не только для ошибок! Но и когда надо напечатать какое-то сообщение из методов менеджера
        protected IPrintMessageSystem MessagePrinter;
        protected ErrorMessageManager ErrorsManager = new ErrorMessageManager();

        #region BaseDbManager Properties & Methods

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public BaseDbManager()
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Runtime)
                SetPrintMessageSystem(new ExceptionPrintMessageSystem());                
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="msg_system">Система оповещения об ошибке</param>
        public BaseDbManager(IPrintMessageSystem msg_system)
        {
            MessagePrinter = msg_system;
        }

        /// <summary>
        /// Состоянние "Соединения с сервером"
        /// </summary>
        public bool Connected
        {
            get
            {
                if (_conn != null)
                    if (_conn.State == System.Data.ConnectionState.Open)
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
        /// Владелец схемы данных
        /// </summary>
        public string SchemaOwner
        {
            get { return _owner; }
            set { _owner = value; }
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
        [Obsolete]
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
                    throw new Exception(ErrorsManager.Messages.TransactionNotEndedMsg);

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
                        MessagePrinter.PrintMessage(e.Message, ErrorsManager.Messages.DBErrorMsg, 1, 1);
                }
            }
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        protected abstract DbDataAdapter CreateDataAdapter(string SQLText);

        /// <summary>
        /// Метод создание DbDataAdapter-а
        /// </summary>
        /// <param name="cmd">Команда которая будет заполнять DbDataAdapter</param>
        /// <returns></returns>
        protected abstract DbDataAdapter CreateDataAdapter(DbCommand cmd);

        /// <summary>
        /// Метод создания DbCommand
        /// </summary>
        /// <param name="CmdText">Текст запроса или имя хранимой процедуры</param>
        /// <returns></returns>
        protected abstract DbCommand CreateCommand(string CmdText);

        /// <summary>
        /// Метод создание параметра для хранимых процедур
        /// </summary>
        /// <param name="ParName">Имя параметра</param>
        /// <param name="ParType">Тип параметра</param>
        /// <param name="ParSize">Размер параметра. Для строковых параметров и параметров с плавающей точкой</param>
        /// <returns></returns>
        protected abstract DbParameter CreateParameter(string ParName, DbType ParType, int ParSize);        

        /// <summary>
        /// Метод создание таблицы
        /// </summary>
        /// <param name="table">Таблица в формате DataTable</param>
        [Experimental]
        public virtual void CreateTable(DataTable table, DataInsertMethod insert_method)
        {
            if (string.IsNullOrEmpty(table.TableName))
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ErrorsManager.Messages.DatatableNameNotFoundMsg, ErrorsManager.Messages.ErrorMsg, 1, 1);

                return;
            }

            if (table.Columns.Count <= 0)
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ErrorsManager.Messages.DatatableColumnsNotFoundMsg, ErrorsManager.Messages.ErrorMsg, 1, 1);

                return;
            }
        }

        /// <summary>
        /// Метод стирания таблицы из базы данных или схемы
        /// </summary>
        /// <param name="TableName"></param>        
        [Experimental]
        public virtual void DropTable(string TableName)
        {
            string sql = string.Format("DROP TABLE {0}", TableName);
            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        /// <summary>
        /// Метод проверки существование объекта
        /// </summary>
        /// <param name="obj_name"></param>
        /// <param name="obj_type"></param>
        /// <param name="CaseSensivity"></param>
        /// <returns></returns>
        [Experimental]
        public abstract bool IsDBObjectExists(string obj_name, DataBaseObjects obj_type, bool CaseSensivity);

        /// <summary>
        /// Метод начинающий транзакцию.       
        /// </summary>
        /// <returns></returns>
        public void BeginTransaction()
        {
            if (Connected)
                _transaction = _conn.BeginTransaction(IsolationLevel.ReadCommitted);
            else
                throw new Exception(ErrorsManager.Messages.NotConnectedMsg);
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
                throw new Exception(ErrorsManager.Messages.TransactionNotStartedMsg);
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
                throw new Exception(ErrorsManager.Messages.TransactionNotStartedMsg);
        }

        public void SetPrintMessageSystem(IPrintMessageSystem MsgSystem)
        {
            MessagePrinter = MsgSystem;
            MessagePrinter.MessagePrinted += OnMessagePrinted;
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

        protected bool IsDataReaderHasColumn(string ColumnName, bool CaseSensivity)
        {
            if (_DR != null && !_DR.IsClosed)
            {
                for (int i = 0; i < _DR.FieldCount; i++)
                {
                    if (CaseSensivity)
                    {
                        if (_DR.GetName(i) == ColumnName)
                            return true;
                    }
                    else
                    {
                        if (_DR.GetName(i).ToLower() == ColumnName.ToLower())
                            return true;
                    }
                }
                return false;
            }

            return false;
        }

        /// <summary>
        /// Метод придуман для того, чтобы тот, кто подписан на этот эвент из вне, мог узнать что произошел сбой выполнения
        /// </summary>
        /// <param name="err_code"></param>
        protected void RaiseExecuteException(int err_code)
        {
            if (ExecutionException != null)
                ExecutionException(err_code, new EventArgs());
        }

        protected virtual void OnMessagePrinted(object sender,EventArgs e)
        {
            if(_last_error!=0)
                RaiseExecuteException(_last_error);
        }

        protected abstract void CreateLogin(string LoginName, string Paswword, string AdditionalOptions);

        protected abstract void DeleteLogin(string LoginName, string AdditionalOptions);
        
        //--пока этот метод вызывается из CreateTable, проверку на _conn.Open делать не надо, но как только будет отдельно, надо будет делать!
        protected virtual void InsertDataToDb(DataTable table, string parameters_prefix)
        {
            string insert_sql = string.Format("insert into {0} values(", table.TableName);
            Array insert_params = new DbParameter[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                insert_sql = string.Format("{0}{2}{1}", insert_sql, table.Columns[i].ColumnName + (i + 1 != table.Columns.Count ? "," : ")"), parameters_prefix);
                DbParameter par = CreateParameter(string.Format("{1}{0}", table.Columns[i].ColumnName, parameters_prefix), Utilites.SystemTypeToDbTypeConverter.Convert(table.Columns[i].DataType), table.Columns[i].MaxLength);
                par.SourceColumn = table.Columns[i].ColumnName;
                insert_params.SetValue(par, i);
            }
            
            _DA = CreateDataAdapter("");
            var ins_cmd = CreateCommand(insert_sql);
            ins_cmd.Parameters.AddRange(insert_params);            

            _DA.InsertCommand = ins_cmd;
            _DA.Update(table);
            
        }

        //--Сделать вытаскивание индексов!!!!
        [Incomplete]//--Работает только с таблицами до 8 миллионов записей, далее вылетает System.OutOfMemoryException
        public virtual DataTable GetTableFromDb(string tablename, bool with_primary_key, bool with_max_string_length, bool with_default_values, bool CaseSensivity)
        {
            _dt = new DataTable(tablename);
            string sql = "";

            if (!CaseSensivity)
            {
                sql = "select * from {0}";
                if (!string.IsNullOrEmpty(_owner))
                {
                    sql = "select * from {0}.{1}";
                    sql = string.Format(sql, _owner, tablename);
                }
                else
                    sql = string.Format(sql, tablename);
            }
            else
            {
                sql = "select * from {0}{1}{2}";
                if (!string.IsNullOrEmpty(_owner))
                {
                    sql = "select * from {0}{1}{2}.{0}{3}{2}";
                    sql = string.Format(sql, _open_bracket, _owner, _close_bracket, tablename);
                }
                else
                    sql = string.Format(sql, _open_bracket, tablename, _close_bracket);
            }
                            
            _DA = CreateDataAdapter(sql);
            _DA.FillLoadOption = LoadOption.Upsert;//--Важднейшая опция, после которой можно вставить эту таблицу в другую базу     
            _DA.FillSchema(_dt, SchemaType.Source);
            PrepareTableSchemeBeforeFill(_dt);
            _DA.Fill(_dt);

#warning перенести код ниже в PrepareTableSchemeBeforeFill
            //--описание колонок 
            foreach (DataColumn col in _dt.Columns)
            {                
                var col_info = GetDbColumnInformation(tablename, col.ColumnName, CaseSensivity);
                if (col_info.AdditionalInfo!=null && col_info.AdditionalInfo.ToString() != "default")
                    col.AllowDBNull = col_info.IsNullable;
                if (with_max_string_length)
                {
                    if (col.DataType == typeof(string))
                        if (col_info.MaxLength > 0 && col_info.MaxLength > col.MaxLength)
                            col.MaxLength = col_info.MaxLength;
                }

                if (with_default_values)
                {
                    //--надо будет придумать какой-то флаг, чтобы сообщалось что не все дефолтные значения распарсились
                    if (col_info.DefaultValue != null && col_info.DefaultValue != DBNull.Value)
                    {
                        try
                        {
                            col.DefaultValue = col_info.DefaultValue;
                        }
                        catch
                        {
                            col.DefaultValue = DBNull.Value;
                        }
                    }
                }
            }

            if (with_primary_key)
                _dt = GetTablePrimaryKey(_dt, CaseSensivity);

            return _dt;
        }

        protected abstract DataTable GetTablePrimaryKey(DataTable table, bool CaseSensivity);

        internal virtual DbColumnInformation GetDbColumnInformation(string tablename, string columnname, bool CaseSensivity)
        {
            DbColumnInformation res = new DbColumnInformation();

            res.MaxLength = 50;
            res.IsNullable = true;
            res.AdditionalInfo = "default";

            return res;
        }

        internal virtual void PrepareTableSchemeBeforeFill(DataTable table) {}        

        #endregion
    }
}
