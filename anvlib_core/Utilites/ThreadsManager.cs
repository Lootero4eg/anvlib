using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace anvlib.Utilites
{
    public delegate void StartThreadingHandler();

    public static class ThreadsManager
    {
        public static Thread StartNewThread(StartThreadingHandler delegate_method)
        {            
            Thread tr = new Thread(new ThreadStart(delegate_method));
            tr.Start();

            return tr;
        }
    }
}
