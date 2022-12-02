using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HydraPlot2
{
    public partial class DbSelector : Form
    {
        private string selectedProbeId;
        public string SelectedProbeId
        {
            get
            {
                return selectedProbeId;
            }
        }

        public DbSelector(string[] probe_ids)
        {
            InitializeComponent();

            foreach (string probe_id in probe_ids)
            {
                comboBox1.Items.Add(probe_id);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selectedProbeId = comboBox1.SelectedItem.ToString();
            this.Close();
        }
    }
}
