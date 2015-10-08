using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;

using anvlib.Enums;
using anvlib.Interfaces;
using anvlib.Classes.Attributes;

namespace anvlib.Data.Database
{
    /// <summary>
    /// Базовый класс для баз данных SQL Lite
    /// </summary>
    public class BaseSQLiteManager: BaseDbManager
    {
        /// <summary>
        /// Конструктор с дефолтными значениями
        /// </summary>
        public BaseSQLiteManager()
            : base()
        {             
            _open_bracket='"';
            _close_bracket = '"';
            _parameters_prefix = ""; //--Параметров тут по определению нет
        }

        /// <summary>
        /// Метод, подключения к базе данных
        /// </summary>
        /// <param name="ConnectionString">Строка соединения</param>
        public override void Connect(string ConnectionString)
        {
            _connectionString = ConnectionString;
            _conn = new SQLiteConnection(ConnectionString);
            try
            {
                _conn.Open();                
            }
            catch (SQLiteException ex)
            {                
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(ex.Message, "Ошибка базы данных", 1, 1);
            }
        }

        /// <summary>
        /// Метод, создает или открывает уже существующий файл базы данных
        /// </summary>
        /// <param name="Filename">Имя файла БД</param>
        public void OpenDbFile(string Filename) 
        {
            Connect(string.Format("URI=file:{0}", Filename));
        }

        /// <summary>
        /// Метод, содает новую базу в памяти
        /// </summary>
        public void CreateDbInMemory()
        {
            Connect("Data Source=:memory:");
        }

        /// <summary>
        /// Коннекция
        /// </summary>
        public override DbConnection Connection
        {
            get
            {
                return (_conn as SQLiteConnection);
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
            _cmd = new SQLiteCommand(CmdText, (_conn as SQLiteConnection));
            DbCommand tmpCmd = _cmd;
            tmpCmd.CommandType = CommandType.Text;

            if (_transaction != null)
                tmpCmd.Transaction = _transaction;

            return tmpCmd;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            _DA = new SQLiteDataAdapter((SQLiteCommand)cmd);
            DbDataAdapter tmpDA = _DA;

            return tmpDA;
        }

        /// <summary>
        /// Метод создание DataAdapter-а
        /// </summary>
        /// <param name="SQLText">Текст запроса</param>
        /// <returns></returns>
        protected override DbDataAdapter CreateDataAdapter(string SQLText)
        {
            _DA = new SQLiteDataAdapter(SQLText, (_conn as SQLiteConnection));
            DbDataAdapter tmpDA = _DA;

            return tmpDA;
        }        

        protected override DbParameter CreateParameter(string ParName, DbType ParType, int ParSize)
        {
            //throw new Exception("В этой СУБД отсутствует понятия хранимых процедур, соответственно и параметры отсутствуют!");

            _param = new SQLiteParameter(ParName, ParType, ParSize);
            DbParameter tmpPar = _param;

            return tmpPar;
        }

        /// <summary>
        /// Метод создания таблицы из DataTable
        /// </summary>
        /// <param name="table">Таблица</param>
        [Experimental]
        public override void CreateTable(DataTable table)
        {
            if (Connected)
            {
                if (string.IsNullOrEmpty(table.TableName))
                {
                    if (MessagePrinter != null)
                        MessagePrinter.PrintMessage("В переменной DataTable, незаполнено поле TableName!", "Ошибка", 1, 1);

                    return;
                }

                if (table.Columns.Count <= 0)
                {
                    if (MessagePrinter != null)
                        MessagePrinter.PrintMessage("В переменной DataTable, нет ни одного столбца!", "Ошибка", 1, 1);

                    return;
                }

                string sqlsc;
                sqlsc = "CREATE TABLE " + table.TableName + "(";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sqlsc += "\n" + table.Columns[i].ColumnName;
                    if (table.Columns[i].DataType.ToString().Contains("System.Int32"))
                        sqlsc += " integer ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.DateTime"))
                        sqlsc += " text ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.String"))
                        sqlsc += " text ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Single"))
                        sqlsc += " integer ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                        sqlsc += " real ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Guid"))
                        sqlsc += " text ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Boolean"))
                        sqlsc += " integer ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte"))
                        sqlsc += " integer ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Int16"))
                        sqlsc += " integer ";
                    else if (table.Columns[i].DataType.ToString().Contains("System.Byte[]"))
                        sqlsc += " blob ";
                    else
                        sqlsc += " text ";



                    if (table.Columns[i].AutoIncrement)
                        sqlsc += " AUTOINCREMENT(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";
                    if (!table.Columns[i].AllowDBNull)
                        sqlsc += " NOT NULL ";
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
                    string insert_sql = string.Format("insert into {0} values(", table.TableName);
                    Array insert_params = new DbParameter[table.Columns.Count];
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        insert_sql = string.Format("{0}@{1}", insert_sql, table.Columns[i].ColumnName + (i + 1 != table.Columns.Count ? "," : ");"));
                        DbParameter par = CreateParameter(string.Format("@{0}", table.Columns[i].ColumnName), Utilites.SystemTypeToDbTypeConverter.Convert(table.Columns[i].DataType), table.Columns[i].MaxLength);
                        par.SourceColumn = table.Columns[i].ColumnName;
                        insert_params.SetValue(par, i);
                    }

                    _DA = new SQLiteDataAdapter();
                    var ins_cmd = CreateCommand(insert_sql);
                    ins_cmd.Parameters.AddRange(insert_params);

                    _DA.InsertCommand = ins_cmd;
                    _DA.Update(table);
                }
            }
            else
            {
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage("Не установлено соединение с базой данных!", "Ошибка", 1, 1);
            }
        }

        /// <summary>
        /// Метод, удаляющий таблицу из базы данных
        /// </summary>
        /// <param name="TableName"></param>
        public override void DropTable(string TableName)
        {
            string sql = string.Format("DROP TABLE IF EXISTS {0};", TableName);
            ExecuteCommand(CreateCommand(sql).ExecuteNonQuery);
        }

        [Incomplete]
        public override bool IsDBObjectExists(string obj_name, DataBaseObjects obj_type, bool CaseSensivity)
        {
            //--надо смотреть табличку sqlite_master
            if (Connected)
            {
                string sql = "";
                if (CaseSensivity)
                {
                    sql = string.Format("select 1 from sqlite_master where type='{0}' and name='{1}'",
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
                    MessagePrinter.PrintMessage("Не установлено соединение с базой данных!", "Ошибка", 1, 1);
            }

            return false;
        }

        [Incomplete]
        protected string GetObjectTypeCode(DataBaseObjects db_object)
        {
            string res = "table";

            switch (db_object)
            {
                /*case DataBaseObjects.foreign_key:
                    res = "foreing key";
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
                    break;*/

                case DataBaseObjects.table:
                    res = "table";
                    break;

                /*case DataBaseObjects.trigger:
                    res = "TR";
                    break;

                case DataBaseObjects.type:
                    res = "T";
                    break;*/
            }

            return res;
        }

        protected override void CreateLogin(string LoginName, string Paswword, string AdditionalOptions)
        {
            throw new Exception("В этой СУБД отсутствует понятия пользователей!");
        }

        protected override void DeleteLogin(string LoginName, string AdditionalOptions)
        {
            throw new Exception("В этой СУБД отсутствует понятия пользователей!");
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
            catch (SQLiteException e)
            {                
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, "Ошибка базы данных", 1, 1);
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
            catch (SQLiteException e)
            {                
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, "Ошибка базы данных", 1, 1);
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
            catch (SQLiteException e)
            {                
                if (MessagePrinter != null)
                    MessagePrinter.PrintMessage(e.Message, "Ошибка базы данных", 1, 1);
            }

            return null;
        }        
    } 
}
