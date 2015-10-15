using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Data.Database.Messages
{
    public sealed class en_US : IDBMessages
    {
        private const string error_text = "Error";
        private const string dberror_text = "Database error";
        private const string not_connected_text = "Connetion to db is not opened";
        private const string transaction_notstarted_text = "You must begin transaction first!";
        private const string transaction_notended_text = "You have to commit or rollback transaction first!";
        private const string datatable_name_not_found_text = "Datatable.TableName can not be empty!";
        private const string datatable_columns_not_found_text = "Datatable object has not Columns!";

        //--Sql Server errors
        private const string sql201 = "Some parameters of stored procedure was not supplied!\r\n\r\nOriginal exception message: ";
        private const string sql229 = "Access to stored procedure is denied!\r\n\r\nOriginal exception message: ";
        private const string sql547 = "There are references from one or more tables on deleted record!\r\n\r\nOriginal exception message: ";
        private const string sql18456 = "Wrong username or password!\r\n\r\nOriginal exception message: ";

        private const string users_managment_is_disabled = "This DB not supported users managment!";

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
