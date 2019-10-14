using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ZedGraph;

namespace HydraPlot2
{
    class EToGraphView:BarGraphView
    {
        public EToGraphView(GraphData gd)
            : base(gd)
        {
            base.Channel = "ETo";
            zed.GraphPane.XAxis.Scale.IsVisible = false;
        }


       

    }
}
