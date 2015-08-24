using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Интерфейс для простой формы коннекции к серверу SQL Server
    /// </summary>
    interface ISimpleSqlConnectionString
    {
        /// <summary>
        /// Имя или айпи сервера
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Имя базы данных
        /// </summary>
        string DBCatalog { get; set; }
    }
}
