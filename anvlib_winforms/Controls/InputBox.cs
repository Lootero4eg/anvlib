using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib.Controls
{
    /// <summary>
    /// Вспомогательный класс по аналогу с VB InputBox
    /// </summary>
    public static class InputBox
    {
        private static void KeyPressed(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                ((sender as System.Windows.Forms.TextBox).Parent as System.Windows.Forms.Form).DialogResult
                    = System.Windows.Forms.DialogResult.OK;

                e.Handled = true;
            }
            if (e.KeyChar == 27)
            {
                ((sender as System.Windows.Forms.TextBox).Parent as System.Windows.Forms.Form).DialogResult
                    = System.Windows.Forms.DialogResult.Cancel;
                e.Handled = true;
            }
        }

        public static string Show(string Prompt, int MaxLength, int WndWidth)
        {

            var frm = GetForm(Prompt, MaxLength, WndWidth);
            System.Windows.Forms.DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                return frm.Controls[0].Text;

            return string.Empty;
        }

        public static string Show(string Prompt,string DefaultText, int MaxLength, int WndWidth)
        {
            var frm = GetForm(Prompt, MaxLength, WndWidth);
            frm.Controls[0].Text = DefaultText;
            System.Windows.Forms.DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                return frm.Controls[0].Text;

            return string.Empty;
        }        

        public static string Show(string Prompt, string DefaultText, int MaxLength, int WndWidth, int ButtonsOffset)
        {
            var frm = GetForm(Prompt, MaxLength, WndWidth);
            frm.Controls[0].Text = DefaultText;
            frm.Controls[1].Location = new System.Drawing.Point(25 + ButtonsOffset, 24);
            frm.Controls[2].Location = new System.Drawing.Point(100 + ButtonsOffset, 24);
            System.Windows.Forms.DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                return frm.Controls[0].Text;

            return string.Empty;            
        }

        public static string Show(string Prompt, int MaxLength, int WndWidth, int ButtonsOffset, char PasswrodChar)
        {
            var frm = GetForm(Prompt, MaxLength, WndWidth);
            (frm.Controls[0] as TextBox).PasswordChar = PasswrodChar;
            frm.Controls[1].Location = new System.Drawing.Point(25 + ButtonsOffset, 24);
            frm.Controls[2].Location = new System.Drawing.Point(100 + ButtonsOffset, 24);
            System.Windows.Forms.DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                return frm.Controls[0].Text;

            return string.Empty;
        }

        private static Form GetForm(string Prompt, int MaxLength, int WndWidth)
        {
            Form frm = new System.Windows.Forms.Form();
            frm.AutoSize = true;
            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            frm.Height = 1;
            frm.Width = 1;
            frm.Text = Prompt;
            System.Windows.Forms.TextBox tb = new System.Windows.Forms.TextBox();
            tb.MaxLength = MaxLength;
            tb.Width = WndWidth;
            tb.Top = 1;
            tb.Left = 1;
            tb.KeyPress += KeyPressed;
            frm.Controls.Add(tb);
            System.Windows.Forms.Button okb = new System.Windows.Forms.Button();
            okb.Text = "ОК";
            okb.Location = new System.Drawing.Point(25, 24);
            okb.DialogResult = System.Windows.Forms.DialogResult.OK;
            frm.Controls.Add(okb);
            System.Windows.Forms.Button cancelb = new System.Windows.Forms.Button();
            cancelb.Text = "Отмена";
            cancelb.Location = new System.Drawing.Point(100, 24);
            cancelb.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            frm.Controls.Add(cancelb);

            return frm;
        }
    }
}
