using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Classes;

namespace anvlib.Presenters
{
    /// <summary>
    /// Базовый класс презентеров
    /// </summary>
    public abstract class BasePresenter
    {
        /// <summary>
        /// Хранилище данных
        /// </summary>
        protected object view_data { get; set; }

        /// <summary>
        /// Дополнительные параметры для перезентеров
        /// </summary>
        protected object[] additionalPrameters { get; private set; }

        /// <summary>
        /// Дополнительные представления данных
        /// </summary>
        protected AdditionalViews additionalViews { get; private set; }

        public abstract void DoInit();

        /// <summary>
        /// Метод установки дополнительных параметров
        /// </summary>
        /// <param name="pars"></param>
        public virtual void SetAdditionalParameters(params object[] pars)
        {
            additionalPrameters = pars;
        }

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
