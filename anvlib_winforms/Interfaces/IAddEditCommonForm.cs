using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Enums;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Интерфейс для форм добавление и редактирвания
    /// Все вроде ничего, но в подчиненных формах придется напрямую указывать тип объектов...
    /// либо писать для каждой группы и юзера свои интерфесы...
    /// Незнаю как обойти такую ситуаию...
    /// </summary>
    public interface IAddEditCommonForm
    {
        /// <summary>
        /// Установить "Редактируемый объект"
        /// </summary>
        /// <param name="editItem"></param>
        void SetItemForEditing(object editItem);

        /// <summary>
        /// Вызвать диалоговую форму
        /// </summary>
        /// <returns></returns>
        System.Windows.Forms.DialogResult ShowDialog();

        /// <summary>
        /// Вызов формы
        /// </summary>
        void Show();

        /// <summary>
        /// Считать полученный DialogResult, по закрытию формы
        /// </summary>
        System.Windows.Forms.DialogResult DialogResult { get; }        

        void SetFormState(AddEditFormState state);
    }
}
