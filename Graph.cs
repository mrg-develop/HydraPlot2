using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    public abstract class Graph
    {


        protected LoggerProfile profile;
        protected Database database;
        private List<Sensor> sensorList;

        public Graph(LoggerProfile profile, Database database)
        {
            this.profile = profile;
            this.database = database;
            this.sensorList = new List<Sensor>(0);
            
        }

        public void AddSensor(Sensor s)
        {
            sensorList.Add(s);
        }

        public void RemoveSensor(Sensor s)
        {
            sensorList.Remove(s);
        }

        public abstract GraphView GetStackedGraph(StackedStyles style);

        public abstract GraphView GetSummedGraph();
    }
}
