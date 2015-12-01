using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Classes.PrintMessageSystems
{
    public class NullPrintMessageSystem : IPrintMessageSystem
    {
        public event EventHandler MessagePrinted;

        public void PrintMessage(string Msg)
        {        
            if (MessagePrinted != null)
                MessagePrinted(Msg, new EventArgs());
        }

        public void PrintMessage(string Msg, string WindowTitle, int Buttons, int Icon)
        {
            if (MessagePrinted != null)
                MessagePrinted(Msg, new EventArgs());
        }
    }
}
