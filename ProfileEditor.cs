using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HydraPlot2
{
    public partial class ProfileEditor : Form
    {
        LoggerProfile profile;

        List<string> freeChannels;
      
        public ProfileEditor(LoggerProfile profile)
        {
            InitializeComponent();

            //profile = Program.GetLoggerProfile(profileId);

            //if (profile == null)
            //{
                // crear un profile en blanco
                //profile = new LoggerProfile(profileId, null, null, null);
                //profile = Program.GetLoggerProfileManagger().CreateNewLoggerProfile(profileId);
            //}

            this.profile = profile;
            string[] allChannels = Program.GetChannels(profile);
            freeChannels = new List<string>(0);

            foreach (string channel in allChannels)
            {
                if (!profile.IsChannelUsed(channel))
                    freeChannels.Add(channel);
            }
            //freeChannels = new List<string>(Program.GetChannels(profileId));

            listBox1.Items.AddRange(freeChannels.ToArray());

            listBox2.Items.AddRange(profile.GetWaterSensors());
            listBox3.Items.AddRange(profile.GetSalinitySensors());
            listBox4.Items.AddRange(profile.GetTempSensors());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // agregar water sensors desde listbox1
            if (listBox1.SelectedItem != null)
            {
                addToProfile(listBox2);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // remover desde water sensors, devolver a listbox1 y actualizar freechannels
            if (listBox2.SelectedItem != null)
            {
                removeFromProfile(listBox2);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // agregar a salinity sensors
            if (listBox1.SelectedItem != null)
            {
                addToProfile(listBox3);
            }
            
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // remover desde salinity sensors, devolver a listbox1 y actualizar freechannels
            if (listBox3.SelectedItem != null)
            {
                removeFromProfile(listBox3);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // agregar a temp sensors
            if (listBox1.SelectedItem != null)
            {
                addToProfile(listBox4);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // remover desde temp sensors, devolver a listbox1 y actualizar freechannels
            if (listBox4.SelectedItem != null)
            {
                removeFromProfile(listBox4);
            }

        }

        private void addToProfile(ListBox dest)
        {
            dest.Items.Add(listBox1.SelectedItem);
            freeChannels.Remove(listBox1.SelectedItem.ToString());
            listBox1.Items.Remove(listBox1.SelectedItem);

        }

        private void removeFromProfile(ListBox source)
        {
            listBox1.Items.Add(source.SelectedItem);
            freeChannels.Add(source.SelectedItem.ToString());
            source.Items.Remove(source.SelectedItem);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // guardar perfil
            List<string> t = new List<string>(0);

            foreach (string sensor in listBox2.Items)
            {
                t.Add(sensor);
            }

            profile.SetWaterSensors(t.ToArray());

            t.Clear();

            foreach (string sensor in listBox3.Items)
            {
                t.Add(sensor);
            }

            profile.SetSalinitySensors(t.ToArray());

            t.Clear();

            foreach (string sensor in listBox4.Items)
            {
                t.Add(sensor);
            }

            profile.SetTempSensors(t.ToArray());

            Program.GetLoggerProfileManagger().SaveLoggerProfile(profile);
            Program.ProfileSensorsChanged = true;

            this.Close();
        }
        

       
    }
}
