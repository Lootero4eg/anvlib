using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;

namespace anvlib.Classes
{
    /// <summary>
    /// Класс для сохранение настроек формы в конфиг
    /// </summary>
    [XmlType("FormProperties")]
    public class FormProperties
    {
        /// <summary>
        /// Имя формы
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// Размер формы
        /// </summary>
        public Size FormSize { get; set; }

        /// <summary>
        /// Местоположение формы на экране
        /// </summary>
        public Point FormLocation { get; set; }

        /// <summary>
        /// Позиция инициализация формы
        /// </summary>
        public FormStartPosition StartPosition { get; set; }

        /// <summary>
        /// Состояние формы (Минимизирована, Скрыта, Максимизированна)
        /// </summary>
        public FormWindowState FormState { get; set; }
    }

    /// <summary>
    /// Коллекция настроек форм
    /// </summary>
    [XmlRoot("FormProperties", Namespace="")]   
    public class FormsSettings : Collection<FormProperties>
    {
        /// <summary>
        /// Метод поиска "первой нпйденой" формы по имени из всей коллекции
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public FormProperties FindFormByName(string formName)
        {
            foreach (var item in this)
                if (item.FormName == formName)
                    return item;

            return null;
        }
    }
}
