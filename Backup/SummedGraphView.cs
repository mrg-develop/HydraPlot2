using System;
using System.Collections.Generic;
using System.Text;
using ZedGraph;
using System.Drawing;

namespace HydraPlot2
{
    class SummedGraphView : GraphView
    {
        public SummedGraphView(GraphData data)
            : base(data)
        {
            zed.GraphPane.Chart.IsRectAuto = false;
            defaultY = defaultY + 15;

        }

        protected virtual void fillPointPairList(string[] channels)
        {
            PointPairList p = new PointPairList();

            SortedDictionary<DateTime, Decimal?> data = gd.GetPonderatedSummedData(channels);

            foreach (KeyValuePair<DateTime, Decimal?> d in data)
            {
                if (d.Value != null)
                {
                    p.Add(new XDate(d.Key), (double)d.Value);
                }
                else
                {
                    p.Add(new XDate(d.Key), double.NaN);
                }
            }

            // FIXME revisar esto
            series.Add("SUMMED", p);

            
        }

        public override void Render()
        {
            List<string> selectedChannels = new List<string>(0);
            foreach (Sensor s in sensorList)
            {
                if (s.Selected)
                {
                    selectedChannels.Add(s.Name);
                }
            }

            

            fillPointPairList(selectedChannels.ToArray());
            Color c = getLineColor();
            PointPairList p = series["SUMMED"];
            LineItem l = zed.GraphPane.AddCurve("PONDERADO", p, Color.Black, SymbolType.None);
            
            //zed.GraphPane.XAxis.Scale.IsVisible = false;
            
            zed.AxisChange();
        }
    }
}