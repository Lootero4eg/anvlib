using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using anvlib.Classes;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Интерфейс для сохранения "настроек форм" в сеттинги
    /// </summary>
    public interface IFormsSettingsForSettings
    {
        [XmlElement("Forms", Namespace="")]
        FormsSettings Forms {get;set;}

        void Save();
    }
}
