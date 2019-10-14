using System;
using System.Collections.Generic;
using System.Text;
using ZedGraph;
using System.Drawing;


namespace HydraPlot2
{
    class WaterSummedGraphView : SummedGraphView      
    {
        public WaterSummedGraphView(GraphData gd)
            :base(gd)
        {
           
        }

        protected override void fillPointPairList(string[] channels)
        {
            PointPairList p = new PointPairList();

            SortedDictionary<DateTime, Decimal?> data = gd.GetPonderatedSummedData(channels);
            //SortedDictionary<DateTime, Decimal?> data = gd.GetWaterPonderatedSummedData(channels);

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
    }
}
