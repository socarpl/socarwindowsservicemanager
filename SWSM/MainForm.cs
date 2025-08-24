using SWSM.Core;
using SWSM.Extensions;

namespace SWSM
{
    public partial class MainForm : Form
    {
        ProgressBarForm pbf = null;

        public MainForm()
        {
            InitializeComponent();
            pbf = new ProgressBarForm();
        }

        private void Form1_Load(object sender, EventArgs e)
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
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            pbf.Text = "Loading services info...";
            pbf.Show();
            var windowsServices = await Task.Run(() => ServicesManager.GetAllSystemServices(_check_GetCommandlines.Checked, this.pbf.UpdateProgress));
            advancedDataGridView1.DataSource = windowsServices.ToDataTable();
            pbf.Hide();
        }

        private void _check_GetCommandlines_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
