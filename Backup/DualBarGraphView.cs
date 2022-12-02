using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ZedGraph;

namespace HydraPlot2
{
    class DualBarGraphView: GraphView
    {
        private bool showSecondBar = false;

        public bool ShowSecondBar
        {
            get { return showSecondBar; }
            set { showSecondBar = value; }
        }

        public class BarElement
        {
            public string Channel;
            public Color BarColor;

            public BarElement(string channel, Color barcolor)
            {
                Channel = channel;
                BarColor = barcolor;
            }
        }

        private List<BarElement> barElements;

        public List<BarElement> BarElements
        {
            get { return barElements; }
        }

        public DualBarGraphView(GraphData gd)
            : base(gd)
        {
            barElements = new List<BarElement>(0);
            zed.GraphPane.Chart.IsRectAuto = false;
            defaultY = defaultY + 15;

            
        }


        public override void Render()
        {

            int j = 1;

            if (showSecondBar)
                j = 2;

            for (int i = 0; i < j; i++) {
                BarElement element = barElements[i];
                    fillPointPairList(element.Channel);
                    PointPairList p = series[element.Channel];

                if (i == 0)
                {
                    BarItem b = zed.GraphPane.AddBar(element.Channel, p, element.BarColor);
                    b.Bar.Fill.Brush = new SolidBrush(element.BarColor);
                    zed.GraphPane.BarSettings.ClusterScaleWidthAuto = false;
                    zed.GraphPane.BarSettings.ClusterScaleWidth = 1;
                    zed.GraphPane.BarSettings.MinClusterGap = 0;
                    //zed.IsShowCursorValues = true;
                    zed.IsShowPointValues = true;
                }
                else
                {
                    LineItem l = zed.GraphPane.AddCurve(element.Channel, p, element.BarColor);
                    l.Symbol.IsVisible = true;
                    l.Symbol.Fill.Brush = new SolidBrush(Color.Cyan);
                    l.Symbol.Fill.IsVisible = true;
                }
            }
            zed.PointValueEvent += new ZedGraphControl.PointValueHandler(zed_PointValueEvent);
            zed.GraphPane.CurveList.Reverse();
            //BarItem.CreateBarLabels(zed.GraphPane, true, "F2", "Arial", 9, Color.White, false, false, false);


            //zed.GraphPane.XAxis.Scale.IsVisible = false;

            zed.AxisChange(); 

        }

        string zed_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {

            string label = curve.Label.Text + " " + XDate.ToString(curve[iPt].X, "dd-MM-yyyy") + ": " + curve[iPt].Y.ToString() + " mm";
            return label;
        }

    }
}
