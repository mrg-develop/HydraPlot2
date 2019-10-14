using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    class TemperatureGraph : Graph
    {
        private GraphData gd;

        public TemperatureGraph(LoggerProfile profile, Database database)
            : base(profile, database)
        {
        }

        public override GraphView GetStackedGraph(StackedStyles style)
        {
            if (style == StackedStyles.CommonY)
            {
                gd = new GraphData(database, profile.GetTempSensors(), GraphData.GraphDataTypes.TEMPERATURESENSOR);
                StackedGraphView gv = new StackedGraphView(gd);
                gv.StackedStyle = StackedStyles.CommonY;
                return gv;
            }
            else if (style == StackedStyles.Stacked)
            {
                gd = new GraphData(database, profile.GetTempSensors(), GraphData.GraphDataTypes.TEMPERATURESENSOR);
                StackedGraphView gv = new StackedGraphView(gd);
                gv.StackedStyle = StackedStyles.Stacked;
                return gv;

            }
            else if (style == StackedStyles.MultiStacked)
            {
                gd = new GraphData(database, profile.GetTempSensors(), GraphData.GraphDataTypes.TEMPERATURESENSOR);
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
            gd = new GraphData(database, profile.GetTempSensors(), GraphData.GraphDataTypes.TEMPERATURESENSOR);
            SummedGraphView gv = new SummedGraphView(gd);
            return gv;
        }
    }
}
