using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    public interface IPrintMessageSystem
    {
        /// <summary>
        /// Напечатать сообщение
        /// </summary>
        /// <param name="Msg">Текст сообщения</param>
        void PrintMessage(string Msg);

        /// <summary>
        /// Напечатать сообщение, по типу месседж бокса
        /// </summary>
        /// <param name="Msg">Текст сообщения</param>
        /// <param name="WindowTitle">Заголовок окна</param>
        /// <param name="Buttons"> Кнопки окна сообщения
        /// 1 - Ok
        /// 2 - OkCancel
        /// 3 - YesNo
        /// 4 - YesNoCancel
        /// 5 - RetryCancel
        /// 6 - AbortRetryIgnore
        /// </param>
        /// <param name="Icon"> Иконки окна сообщения
        /// 1 - Error
        /// 2 - Warning
        /// 3 - Information
        /// 4 - Question
        /// 5 - Asterisk
        /// 6 - Exclamation
        /// 7 - Hand
        /// 8 - Stop
        /// 9 - None
        /// </param>
        void PrintMessage(string Msg, string WindowTitle, int Buttons, int Icon);
    }
}
