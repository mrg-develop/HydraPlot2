using System;
using System.Collections.Generic;
using System.Text;

namespace ZedGraph
{
    class Utility
    {
        public static double MinuteSpan(DateTime  d2, DateTime  d1)
        {
            TimeSpan s = TimeSpan.FromTicks(d2.Ticks - d1.Ticks);
            return s.TotalMinutes;
            
        }
    }
}
