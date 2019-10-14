using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace HydraPlot2
{
    public partial class SensorSelector : Form
    {



        private List<Sensor> sensorListCollection;

        public SensorSelector(ref List<Sensor> sensorList)
        {
            InitializeComponent();
            sensorListCollection = sensorList;
            //CheckedListBox.ObjectCollection c = new CheckedListBox.ObjectCollection(checkedListBox1);
            
            foreach (Sensor s in sensorListCollection)
            {

                checkedListBox1.Items.Add(s, s.Selected);
            }

            
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Sensor s = (Sensor)(((CheckedListBox)sender).Items[e.Index]);
            if (e.NewValue == CheckState.Checked)
            {
                s.Selected = true;
            }
            else
            {
                s.Selected = false;
            }
        }
    }
}
