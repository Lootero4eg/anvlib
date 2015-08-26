using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib.Controls.EventHandlers
{
    public static class KeyboardHandlers
    {
        public static void OnlyDigitsTextBox_KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Shift || e.KeyData == Keys.Delete || e.KeyData == Keys.Back || e.KeyData == Keys.Left || e.KeyData == Keys.Right)
            {
                e.SuppressKeyPress = false;
                return;
            }

            if (!e.Control)
            {
                if ((e.KeyValue >= 48 && e.KeyValue <= 57) || (e.KeyValue >= 96 && e.KeyValue <= 105))
                    e.SuppressKeyPress = false;
                else
                    e.SuppressKeyPress = true;                                
            }
            else
            {
                if (e.KeyCode == Keys.C || e.KeyCode == Keys.V)
                    e.SuppressKeyPress = false;
            }                        
        }

        public static void FloatDataTextBox_KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 110 || e.KeyValue == 188 || e.KeyValue == 190)
            {
                if (IsValidFloatData((sender as TextBox).Text + GetDotOrComma(e.KeyValue)))
                    e.SuppressKeyPress = false;
                else
                    e.SuppressKeyPress = true;
                return;
            }
            OnlyDigitsTextBox_KeyDownEventHandler(sender, e);
        }

        public static void FloatDataCommaOnlyTextBox_KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 110)
            {
                if (IsValidFloatData((sender as TextBox).Text + ','))
                    e.SuppressKeyPress = false;
                else
                    e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyValue == 188)
            {
                if (IsValidFloatData((sender as TextBox).Text + GetDotOrComma(e.KeyValue)))
                    e.SuppressKeyPress = false;
                else
                    e.SuppressKeyPress = true;
                return;
            }
            OnlyDigitsTextBox_KeyDownEventHandler(sender, e);
        }

        public static void FloatDataDotOnlyTextBox_KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 110)
            {
                if (IsValidFloatData((sender as TextBox).Text + '.'))
                    e.SuppressKeyPress = false;
                else
                    e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyValue == 190)
            {
                if (IsValidFloatData((sender as TextBox).Text + GetDotOrComma(e.KeyValue)))
                    e.SuppressKeyPress = false;
                else
                    e.SuppressKeyPress = true;
                return;
            }
            OnlyDigitsTextBox_KeyDownEventHandler(sender, e);
        }

        private static bool IsValidFloatData(string fdata)
        {
            if (fdata.Length > 0)
            {
                if (fdata[0] == '.' || fdata[0] == ',')
                    return false;
                if (fdata.Split('.').Length > 2)
                    return false;
                if (fdata.Split(',').Length > 2)
                    return false;
                if (fdata.IndexOf('.') > -1 && fdata.IndexOf(',') > -1)
                    return false;
            }

            return true;
        }

        private static char GetDotOrComma(int code)
        {
            if (code == 188)
                return ',';
            if (code == 190)
                return '.';
            return ' ';
        }
    }
}
