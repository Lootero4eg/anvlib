﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Classes.PrintMessageSystems
{
    public class ConsolePrintMessageSystem : IPrintMessageSystem
    {
        public event EventHandler MessagePrinted;

        public void PrintMessage(string Msg)
        {
            Console.WriteLine(Msg);
            if (MessagePrinted != null)
                MessagePrinted(Msg, new EventArgs());
        }

        public void PrintMessage(string Msg, string WindowTitle, int Buttons, int Icon)
        {
            PrintMessage(Msg);
        }
    }
}
