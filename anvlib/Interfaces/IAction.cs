using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    public interface IAction
    {
        int ActionId { get; set; }
        string ActionName { get; set; }
        void Execute();
        void ExecuteWithParams(params object[] pars);
        object ActionResult { get; set; }
    }
}
