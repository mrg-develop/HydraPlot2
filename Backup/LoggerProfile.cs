using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    public class LoggerProfile
    {
        private string[] w;
        private string[] s;
        private string[] t;

        private string loggerID;

        public string LoggerID { 
            get { return loggerID; } 
        }

        private string databasePath;
        private string databaseName;

        public string DatabaseName
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(databasePath);
            }

        }

        public string DataBasePath
        {
            get {
                return databasePath;
            }

        }

        public LoggerProfile(string loggerID, string[] waterSensors,
                             string[] salinitySensors,
                             string[] tempSensors)
        {
            new LoggerProfile(loggerID, waterSensors, salinitySensors, tempSensors, null);
        }

        public LoggerProfile(string loggerID, string[] waterSensors,
                             string[] salinitySensors,
                             string[] tempSensors, string Database)
        {
            w = waterSensors;
            s = salinitySensors;
            t = tempSensors;
            this.loggerID = loggerID;
            this.databasePath = Database;
        }

        public string[] GetWaterSensors()
        {
            return w;
        }

        public string[] GetSalinitySensors()
        {
            return s;
        }

        public string[] GetTempSensors()
        {
            return t;
        }

        public void SetWaterSensors(string[] sensors)
        {
            w = sensors;
        }

        public void SetSalinitySensors(string[] sensors)
        {
            s = sensors;
        }

        public void SetTempSensors(string[] sensors)
        {
            t = sensors;
        }


        public bool IsChannelUsed(string channelName)
        {
            if (Array.Exists<string>(w, x => x.Equals(channelName)))
                return true;

            if (Array.Exists<string>(s, x => x.Equals(channelName)))
                return true;

            if (Array.Exists<string>(t, x => x.Equals(channelName)))
                return true;

            return false;
        }

        


    }
}
