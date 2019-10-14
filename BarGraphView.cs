using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ZedGraph;

namespace HydraPlot2
{
    class BarGraphView : GraphView
    {

        public BarGraphView(GraphData gd)
            : base(gd)
        {
            barColor = Color.Black;
            zed.GraphPane.Chart.IsRectAuto = false;            
            defaultY = defaultY + 15;
        }

        private string channel;
        public string Channel 
        {
            get
            {
                return channel;
            }
            set
            {
                channel = value;
            }
        }

        private Color barColor;
        public Color BarColor
        {
            get
            {
                return barColor;
            }

            set
            {
                barColor = value;
            }
        }

        public override void Render()
        {
            fillPointPairList(channel);
            PointPairList p = series[channel];
            BarItem b = zed.GraphPane.AddBar(channel, p, barColor);
            b.Bar.Fill.Brush = new SolidBrush(barColor);
            zed.GraphPane.BarSettings.ClusterScaleWidthAuto = false;
            zed.GraphPane.BarSettings.ClusterScaleWidth = 1;
            zed.GraphPane.BarSettings.MinClusterGap = 0;
            //zed.IsShowCursorValues = true;
            zed.IsShowPointValues = true;
            zed.PointValueEvent += new ZedGraphControl.PointValueHandler(zed_PointValueEvent);
            BarItem.CreateBarLabels(zed.GraphPane, true, "F2", "Arial", 9, Color.White, false, false, false);

            
            //zed.GraphPane.XAxis.Scale.IsVisible = false;

            zed.AxisChange(); 
        }

        string zed_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            string label = curve[iPt].Y.ToString() + " mm";
            return label;
        }
    }
}
