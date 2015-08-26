using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Presenters.Classes;

namespace anvlib.Presenters
{
    /// <summary>
    /// Базовый класс для перезентеров со сменными видами представления данных (grid,listview,diagramm)
    /// </summary>
    public abstract class BaseFormChangableViewPresenter: BaseChangableViewPresenter
    {        
        /// <summary>
        /// Смена представления данных по имени
        /// Пока не использовалось, но чем черт не шутит =)
        /// </summary>
        /// <param name="CtrlName">Имя представления данных</param>
        public virtual void ChangeViewByControlName(string CtrlName)
        {
            CheckViews();
            
            for (int i = 0; i < views.Length; i++)
            {
                if ((views[i].View as Control).Name == CtrlName)
                {
                    ChangeView(i);
                    break;
                }
            }
        }        

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
