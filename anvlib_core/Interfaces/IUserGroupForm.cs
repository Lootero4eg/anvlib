using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    /// <summary>
    /// Интерфейс для ЮзерГруп форм, правда мне кажется что он уже не нужен, но все может быть...
    /// </summary>
    public interface IUserGroupControl
    {
        object SelectedGroup { get; set; }
        object SelectedUser { get; set; }        
    }
}
