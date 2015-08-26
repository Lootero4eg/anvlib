using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Базовый враппер.
    /// Незнаю буду ли пользоваться ими в дальнейшем...
    /// </summary>
    /// <typeparam name="T">Перезентер опеределенного типа</typeparam>
    public interface IBasePresenterWrapper<T>
    {       
        /// <summary>
        /// Создать презентер
        /// </summary>
        /// <param name="presenter"></param>
        void CreatePresenter(T presenter);
    }
}
