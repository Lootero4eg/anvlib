using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Data.Database.Messages
{
    public interface IDBMessages
    {
        string ErrorMsg { get; }
        string DBErrorMsg { get; }
        string NotConnectedMsg { get; }
        string TransactionNotStartedMsg { get; }
        string TransactionNotEndedMsg { get; }
        string DatatableNameNotFoundMsg { get; }
        string DatatableColumnsNotFoundMsg { get; }

        string SQLSERVER_201 { get; }
        string SQLSERVER_229 { get; }
        string SQLSERVER_547 { get; }
        string SQLSERVER_18456 { get; }

        string UsersManagmentIsNotSupported { get; }
    }
}
