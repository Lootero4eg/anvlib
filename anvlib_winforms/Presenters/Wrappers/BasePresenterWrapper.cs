using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Presenters.Wrappers
{
    /// <summary>
    /// Базовый класс для Презентерных Оберток 
    /// </summary>
    /// <typeparam name="T">Тип конкретного презентера</typeparam>
    public abstract class BasePresenterWrapper<T>: IBasePresenterWrapper<T>
    {
        /// <summary>
        /// Перезентер
        /// </summary>
        public T Presenter { get; private set; }

        /// <summary>
        /// Метод создающий конкретный экземпляр презентера
        /// </summary>
        /// <param name="presenter"></param>
        public virtual void CreatePresenter(T presenter)
        {
            Presenter = presenter;
        }

        /// <summary>
        /// Инициализация презентера
        /// </summary>
        public virtual void DoInit()
        {
            (Presenter as BaseFormPresenter).DoInit();
        }
    }
}
