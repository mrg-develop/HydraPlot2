using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    class EtoIPPGraph : EToGraph
    {
        public EtoIPPGraph(LoggerProfile profile, Database database)
            : base(profile, database)
        {
            channels = new string[2];
            channels[0] = "ETo";
            channels[1] = "IPP";
        }

        public GraphView GetETOIPPBarGraph()
        {
            gd = new GraphData(database, channels, GraphData.GraphDataTypes.IPP);
            EToIPPGraphView gv = new EToIPPGraphView(gd);
            return gv;

        }

    }
}
