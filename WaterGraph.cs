using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    class WaterGraph : Graph
    {
        private GraphData gd;

        public WaterGraph(LoggerProfile profile, Database database)
            : base(profile, database)
        {
        }

        public override GraphView GetStackedGraph(StackedStyles style)
        {
            if (style == StackedStyles.CommonY)
            {
                gd = new GraphData(database, profile.GetWaterSensors(), GraphData.GraphDataTypes.WATERSENSOR);
                StackedGraphView gv = new StackedGraphView(gd);
                gv.StackedStyle = StackedStyles.CommonY;
                return gv;
            }
            else if (style == StackedStyles.Stacked)
            {
                gd = new GraphData(database, profile.GetWaterSensors(), GraphData.GraphDataTypes.WATERSENSOR);
                StackedGraphView gv = new StackedGraphView(gd);
                gv.StackedStyle = StackedStyles.Stacked;
                return gv;

            }
            else if (style == StackedStyles.MultiStacked)
            {
                gd = new GraphData(database, profile.GetWaterSensors(), GraphData.GraphDataTypes.WATERSENSOR);
                MultiStackedGraphView gv = new MultiStackedGraphView(gd);
                //gv.StackedStyle = StackedStyles.Stacked;
                return gv;

            }
            else
            {
                return null;
            }
        }

        public override GraphView GetSummedGraph()
        {
            gd = new GraphData(database, profile.GetWaterSensors(),GraphData.GraphDataTypes.WATERSENSOR);
            //SummedGraphView gv = new SummedGraphView(gd);
            WaterSummedGraphView gv = new WaterSummedGraphView(gd);
            return gv;
        }
    }
}
