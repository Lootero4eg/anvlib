using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using anvlib.Interfaces;

namespace anvlib.Forms
{
    public partial class ProgressForm : Form
    {
        public int Cnt
        {
            get { return ProgressStatus.Value; }
        }

        public void SetFormIcon(Icon icon)
        {
            this.Icon = icon;
        }

        public void SetFormCaption(string Caption)
        {
            this.Text = Caption;
        }

        public void SetAnimationDelay(int milleseconds)
        {
            ProgressStatus.MarqueeAnimationSpeed = milleseconds;
        }

        public void IncreaseProgressBar(object sender, EventArgs e)
        {
            if (!this.InvokeRequired)
                ProgressStatus.Increment(1);
            else
            {
                this.Invoke(new ThreadStart(
                    delegate
                    {
                        ProgressStatus.Increment(1);
                    }
                    ));
            }
        }

        public void SetProgressTask(string caption)
        {
            ProgressTask.Text = caption;
        }

        public void SetMaxValue(int MaxValue)
        {
            ProgressStatus.Maximum = MaxValue;
        }

        public ProgressForm()
        {
            InitializeComponent();
        }
    }
}
