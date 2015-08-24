using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Presenters;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Интерфейс для "Профильных" форм
    /// </summary>
    public interface IProfileForm
    {
        /// <summary>
        /// Выбранный профиль
        /// </summary>
        object SelectedProfile { get; }

        /// <summary>
        /// Установить коллецию профилей
        /// </summary>
        /// <param name="ProfilesCollection"></param>
        void SetProfilesCollection(object ProfilesCollection);

        /// <summary>
        /// Установить форму редактор профилей
        /// </summary>
        /// <param name="form"></param>
        void SetAddEditProfileForm(IAddEditCommonForm form);

        /// <summary>
        /// Создать конкретный перезентор для профилей
        /// </summary>
        /// <param name="presenter"></param>
        void CreatePresenter(BaseProfilePresenter presenter);

        /// <summary>
        /// Установить контролс отображения профилей
        /// </summary>
        Control ProfileDisplayControl { get; set; }

        /// <summary>
        /// Ссылка на форму редактированияa
        /// </summary>
        IAddEditCommonForm AddEditProfileForm { get; set; }
    }
}
