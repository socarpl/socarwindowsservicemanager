using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSM
{
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {
            InitializeComponent();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void ProgressBarForm_Load(object sender, EventArgs e)
        {

        }

        public void UpdateProgress(string msg, int progress, int total)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgress(msg, progress, total)));

            }
            else
            {
                label1.Text = $"[{progress.ToString().PadLeft(total.ToString().Length, '0')}/{total}] {msg}";
                progressBar1.Value = (int)((double)progress / total * 100); 
            }
        }
    }
}
