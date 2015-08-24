using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Интерфейс для формы, чтобы можно было задавать кэпшен из вне
    /// </summary>
    public interface ISetCaption
    {
        void SetCaption(string caption);
    }
}
