using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    class EToGraph:Graph
    {
        protected string[] channels = { "ETo"};
        protected GraphData gd;

        public EToGraph(LoggerProfile profile, Database database)
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
            gd = new GraphData(database, channels, GraphData.GraphDataTypes.ETO);
            EToGraphView gv = new EToGraphView(gd);
            return gv;
        }
    }
}
