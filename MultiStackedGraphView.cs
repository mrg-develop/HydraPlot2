using System;
using System.Collections.Generic;
using System.Text;
using ZedGraph;
using System.Drawing;

namespace HydraPlot2
{
    class MultiStackedGraphView : GraphView
    {
        protected MasterPane masterPane;

        public MultiStackedGraphView(GraphData graph) : base(graph)
        {
            masterPane = zed.MasterPane;
            masterPane.IsFontsScaled = false;
            masterPane.PaneList.Clear();
            masterPane.SetLayout(zed.CreateGraphics(), PaneLayout.SingleColumn);
            masterPane.Legend.IsVisible = true;
            masterPane.Legend.FontSpec.Size = 10;
            masterPane.InnerPaneGap = 2;
            masterPane.Margin.All = 2;
            
            defaultY = 2;
            zed.IsSynchronizeXAxes = true;
            
        }

        public override void Render()
        {
            foreach (string channel in gd.Channels)
            {
                fillPointPairList(channel);
            }

            int i = 0;
            foreach (KeyValuePair<string, PointPairList> serie in series)
            {
                
                System.Drawing.Color c = getLineColor();
                PointPairList p = serie.Value;
                GraphPane pane = new GraphPane();                
                applyStandardGraphFormat(pane);
                pane.XAxis.Scale.IsVisible = false;

                pane.Chart.IsRectAuto = false;
                
                pane.Legend.IsVisible = false;
                pane.Border.IsVisible = false;
                pane.Border.Color = c;
                
                //pane.Margin.All = 5;
                LineItem l = pane.AddCurve(serie.Key, p, c, SymbolType.None);

                if (i == series.Count - 1)
                {
                    pane.XAxis.Scale.IsVisible = true;
                    
                }
                pane.Chart.Rect = new RectangleF(0, 0, 10, 10);
                masterPane.Add(pane);
                i++;
            }
            
            zed.AxisChange();
            //zed.Invalidate();
            //setupPanes();

        }

        private void setupPanes()
        {
            if (zed.GraphPane.Chart.IsRectAuto == false)
            {

                float height = 0;
                float width = 0;

                foreach (GraphPane pane in zed.MasterPane.PaneList)
                {

                    if (pane.XAxis.Scale.IsVisible)
                    {
                        height = pane.Rect.Height - zed.MasterPane.InnerPaneGap * 2 - 20;
                    }
                    else
                    {
                        height = pane.Rect.Height - zed.MasterPane.InnerPaneGap * 2 - 20;
                    }

                    width = pane.Rect.Width - defaultX - marginRight + zed.MasterPane.Margin.Left + zed.MasterPane.Margin.Right;

                    pane.Chart.Rect = new RectangleF(pane.Rect.X + defaultX, pane.Rect.Y + defaultY, width, height);


                }
            }
        }


        protected override void zed_Resize(object sender, EventArgs e)
        {
            if (zed.GraphPane != null)
            {
                setupPanes();
            }
        }
    }
}
