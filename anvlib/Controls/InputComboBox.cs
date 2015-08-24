using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace anvlib.Controls
{
    /// <summary>
    /// Вспомогательный класс по аналогу с VB InputBox
    /// </summary>
    public static class InputComboBox
    {
        private static void KeyPressed(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                ((sender as ComboBox).Parent as System.Windows.Forms.Form).DialogResult
                    = System.Windows.Forms.DialogResult.OK;

                e.Handled = true;
            }
            if (e.KeyChar == 27)
            {
                ((sender as ComboBox).Parent as System.Windows.Forms.Form).DialogResult
                    = System.Windows.Forms.DialogResult.Cancel;
                e.Handled = true;
            }
        }

        public static object Show(string Prompt, int WndWidth, ComboBoxStyle style, object datasource, string displaymember)
        {

            var frm = GetForm(Prompt, WndWidth, style, null);
            (frm.Controls[0] as ComboBox).DataSource = datasource;
            (frm.Controls[0] as ComboBox).DisplayMember = displaymember;
            System.Windows.Forms.DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                return frm.Controls[0].Text;

            return string.Empty;
        }

        public static object Show(string Prompt, int WndWidth, int ButtonsOffset, ComboBoxStyle style, object datasource, string displaymember)
        {

            var frm = GetForm(Prompt, WndWidth, style, null);
            (frm.Controls[0] as ComboBox).DataSource = datasource;
            (frm.Controls[0] as ComboBox).DisplayMember = displaymember;
            frm.Controls[1].Left += ButtonsOffset;
            frm.Controls[2].Left += ButtonsOffset;
            System.Windows.Forms.DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                return frm.Controls[0].Text;

            return string.Empty;
        }

        public static object Show(string Prompt, int WndWidth, ComboBoxStyle style, Font font,object datasource, string displaymember)
        {

            var frm = GetForm(Prompt, WndWidth, style, font);
            (frm.Controls[0] as ComboBox).DataSource = datasource;
            (frm.Controls[0] as ComboBox).DisplayMember = displaymember;
            System.Windows.Forms.DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                return frm.Controls[0].Text;

            return string.Empty;
        }

        public static object Show(string Prompt, int WndWidth, int ButtonsOffset, ComboBoxStyle style, Font font, object datasource, string displaymember)
        {

            var frm = GetForm(Prompt, WndWidth, style, font);
            (frm.Controls[0] as ComboBox).DataSource = datasource;
            (frm.Controls[0] as ComboBox).DisplayMember = displaymember;
            frm.Controls[1].Left += ButtonsOffset;
            frm.Controls[2].Left += ButtonsOffset;
            System.Windows.Forms.DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                return frm.Controls[0].Text;

            return string.Empty;
        }

        private static Form GetForm(string Prompt, int WndWidth,ComboBoxStyle style, Font font)
        {
            Form frm = new System.Windows.Forms.Form();
            frm.AutoSize = true;
            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            frm.Height = 1;
            frm.Width = 1;
            frm.Text = Prompt;
            ComboBox cb = new ComboBox();
            cb.DropDownStyle = style;
            if (font != null)
                cb.Font = font;
            cb.Width = WndWidth;
            cb.KeyPress += KeyPressed;
            frm.Controls.Add(cb);
            System.Windows.Forms.Button okb = new System.Windows.Forms.Button();
            okb.Text = "ОК";
            if (font == null)
                okb.Location = new System.Drawing.Point(25, 24);
            else
                okb.Location = new System.Drawing.Point(25, 24 + (int)(font.Size / 2));
            okb.DialogResult = System.Windows.Forms.DialogResult.OK;
            if (font != null)
                okb.Font = font;
            okb.AutoSize = true;
            frm.Controls.Add(okb);
            System.Windows.Forms.Button cancelb = new System.Windows.Forms.Button();
            cancelb.Text = "Отмена";
            cancelb.Location = new System.Drawing.Point(100, 24 + (int)(font.Size / 2));
            cancelb.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelb.AutoSize = true;
            if (font != null)
                cancelb.Font = font;
            frm.Controls.Add(cancelb);

            return frm;
        }
    }
}
