using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Базовый интерфейс для любых типов плагинов
    /// </summary>
    interface IBasePlugin
    {
        /// <summary>
        /// Имя плагина
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание плагина
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Имя dll сборки
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        /// Метод инициализации плагина
        /// </summary>
        void PluginInitialize();
    }
}
