using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Classes
{
    public class ConnectionFormSettings: IConnectionFormSettings
    {
        private string _srvAddr;
        private string _sqlLogin;
        private byte[] _sqlPassword;
        private bool _sqlType;
        private string _sqlDBName;

        public string SQLServAddr
        {
            get { return _srvAddr; }
            set { _srvAddr = value; }
        }

        public bool IsSQLTypeAuth
        {
            get { return _sqlType; }
            set { _sqlType = value; }
        }

        public string SQLLogin
        {
            get { return _sqlLogin; }
            set { _sqlLogin = value; }
        }

        public byte[] SQLPassword
        {
            get { return _sqlPassword; }
            set { _sqlPassword = value; }
        }

        public string SQLDBName
        {
            get { return _sqlDBName; }
            set { _sqlDBName = value; }
        }

        public void Save() { }
    }
}
