using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace anvlib.Forms
{
    public partial class LoadingForm : Form
    {        
        public LoadingForm()
        {
            InitializeComponent();
        }

        public void SetLables(string text1, string text2)
        {
            label1.Text = text1;
            label2.Text = text2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();
            label1.Visible = !label1.Visible;
            label2.Visible = !label2.Visible;
            Application.DoEvents();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            timer1.Start();            
        }

        public void start()
        {
            Application.DoEvents();
            timer1.Start();
            Application.DoEvents();
        }

        public virtual void OnStopShowing(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ThreadStart(delegate
                {
                    timer1.Stop();
                    Close();
                }));
            }
            else
            {
                timer1.Stop();
                Close();
            }
        }        
    }
}
