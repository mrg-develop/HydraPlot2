using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    public class Sensor
    {

        private string name;
        private bool selected;
        private double depth;

        public Sensor(string name, bool selected, double depth)
        {
            this.name = name;
            this.selected = selected;
            this.depth = depth;
        }

        public string Name
        {
            get { return name; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public override string ToString()
        {
            return name;
        } 
    }
}
