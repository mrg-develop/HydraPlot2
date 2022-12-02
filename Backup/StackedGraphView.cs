using System;
using System.Collections.Generic;
using System.Text;
using ZedGraph;
using System.Drawing;

namespace HydraPlot2
{
    class StackedGraphView : GraphView
    {
        private struct range
        {
            public string key;
            public double min;
            public double max;
            public double bias;
        }

        
        private StackedStyles style;

        public StackedStyles Style
        {
            get { return style; }
        }

        public StackedGraphView(GraphData data)
            : base(data)
        {
            style = StackedStyles.CommonY;
            zed.GraphPane.Chart.IsRectAuto = false;
            defaultY = defaultY + 15;
        }

        public StackedStyles StackedStyle {
            get { return style;  }
            set { style = value; }
        }

        public override void Render()
        {
            Color c;
            // Multi Y approach

            for (int i = 0; i < gd.Channels.Length; i++)
            {
                fillPointPairList(gd.Channels[i]);
            }

            if (style == StackedStyles.Stacked)
            {
                stackAllPointPairList();
            }

            foreach (KeyValuePair<string, PointPairList> serie in series) {
                // FIXME: revisar el uso de indice o nombre de canal

                c = getLineColor();
                //PointPairList p = series[graph.Channels[i]];
                PointPairList p = serie.Value;
                //LineItem l = zed.GraphPane.AddCurve(graph.Channels[i], p, c, SymbolType.None);
                LineItem l = zed.GraphPane.AddCurve(serie.Key, p, c, SymbolType.None);
                
                //l.YAxisIndex = i;
                l.YAxisIndex = 0;
                //zed.GraphPane.YAxis.Color = c;
                
                // FIXME:  fix parcial para la sincronía de posición del
                // eje Y entre gráficos
                if (style == StackedStyles.CommonY)
                {
                    zed.GraphPane.YAxis.IsVisible = true;
                    zed.GraphPane.YAxis.Scale.FontSpec.FontColor = Color.Black;
                }
                else
                {
                    zed.GraphPane.YAxis.IsVisible = true;
                    zed.GraphPane.YAxis.Scale.FontSpec.FontColor = Color.White;
                }

                // FIXME: unificar con acceso via YAxisList
                /*
                if (i > 0)
                {
                    YAxis axis = new YAxis();
                    axis.Title.IsVisible = false;
                    axis.Scale.FontSpec.Size = 10;
                    zed.GraphPane.YAxisList.Add(axis);
                    zed.GraphPane.YAxisList[i].Color = c;
                }
                else
                {
                    zed.GraphPane.YAxis.Color = c;
                }*/

                
            }
            //l.Line.IsSmooth = true;
            //l.Line.SmoothTension = 1.0F;
            //zed.GraphPane.XAxis.Color = Color.Black;
            zed.AxisChange();

        }

        private void stackAllPointPairList()
        {
            if (series.Count > 1)
            {
                List<range> Ranges = new List<range>(0);

                foreach (KeyValuePair<string, PointPairList> serie in series)
                {

                    double min = double.MaxValue;
                    double max = double.MinValue;

                    foreach (PointPair p in serie.Value)
                    {
                        if (p.Y < min)
                        {
                            min = p.Y;
                        }

                        if (p.Y > max)
                        {
                            max = p.Y;
                        }
                    }
                    range r = new range();
                    r.max = max;
                    r.min = min;
                    r.key = serie.Key;
                    Ranges.Add(r);
                }

                // Ranges tiene los mínimos y máximos para cada serie
                // ahora aplicar bias para ordenar relativamente en el GraphPane
                // según orden de aparición
                double gap = 0.0;
                double bias = 0;
                for (int i = Ranges.Count - 2; i >= 0; i--)
                {
                    bias = bias + Ranges[i+1].max + gap - Ranges[i].min;
                    PointPairList p = series[Ranges[i].key];
                    p.Bias = bias;
                    foreach (PointPair point in p)
                    {
                        point.Y = point.Y + bias;
                    }
                    
                }
            } // if series.Count > 1
        }
    }
}
