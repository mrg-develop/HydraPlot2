using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    class IPPGraphView : BarGraphView
    {

        public IPPGraphView(GraphData gd)
            : base(gd)
        {
            base.Channel = "IPP";
        }

    }
}
