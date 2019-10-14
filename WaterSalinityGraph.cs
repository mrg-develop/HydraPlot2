using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    class WaterSalinityGraph : Graph
    {
        private GraphData gd;

        public WaterSalinityGraph(LoggerProfile profile, Database database)
            : base(profile, database)
        {
        }

        private enum GraphType
        {
            WaterGraph,
            SalinityGraph,
            TemperatureGraph
        }

        public GraphView GetWaterStackedGraph(StackedStyles style)
        {
            return getStackedGraph(style, GraphType.WaterGraph);
        }

        public GraphView GetSalinityStackedGraph(StackedStyles style)
        {
            return getStackedGraph(style, GraphType.SalinityGraph);
        }

        public GraphView GetWaterSummedGraph()
        {
            return getSummedGraph(GraphType.WaterGraph);
        }

        public GraphView GetSalinitySummedGraph()
        {
            return getSummedGraph(GraphType.SalinityGraph);
        }

        private GraphView getSummedGraph(GraphType graphType)
        {
            string[] sensors;

            switch (graphType)
            {
                case GraphType.WaterGraph:
                    sensors = profile.GetWaterSensors();
                    return buildSummedGraphView(sensors);
                    break;

                case GraphType.SalinityGraph:
                    sensors = profile.GetSalinitySensors();
                    return buildSummedGraphView(sensors);

                    break;

                case GraphType.TemperatureGraph:
                    sensors = profile.GetTempSensors();
                    return buildSummedGraphView(sensors);

                    break;
                default:
                    return null;
                    break;
            }
        }


        private GraphView getStackedGraph(StackedStyles style, GraphType graphType)
        {
            string[] sensors;
            switch (graphType)
            {
                case GraphType.WaterGraph:
                    sensors = profile.GetWaterSensors();
                    return buildStackedGraphView(style, sensors, GraphData.GraphDataTypes.WATERSENSOR);
                    break;

                case GraphType.SalinityGraph:
                    sensors = profile.GetSalinitySensors();
                    return buildStackedGraphView(style, sensors, GraphData.GraphDataTypes.SALINITYSENSOR);

                    break;

                case GraphType.TemperatureGraph:
                    sensors = profile.GetTempSensors();
                    return buildStackedGraphView(style, sensors, GraphData.GraphDataTypes.TEMPERATURESENSOR);

                    break;
                default:
                    return null;
                    break;
            }
        }

        private GraphView buildSummedGraphView(string[] sensors)
        {
            gd = new GraphData(database, sensors, GraphData.GraphDataTypes.UNDEFINED);
            SummedGraphView gv = new SummedGraphView(gd);
            return gv;

        }

        private GraphView buildStackedGraphView(StackedStyles style, string[] sensors, GraphData.GraphDataTypes graphDataType)
        {
            if (style == StackedStyles.CommonY)
            {
                gd = new GraphData(database, sensors, graphDataType);
                StackedGraphView gv = new StackedGraphView(gd);
                gv.StackedStyle = StackedStyles.CommonY;
                return gv;
            }
            else if (style == StackedStyles.Stacked)
            {
                gd = new GraphData(database, sensors, graphDataType);
                StackedGraphView gv = new StackedGraphView(gd);
                gv.StackedStyle = StackedStyles.Stacked;
                return gv;

            }
            else if (style == StackedStyles.MultiStacked)
            {
                gd = new GraphData(database, profile.GetWaterSensors(), graphDataType);
                MultiStackedGraphView gv = new MultiStackedGraphView(gd);
                //gv.StackedStyle = StackedStyles.Stacked;
                return gv;

            }
            else
            {
                return null;
            }

        }

        public override GraphView GetStackedGraph(StackedStyles style)
        {
            return GetWaterStackedGraph(style);
        }

        public override GraphView GetSummedGraph()
        {
            return GetWaterSummedGraph();
        }
    }
}
