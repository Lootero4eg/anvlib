using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Интерфейс для сохранения всех учеттных данных
    /// БД MS SQL Server
    /// </summary>
    public interface IConnectionFormSettings
    {
        /// <summary>
        /// Имя или IP Сервера
        /// </summary>
        string SQLServAddr { get; set; }

        /// <summary>
        /// Фалг SQL или Windows аутентификации
        /// </summary>
        bool IsSQLTypeAuth { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        string SQLLogin { get; set; }

        /// <summary>
        /// Парль в виде ХЭШа
        /// </summary>
        byte[] SQLPassword { get; set; }

        /// <summary>
        /// Каталог БД
        /// </summary>
        string SQLDBName { get; set; }

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        void Save();
    }
}
