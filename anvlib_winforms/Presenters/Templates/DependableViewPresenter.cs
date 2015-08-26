using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Classes;
using anvlib.Presenters;
using anvlib.Presenters.Interfaces;

namespace anvlib.Presenters.Templates
{
    public class DependableViewPresenter: BaseDependableViewPresenter<System.Windows.Forms.Control>
    {
        /// <summary>
        /// Основной метод инициализации Презентера
        /// </summary>
        public override void DoInit()
        {            
            throw new NotImplementedException();
        }

        /// <summary>
        /// Делегат "Заполнителя" представления верхнего уровня
        /// </summary>
        /// <param name="sender"></param>
        protected override void TopLevelView_FillHandler(object sender)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Делегат очистки представления верхнего уровня
        /// </summary>
        protected override void TopLevelView_ClearHandler()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Делегат "Заполнителя" представления второго уровня
        /// </summary>
        /// <param name="sender"></param>
        protected override void TopLevelDependableView_FillHandler(object sender)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Делегат очистки представления второго уровня
        /// </summary>
        protected override void TopLevelDependableView_ClearHandler()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Делегат изменения преставления верхнего уровня
        /// </summary>
        /// <param name="sender"></param>
        protected override void TopLevelView_ChangedHandler(object sender)
        {
            throw new NotImplementedException();
        }
    }
}
