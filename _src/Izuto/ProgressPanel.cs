using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Izuto
{
    public partial class ProgressPanel : UserControl
    {
        public ProgressPanel()
        {
            InitializeComponent();
        }

        public void UpdateProgress(string text, int value, int maxvalue)
        {
            Invoke(new Action(() =>
            {
                pictureBox1.Image = MainForm.Logo;
                textBox1.Text = text;
                progressBar1.Maximum = maxvalue;
                progressBar1.Value = value;
                progressBar1.Refresh();
                textBox1.Refresh();
            }));
        }
        public void EndProgressUpdates()
        {
            Invoke(new Action(() =>
            {
                textBox1.Text = "Izuto is working on it";
                progressBar1.Maximum = 1;
                progressBar1.Value = 0;
            }));
        }
    }
}
