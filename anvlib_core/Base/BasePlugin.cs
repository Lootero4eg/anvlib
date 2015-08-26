using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Base
{
    /// <summary>
    /// Базовый класс для любых типов плагинов
    /// </summary>
    public abstract class BasePlugin: IBasePlugin
    {
        /// <summary>
        /// Имя плагина
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Описание плагина
        /// </summary>
        public abstract string Description { get; set; }

        /// <summary>
        /// Имя dll сборки
        /// </summary>
        public abstract string ModuleName { get; set; }

        /// <summary>
        /// Метод инициализации плагина
        /// </summary>
        public abstract void PluginInitialize();
    }
}
