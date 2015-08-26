using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Presenters
{
    public abstract class BaseSimplePresenter: BasePresenter
    {
        /// <summary>
        /// Пердставление данных
        /// </summary>
        protected object[] views;        

        /// <summary>
        /// Метод установки Представлений данных
        /// </summary>
        /// <param name="View">Контролсы или набор контролсов</param>
        public virtual void SetViews(params object[] Views)
        {
            views = Views;
        }

        /// <summary>
        /// Метод заполнения Представлений данных, данными
        /// </summary>
        public abstract void FillView(int Index);
    }
}
