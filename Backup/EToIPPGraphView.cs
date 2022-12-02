using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HydraPlot2
{
    class EToIPPGraphView : DualBarGraphView
    {
        public EToIPPGraphView(GraphData gd)
            : base(gd)
        {
            BarElements.Add(new BarElement("ETo", Color.LightGray));
            BarElements.Add(new BarElement("IPP", Color.Blue));

        }
    }
}
