using System;
using System.Collections.Generic;
using System.Text;
using ZedGraph;

namespace HydraPlot2
{
    class SingleGraphView : GraphView
    {

        public SingleGraphView(GraphData data)
            : base(data)
        {
        }

        public override void Render()
        {
            fillPointPairList(gd.Channels[0]);
            // FIXME: revisar el uso de indice o nombre de canal
            PointPairList p = series[gd.Channels[0]];
            LineItem l = zed.GraphPane.AddCurve(gd.Channels[0], p, System.Drawing.Color.Red, SymbolType.None);
            //l.Line.IsSmooth = true;
            //l.Line.SmoothTension = 1.0F;

            zed.AxisChange();

        }
    }
}
