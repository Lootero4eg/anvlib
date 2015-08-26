using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using anvlib.Presenters.Classes;

namespace anvlib.Presenters
{
    public abstract class BaseFormDependableViewPresenter<T>: BaseDependableViewPresenter<Control>
    {
        //--пришлось вставить сюда, т.к. нельзя унаследоваться от 2х классов, а в интерфейсах все должно быть пабликом.

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
