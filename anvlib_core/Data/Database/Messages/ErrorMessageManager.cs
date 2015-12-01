using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Data.Database.Messages
{
    /// <summary>
    /// Менеджер сообщений, возвращающий сообщения на нужном языке. пока 2: русский и английский
    /// </summary>
    public class ErrorMessageManager
    {
        public IDBMessages Messages
        {
            get
            {
                var cur = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (cur.Name == "ru-RU")
                    return new ru_RU_ErrorDbMessages();

                return new en_US_ErrorDbMessages();
            }
        }
    }
}
