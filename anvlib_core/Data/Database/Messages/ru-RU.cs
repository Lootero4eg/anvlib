using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Data.Database.Messages
{
    public sealed class ru_RU : IDBMessages
    {
        private const string error_text = "Ошибка";
        private const string dberror_text = "Ошибка базы данных";
        private const string not_connected_text = "Не установлено соединение с базой данных!";
        private const string transaction_notstarted_text = "Для того чтобы завершить транзанкцию, вам необходимо ее начать!";
        private const string transaction_notended_text = "Необходимо завершить или откатить начатую Транзанкцию!";
        private const string datatable_name_not_found_text = "В переменной DataTable, незаполнено поле TableName!";
        private const string datatable_columns_not_found_text = "В переменной DataTable, нет ни одного столбца!";

        //--ошибки Sql Server-а
        private const string sql201 = "Отсутствует параметр хранимой процедуры!\r\n\r\nОригинальный текст: ";
        private const string sql229 = "Отсутствуют права доступа к хранимой процедуре!\r\n\r\nОригинальный текст: ";
        private const string sql547 = "Имеются ссылки из других таблиц на удаляемую запись!\r\n\r\nОригинальный текст: ";
        private const string sql18456 = "Неверные имя пользователя или пароль!\r\n\r\nОригинальный текст: ";

        private const string users_managment_is_disabled = "В этой СУБД отсутствует понятия пользователей!";

        public string ErrorMsg { get { return error_text; } }
        public string DBErrorMsg { get { return dberror_text; } }
        public string NotConnectedMsg { get { return not_connected_text; } }
        public string TransactionNotStartedMsg { get { return transaction_notstarted_text; } }
        public string TransactionNotEndedMsg { get { return transaction_notended_text; } }
        public string DatatableNameNotFoundMsg { get { return datatable_name_not_found_text; } }
        public string DatatableColumnsNotFoundMsg { get { return datatable_columns_not_found_text; } }        

        public string SQLSERVER_201 { get { return sql201; } }
        public string SQLSERVER_229 { get { return sql229; } }
        public string SQLSERVER_547 { get { return sql547; } }
        public string SQLSERVER_18456 { get { return sql18456; } }

        public string UsersManagmentIsNotSupported { get { return users_managment_is_disabled; } }
    }
}
