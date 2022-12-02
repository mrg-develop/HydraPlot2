using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    class GraphParams
    {
        private string databaseName;
        private string[] channelNames;

        public GraphParams(string dbName, string[] chNames)
        {
            databaseName = dbName;
            channelNames = new string[chNames.Length];
            chNames.CopyTo(channelNames, 0);
        }

        public string DatabaseName { 
            get {
                return databaseName;
            }
        }

        public string[] ChannelNames {
            get { return channelNames; }
        }
    }
}
