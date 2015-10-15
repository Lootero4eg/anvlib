using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Data.Database.Messages
{
    public class DbMsgManager
    {
        public IDBMessages MessageText
        {
            get
            {
                var cur = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (cur.Name == "ru-RU")
                    return new ru_RU();

                return new en_US();
            }
        }
    }
}
