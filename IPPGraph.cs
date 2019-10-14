using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    class IPPGraph:Graph
    {
        private string[] channels = { "IPP" };
        private GraphData gd;

        public IPPGraph(LoggerProfile profile, Database database)
            : base(profile, database)
        {
        }

        public override GraphView GetStackedGraph(StackedStyles style)
        {
            throw new NotImplementedException();
        }

        public override GraphView GetSummedGraph()
        {
            throw new NotImplementedException();
        }

        public GraphView GetBarGraph()
        {
            gd = new GraphData(database, channels, GraphData.GraphDataTypes.IPP);
            IPPGraphView gv = new IPPGraphView(gd);
            return gv;
        }

    }
}
