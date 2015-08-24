using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using anvlib.Classes;

namespace anvlib.Presenters
{
    public abstract class BaseFormViewPresenter<T> : BasePresenter//--незнаю зачем я писал этот презентер...нигде не используется!
    {
        /// <summary>
        /// Непосредственно форма или юзерконтрол
        /// </summary>
        protected T view;

        /// <summary>
        /// Данные, которые будут выводиться на экран для основного вида
        /// </summary>
        protected object _view_data;                                                

        /// <summary>
        /// Метод проверяющий наличие представления данных
        /// </summary>
        protected void CheckView()
        {
            if (view == null)
                throw new Exception("This presenter won't work without initialize Views!!!");
        }

        /// <summary>
        /// Метод вызывающий делегат у вида найденного под Контролсу
        /// </summary>
        /// <param name="ctrl"></param>
        protected virtual void FillView(T ctrl)
        {
            CheckView();            
        }

        /// <summary>
        /// Метод вызывающий делегат у вида найденного под Контролсу
        /// </summary>
        /// <param name="ctrl"></param>
        protected virtual void Clear(T ctrl)
        {
            CheckView();            
        }        
    }
}
