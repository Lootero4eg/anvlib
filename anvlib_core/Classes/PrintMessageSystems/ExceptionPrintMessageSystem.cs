using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Classes.PrintMessageSystems
{
    public class ExceptionPrintMessageSystem: IPrintMessageSystem
    {
        public event EventHandler MessagePrinted;

        public void PrintMessage(string Msg)
        {
            if (MessagePrinted != null)
                MessagePrinted(Msg, new EventArgs());

            throw new Exception(Msg);
        }

        public void PrintMessage(string Msg, string WindowTitle, int Buttons, int Icon)
        {
            PrintMessage(Msg);
        }
    }
}
