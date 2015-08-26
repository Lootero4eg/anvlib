using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace anvlib.Presenters.Classes
{
    /// <summary>
    /// Сменный вид
    /// </summary>
    public class ChangableView
    {
        /// <summary>
        /// Вид(он же контролс)
        /// </summary>
        public object View { get; set; }

        /// <summary>
        /// Делегат для функции заполняющей контрол, должна быть привязана в презентере
        /// </summary>
        public FillViewData FillView { get; set; }

        /// <summary>
        /// Делегат для функции очищающей контрол, должна быть привязана в презентере
        /// </summary>
        public ClearView Clear { get; set; }

        /// <summary>
        /// Массив дополнительных параметров View-а
        /// </summary>
        public object[] AdditionalParams { get; set; }
    }

    /// <summary>
    /// Коллекция сменных видов
    /// </summary>
    public class ChangableViews : Collection<ChangableView> { }
}
