using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Classes.PrintMessageSystems
{
    public class ExceptionPrintMessageSystem: IPrintMessageSystem
    {        
        public void PrintMessage(string Msg)
        {
            throw new Exception(Msg);
        }

        public void PrintMessage(string Msg, string WindowTitle, int Buttons, int Icon)
        {
            PrintMessage(Msg);
        }
    }
}
