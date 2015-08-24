using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Base;
using anvlib.Classes;
using anvlib.Presenters;
using anvlib.Presenters.Interfaces;

namespace anvlib.Presenters
{
    public class ItemListPresenterTemplate: BaseItemListPresenter
    {
        /// <summary>
        /// Метод базового класса
        /// </summary>
        public override void DoInit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод заполняющий Основной Лист контрол
        /// </summary>
        public override void FillList()
        {            
            throw new NotImplementedException();
        }        

        /// <summary>
        /// Метод устанавливающий редактируемый объект
        /// </summary>
        /// <param name="Item"></param>
        public override void SetEditableItem(object Item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод возврата нового объекта
        /// </summary>
        /// <returns></returns>
        public override object CreateNewListItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод очищающий все Итемы в лист контролсе
        /// </summary>
        protected override void ClearItems()
        {
            throw new NotImplementedException();
        }
    }
}
