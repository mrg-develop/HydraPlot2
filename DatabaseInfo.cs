using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    public class DatabaseInfo
    {
        private int majorVersion;

        public int MajorVersion
        {
            get { return majorVersion; }
            set { majorVersion = value; }
        }

        private int minorVersion;

        public int MinorVersion
        {
            get { return minorVersion; }
            set { minorVersion = value; }
        }

        private string probe_id;

        public string ProbeId
        {
            get { return probe_id; }
            set { probe_id = value; }
        }

    }
}
