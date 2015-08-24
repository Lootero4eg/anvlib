using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Interfaces.Templates
{
    public class ActionTemplate: IAction
    {
        /// <summary>
        /// Id действия/команды
        /// </summary>
        public int ActionId { get; set; }

        /// <summary>
        /// Имя действия/команды
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Результат возвращаемый действием/командой
        /// </summary>
        public object ActionResult
        {
            get;
            set;
        }

        /// <summary>
        /// Метод выполнения действия/команды
        /// </summary>
        public void Execute()
        { 
        }

        /// <summary>
        /// Метод выполнения действия/команды
        /// </summary>
        /// <param name="pars">Параметры передаваемые на выполнение действия/команды</param>
        public void ExecuteWithParams(params object[] pars)
        { 
        }
    }
}
