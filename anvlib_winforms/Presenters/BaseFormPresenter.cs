using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Presenters.Classes;

namespace anvlib.Presenters
{
    /// <summary>
    /// Базовый класс презентеров
    /// </summary>
    public abstract class BaseFormPresenter: BasePresenter
    {        
        /// <summary>
        /// Дополнительные представления данных
        /// </summary>
        protected AdditionalViews additionalViews { get; private set; }

        /// <summary>
        /// Метод установки дополнительных представлений данных
        /// </summary>
        /// <param name="pars"></param>
        public virtual void SetAdditionalViews(params object[] pars)
        {
            additionalViews = new AdditionalViews();

            for (int i = 0; i < pars.Length; i++)
                additionalViews.Add(pars[i]);
        }
    }
}
