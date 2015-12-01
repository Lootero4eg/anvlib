using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;
using System.Windows.Forms;

namespace anvlib.Classes.PrintMessageSystems
{
    public class MessageBoxPrintSystem: IPrintMessageSystem
    {
        public event EventHandler MessagePrinted;

        public void PrintMessage(string Msg)
        {
            MessageBox.Show(Msg);

            if (MessagePrinted != null)
                MessagePrinted(Msg, new EventArgs());
        }

        public void PrintMessage(string Msg, string WindowTitle, int Buttons, int Icon)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Error;
            switch (Buttons)
            { 
                case 1:
                    buttons = MessageBoxButtons.OK;
                    break;
                case 2:
                    buttons = MessageBoxButtons.OKCancel;
                    break;
                case 3:
                    buttons = MessageBoxButtons.YesNo;
                    break;
                case 4:
                    buttons = MessageBoxButtons.YesNoCancel;
                    break;
                case 5:
                    buttons = MessageBoxButtons.RetryCancel;
                    break;
                case 6:
                    buttons = MessageBoxButtons.AbortRetryIgnore;
                    break;
            }

            switch (Icon)
            {
                case 1:
                    icon = MessageBoxIcon.Error;
                    break;
                case 2:
                    icon = MessageBoxIcon.Warning;
                    break;
                case 3:
                    icon = MessageBoxIcon.Information;
                    break;
                case 4:
                    icon = MessageBoxIcon.Question;
                    break;
                case 5:
                    icon = MessageBoxIcon.Asterisk;
                    break;
                case 6:
                    icon = MessageBoxIcon.Exclamation;
                    break;
                case 7:
                    icon = MessageBoxIcon.Hand;
                    break;
                case 8:
                    icon = MessageBoxIcon.Stop;
                    break;
                case 9:
                    icon = MessageBoxIcon.None;
                    break;
            }

            MessageBox.Show(Msg, WindowTitle, buttons, icon);

            if (MessagePrinted != null)
                MessagePrinted(Msg, new EventArgs());
        }
    }
}
