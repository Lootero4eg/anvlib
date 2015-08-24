using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Classes;

namespace anvlib.Presenters
{
    /// <summary>
    /// Базовый класс для перезентеров со сменными видами представления данных (grid,listview,diagramm)
    /// </summary>
    public abstract class BaseChangableViewPresenter: BasePresenter
    {
        /// <summary>
        /// Массив "Представлений данных"
        /// </summary>
        protected ChangableView[] views;

        /// <summary>
        /// Данные, которые будут выводиться на экран
        /// </summary>
        protected object view_data;

        /// <summary>
        /// Индекс текущего представления данных
        /// </summary>
        protected int currentViewIndex = 0;

        /// <summary>
        /// Индекс предыдущего представления данных
        /// </summary>
        protected int previousViewIndex = -1;        

        /// <summary>
        /// Метод для задание "Представлений данных"
        /// </summary>
        /// <param name="Views">Представления</param>
        public virtual void SetChangableViews(params object[] Views)
        {
            views = new ChangableView[Views.Length];
            for (int i = 0; i < Views.Length; i++)
            {
                views[i] = new ChangableView();
                views[i].View = Views[i];
            }
        }

        /// <summary>
        /// Метод проверяющий наличие хотябы одного представления данных
        /// </summary>
        protected void CheckViews()
        {
            if (views == null || views.Length == 0)
                throw new Exception("This presenter won't work without initialize Views!!!");
        }

        /// <summary>
        /// Метод вызывающий делегата у текущего представления данных
        /// </summary>
        public virtual void FillCurrentView()
        {
            CheckViews();
            if (views[currentViewIndex] !=null && views[currentViewIndex].FillView != null)
                views[currentViewIndex].FillView(views[currentViewIndex]);
        }

        /// <summary>
        /// Метод вызывающий делегат на очистку заданного по индексу представления данных
        /// </summary>
        /// <param name="viewIndex"></param>
        public virtual void ClearView(int viewIndex)
        {
            CheckViews();
            if (views[viewIndex] != null && views[viewIndex].Clear != null)
                views[viewIndex].Clear();
        }

        /// <summary>
        /// Метод очищающий все представления данных
        /// </summary>
        public virtual void ClearAllViews()
        {
            CheckViews();
            for (int i = 0; i < views.Length; i++)
                ClearView(i);
        }

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

        /// <summary>
        /// Метод смены представления данных по индексу
        /// </summary>
        /// <param name="viewIndex">Индекс представления данных</param>
        public virtual void ChangeView(int viewIndex)
        {
            CheckViews();
            if (viewIndex != currentViewIndex)
            {
                previousViewIndex = currentViewIndex;
                currentViewIndex = viewIndex;
                DoChangeView();
            }
        }

        //--тут надо будет либо как то скрывать, либо выдвигать/задвигать сменные контролсы
        protected abstract void DoChangeView();

        /// <summary>
        /// Метод установки дополнительных параметров у Представления данных
        /// </summary>
        /// <param name="viewIndex"></param>
        /// <param name="parameters"></param>
        public virtual void SetAdditionalParametersForView(int viewIndex,params object[] parameters)
        {
            CheckViews();
            views[viewIndex].AdditionalParams = parameters;
        }
    }
}
